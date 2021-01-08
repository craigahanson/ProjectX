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
        private AppSettings appSettings;

        public TestBase()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(TestContext.CurrentContext.TestDirectory, @"appsettings.json"), optional: false, reloadOnChange: true);
            appSettings = builder.Build().CreateAppSettings();
        }

        public ProjectXDbContext DbContextForArrange { get; set; }
        public ProjectXDbContext DbContextForAssert { get; set; }
        
        public DbContextScopeFactory CreateDbContextScopeFactory()
        {
            return new DbContextScopeFactory(new DbContextFactory(appSettings.Database.ConnectionString, appSettings.Database.CommandTimeout));
        }

        [SetUp]
        public void Setup()
        {
            DeleteEverything();

            DbContextForArrange = new ProjectXDbContext(appSettings.Database.ConnectionString, appSettings.Database.CommandTimeout);
            DbContextForAssert = new ProjectXDbContext(appSettings.Database.ConnectionString, appSettings.Database.CommandTimeout);
        }

        public void DeleteEverything()
        {
            using (var dbContext = new ProjectXDbContext(appSettings.Database.ConnectionString, appSettings.Database.CommandTimeout))
            {
                dbContext.Database.ExecuteSqlRaw("delete from Versions");
            }
        }
    }
}