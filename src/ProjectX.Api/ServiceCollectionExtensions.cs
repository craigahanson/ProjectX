using Microsoft.Extensions.DependencyInjection;

namespace ProjectX.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApi(this IServiceCollection services)
        {
            services.AddTransient<IVersionService, VersionService>();
        }
    }
}