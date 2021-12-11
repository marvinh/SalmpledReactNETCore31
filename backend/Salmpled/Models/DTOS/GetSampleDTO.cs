using System;

namespace Salmpled.Models.DTOS
{
    public class GetSampleDTO
    {
        public Guid Id { get; set; }
        public string FileName {get;set;}
        public string UserId { get; set; }
        public string Username {get; set;}
        public string SignedMP3URL {get; set;}
        public string Description {get; set;}
        public Guid SamplePackId {get; set;}
        public GetSamplePackDTO SamplePack {get; set;}
    }
}