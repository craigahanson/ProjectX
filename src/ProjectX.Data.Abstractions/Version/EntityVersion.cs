using System;

namespace ProjectX.Data.Version
{
    public class EntityVersion : ISupportCreatedDateTime, ISupportUpdatedDateTime
    {
        public long Id { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public DateTimeOffset UpdatedDateTime { get; set; }
    }
}