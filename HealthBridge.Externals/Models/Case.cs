using Newtonsoft.Json;

namespace HealthBridge.External.Service.Models
{
    public class Case
    {

        [JsonProperty(PropertyName = "active")]
        public int? Active { get; set; }

        [JsonProperty(PropertyName = "new")]
        public string New { get; set; }
        [JsonProperty(PropertyName = "total")]
        public int? Total { get; set; }
    }
}