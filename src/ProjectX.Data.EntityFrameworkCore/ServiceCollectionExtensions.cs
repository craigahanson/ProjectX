using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Data.EntityFrameworkCore.Scope;
using ProjectX.Data.Scope;

namespace ProjectX.Data.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEfCore(this IServiceCollection services)
        {
            services.AddTransient<IAmbientDbContextLocator, AmbientDbContextLocator>();
            services.AddTransient<IDbContextFactory>(provider => new DbContextFactory("Server=(local);Database=ProjectX;User ID=sa;Password=Welcome123;Trusted_Connection=False;", 5)); //need to get this from appsettings
            services.AddTransient<IDbContextScopeFactory, DbContextScopeFactory>();

            return services;
        }
    }
}
