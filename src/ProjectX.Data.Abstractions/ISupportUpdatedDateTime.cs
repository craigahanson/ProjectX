using System;

namespace ProjectX.Data
{
    public interface ISupportUpdatedDateTime
    {
        DateTimeOffset UpdatedDateTime { get; set; }
    }
}