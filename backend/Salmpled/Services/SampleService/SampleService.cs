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
    public class SampleService : ISampleService
    {
        private readonly SalmpledContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMyS3Service _myS3Service;

        public SampleService(SalmpledContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMyS3Service myS3Service)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _myS3Service = myS3Service;
        }

        public async Task<ServiceResponse<List<GetSampleDTO>>> AddSamples(AddSampleDTO newSamples)
        {
            ServiceResponse<List<GetSampleDTO>> response = new ServiceResponse<List<GetSampleDTO>>();
            try
            {

                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                SamplePack samplePack = await _context.SamplePack.FirstOrDefaultAsync(sp =>
                sp.UserId == uid && sp.Id == newSamples.SamplePackId);
                User user = await _context.User.FindAsync(uid);
                if(user == null) {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }
                
                if (samplePack == null)
                {
                    response.Success = false;
                    response.Message = "Sample Pack Not Found Or You do not have rights to upload";
                    return response;
                }

                List<Sample> samples = new List<Sample>();

                foreach (var sampleFile in newSamples.files)
                {

                    var filePath = Path.GetTempFileName();
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await sampleFile.CopyToAsync(stream);
                    }
                    var extension = Path.GetExtension(sampleFile.FileName);
                    var flac = ConvertToFLAC(filePath, ".flac");
                    var mp3 = ConvertToMp3(filePath, ".mp3");

                    var flacKey = "audio/flac/" + Guid.NewGuid() + ".flac";
                    var mp3Key = "audio/mp3/" + Guid.NewGuid() + ".mp3";

                    await _myS3Service.UploadToS3(flac, flacKey);
                    await _myS3Service.UploadToS3(mp3, mp3Key);

                    var mp3URL = MyS3Service.GeneratePreSignedURL(mp3Key);

                    samples.Add(
                        new Sample
                        {
                            SamplePackId = samplePack.Id,
                            SamplePack = samplePack,
                            UserId = uid,
                            User = user,
                            OrginalFileName = Path.GetFileNameWithoutExtension(sampleFile.FileName),
                            RenamedFileName = Path.GetFileNameWithoutExtension(sampleFile.FileName),
                            Region = MyS3Service.bucketRegion.ToString(),
                            Bucket = MyS3Service.bucketName,
                            CompressedSampleKey = mp3Key,
                            UncompressedSampleKey = flacKey,
                        }
                    );

                }

                await _context.Sample.AddRangeAsync(samples);
                await _context.SaveChangesAsync();

                var dtoList = new List<GetSampleDTO>();

                foreach (var sample in samples)
                {
                    dtoList.Add(new GetSampleDTO()
                    {
                        Id = sample.Id,
                        FileName = sample.RenamedFileName,
                        SamplePackId = sample.SamplePackId,
                        UserId = sample.UserId,
                        SignedMP3URL = sample.CompressedSampleKey != "" ? MyS3Service.GeneratePreSignedURL(sample.CompressedSampleKey) : null,
                    });
                }
                response.Success = true;
                response.Data = dtoList;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetSampleDTO>> DeleteSample(Guid id) {
            ServiceResponse<GetSampleDTO> response = new ServiceResponse<GetSampleDTO>();
            try
            {
                String header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;

                Sample sample = await _context.Sample.FirstOrDefaultAsync(sp =>
                sp.UserId == uid && sp.Id == id);

                if(sample == null) {
                    response.Success = false;
                    response.Message = "Sample DNE or DN Belong to you";
                    return response;
                }

                await _myS3Service.DeleteFromS3(sample.CompressedSampleKey);
                await _myS3Service.DeleteFromS3(sample.UncompressedSampleKey);

                _context.Sample.Remove(sample);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = null;

                return response;

            }catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        static string ConvertToFLAC(string file, string convertedExtension)
        {
            string result = string.Empty;
            string input = string.Empty;
            string output = string.Empty;
            try
            {
                string ffmpegFilePath = "ffmpeg"; // path of ffmpeg.exe - please replace it for your options.
                FileInfo fi = new FileInfo(file);
                string filename = Path.GetFileNameWithoutExtension(fi.Name);
                string extension = Path.GetExtension(fi.Name);
                input = file;
                output = fi.DirectoryName + filename + convertedExtension;

                var processInfo = new ProcessStartInfo(ffmpegFilePath)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                processInfo.Arguments = $"-i {input} -c:a flac {output}";
                try
                {
                    Process process = System.Diagnostics.Process.Start(processInfo);
                    result = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    process.Close();
                }
                catch (Exception)
                {
                    result = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                string error = ex.Message;
            }
            Console.WriteLine(result);
            return output;
        }

        public static string ConvertToMp3(string file, string convertedExtension)
        {
            string result = string.Empty;
            string input = string.Empty;
            string output = string.Empty;
            try
            {
                string ffmpegFilePath = "ffmpeg"; // path of ffmpeg.exe - please replace it for your options.
                FileInfo fi = new FileInfo(file);
                string filename = Path.GetFileNameWithoutExtension(fi.Name);
                string extension = Path.GetExtension(fi.Name);
                input = file;
                output = fi.DirectoryName + filename + convertedExtension;

                var processInfo = new ProcessStartInfo(ffmpegFilePath)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                processInfo.Arguments = $"-i {input} {output}";
                try
                {
                    Process process = System.Diagnostics.Process.Start(processInfo);
                    result = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    process.Close();
                }
                catch (Exception)
                {
                    result = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                string error = ex.Message;
            }

            Console.WriteLine(result);
            return output;
        }
    }

}