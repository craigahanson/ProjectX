using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Authentication.Context
{
    public class ProjectXPersistedGrantDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            return new ProjectXPersistedGrantDbContext(args[0], int.TryParse(args[1], out int timeout) ? timeout : null);
        }
    }
}