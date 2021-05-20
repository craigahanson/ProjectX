using System.Net.Http;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProjectX.Authentication.Context;
using ProjectX.Testing;

namespace ProjectX.Authentication.Tests
{
    public class AuthenticationTestBase : TestBase
    {
        public HttpClient HttpClient;

        public ProjectXPersistedGrantDbContext PersistedGrantDbContextForArrange;
        public ProjectXConfigurationDbContext ConfigurationDbContextForArrange;

        [SetUp]
        public new void Setup()
        {
            DeleteEverything();

            PersistedGrantDbContextForArrange = new ProjectXPersistedGrantDbContext(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout);
            ConfigurationDbContextForArrange = new ProjectXConfigurationDbContext(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout);
            
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(builder);
            HttpClient = server.CreateClient();
        }

        public new void DeleteEverything()
        {
            using (var dbContext = new ProjectXPersistedGrantDbContext(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout))
            {
                dbContext.Database.ExecuteSqlRaw("delete from PersistedGrants");
                dbContext.Database.ExecuteSqlRaw("delete from DeviceCodes");
            }
            
            using (var dbContext = new ProjectXConfigurationDbContext(AppSettings.Database.ConnectionString, AppSettings.Database.CommandTimeout))
            {
                dbContext.Database.ExecuteSqlRaw("delete from Clients");
                dbContext.Database.ExecuteSqlRaw("delete from ApiScopes");
                dbContext.Database.ExecuteSqlRaw("delete from IdentityResources");
            }
            
            base.DeleteEverything();
        }
    }
}