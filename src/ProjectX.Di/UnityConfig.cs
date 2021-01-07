using Microsoft.Extensions.DependencyInjection;
using ProjectX.Api;
using ProjectX.Data.EntityFrameworkCore;

namespace ProjectX.Di
{
    public static class UnityConfig
    {
        public static void Build(IServiceCollection services)
        {
            services.RegisterApi();
            services.RegisterEntityFrameworkCore();
        }
    }
}