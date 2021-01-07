using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectX.Data.Version;

namespace ProjectX.Data.EntityFrameworkCore
{
    public class ProjectXDbContext : DbContext
    {
        private readonly string connectionString;
        private readonly int? commandTimeout;

        public ProjectXDbContext(string connectionString, int?commandTimeout)
        {
            this.connectionString = connectionString;
            this.commandTimeout = commandTimeout;
        }

        public DbSet<EntityVersion> Versions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString, o =>
            {
                o.CommandTimeout(this.commandTimeout);
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
