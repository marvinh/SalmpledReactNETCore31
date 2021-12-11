using System;
using System.Collections.Generic;

namespace Salmpled.Models.DTOS
{
    public class GetSamplePlaylistDTO
    {
        public Guid Id { get; set; }
        public string SamplePlaylistName { get; set; }
        public string UserId {get; set;}
        public string Username {get;set;}
        public List<GetSampleDTO> Samples {get; set;}
        //TODO
        //public  List<SamplePlaylistSamplePlaylistGenre> SamplePlaylistSamplePlaylistGenres { get; set; }
        public string SignedImage {get;set;}

    }
}