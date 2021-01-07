using System;

namespace ProjectX.Data
{
    public interface ISupportCreatedDateTime
    {
        DateTimeOffset CreatedDateTime { get; set; }
    }
}