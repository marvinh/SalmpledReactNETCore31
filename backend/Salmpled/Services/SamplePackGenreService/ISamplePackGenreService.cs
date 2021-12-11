using System.Threading.Tasks;
using Salmpled.Models.ServiceResponse;
using Salmpled.Models.DTOS;

namespace Salmpled.Services
{
    public interface ISamplePackGenreService
    {
        Task<ServiceResponse<GetSamplePackDTO>> AddSamplePackGenre(AddSamplePackSamplePackGenreDTO newGenre);
    }

}
