using System;
using System.Threading.Tasks;
using System.Linq;
using Salmpled.Models.DTOS;
using Salmpled.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Salmpled.Models.ServiceResponse;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Salmpled.Services
{
    public class UserService : IUserService
    {
        private readonly SalmpledContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMyS3Service _myS3Service;

        public UserService(SalmpledContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMyS3Service myS3Service)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _myS3Service = myS3Service;
        }

        public async Task<ServiceResponse<GetUserDTO>> GetUserAuthorized()
        {
            ServiceResponse<GetUserDTO> response = new ServiceResponse<GetUserDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                User user = await _context.User
                .Include(u => u.SamplePacks)
                .Include(u => u.SamplePlaylists)
                .Where(u => u.Id == uid)
                .Include(u => u.SamplePlaylists).FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                }

                response.Success = true;
                response.Data = _mapper.Map<GetUserDTO>(user);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetUserDTO>> GetUserByUsername(string username)
        {
            ServiceResponse<GetUserDTO> response = new ServiceResponse<GetUserDTO>();
            try
            {

                User user = await _context.User
                .Include(u => u.SamplePacks)
                .Include(u => u.SamplePlaylists)
                .Where(u => u.Username == username)
                .Include(u => u.SamplePlaylists).FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                }

                response.Success = true;
                response.Data = _mapper.Map<GetUserDTO>(user);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetUserDTO>> UpdateUserInfo(UpdateUserDTO newData)
        {
            ServiceResponse<GetUserDTO> response = new ServiceResponse<GetUserDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                User user = await _context.User.FindAsync(uid);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                }


                user.Headline = newData.Headline;
                user.Bio = newData.Bio;
                await _context.SaveChangesAsync();

                if (newData.UserImageFile == null)
                {
                    response.Success = true;
                    response.Data = _mapper.Map<GetUserDTO>(user);
                    return response;
                }
                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    await newData.UserImageFile.CopyToAsync(stream);
                }
                var key = "UserImages/" + Guid.NewGuid() + Path.GetExtension(newData.UserImageFile.FileName);

                await _myS3Service.UploadToS3(filePath, key);

                if (user.UserImageKey != null)
                {
                    await _myS3Service.DeleteFromS3(user.UserImageKey);
                }
                user.UserImageBucket = MyS3Service.bucketName;
                user.UserImageRegion = MyS3Service.bucketRegion.ToString();
                user.UserImageKey = key;

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = _mapper.Map<GetUserDTO>(user);
                return response;


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ServiceResponse<GetUserDTO>> SetUsername(SetUsernameDTO data)
        {
            ServiceResponse<GetUserDTO> response = new ServiceResponse<GetUserDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                User user = await _context.User.FindAsync(uid);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                user.Username = data.Username;

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = _mapper.Map<GetUserDTO>(user);
                return response;
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;

            }

            return response;
        }
    }
}
