using System.Threading.Tasks;
using Salmpled.Models.ServiceResponse;
using Salmpled.Models.DTOS;
using System;
using System.Collections.Generic;
namespace Salmpled.Services
{
    public interface ISamplePlaylistService
    {
        Task<ServiceResponse<GetSamplePlaylistDTO>> AddSampleToPlaylist(AddSampleSamplePlaylistDTO newItem);
        Task<string> DownloadPlaylistById(Guid id);
        Task<ServiceResponse<GetSamplePlaylistDTO>> DeleteSampleFromPlaylist(DeleteSampleSamplePlaylistDTO item);
        Task<ServiceResponse<GetSamplePlaylistDTO>> AddSamplePlaylist(AddSamplePlaylistDTO newPlaylist);
        Task<ServiceResponse<GetSamplePlaylistDTO>> GetSamplePlaylistByIdAuthorized(Guid id);

        Task<ServiceResponse<GetSamplePlaylistDTO>> GetSamplePlaylistById(Guid id);

        Task<ServiceResponse<GetSamplePlaylistDTO>> DeleteById(Guid id);
        Task<ServiceResponse<List<GetSamplePlaylistDTO>>> All();

        Task<ServiceResponse<List<GetSamplePlaylistDTO>>> PlaylistsByOwner();

    }

}
