using Newtonsoft.Json;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using System;

namespace nibss_orchad_azure.Models
{
    public class Job : ContentPart
    {
        public TextField Description { get; set; }
        public TextField Experience { get; set; }
        public TextField Location { get; set; }
        public TextField Role { get; set; }
        public DateField DeadLine { get; set; }
    }

    public class JobViewModel
    {
        [JsonProperty("Role")]
        public string Role { get; set; }
        [JsonProperty("MinQ")]
        public string MinQ { get; set; }
        [JsonProperty("Experience")]
        public int Experience { get; set; }
        [JsonProperty("Location")]
        public string Location { get; set; }
        [JsonProperty("DeadLine")]
        public DateTime DeadLine { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}
