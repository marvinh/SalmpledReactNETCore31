using System;
namespace Salmpled.Models
{
    public class SampleSamplePlaylist {
    public Guid SampleId { get; set; }
    public  Sample Sample { get; set; }
    public Guid SamplePlaylistId { get; set; }
    public  SamplePlaylist SamplePlaylist { get; set; }

    }
    
}