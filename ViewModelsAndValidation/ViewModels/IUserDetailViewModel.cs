using CommonServices.Model;
using CommonServices.Model.City;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IUserDetailViewModel
    {
        UsersPersonalDetail UsersPersonalDetail { get; set; }
        ICommand SaveUserDetailCommand { get; }
        ICommand ChangedCountryCommand { get; }
        ICommand ChangedStateCommand { get; }
        ICommand ChangedCityCommand { get; }
        ICommand RetakePhotoCommand { get; }
        ICommand PreviousPhotoCommand { get; }
        ICommand RemovePhotoCommand { get; }
        bool IsPreviousButtonVisible { get; set; }
        bool IsRemoveButtonVisible { get; set; }
        string FullName { get; set; }
        string Address { get; set; }
        string PhotoName { get; set; }
        BitmapImage ImageSource { get; set; }
        SingleSelection SelectedCountry { get; set; }
        SingleSelection SelectedState { get; set; }
        SingleSelection SelectedCity { get; set; }
        bool IsStateVisible { get; set; }
        bool IsCityVisible { get; set; }
    }
}
