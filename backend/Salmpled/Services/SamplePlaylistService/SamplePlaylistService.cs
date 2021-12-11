
using System;
using System.Threading.Tasks;
using Salmpled.Models.DTOS;
using Salmpled.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Salmpled.Models.ServiceResponse;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
namespace Salmpled.Services
{
    class MyEncoder : UTF8Encoding
    {
        public MyEncoder() : base(true)
        {

        }
        public override byte[] GetBytes(string s)
        {
            s = s.Replace("\\", "/");
            return base.GetBytes(s);
        }
    }
    public class SamplePlaylistService : ISamplePlaylistService
    {
        private readonly SalmpledContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        private readonly IMyS3Service _s3Client;
        public SamplePlaylistService(SalmpledContext context, IMyS3Service client, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _s3Client = client;
        }

        public async Task<ServiceResponse<GetSamplePlaylistDTO>> DeleteSampleFromPlaylist(DeleteSampleSamplePlaylistDTO item)
        {
            ServiceResponse<GetSamplePlaylistDTO> response = new ServiceResponse<GetSamplePlaylistDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;
                SamplePlaylist playlist = await _context.SamplePlaylist
                .Include(c => c.User)
                .Include(c => c.SampleSamplePlaylists).ThenInclude(cs => cs.Sample)
                .FirstOrDefaultAsync(c => c.Id == item.SamplePlaylistId && c.User.Id == uid);

                if (playlist == null)
                {
                    response.Success = false;
                    response.Message = "Playlist not found.";
                    return response;
                }


                Sample sample = await _context.Sample
                    .FirstOrDefaultAsync(s => s.Id == item.SampleId);

                if (sample == null)
                {
                    response.Success = false;
                    response.Message = "Sample not found.";
                    return response;
                }

                SampleSamplePlaylist sspcheck = await _context.SampleSamplePlaylists.
                Where(s => s.Sample == sample && s.SamplePlaylist == playlist)
                 .FirstOrDefaultAsync();

                if (sspcheck == null)
                {
                    response.Success = false;
                    response.Message = "Sample Not in Playlist";
                    return response;
                }

                _context.SampleSamplePlaylists.Remove(sspcheck);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = _mapper.Map<GetSamplePlaylistDTO>(playlist);
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

        }
        public async Task<ServiceResponse<GetSamplePlaylistDTO>> AddSamplePlaylist(AddSamplePlaylistDTO newPlaylist)
        {
            ServiceResponse<GetSamplePlaylistDTO> response = new ServiceResponse<GetSamplePlaylistDTO>();
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
                SamplePlaylist playlist = new SamplePlaylist
                {
                    User = user,
                    UserId = uid,
                    SamplePlaylistName = newPlaylist.SamplePlaylistName,
                };
                _context.SamplePlaylist.Add(playlist);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<GetSamplePlaylistDTO>(playlist);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetSamplePlaylistDTO>>> All()
        {
            ServiceResponse<List<GetSamplePlaylistDTO>> response = new ServiceResponse<List<GetSamplePlaylistDTO>>();

            try
            {
                List<SamplePlaylist> samplePlaylists = await _context.SamplePlaylist
                .Include(sp => sp.User)
                .Where(sp => sp.SampleSamplePlaylists.Count() > 0).ToListAsync();
                response.Success = true;
                response.Data = _mapper.Map<List<GetSamplePlaylistDTO>>(samplePlaylists);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ServiceResponse<GetSamplePlaylistDTO>> AddSampleToPlaylist(AddSampleSamplePlaylistDTO newItem)
        {
            ServiceResponse<GetSamplePlaylistDTO> response = new ServiceResponse<GetSamplePlaylistDTO>();

            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                // SamplePlaylist playlist = await _context.SamplePlaylist
                // .Include(c => c.SampleSamplePlaylists).ThenInclude(cs => cs.Sample)
                // .FirstOrDefaultAsync(c => c.Id == newItem.SamplePlaylistId &&
                // c.User.Id == uid);
                Console.WriteLine("playlist id", newItem.SamplePlaylistId.ToString());
                Console.WriteLine("sample id", newItem.SampleId.ToString());

                SamplePlaylist playlist = await _context.SamplePlaylist
                .Include(c => c.User)
                .Include(c => c.SampleSamplePlaylists).ThenInclude(cs => cs.Sample)
                .FirstOrDefaultAsync(c => c.Id == newItem.SamplePlaylistId && c.User.Id == uid);


                if (playlist == null)
                {
                    response.Success = false;
                    response.Message = "Playlist not found.";
                    return response;
                }

                Sample sample = await _context.Sample
                    .FirstOrDefaultAsync(s => s.Id == newItem.SampleId);
                if (sample == null)
                {
                    response.Success = false;
                    response.Message = "Sample not found.";
                    return response;
                }
                SampleSamplePlaylist sspcheck = await _context.SampleSamplePlaylists.
                Where(s => s.Sample == sample && s.SamplePlaylist == playlist)
                .FirstOrDefaultAsync();
                if (sspcheck != null)
                {
                    response.Success = true;
                    response.Message = "Already Exists!";
                    response.Data = _mapper.Map<GetSamplePlaylistDTO>(playlist);
                    return response;
                }

                SampleSamplePlaylist ssp = new SampleSamplePlaylist
                {
                    SamplePlaylist = playlist,
                    Sample = sample
                };

                await _context.SampleSamplePlaylists.AddAsync(ssp);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Data = _mapper.Map<GetSamplePlaylistDTO>(playlist);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

        }

        public async Task<ServiceResponse<GetSamplePlaylistDTO>> GetSamplePlaylistByIdAuthorized(Guid id)
        {
            ServiceResponse<GetSamplePlaylistDTO> response = new ServiceResponse<GetSamplePlaylistDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                SamplePlaylist samplePlaylist = await _context.SamplePlaylist
                .Include(c => c.SampleSamplePlaylists)
                .ThenInclude(g => g.Sample)
                .ThenInclude(u => u.User)
                .Where(sp => (sp.Id == id && sp.UserId == uid))
                .FirstOrDefaultAsync();

                if (samplePlaylist == null)
                {
                    response.Success = false;
                    response.Message = "Sample Playlist Not Found";
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<GetSamplePlaylistDTO>(samplePlaylist);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<string> DownloadPlaylistById(Guid id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                SamplePlaylist samplePlaylist = await _context.SamplePlaylist
                .Include(c => c.User)
                .Include(c => c.SampleSamplePlaylists)
                .ThenInclude(g => g.Sample)
                .Where(sp => (sp.Id == id))
                .FirstOrDefaultAsync();

                if (samplePlaylist == null)
                {
                    return "err";
                }
                String dname = samplePlaylist.SamplePlaylistName.Replace(" ", String.Empty);
                string path = dname + Guid.NewGuid();
                DirectoryInfo di = Directory.CreateDirectory($@"{path}");
                foreach (var samp in samplePlaylist.SampleSamplePlaylists)
                {
                    String sname = samp.Sample.RenamedFileName.Replace(" ", String.Empty) + ".flac";
                    await _s3Client.DownloadFromS3(di.FullName + "/" + sname, samp.Sample.UncompressedSampleKey);
                }
                string startPath = di.FullName;
                string zipPath = di.FullName + ".zip";
                ZipFile.CreateFromDirectory(startPath,zipPath, CompressionLevel.Fastest, false, new MyEncoder());
                return zipPath;

            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public async Task<ServiceResponse<List<GetSamplePlaylistDTO>>> PlaylistsByOwner()
        {
            ServiceResponse<List<GetSamplePlaylistDTO>> response = new ServiceResponse<List<GetSamplePlaylistDTO>>();

            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;
                List<SamplePlaylist> samplePlaylists =
                await _context.SamplePlaylist
                .Include(sp => sp.User)
                .Where(sp => sp.UserId == uid).ToListAsync();

                if (samplePlaylists == null)
                {
                    response.Success = false;
                    response.Message = "Not Found";
                    return response;
                }
                response.Success = true;
                response.Data = _mapper.Map<List<GetSamplePlaylistDTO>>(samplePlaylists);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }


        public async Task<ServiceResponse<GetSamplePlaylistDTO>> DeleteById(Guid id)
        {
            ServiceResponse<GetSamplePlaylistDTO> response = new ServiceResponse<GetSamplePlaylistDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                SamplePlaylist samplePlaylist = await _context.SamplePlaylist
                .Include(sp => sp.User)
                .Include(c => c.SampleSamplePlaylists)
                .ThenInclude(g => g.Sample)
                .Where(sp => (sp.Id == id && sp.UserId == uid))
                .FirstOrDefaultAsync();

                if (samplePlaylist == null)
                {
                    response.Success = false;
                    response.Message = "Playlist not found";
                    return response;
                }

                _context.SamplePlaylist.Remove(samplePlaylist);

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = $"Successfully Deleted Playlist: {id}";

            }
            catch (Exception ex)
            {
                response.Success = true;
                response.Message = ex.Message;
            }

            return response;

        }
        public async Task<ServiceResponse<GetSamplePlaylistDTO>> GetSamplePlaylistById(Guid id)
        {
            ServiceResponse<GetSamplePlaylistDTO> response = new ServiceResponse<GetSamplePlaylistDTO>();
            try
            {

                SamplePlaylist samplePlaylist = await _context.SamplePlaylist
                .Include(c => c.User)
                .Include(c => c.SampleSamplePlaylists)
                .ThenInclude(g => g.Sample)
                .ThenInclude(u => u.User)
                .Where(sp => (sp.Id == id))
                .FirstOrDefaultAsync();

                if (samplePlaylist == null)
                {
                    response.Success = false;
                    response.Message = "Sample Playlist Not Found";
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<GetSamplePlaylistDTO>(samplePlaylist);

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

