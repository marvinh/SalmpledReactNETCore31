
using System;
using System.Threading.Tasks;
using System.Linq;
using Salmpled.Models.DTOS;
using Salmpled.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Salmpled.Models.ServiceResponse;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Salmpled.Services
{
    public class SamplePackService : ISamplePackService
    {
        private readonly SalmpledContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        private readonly IMyS3Service _myS3Service;

        public SamplePackService(SalmpledContext context, IMyS3Service myS3Service, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _myS3Service = myS3Service;
        }

        public async Task<ServiceResponse<List<GetSamplePackDTO>>> All()
        {
            ServiceResponse<List<GetSamplePackDTO>> response = new ServiceResponse<List<GetSamplePackDTO>>();

            try
            {
                List<SamplePack> samplePacks = await 
                _context.SamplePack.Include(sp => sp.Samples)
                .Include(sp => sp.User)
                .Where(sp => sp.Samples.Count() > 0).ToListAsync();

                response.Success = true;
                response.Data = _mapper.Map<List<GetSamplePackDTO>>(samplePacks);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }
        public async Task<ServiceResponse<GetSamplePackDTO>> AddSamplePack(AddSamplePackDTO newSamplePack)
        {
            ServiceResponse<GetSamplePackDTO> response = new ServiceResponse<GetSamplePackDTO>();
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
                    response.Message = "User DNE or is not authorized";
                }
                SamplePack samplePack = new SamplePack
                {
                    UserId = user.Id,
                    User = user,
                    SamplePackName = newSamplePack.SamplePackName,
                };
                _context.SamplePack.Add(samplePack);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<GetSamplePackDTO>(samplePack);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetSamplePackDTO>> GetSamplePackByIdAuthorized(Guid id)
        {
            ServiceResponse<GetSamplePackDTO> response = new ServiceResponse<GetSamplePackDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                SamplePack samplePack = await _context.SamplePack
                .Include(sp => sp.Samples)
                .Include(c => c.SamplePackSamplePackGenres)
                .ThenInclude(g => g.SamplePackGenre)
                .Where(sp => (sp.Id == id && sp.UserId == uid))
                .FirstOrDefaultAsync();

                if (samplePack == null)
                {
                    response.Success = false;
                    response.Message = "Sample Pack Not Found";
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<GetSamplePackDTO>(samplePack);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetSamplePackDTO>> DeleteById(Guid id)
        {
            ServiceResponse<GetSamplePackDTO> response = new ServiceResponse<GetSamplePackDTO>();
            try{
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                SamplePack samplePack = await _context.SamplePack
                .Include(sp => sp.Samples)
                .Include(c => c.SamplePackSamplePackGenres)
                .ThenInclude(g => g.SamplePackGenre)
                .Where(sp => (sp.Id == id && sp.UserId == uid))
                .FirstOrDefaultAsync();
                if(samplePack == null){
                    response.Success = false;
                    response.Message = "Sample Pack Not Found";
                    return response;
                }

                foreach(Sample sample in samplePack.Samples){
                    await _myS3Service.DeleteFromS3(sample.CompressedSampleKey);
                    await _myS3Service.DeleteFromS3(sample.UncompressedSampleKey);

                    List<SampleSamplePlaylist> list = _context.SampleSamplePlaylists.Where(s => s.SampleId == sample.Id).ToList();
                    _context.SampleSamplePlaylists.RemoveRange(list);
                    await _context.SaveChangesAsync();
                }

                _context.SamplePack.Remove(samplePack);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = $"Delete Sample Pack: {id}";

                return response;

            }catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<GetSamplePackDTO>> GetSamplePackById(Guid id)
        {
            ServiceResponse<GetSamplePackDTO> response = new ServiceResponse<GetSamplePackDTO>();
            try
            {

                SamplePack samplePack = await _context.SamplePack
                .Include(u => u.User)
                .Include(sp => sp.Samples)
                .Include(c => c.SamplePackSamplePackGenres)
                .ThenInclude(g => g.SamplePackGenre)
                .Where(sp => (sp.Id == id))
                .FirstOrDefaultAsync();

                if (samplePack == null)
                {
                    response.Success = false;
                    response.Message = "Sample Pack Not Found";
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<GetSamplePackDTO>(samplePack);

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
