using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace ProjectX.Authentication.Context
{
    public class ProjectXPersistedGrantDbContext : PersistedGrantDbContext
    {
        private readonly int? commandTimeout;
        private readonly string connectionString;

        public ProjectXPersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
            : base(options, storeOptions)
        {
            
        }
        
        public ProjectXPersistedGrantDbContext(string connectionString, int? commandTimeout) : base(new DbContextOptions<PersistedGrantDbContext>(), new OperationalStoreOptions())
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