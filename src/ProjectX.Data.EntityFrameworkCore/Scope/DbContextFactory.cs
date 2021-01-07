using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectX.Data.Scope;

namespace ProjectX.Data.EntityFrameworkCore.Scope
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly string connectionString;
        private readonly int? commandTimeout;

        public DbContextFactory(string connectionString, int? commandTimeout)
        {
            this.connectionString = connectionString;
            this.commandTimeout = commandTimeout;
        }

        public TDbContext CreateDbContext<TDbContext>() where TDbContext : class
        {
            return (TDbContext)(object)new ProjectXDbContext(connectionString, commandTimeout);
        }
    }
}
