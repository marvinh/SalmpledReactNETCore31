using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Salmpled.Models
{
    public class SamplePack : TimestampBaseEntity
    {
        public Guid Id { get; set; }
        [MaxLength(128)]
        public string SamplePackName { get; set; }
        public  List<Sample> Samples { get; set; }
        public  List<SamplePackSamplePackGenre> SamplePackSamplePackGenres { get; set; }
        public string SamplePackImageRegion { get; set; }
        public string SamplePackImageBucket { get; set; }
        public string SamplePackImageKey { get; set; }
        public string UserId {get; set;}
        public  User User {get; set;}
        public string Description {get; set;}

        [DefaultValue(false)]
        public bool Published {get; set;} 

        public DateTime PublishedDate {get; set;}

    }
}