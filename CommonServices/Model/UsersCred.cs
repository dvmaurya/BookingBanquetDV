using Newtonsoft.Json;

namespace CommonServices.Model
{
    public class UsersCred
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("mobilePassword")]
        public string MobilePassword { get; set; }

        [JsonProperty("emailPassword")]
        public string EmailPassword { get; set; }

        [JsonProperty("creationDate")]
        public string CreationDate { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("passwordChanged")]
        public int PasswordChanged { get; set; }

        [JsonProperty("phoneCode")]
        public string PhoneCode { get; set; }
    }
}
