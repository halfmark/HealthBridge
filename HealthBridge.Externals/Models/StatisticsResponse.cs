using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthBridge.External.Service.Models
{
    public class StatisticsResponse
    {
        [JsonProperty(PropertyName = "continent")]
        public string Continent { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "cases")]
        public Case Cases { get; set; }
        [JsonProperty(PropertyName = "deaths")]
        public Death Deaths { get; set; }
    }

    public class RootObject
    {
        [JsonProperty(PropertyName = "response")]
        public List<StatisticsResponse> Response { get; set; }
    }
 
}
