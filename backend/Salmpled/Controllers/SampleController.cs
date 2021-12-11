using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Salmpled.Models;
using FirebaseAdmin;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using Salmpled.Services;
using Salmpled.Models.DTOS;
namespace Salmpled.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class SampleController : ControllerBase
    {
        private readonly ISampleService _sampleService;

        public SampleController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSample(Guid id)
        {
            return Ok(await _sampleService.DeleteSample(id));
        }
        [HttpPost("uploadsamples")]
        [Authorize]
        public async Task<IActionResult> UploadSample([FromForm] AddSampleDTO newSamples)
        {
            return Ok(await _sampleService.AddSamples(newSamples));
        }


    }
}