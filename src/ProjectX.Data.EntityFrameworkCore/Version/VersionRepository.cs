using System;
using System.Collections.Generic;
using System.Text;
using ProjectX.Data.Scope;
using ProjectX.Data.Version;

namespace ProjectX.Data.EntityFrameworkCore.Version
{
    public class VersionRepository : RepositoryBase<EntityVersion>, IRepository<EntityVersion>
    {
        public VersionRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }
    }
}
