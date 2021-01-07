using Microsoft.EntityFrameworkCore;
using ProjectX.Data.Version;

namespace ProjectX.Data.EntityFrameworkCore
{
    public class ProjectXDbContext : DbContext
    {
        private readonly int? commandTimeout;
        private readonly string connectionString;

        public ProjectXDbContext(string connectionString, int? commandTimeout)
        {
            this.connectionString = connectionString;
            this.commandTimeout = commandTimeout;
        }

        public DbSet<EntityVersion> Versions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, o => { o.CommandTimeout(commandTimeout); });

            base.OnConfiguring(optionsBuilder);
        }
    }
}