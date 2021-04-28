using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace ProjectX.Authentication.Context
{
    public class ProjectXConfigurationDbContext : ConfigurationDbContext
    {
        private readonly int? commandTimeout;
        private readonly string connectionString;

        public ProjectXConfigurationDbContext(string connectionString, int? commandTimeout) : base(new DbContextOptions<ConfigurationDbContext>(), new ConfigurationStoreOptions())
        {
            this.connectionString = connectionString;
            this.commandTimeout = commandTimeout;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, o => { o.CommandTimeout(commandTimeout); });

            base.OnConfiguring(optionsBuilder);
        }
    }
}