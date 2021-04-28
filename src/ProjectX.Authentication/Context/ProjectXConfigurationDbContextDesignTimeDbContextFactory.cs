using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Authentication.Context
{
    public class ProjectXConfigurationDbContextDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            return new ProjectXConfigurationDbContext(args[0], int.TryParse(args[1], out int timeout) ? timeout : null);
        }
    }
}