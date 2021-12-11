using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Salmpled.Models
{
    public class User : TimestampBaseEntity
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [MaxLength(64)]
        public string Username { get; set; }
        public string AuthProvider { get; set; }
        [MaxLength(128)]
        public string Headline { get; set; }
        public string Bio { get; set; }
        public  List<Sample> Samples {get; set;}
        public  List<SamplePack> SamplePacks {get; set;}
        public  List<SamplePlaylist> SamplePlaylists{get; set;}
        public string UserImageRegion { get; set; }
        public string UserImageBucket { get; set; }
        public string UserImageKey { get; set; }

        //TODO FOR LATER 
        //public  List<SamplePackLike> SamplePackLikes{get; set;}
        //public  List<SamplePlaylistLike> SamplePlaylistLikes{get; set;}
        //public  List<Follow> Follower { get; set; }
        //public  List<Follow> Followee { get; set; }

    }

}

