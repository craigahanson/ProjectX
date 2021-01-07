using System;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Api;
using ProjectX.Api.Abstractions;
using ProjectX.Data.EntityFrameworkCore;

namespace ProjectX.Di
{
    public static class UnityConfig
    {
        public static void Build(IServiceCollection services)
        {
            services.AddScoped<IVersionService, VersionService>();

            services.RegisterEfCore();
        }
    }
}