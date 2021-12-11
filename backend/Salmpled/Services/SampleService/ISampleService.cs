using System.Threading.Tasks;
using Salmpled.Models.ServiceResponse;
using Salmpled.Models.DTOS;
using System.Collections.Generic;
using System;
namespace Salmpled.Services
{
    public interface ISampleService
    {
        Task<ServiceResponse<List<GetSampleDTO>>> AddSamples(AddSampleDTO newSamples);

        Task<ServiceResponse<GetSampleDTO>> DeleteSample(Guid id);
    }

}
