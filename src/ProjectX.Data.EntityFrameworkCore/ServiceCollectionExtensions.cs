using Microsoft.Extensions.DependencyInjection;
using ProjectX.Data.EntityFrameworkCore.Scope;
using ProjectX.Data.EntityFrameworkCore.Version;
using ProjectX.Data.Scope;
using ProjectX.Data.Version;

namespace ProjectX.Data.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterEntityFrameworkCore(this IServiceCollection services)
        {
            services.AddTransient<IAmbientDbContextLocator, AmbientDbContextLocator>();
            services.AddTransient<IDbContextFactory>(provider => new DbContextFactory("Server=(local);Database=ProjectX;User ID=sa;Password=Welcome123;Trusted_Connection=False;", 5)); //need to get this from appsettings
            services.AddTransient<IDbContextScopeFactory, DbContextScopeFactory>();

            services.AddTransient<IVersionRepository, VersionRepository>();
        }
    }
}