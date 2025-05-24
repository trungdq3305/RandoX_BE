namespace RandoX.API.Helper
{
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Transfer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    public class S3Service
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;

        public S3Service(IConfiguration configuration)
        {
            _bucketName = configuration["AWS:BucketName"];

            _s3Client = new AmazonS3Client(
                configuration["AWS:AccessKey"],
                configuration["AWS:SecretKey"],
                RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
            );
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            using (var newMemoryStream = new MemoryStream())
            {
                await file.CopyToAsync(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = fileName,
                    BucketName = _bucketName,
                    ContentType = file.ContentType,
                    //CannedACL = S3CannedACL.PublicRead // hoặc Private nếu muốn bảo mật
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);

                // Trả về URL ảnh
                return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            }
        }
    }

}
