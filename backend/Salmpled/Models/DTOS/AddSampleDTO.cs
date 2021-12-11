
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace Salmpled.Models.DTOS
{
    public class AddSampleDTO
    {
        public List<IFormFile> files {get; set;}
        public Guid SamplePackId {get; set;}
    }
}