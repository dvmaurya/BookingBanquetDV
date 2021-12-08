using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;

namespace CommonServices.Model
{
    public class UsersPersonalDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("emialId")]
        public string EmailId { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("phoneCode")]
        public string PhoneCode { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonIgnore]
        public BitmapImage ImageSource { get; set; }
    }
}
