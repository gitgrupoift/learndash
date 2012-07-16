namespace Core
{
    using System;
    using System.Configuration;

    public static class Settings
    {
        public static class Redis
        {
            public static bool UseLocalRedis
            {
                get { return bool.Parse(GetValue("UseLocalRedis")); }
            }

            public static string RedisLocalHost
            {
                get { return GetValue("RedisLocalHost"); }
            }

            public static string RedisExternalHost
            {
                get { return GetValue("RedisExternalHost"); }
            }

            public static string ExternalRedisPassword
            {
                get { return GetValue("ExternalRedisPassword"); }
            }

            private static string GetValue(string key)
            {
                return ConfigurationManager.AppSettings[key] ?? string.Empty;
            }
        }
    }
}