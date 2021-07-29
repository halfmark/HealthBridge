using Newtonsoft.Json;

namespace HealthBridge.External.Service.Models
{
    public class Death
    {
        [JsonProperty(PropertyName = "new")]
        public string New { get; set; }
        [JsonProperty(PropertyName = "total")]
        public int? Total { get; set; }
    }
}