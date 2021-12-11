using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Salmpled.Models
{
    public class SamplePackGenre : TimestampBaseEntity
    {
        public Guid Id { get; set; }

        [MaxLength(64)]
        public string GenreName { get; set; }
        public  List<SamplePackSamplePackGenre> SamplePackSamplePackGenres { get; set; }
    }
}