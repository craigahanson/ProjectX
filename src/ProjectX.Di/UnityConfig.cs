using System;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Api;
using ProjectX.Api.Abstractions;

namespace ProjectX.Di
{
    public static class UnityConfig
    {
        public static void Build(IServiceCollection services)
        {
            services.AddScoped<IVersionService, VersionService>();
        }
    }
}