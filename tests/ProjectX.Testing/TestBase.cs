using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProjectX.Data.EntityFrameworkCore;
using ProjectX.Data.EntityFrameworkCore.Scope;

namespace ProjectX.Testing
{
    public class TestBase
    {
        private static readonly string DbConnectionString = "Server=(local);Database=ProjectX;User ID=sa;Password=Welcome123;Trusted_Connection=False;";
        private static readonly int? DbCommandTimeout = 5;

        public ProjectXDbContext DbContextForArrange { get; set; }
        public ProjectXDbContext DbContextForAssert { get; set; }

        public DbContextScopeFactory CreateDbContextScopeFactory()
        {
            return new(new DbContextFactory(DbConnectionString, DbCommandTimeout));
        }

        [SetUp]
        public void Setup()
        {
            DeleteEverything();

            DbContextForArrange = new ProjectXDbContext(DbConnectionString, DbCommandTimeout);
            DbContextForAssert = new ProjectXDbContext(DbConnectionString, DbCommandTimeout);
        }

        public void DeleteEverything()
        {
            using (var dbContext = new ProjectXDbContext(DbConnectionString, DbCommandTimeout))
            {
                dbContext.Database.ExecuteSqlRaw("delete from Versions");
            }
        }
    }
}