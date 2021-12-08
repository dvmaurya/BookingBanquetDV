using Newtonsoft.Json;

namespace CommonServices.Model
{
    public class LoginDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("IsRememberChecked")]
        public bool IsRememberChecked { get; set; }

        [JsonProperty("NumberOfLogin")]
        public string NumberOfLogin { get; set; }

        [JsonProperty("LastLogin")]
        public string LastLogin { get; set; }
    }
}
