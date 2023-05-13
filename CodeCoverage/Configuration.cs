using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CodeCoverage
{
    public static class Configuration
    {
        public static IConfiguration GetFakeIconfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                  {"MarkEmail", "jaidadelahuya@gmail.com"},
                  {"smtp_ssl", "true"},
                  {"smtp_password", "Nw$$12345!"},
                  {"smtp_port", "587"},
                  {"smtp_host", "smtp.office365.com"},
                  {"HrEmail","frankyotana@yahoo.com" },
                  {"smtp_user", "website@nibss-plc.com.ng"}
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration)
            .Build();

            return configuration;
        }

        public static IConfiguration GetFakeIconfigurationNoPort()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                  {"MarkEmail", "jaidadelahuya@gmail.com"},
                  {"smtp_ssl", "true"},
                  {"smtp_password", "BDwW4GY8eJ8VzkUeSCS/yaPp8KSPY2Ry8J/DZtZn8DSl"},
                 
                  {"smtp_host", "email-smtp.eu-west-2.amazonaws.com"},
                  {"HrEmail","jaidadelahuya@gmail.com" },
                  {"smtp_user", "AKIA3N3ZEAA6XKDM7MHZ"}
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration)
            .Build();

            return configuration;
        }

        public static IConfiguration GetFakeIconfigurationNoSSL()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                  {"MarkEmail", "jaidadelahuya@gmail.com"},
                  {"smtp_ssl", "false"},
                  {"smtp_password", "BDwW4GY8eJ8VzkUeSCS/yaPp8KSPY2Ry8J/DZtZn8DSl"},
                  {"smtp_port", "587"},
                  {"smtp_host", "email-smtp.eu-west-2.amazonaws.com"},
                  {"HrEmail","jaidadelahuya@gmail.com" },
                  {"smtp_user", "AKIA3N3ZEAA6XKDM7MHZ"}
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration)
            .Build();

            return configuration;
        }

        public static string getBasePath(string relativePath)
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                      .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return Path.Combine(appRoot, relativePath);
        }
    }
}
