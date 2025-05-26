using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RandoX.Data.DBContext;
using RandoX.Data.Entities;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace RandoX.Service.Helper
{
    // 1. VNPay Configuration Model
    public class VNPayConfig
    {
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string PaymentUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string Version { get; set; } = "2.1.0";
        public string Command { get; set; } = "pay";
    }

    // 2. VNPay Request Models
    public class VNPayCreatePaymentRequest
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }
        public string IpAddress { get; set; }
        public string Locale { get; set; } = "vn";
        public string CurrCode { get; set; } = "VND";
    }

    public class VNPayCreatePaymentResponse
    {
        public string PaymentUrl { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class VNPayCallbackRequest
    {
        public string vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string vnp_BankTranNo { get; set; }
        public string vnp_CardType { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_PayDate { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public string vnp_TxnRef { get; set; }
        //public string vnp_SecureHashType { get; set; }
        public string vnp_SecureHash { get; set; }
    }

    // 3. VNPay Service Interface
    public interface IVNPayService
    {
        Task<VNPayCreatePaymentResponse> CreatePaymentAsync(VNPayCreatePaymentRequest request);
        Task<bool> ValidateCallbackAsync(VNPayCallbackRequest callback);
        Task  ProcessPaymentCallbackAsync(VNPayCallbackRequest callback);
    }

    // 4. VNPay Service Implementation
    public class VNPayService : IVNPayService
    {
        private readonly VNPayConfig _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<VNPayService> _logger;

        public VNPayService(
            IOptions<VNPayConfig> config,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<VNPayService> logger)
        {
            _config = config.Value;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task<VNPayCreatePaymentResponse> CreatePaymentAsync(VNPayCreatePaymentRequest request)
        {
            try
            {
                var vnpay = new VnPayLibrary();

                // Thêm các tham số bắt buộc
                vnpay.AddRequestData("vnp_Version", _config.Version);
                vnpay.AddRequestData("vnp_Command", _config.Command);
                vnpay.AddRequestData("vnp_TmnCode", _config.TmnCode);
                vnpay.AddRequestData("vnp_Amount", ((long)(request.Amount * 100)).ToString());
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", request.CurrCode);
                vnpay.AddRequestData("vnp_IpAddr", request.IpAddress);
                vnpay.AddRequestData("vnp_Locale", request.Locale);
                vnpay.AddRequestData("vnp_OrderInfo", request.OrderInfo);
                vnpay.AddRequestData("vnp_OrderType", "other");
                vnpay.AddRequestData("vnp_ReturnUrl", _config.ReturnUrl);
                vnpay.AddRequestData("vnp_TxnRef", request.OrderId);

                // Tạo URL thanh toán
                string paymentUrl = vnpay.CreateRequestUrl(_config.PaymentUrl, _config.HashSecret);

                // Lưu thông tin transaction với status pending
                await CreatePendingTransactionAsync(request.OrderId, request.Amount);

                return new VNPayCreatePaymentResponse
                {
                    PaymentUrl = paymentUrl,
                    Success = true,
                    Message = "Tạo link thanh toán thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo link thanh toán VNPay cho Order: {OrderId}", request.OrderId);
                return new VNPayCreatePaymentResponse
                {
                    Success = false,
                    Message = "Có lỗi xảy ra khi tạo link thanh toán"
                };
            }
        }

        public async Task<bool> ValidateCallbackAsync(VNPayCallbackRequest callback)
        {
            try
            {
                var vnpay = new VnPayLibrary();

                // Thêm tất cả tham số trừ secure hash
                foreach (var prop in typeof(VNPayCallbackRequest).GetProperties())
                {
                    if (prop.Name == "vnp_SecureHash") continue;

                    var value = prop.GetValue(callback)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        vnpay.AddResponseData(prop.Name, value);
                    }
                }

                // Validate secure hash
                bool isValid = vnpay.ValidateSignature(callback.vnp_SecureHash, _config.HashSecret);
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi validate callback VNPay");
                return false;
            }
        }

        public async Task ProcessPaymentCallbackAsync(VNPayCallbackRequest callback)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<randox_dbContext>();

            try
            {
                var transaction = await dbContext.Transactions
                    .FirstOrDefaultAsync(t => t.Id == callback.vnp_TxnRef );

                if (transaction == null)
                {
                    _logger.LogWarning("Không tìm thấy transaction với ID: {TransactionId}", callback.vnp_TxnRef);
                    return;
                }

                // Cập nhật trạng thái transaction
                string statusId = GetTransactionStatusId(callback.vnp_ResponseCode);
                transaction.TransactionStatusId = statusId;
                transaction.UpdatedAt = DateTime.Now;

                if (callback.vnp_ResponseCode == "00") // Thành công
                {
                    transaction.PayDate = DateOnly.FromDateTime(DateTime.ParseExact(callback.vnp_PayDate, "yyyyMMddHHmmss", null));
                    transaction.Description = $"VNPay - {callback.vnp_OrderInfo} - Mã giao dịch: {callback.vnp_TransactionNo}";
                }
                else
                {
                    transaction.Description = $"VNPay - Thanh toán thất bại - Mã lỗi: {callback.vnp_ResponseCode}";
                }

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Cập nhật trạng thái transaction thành công: {TransactionId}", callback.vnp_TxnRef);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý callback VNPay cho transaction: {TransactionId}", callback.vnp_TxnRef);
                throw;
            }
        }

        private async Task CreatePendingTransactionAsync(string orderId, decimal amount)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<randox_dbContext>();

            var transaction = new Transaction
            {
                Id = orderId, // Sử dụng OrderId làm TransactionId
                Amount = amount,
                Description = "VNPay - Đang chờ thanh toán",
                TransactionStatusId = "9c13c916-38a5-11f0-b8f3-06cb1c8b9843", // ID của status pending trong bảng TransactionStatus
                //PaymentTypeId = "vnpay", // ID của payment type VNPay
                PaymentLocation = 1, // Online payment
                CreatedAt = DateTime.Now
            };

            dbContext.Transactions.Add(transaction);
            await dbContext.SaveChangesAsync();
        }

        private string GetTransactionStatusId(string responseCode)
        {
            return responseCode switch
            {
                "00" => "9c13ccf6-38a5-11f0-b8f3-06cb1c8b9843", // ID của status success
                _ => "9c13cba0-38a5-11f0-b8f3-06cb1c8b9843" // ID của status fail
            };
        }
    }

    // 5. VNPay Library Helper (Tải từ VNPay documentation)
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new();
        private readonly SortedList<string, string> _responseData = new();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            string queryString = data.ToString();
            if (queryString.Length > 0)
            {
                queryString = queryString.Remove(data.Length - 1, 1);
            }

            string signData = queryString;
            string vnpSecureHash = HmacSha512(vnp_HashSecret, signData);
            string paymentUrl = baseUrl + "?" + queryString + "&vnp_SecureHash=" + vnpSecureHash;

            return paymentUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseData()
        {
            var data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }

            foreach (var kv in _responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }

            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        private static string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (byte theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }
}
