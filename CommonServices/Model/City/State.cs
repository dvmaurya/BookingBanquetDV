using Newtonsoft.Json;
using System.Collections.Generic;

namespace CommonServices.Model.City
{
    public class State
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state_code")]
        public string StateCode { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("type")]
        public object Type { get; set; }

        [JsonProperty("cities")]
        public List<City> Cities { get; set; }

        [JsonIgnore]
        public long CountryId { get; set; }
    }
}
