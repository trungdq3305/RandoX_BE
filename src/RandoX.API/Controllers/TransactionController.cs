using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandoX.Data.DBContext;
using RandoX.Data.Entities;
using RandoX.Common;
using RandoX.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RandoX.API.Controllers
{
    public class TransactionController : BaseAPIController
    { 
        private readonly IVNPayService _vnPayService;
        private readonly randox_dbContext _dbContext;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(
            IVNPayService vnPayService,
            randox_dbContext dbContext,
            ILogger<TransactionController> logger)
        {
            _vnPayService = vnPayService;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("vnpay/create")]
        public async Task<IActionResult> CreateVNPayPayment([FromQuery] CreatePaymentRequest request)
        {
            try
            {
                // Lấy thông tin order
                var order = await _dbContext.Orders
                    .Include(o => o.Cart)
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId);

                if (order == null)
                {
                    return NotFound(new { Message = "Không tìm thấy đơn hàng" });
                }

                // Tính tổng tiền (bao gồm shipping cost nếu có)
                decimal totalAmount = (order.TotalAmount ?? 0) + (order.ShippingCost ?? 0);

                if (totalAmount <= 0)
                {
                    return BadRequest(new { Message = "Số tiền thanh toán không hợp lệ" });
                }

                // Tạo request cho VNPay
                var vnpayRequest = new VNPayCreatePaymentRequest
                {
                    OrderId = order.Id,
                    Amount = totalAmount,
                    OrderInfo = $"Thanh toán đơn hàng {order.Id}",
                    IpAddress = GetClientIpAddress(),
                    Locale = "vn"
                };

                // Gọi service tạo link thanh toán
                var result = await _vnPayService.CreatePaymentAsync(vnpayRequest);

                if (result.Success)
                {
                    return Ok(new
                    {
                        Success = true,
                        PaymentUrl = result.PaymentUrl,
                        OrderId = order.Id,
                        Amount = totalAmount,
                        Message = "Tạo link thanh toán thành công"
                    });
                }

                return BadRequest(new { Success = false, Message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thanh toán VNPay cho Order: {OrderId}", request.OrderId);
                return StatusCode(500, new { Message = "Có lỗi xảy ra khi tạo thanh toán" });
            }
        }

        [HttpGet("vnpay/callback")]
        public async Task<IActionResult> VNPayCallback([FromQuery] VNPayCallbackRequest callback)
        {
            try
            {
                bool isValid = await _vnPayService.ValidateCallbackAsync(callback);

                if (!isValid)
                {
                    _logger.LogWarning("VNPay callback có chữ ký không hợp lệ: {TxnRef}", callback.vnp_TxnRef);
                    return Ok("RspCode=97&Message=Invalid signature");
                }

                await _vnPayService.ProcessPaymentCallbackAsync(callback);

                // VNPay yêu cầu response format cụ thể
                string returnMessage = (callback.vnp_ResponseCode == "00" || callback.vnp_ResponseCode == "24")
                    ? "RspCode=00&Message=Confirm Success"
                    : "RspCode=00&Message=Order not found";

                return Ok(returnMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý VNPay callback: {TxnRef}", callback.vnp_TxnRef);
                return Ok("RspCode=99&Message=Unknown error");
            }
        }

        [HttpGet("vnpay/return")]
        public async Task<IActionResult> VNPayReturn([FromQuery] VNPayCallbackRequest returnData)
        {
            try
            {
                // Validate signature
                bool isValid = await _vnPayService.ValidateCallbackAsync(returnData);

                if (!isValid)
                {
                    return BadRequest(new { Success = false, Message = "Chữ ký không hợp lệ" });
                }

                // Lấy thông tin transaction
                var transaction = await _dbContext.Transactions
                    .Include(t => t.TransactionStatus)
                    .FirstOrDefaultAsync(t => t.Id == returnData.vnp_TxnRef);

                if (transaction == null)
                {
                    return NotFound(new { Success = false, Message = "Không tìm thấy giao dịch" });
                }

                var response = new
                {
                    Success = returnData.vnp_ResponseCode == "00",
                    OrderId = returnData.vnp_TxnRef,
                    TransactionId = returnData.vnp_TransactionNo ?? returnData.vnp_TxnRef, // Fallback nếu không có vnp_TransactionNo
                    Amount = decimal.Parse(returnData.vnp_Amount) / 100,
                    ResponseCode = returnData.vnp_ResponseCode,
                    Status = transaction.TransactionStatus?.Id,
                    Message = GetResponseMessage(returnData.vnp_ResponseCode),
                    PaymentDate = returnData.vnp_PayDate, // Có thể null nếu thanh toán thất bại
                    BankCode = returnData.vnp_BankCode, // Có thể null
                    BankTransactionNo = returnData.vnp_BankTranNo // Có thể null
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý VNPay return: {TxnRef}", returnData.vnp_TxnRef);
                return StatusCode(500, new { Success = false, Message = "Có lỗi xảy ra" });
            }
        }

        [HttpGet("transaction/{orderId}/status")]
        public async Task<IActionResult> GetTransactionStatus(string orderId)
        {
            try
            {
                var transaction = await _dbContext.Transactions
                    .Include(t => t.TransactionStatus)
                    .FirstOrDefaultAsync(t => t.Id == orderId );

                if (transaction == null)
                {
                    return NotFound(new { Message = "Không tìm thấy giao dịch" });
                }

                return Ok(new
                {
                    OrderId = orderId,
                    TransactionId = transaction.Id,
                    Status = transaction.TransactionStatus?.Id,
                    Amount = transaction.Amount,
                    PaymentDate = transaction.PayDate,
                    Description = transaction.Description,
                    CreatedAt = transaction.CreatedAt,
                    UpdatedAt = transaction.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy trạng thái giao dịch: {OrderId}", orderId);
                return StatusCode(500, new { Message = "Có lỗi xảy ra" });
            }
        }

        private string GetClientIpAddress()
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Kiểm tra header X-Forwarded-For (cho trường hợp sử dụng proxy)
            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
            }

            // Nếu là localhost IPv6, chuyển về IPv4
            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            return ipAddress ?? "127.0.0.1";
        }

        private string GetResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dịch thành công",
                "07" => "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường)",
                "09" => "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng",
                "10" => "Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần",
                "11" => "Giao dịch không thành công do: Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch",
                "12" => "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng bị khóa",
                "13" => "Giao dịch không thành công do Quý khách nhập sai mật khẩu xác thực giao dịch (OTP)",
                "24" => "Giao dịch không thành công do: Khách hàng hủy giao dịch",
                "51" => "Giao dịch không thành công do: Tài khoản của quý khách không đủ số dư để thực hiện giao dịch",
                "65" => "Giao dịch không thành công do: Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày",
                "75" => "Ngân hàng thanh toán đang bảo trì",
                "79" => "Giao dịch không thành công do: KH nhập sai mật khẩu thanh toán quá số lần quy định",
                "99" => "Các lỗi khác (lỗi còn lại, không có trong danh sách mã lỗi đã liệt kê)",
                _ => "Giao dịch không thành công"
            };
        }
    }
    public class CreatePaymentRequest
    {
        [Required]
        public string OrderId { get; set; }
    }
}