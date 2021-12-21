using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace CommonServices.Model
{
    public class PhotoDetails : NotificationModel
    {
        private string _photoName;
        private BitmapImage _imageSource;
        private string _date;
        private bool _isChecked;

        public int Id { get; set; }
        public string FileType { get; set; }
        public ulong Size { get; set; }
        public string EmailId { get; set; }

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
            get { return _photoName; }
            set
            {
                _photoName = value;
                OnPropertyChanged("PhotoName");
            }
        }

        public bool IsPhotoRemoved { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
    }
}
