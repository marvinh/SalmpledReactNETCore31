using System;
using System.Collections.Generic;

namespace  Salmpled.Models.DTOS
{
    public class GetUserDTO {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Headline { get; set; }
        public string Bio { get; set; }
        public string SignedUserImage {get; set;}
        public  List<GetSamplePackDTO> SamplePacks {get; set;}
        public  List<GetSamplePlaylistDTO> SamplePlaylists{get; set;}
        

        //Dont Need This Better to update part by part
        //TODO
        //SamplePack DTO Contains Samples
        //public  List<SampleDTO> Samples {get; set;}
        
        // public  List<SamplePlaylistDTO> SamplePlaylists{get; set;}
        // public  List<SamplePackLike> SamplePackLikes{get; set;}
        // public  List<SamplePlaylistLike> SamplePlaylistLikes{get; set;}
        // public  List<Follow> Follower { get; set; }
        // public  List<Follow> Followee { get; set; }

    }
}