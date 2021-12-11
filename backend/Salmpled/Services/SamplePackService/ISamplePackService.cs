using System.Threading.Tasks;
using Salmpled.Models.ServiceResponse;
using Salmpled.Models.DTOS;
using System;
using System.Collections.Generic;
namespace Salmpled.Services
{
    public interface ISamplePackService
    {
        Task<ServiceResponse<GetSamplePackDTO>> AddSamplePack(AddSamplePackDTO newSamplePack);
        Task<ServiceResponse<GetSamplePackDTO>> GetSamplePackByIdAuthorized(Guid id);
        Task<ServiceResponse<List<GetSamplePackDTO>>> All();

        Task<ServiceResponse<GetSamplePackDTO>> GetSamplePackById(Guid id);

        Task<ServiceResponse<GetSamplePackDTO>> DeleteById(Guid id);

    }

}
