using Newtonsoft.Json;

namespace CommonServices.Model
{
    public class BookingDetails : NotificationModel
    {
        private bool _canShowPhoto;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("bookingDate")]
        public string BookingDate { get; set; }

        [JsonProperty("bookedBy")]
        public string BookedBy { get; set; }

        [JsonProperty("totalRooms")]
        public string TotalRooms { get; set; }

        [JsonProperty("totalFlours")]
        public string TotalFlours { get; set; }

        [JsonProperty("totalAmount")]
        public string TotalAmount { get; set; }

        [JsonProperty("advanced")]
        public string Advanced { get; set; }

        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("confirmation")]
        public string Confirmation { get; set; }

        [JsonProperty("creationDate")]
        public string CreationDate { get; set; }

        [JsonProperty("mobileNo")]
        public string MobileNo { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonIgnore]
        public bool CanShowPhoto
        {
            get => _canShowPhoto;
            set
            {
                _canShowPhoto = value;
                OnPropertyChanged("CanShowPhoto");
            }
        }

        [JsonIgnore]
        public bool IsEmailSent { get; set; }
    }
}
