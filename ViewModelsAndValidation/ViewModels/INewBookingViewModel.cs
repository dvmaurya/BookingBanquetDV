using CommonServices.Model;
using CommonServices.Model.City;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace ViewModelsAndValidation.ViewModels
{
    public interface INewBookingViewModel
    {
        string CustomerName { get; set; }
        DateTimeOffset BookingDate { get; set; }
        DateTimeOffset CreationDate { get; }
        string BookedBy { get; set; }
        string TotalRooms { get; set; }
        string TotalFlours { get; set; }
        string TotalAmount { get; set; }
        string Advanced { get; set; }
        string Balance { get; set; }
        List<string> ConfirmList { get; }
        DateTimeOffset MaxDate { get; set; }
        DateTimeOffset MinDateTime { get; set; }
        string SelectedConfirmation { get; set; }
        string MobileNo { get; set; }
        string EmailId { get; set; }
        bool IsEmailSent { get; }
        ICommand SaveCommand { get; }
        BookingDetails BookingDetails { get; set; }
        SingleSelection SelectedCountry { get; set; }
        SingleSelection SelectedState { get; set; }
        SingleSelection SelectedCity { get; set; }
        bool IsCityVisible { get; set; }
        bool IsStateVisible { get; set; }
        ICommand ChangedCountryCommand { get; }
        ICommand ChangedStateCommand { get; }
        ICommand ChangedCityCommand { get; }
        long MaxLength { get; }
        string Address { get; set; }
        ICommand TakePhotoCommand { get; }
        ICommand RemovePhotoCommand { get; }
        IReadOnlyCollection<BookingDetails> MasterBookingDetailList { get; set; }
        CustomerPhotoDetail SelectedPhotoDetails { get; set; }
    }
}
