using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace EmailProducer
{
    public static class JsonConfig
    {
        private static IConfiguration _configuration { get; set; }

        public static string GetJsonValue(string key)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appConfig.json").Build();

            string value = builder[key];

            return value;
        }
    }
}
