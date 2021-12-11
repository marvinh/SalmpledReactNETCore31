using System;
using System.Collections.Generic;
using Salmpled.Models.DTOS;

namespace Salmpled.Models.DTOS
{
    public class GetSamplePackDTO
    {
        public Guid Id { get; set; }
        public string SamplePackName { get; set; }
        public List<GetSamplePackGenreDTO> Genres { get; set; }
        public List<GetSampleDTO> Samples {get;set;}
        public string SignedImageURL {get;set;}
        public string UserId {get; set;}
        public string Username{get; set;}
        public string Description {get; set;}
        public bool Published {get; set;} 
        public DateTime PublishedDate {get;set;}
        public DateTime CreatedDate {get;set;}
        public DateTime UpdatedDate {get; set;}

    }
}