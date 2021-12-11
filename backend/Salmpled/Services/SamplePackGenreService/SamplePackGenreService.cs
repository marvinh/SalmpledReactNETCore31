
using System;
using System.Threading.Tasks;
using Salmpled.Models.DTOS;
using Salmpled.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Salmpled.Models.ServiceResponse;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Salmpled.Services
{
    public class SamplePackGenreService : ISamplePackGenreService
    {
        private readonly SalmpledContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public SamplePackGenreService(SalmpledContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<ServiceResponse<GetSamplePackDTO>> AddSamplePackGenre(AddSamplePackSamplePackGenreDTO newGenre)
        {
            ServiceResponse<GetSamplePackDTO> response = new ServiceResponse<GetSamplePackDTO>();

            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                SamplePackGenre genre = await _context.SamplePackGenre
                .Where(g => g.GenreName == newGenre.SamplePackGenreName)
                .FirstOrDefaultAsync();
                Guid genreId;
                if (genre == null)
                {
                    SamplePackGenre newG = new SamplePackGenre
                    {
                        GenreName = newGenre.SamplePackGenreName
                    };
                    await _context.AddAsync(newG);
                    await _context.SaveChangesAsync();
                    genreId = newG.Id;
                }
                else
                {
                    genreId = genre.Id;
                }

                SamplePack samplePack = await _context.SamplePack
                .Include(c => c.SamplePackSamplePackGenres).ThenInclude(cs => cs.SamplePackGenre)
                .FirstOrDefaultAsync(c => c.Id == genreId &&
                c.User.Id == uid);

                if (samplePack == null)
                {
                    response.Success = false;
                    response.Message = "SamplePack not found.";
                    return response;
                }
                SamplePackGenre samplePackGenre = await _context.SamplePackGenre
                    .FirstOrDefaultAsync(s => s.Id == genreId);
                if (samplePackGenre == null)
                {
                    response.Success = false;
                    response.Message = "Genre not found.";
                    return response;
                }
                SamplePackSamplePackGenre spspg = new SamplePackSamplePackGenre
                {
                    SamplePack = samplePack,
                    SamplePackGenre = samplePackGenre
                };

                await _context.SamplePackSamplePackGenres.AddAsync(spspg);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<GetSamplePackDTO>(samplePack);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            
        }

    }
}

