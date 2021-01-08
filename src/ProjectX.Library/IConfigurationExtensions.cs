using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ProjectX.Library
{
    public static class IConfigurationExtensions
    {
        public static AppSettings CreateAppSettings(this IConfiguration configuration)
        {
            return new AppSettings
            {
                Database = new AppSettingsDatabase
                {
                    ConnectionString = configuration["Database:ConnectionString"],
                    CommandTimeout = int.TryParse(configuration["Database:CommandTimeout"], out var result) ? result : null
                }
            };
        }
    }
}
