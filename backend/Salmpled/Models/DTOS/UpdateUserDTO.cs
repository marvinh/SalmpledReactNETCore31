

using System;
using System.IO;
using Microsoft.AspNetCore.Http;
namespace Salmpled.Models.DTOS
{
    public class UpdateUserDTO {
        public IFormFile UserImageFile {get;set;}
        public string Headline {get;set;}
        public string Bio {get;set;}
    }
}