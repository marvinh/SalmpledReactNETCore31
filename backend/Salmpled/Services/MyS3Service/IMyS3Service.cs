using System.Threading.Tasks;
using Salmpled.Models.ServiceResponse;
using Salmpled.Models.DTOS;
using Amazon.S3.Model;
namespace Salmpled.Services
{
    public interface IMyS3Service
    {

        Task<string> UploadToS3(string fileName, string objectKey);

        Task<DeleteObjectResponse> DeleteFromS3(string objectKey);

        Task<string> DownloadFromS3(string filePath, string objectKey) ;

    }

}
