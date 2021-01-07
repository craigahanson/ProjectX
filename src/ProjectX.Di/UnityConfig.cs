using Microsoft.Extensions.DependencyInjection;
using ProjectX.Api;
using ProjectX.Api.Abstractions;
using ProjectX.Data.EntityFrameworkCore;
using ProjectX.Data.EntityFrameworkCore.Version;
using ProjectX.Data.Version;

namespace ProjectX.Di
{
    public static class UnityConfig
    {
        public static void Build(IServiceCollection services)
        {
            services.AddScoped<IVersionService, VersionService>();

            services.AddScoped<IVersionRepository, VersionRepository>();

            services.RegisterEfCore();
        }
    }
}