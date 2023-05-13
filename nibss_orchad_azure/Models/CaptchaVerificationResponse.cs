using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nibss_orchad_azure.Models
{
    public class CaptchaVerificationResponse
    {
        [JsonProperty]
        internal bool Success { get; set; }
        [JsonProperty]
        internal DateTime Challenge_ts { get; set; }  // timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        [JsonProperty]
        internal string Hostname { get; set; }        // the hostname of the site where the reCAPTCHA was solved
        [JsonProperty]
        internal string[] Error_codes { get; set; }   // optional
    }
}
