
using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using System.Threading.Tasks;

namespace Salmpled.Services
{
    public class MyS3Service : IMyS3Service
    {
        private static IAmazonS3 _s3Client;
        public static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        public static readonly string bucketName = "salmpled-demo";
        public static readonly double duration = 12;

        public MyS3Service(IAmazonS3 s3Client) {
            _s3Client = s3Client;
        }
        public static string GeneratePreSignedURL(string objectKey)
        {
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    Expires = DateTime.UtcNow.AddHours(duration)
                };
                urlString = _s3Client.GetPreSignedURL(request1);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return urlString;
        }

         public async Task<string> UploadToS3(string fileName, string objectKey) {
              TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
              await fileTransferUtility.UploadAsync(fileName, bucketName, objectKey);
              return GeneratePreSignedURL(objectKey);
         }

         public async Task<DeleteObjectResponse> DeleteFromS3(string objectKey) {
             return await _s3Client.DeleteObjectAsync(bucketName, objectKey);
         }

         public async Task<string> DownloadFromS3(string filePath, string objectKey) {
            TransferUtility fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.DownloadAsync(filePath, bucketName ,objectKey);
            return filePath;
         }

    }

   
}