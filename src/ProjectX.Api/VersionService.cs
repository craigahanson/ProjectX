using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ProjectX.Api.Abstractions;
using ProjectX.Data.Scope;
using ProjectX.Data.Version;

namespace ProjectX.Api
{
    public class VersionService : IVersionService
    {
        private readonly IDbContextScopeFactory dbContextScopeFactory;
        private readonly IVersionRepository versionRepository;

        public VersionService(IDbContextScopeFactory dbContextScopeFactory, IVersionRepository versionRepository)
        {
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.versionRepository = versionRepository;
        }

        public async Task<ApiVersion> Get()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                var entityVersions = await versionRepository.GetAllAsync();

                if (entityVersions.Any() == false) throw new AmbiguousMatchException("No version found");

                if (entityVersions.Count > 1) throw new AmbiguousMatchException("Multiple versions found");

                var entityVersion = entityVersions.Single();

                return new ApiVersion
                {
                    Major = entityVersion.Major,
                    Minor = entityVersion.Minor,
                    Build = entityVersion.Build,
                    Revision = entityVersion.Revision
                };
            }
        }
    }
}