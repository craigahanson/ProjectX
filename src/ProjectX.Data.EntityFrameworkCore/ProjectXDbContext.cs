using System;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntityVersion>().Property(p => p.Major).IsRequired();
            modelBuilder.Entity<EntityVersion>().Property(p => p.Minor).IsRequired();
            modelBuilder.Entity<EntityVersion>().Property(p => p.Build).IsRequired();
            modelBuilder.Entity<EntityVersion>().Property(p => p.Revision).IsRequired();

            var dateTimeNow = DateTimeOffset.UtcNow;
            modelBuilder.Entity<EntityVersion>().HasData(new []
            {
                new EntityVersion
                {
                    Id = 1,
                    Major = 1,
                    Minor = 0,
                    Build = 0,
                    Revision = 0,
                    CreatedDateTime = dateTimeNow,
                    UpdatedDateTime = dateTimeNow
                }
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}