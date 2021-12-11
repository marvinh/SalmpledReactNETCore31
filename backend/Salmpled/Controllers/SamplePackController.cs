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

    public class SamplePackController : ControllerBase
    {
 
        private readonly ISamplePackService _samplePackService;
        public SamplePackController(ISamplePackService samplePackService)
        {
           _samplePackService = samplePackService;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> All() {
            return Ok(await _samplePackService.All());
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> AddSamplePack(AddSamplePackDTO newSamplePack)
        {   
            return Ok(await _samplePackService.AddSamplePack(newSamplePack));
        }

        [HttpGet("edit/{samplePackId}")]
        [Authorize]
        public async Task<ActionResult<GetSamplePackDTO>> GetSamplePackByIdAuthorized(Guid samplePackId)
        {
            return Ok(await _samplePackService.GetSamplePackByIdAuthorized(samplePackId));
        }

        [HttpGet("view/{samplePackId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSamplePlaylistById(Guid samplePackId)
        {
            return Ok(await _samplePackService.GetSamplePackById(samplePackId));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSample(Guid id)
        {
            return Ok(await _samplePackService.DeleteById(id));
        }


    }
}