using System;

namespace ProjectX.Library
{
    public class AppSettings
    {
        public AppSettingsDatabase Database { get; set; }
    }

    public class AppSettingsDatabase
    {
        public string ConnectionString { get; set; }
        public int? CommandTimeout { get; set; }
    }
}
