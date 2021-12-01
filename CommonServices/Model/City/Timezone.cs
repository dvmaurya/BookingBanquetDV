using Newtonsoft.Json;

namespace CommonServices.Model.City
{
    public class Timezone
    {
        [JsonProperty("zoneName")]
        public string ZoneName { get; set; }

        [JsonProperty("gmtOffset")]
        public long GmtOffset { get; set; }

        [JsonProperty("gmtOffsetName")]
        public string GmtOffsetName { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("tzName")]
        public string TzName { get; set; }
    }
}
