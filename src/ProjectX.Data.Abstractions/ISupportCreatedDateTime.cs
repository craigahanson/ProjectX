using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Data
{
    public interface ISupportCreatedDateTime
    {
        DateTimeOffset CreatedDateTime { get; set; }
    }
}
