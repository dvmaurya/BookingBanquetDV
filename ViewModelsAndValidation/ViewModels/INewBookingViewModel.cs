using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface INewBookingViewModel
    {
        string CustomerName { get; set; }
        DateTimeOffset BookingDate { get; set; }
        string BookedBy { get; set; }
        string NumberOfRooms { get; set; }
        string NumberOfFlours { get; set; }
        string TotalAmount { get; set; }
        string Advanced { get; set; }
        string Balance { get; set; }
        List<string> ConfirmList { get; }
        DateTimeOffset MaxDate { get; set; }
        DateTimeOffset MinDateTime { get; set; }
        LoginDetail LoginDetail { get; set; }
        ICommand DialogLoadingCommand { get; }
        string SelecteConfirmation { get; set; }
    }
}
