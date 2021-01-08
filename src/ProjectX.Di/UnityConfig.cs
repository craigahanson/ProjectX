using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Api;
using ProjectX.Data.EntityFrameworkCore;
using ProjectX.Library;

namespace ProjectX.Di
{
    public static class UnityConfig
    {
        public static void BuildServicesForRest(IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.CreateAppSettings();

            services.RegisterApi();
            services.RegisterEntityFrameworkCore(appSettings);
        }
    }
}