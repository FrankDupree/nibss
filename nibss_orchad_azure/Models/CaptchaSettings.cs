using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nibss_orchad_azure.Models
{
    //This is the class that holds captcha information
    //This is the class that has two methods for captcha verification
    public class CaptchaSettings
    {
        public string ClientKey { get; set; } //This is the client key from appsettings
        public string ServerKey { get; set; } //This is the server key from appsettings
    }
}
