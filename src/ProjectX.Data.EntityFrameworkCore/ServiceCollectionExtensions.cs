using Microsoft.Extensions.DependencyInjection;
using ProjectX.Data.EntityFrameworkCore.Scope;
using ProjectX.Data.EntityFrameworkCore.Version;
using ProjectX.Data.Scope;
using ProjectX.Data.Version;
using ProjectX.Library;

namespace ProjectX.Data.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterEntityFrameworkCore(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<IAmbientDbContextLocator, AmbientDbContextLocator>();
            services.AddTransient<IDbContextFactory>(provider => new DbContextFactory(appSettings.Database.ConnectionString, appSettings.Database.CommandTimeout));
            services.AddTransient<IDbContextScopeFactory, DbContextScopeFactory>();

            services.AddTransient<IVersionRepository, VersionRepository>();
        }
    }
}