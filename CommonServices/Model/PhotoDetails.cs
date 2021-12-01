using CommonServices.ConstantData;
using CommonServices.Extensions;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace CommonServices.Model
{
    public class PhotoDetails : NotificationModel
    {
        private string _photoName;
        private BitmapImage _imageSource;
        private string _date;

        public int PhotoId { get; set; }
        public string PhotoName
        {
            get { return _photoName; }
            set
            {
                _photoName = value;
                OnPropertyChanged("PhotoName");
            }
        }
        public string FileType { get; set; }
        public long Size { get; set; }
        public bool IsAvailable { get; set; }

        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public StorageFile StorageFile { get; set; }

        public string PhotoGuid { get; set; }

        public string LocalPhotoName
        {
            get { return PhotoGuid.ConcatString(ApplicationConstants.FileFormat); }
        }
    }
}
