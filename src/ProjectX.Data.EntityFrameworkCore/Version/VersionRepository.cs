using ProjectX.Data.Scope;
using ProjectX.Data.Version;

namespace ProjectX.Data.EntityFrameworkCore.Version
{
    public class VersionRepository : RepositoryBase<EntityVersion>, IVersionRepository
    {
        public VersionRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }
    }
}