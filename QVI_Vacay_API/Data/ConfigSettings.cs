using Microsoft.Extensions.Configuration;
using System.IO;

namespace QVI_Vacay_API.Data
{
    public class ConfigSettings
    {
        public static string SSMSconnection { get; }
        public static string MySqlconnection { get; }
        public static string ClientSystemName { get; }
        public static string ClientScreenNameCU { get; }
        public static string ClientScreenNameCDT { get; }
        public static string ClientScreenNameLGN { get; }
        public static string ClientScreenNameUA { get; }
        public static string ClientScreenNameGR { get; }
        public static string ClientScreenNameCC { get; }
        public static string ClientScreenNameCR { get; }
        public static string ClientURLHash { get; }
        public static string ClientURLToken { get; }
        public static string ClientURLSignup { get; }
        public static string TokenExtendTime { get; }

        static ConfigSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            SSMSconnection = configurationBuilder.Build().GetSection("ConnectionStrings:SSMSconnection").Value;
            MySqlconnection = configurationBuilder.Build().GetSection("ConnectionStrings:MySqlconnection").Value;
            ClientSystemName = configurationBuilder.Build().GetSection("ConfigSettings:ClientSystemName").Value;
            ClientScreenNameCU = configurationBuilder.Build().GetSection("ConfigSettings:ClientScreenNameCU").Value;
            ClientScreenNameCDT = configurationBuilder.Build().GetSection("ConfigSettings:ClientScreenNameCDT").Value;
            ClientScreenNameLGN = configurationBuilder.Build().GetSection("ConfigSettings:ClientScreenNameLGN").Value;
            ClientScreenNameUA = configurationBuilder.Build().GetSection("ConfigSettings:ClientScreenNameUA").Value;
            ClientScreenNameGR = configurationBuilder.Build().GetSection("ConfigSettings:ClientScreenNameGR").Value;
            ClientScreenNameCC = configurationBuilder.Build().GetSection("ConfigSettings:ClientScreenNameCC").Value;
            ClientScreenNameCR = configurationBuilder.Build().GetSection("ConfigSettings:ClientScreenNameCR").Value;
            ClientURLHash = configurationBuilder.Build().GetSection("ConfigSettings:ClientURLHash").Value;
            ClientURLToken = configurationBuilder.Build().GetSection("ConfigSettings:ClientURLToken").Value;
            ClientURLSignup = configurationBuilder.Build().GetSection("ConfigSettings:ClientURLSignup").Value;
            TokenExtendTime = configurationBuilder.Build().GetSection("ConfigSettings:TokenExtendTime").Value;
        }
    }
}
