using BookingBanquetDV.Validation;
using CommonServices.ConstantData;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Source.Welcome
{
    public class NewBookingViewModel : ValidationViewModelBase, INewBookingViewModel
    {
        private string _name;
        private string _bookedBy;
        private string _numberOfRooms;
        private string _numberOfFlours;
        private string _amount;
        private string _advanced;
        private string _balance;

        private List<string> _confirmList;
        private DateTimeOffset _maxDate;
        private DateTimeOffset _minDateTime;
        private DateTimeOffset _bookingDate;
        private LoginDetail _loginDetail;
        private string _selecteConfirmation;
        private readonly ICommand _dialogLoadingCommand;

        public NewBookingViewModel()
        {
            _dialogLoadingCommand = new DelegateCommand(parameter => OnDialogLoading());
        }

        private void OnDialogLoading()
        {
            if (_loginDetail != null)
            {
                _bookedBy = _loginDetail.UserName;
                _maxDate = DateTimeOffset.Now.Date;
                _minDateTime = DateTimeOffset.Now.Date;
                _bookingDate = DateTimeOffset.Now.Date;

                GetConfirmationList();

                OnPropertyChanged(() => BookedBy);
                OnPropertyChanged(() => MaxDate);
                OnPropertyChanged(() => MinDateTime);
                OnPropertyChanged(() => BookingDate);
            }
        }

        private void GetConfirmationList()
        {
            _confirmList = new List<string>
            {
                "Yes",
                "No",
                "Not Decided"
            };
        }

        public string CustomerName
        {
            get => _name;
            set => _name = value;
        }

        [DateRequiredAttribute(ErrorMessage = ApplicationConstants.DateRequired)]
        public DateTimeOffset BookingDate
        {
            get { return _bookingDate; }
            set { SetValue(() => BookingDate, ref _bookingDate, value); }
        }

        public DateTimeOffset MaxDate
        {
            get { return _maxDate; }
            set { SetValue(() => MaxDate, ref _maxDate, value); }
        }

        public DateTimeOffset MinDateTime
        {
            get { return _minDateTime; }
            set { SetValue(() => MinDateTime, ref _minDateTime, value); }
        }

        public string BookedBy
        {
            get => _bookedBy;
            set => _bookedBy = value;
        }

        public string NumberOfRooms
        {
            get => _numberOfRooms;
            set => _numberOfRooms = value;
        }

        public string NumberOfFlours
        {
            get => _numberOfFlours;
            set => _numberOfFlours = value;
        }

        public string TotalAmount
        {
            get => _amount;
            set => _amount = value;
        }

        public string Advanced
        {
            get => _advanced;
            set => _advanced = value;
        }

        public string Balance
        {
            get => _balance;
            set => _balance = value;
        }

        public List<string> ConfirmList => _confirmList;

        public LoginDetail LoginDetail
        {
            get => _loginDetail;
            set => _loginDetail = value;
        }

        public string SelecteConfirmation
        {
            get => _selecteConfirmation;
            set => _selecteConfirmation = value;
        }

        public ICommand DialogLoadingCommand => _dialogLoadingCommand;
    }
}
