using System.Threading.Tasks;
using Salmpled.Models.ServiceResponse;
using Salmpled.Models.DTOS;
using System.Collections.Generic;
using System;
namespace Salmpled.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDTO>> GetUserAuthorized();

        Task<ServiceResponse<GetUserDTO>> GetUserByUsername(string username);

        Task<ServiceResponse<GetUserDTO>> UpdateUserInfo(UpdateUserDTO newData);

        Task<ServiceResponse<GetUserDTO>> SetUsername(SetUsernameDTO username);
 
    }

}