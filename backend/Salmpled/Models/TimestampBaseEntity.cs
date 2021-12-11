using System;

namespace Salmpled.Models
{

    public abstract class TimestampBaseEntity{ 
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }    
}
    
}
