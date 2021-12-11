using AutoMapper;
using Salmpled.Models;
using Salmpled.Models.DTOS;
using System.Linq;
using Salmpled.Services;
namespace Salmpled
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Sample, GetSampleDTO>()
            .ForMember(sampmodel => sampmodel.FileName, sampto => sampto.MapFrom(sampto => sampto.RenamedFileName))
            .ForMember(sampmodal => sampmodal.Username, sampto => sampto.MapFrom(sampto => sampto.User.Username))
            .ForMember(sampmodel => sampmodel.SignedMP3URL, 
            sampto => sampto.MapFrom(sampto => 
            sampto.CompressedSampleKey != null ? MyS3Service.GeneratePreSignedURL(sampto.CompressedSampleKey) : ""))
            ;

            CreateMap<SamplePlaylist, GetSamplePlaylistDTO>()
            .ForMember(dto => dto.Samples,
            opt => opt.MapFrom(x => x.SampleSamplePlaylists.Select(y => y.Sample).ToList()))
            .ForMember(dto => dto.Username, opt => opt.MapFrom(x => x.User.Username));

            CreateMap<User, GetUserDTO>()
            .ForMember(usermodel => usermodel.SignedUserImage, 
            userto => userto.MapFrom(userto => userto.UserImageKey != null ? MyS3Service.GeneratePreSignedURL(userto.UserImageKey) : ""));

            CreateMap<SamplePack, GetSamplePackDTO>()
            .ForMember(dto => dto.Genres,
            s => s.MapFrom(s => s.SamplePackSamplePackGenres.Select(s => s.SamplePackGenre)))
            .ForMember(dto => dto.Username, u => u.MapFrom(u => u.User.Username));

            CreateMap<SamplePackGenre, GetSamplePackGenreDTO>(); 
        }        
    }
}