using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using ProjectX.Data.EntityFrameworkCore;
using ProjectX.Data.EntityFrameworkCore.Scope;
using ProjectX.Library;

namespace ProjectX.Testing
{
    public class TestBase
    {
        public AppSettings AppSettings;

        public TestBase()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(TestContext.CurrentContext.TestDirectory, @"appsettings.json"), optional: false, reloadOnChange: true);
            AppSettings = builder.Build().CreateAppSettings();
        }

        public ProjectXDbContext DbContextForArrange { get; set; }
        public ProjectXDbContext DbContextForAssert { get; set; }
        
        public DbContextScopeFactory CreateDbContextScopeFactory()
        {
            return new DbContextScopeFactory(new DbContextFactory(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout));
        }

        [SetUp]
        public void Setup()
        {
            DeleteEverything();

            DbContextForArrange = new ProjectXDbContext(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout);
            DbContextForAssert = new ProjectXDbContext(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout);
        }

        public void DeleteEverything()
        {
            using (var dbContext = new ProjectXDbContext(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout))
            {
                dbContext.Database.ExecuteSqlRaw("delete from Versions");
            }
        }
    }
}