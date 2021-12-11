using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Salmpled.Models;
using FirebaseAdmin;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Salmpled.Services;
using Salmpled.Models.DTOS;

namespace Salmpled.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class SamplePlaylistController : ControllerBase
    {

        private readonly ISamplePlaylistService _samplePlaylistService;
        public SamplePlaylistController(ISamplePlaylistService samplePlaylistService)
        {
            _samplePlaylistService = samplePlaylistService;
        }


        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            return Ok(await _samplePlaylistService.All());
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> AddSamplePlaylist(AddSamplePlaylistDTO newSamplePlaylist)
        {
            return Ok(await _samplePlaylistService.AddSamplePlaylist(newSamplePlaylist));
        }

        [HttpPost("addsample")]
        [Authorize]
        public async Task<IActionResult> AddSampleToPlaylist(AddSampleSamplePlaylistDTO newItem)
        {
            return Ok(await _samplePlaylistService.AddSampleToPlaylist(newItem));
        }

        [HttpGet("edit/{samplePlaylistId}")]
        [Authorize]
        public async Task<IActionResult> GetSamplePlaylistByIdAuthorized(Guid samplePlaylistId)
        {
            return Ok(await _samplePlaylistService.GetSamplePlaylistByIdAuthorized(samplePlaylistId));
        }

        [HttpGet("view/{samplePlaylistId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSamplePlaylistById(Guid samplePlaylistId)
        {
            return Ok(await _samplePlaylistService.GetSamplePlaylistById(samplePlaylistId));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            return Ok(await _samplePlaylistService.DeleteById(id));
        }

        [HttpPost("deletesample")]
        [Authorize]
        public async Task<IActionResult> DeleteSampleFromPlaylist(DeleteSampleSamplePlaylistDTO item)
        {
            return Ok(await _samplePlaylistService.DeleteSampleFromPlaylist(item));
        }
        [HttpGet("download/{id}")]
        [Authorize]
        public async Task<IActionResult> DownloadPlaylistById(Guid id)
        {
            const string contentType = "application/zip";
            HttpContext.Response.ContentType = contentType;

            string zipFilePath = await _samplePlaylistService.DownloadPlaylistById(id);
            
            var result = new FileContentResult(System.IO.File.ReadAllBytes(zipFilePath), contentType)
            {
                FileDownloadName = zipFilePath
            };

            return result;
        }

        [HttpGet("yourplaylists")]
        [Authorize]
        public async Task<IActionResult> PlaylistsByOwner()
        {
            return Ok(await _samplePlaylistService.PlaylistsByOwner());
        }

    }
}