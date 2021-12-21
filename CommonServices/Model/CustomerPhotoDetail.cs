using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace CommonServices.Model
{
    public class CustomerPhotoDetail : NotificationModel
    {
        private BitmapImage _imageSource;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("photoName")]
        public string PhotoName { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("size")]
        public ulong Size { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("BookingId")]
        public int BookingId { get; set; }

        [JsonIgnore]
        public string PhotoGuid { get; set; }

        [JsonIgnore]
        public BitmapImage ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        [JsonIgnore]
        public StorageFile StorageFile { get; set; }
    }
}
