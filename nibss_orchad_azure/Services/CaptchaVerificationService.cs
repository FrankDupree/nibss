using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using nibss_orchad_azure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace nibss_orchad_azure.Services
{

    //This is the verification class
    public class CaptchaVerificationService
    {
        private readonly CaptchaSettings captchaSettings;
        private readonly IConfiguration Configuration;

        public CaptchaVerificationService(IConfiguration configuration)
        {         
            Configuration = configuration;
            captchaSettings = new CaptchaSettings();
            captchaSettings.ClientKey = Configuration["Captcha:ClientKey"];
            captchaSettings.ServerKey = Configuration["Captcha:ServerKey"];
        }

        //This is the verification method that handles the calls
        public async Task<bool> IsCaptchaValid(string token)
        {
            var result = false;
            
            try
            {
                using var client = new HttpClient();

                var response = await client.PostAsync($"{Configuration["Captcha:VerificationUrl"]}?secret={captchaSettings.ServerKey}&response={token}", null);
                var jsonString = await response.Content.ReadAsStringAsync();
                var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

                result = captchaVerfication.Success;
            }
            catch (Exception e)
            {
                //Fail gracefully
                Console.WriteLine("Error" + e.StackTrace);
            }

            return result;
        }


    }
}
