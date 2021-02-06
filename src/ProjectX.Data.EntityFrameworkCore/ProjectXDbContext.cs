using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetAutomaticEntityData();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetAutomaticEntityData()
        {
            this.ChangeTracker.DetectChanges();

            var objectStateEntries = this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var objectStateEntry in objectStateEntries)
            {
                var iSupportCreated = objectStateEntry.Entity as ISupportCreatedDateTime;
                var iSupportUpdated = objectStateEntry.Entity as ISupportUpdatedDateTime;
                var dateTime = DateTimeOffset.UtcNow;

                if (iSupportCreated != null && objectStateEntry.State == EntityState.Added)
                {
                    iSupportCreated.CreatedDateTime = dateTime;
                }
                if (iSupportUpdated != null && (objectStateEntry.State == EntityState.Added || objectStateEntry.State == EntityState.Modified))
                {
                    iSupportUpdated.UpdatedDateTime = dateTime;
                }
            }
        }
    }
}