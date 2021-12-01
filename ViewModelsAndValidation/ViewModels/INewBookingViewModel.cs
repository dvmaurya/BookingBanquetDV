using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface INewBookingViewModel
    {
        int Id { get; }
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
        ICommand DialogLoadingCommand { get; }
        string SelecteConfirmation { get; set; }
        string MobileNo { get; set; }
        string EmailId { get; set; }
        string PhotoName { get; }
        bool IsValidationSuccess { get; }
        bool IsEmailSent { get; }
        ICommand SaveCommand { get; }
        BookingParameter BookingParameter { get; set; }
    }
}
