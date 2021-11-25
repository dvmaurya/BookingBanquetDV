using BookingBanquetDV.Validation;
using CommonServices.ConstantData;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Source.Welcome
{
    public class NewBookingViewModel : ValidationViewModelBase, INewBookingViewModel
    {
        private string _customerName;
        private string _bookedBy;
        private string _totalRooms;
        private string _totalFlours;
        private string _amount;
        private string _advanced;
        private string _balance;

        private List<string> _confirmList;
        private DateTimeOffset _maxDate;
        private DateTimeOffset _minDateTime;
        private DateTimeOffset _bookingDate;
        private BookingParameter _bookingParameter;
        private string _selecteConfirmation;
        private bool _isValidationSuccess;
        private ICommand _saveCommand;
        private List<BookingDetails> _bookingDetailList;
        private DateTimeOffset _creationDate;
        private int _id;
        private string _photoName;
        private readonly ICommand _dialogLoadingCommand;

        public NewBookingViewModel()
        {
            _dialogLoadingCommand = new DelegateCommand(parameter => OnDialogLoading());
            _saveCommand = new DelegateCommand(parameter => OnSaveNewBooking());
        }

        private void OnDialogLoading()
        {
            if (_bookingParameter != null)
            {
                _bookedBy = _bookingParameter.LoginDetail?.UserName;
                _maxDate = DateTimeOffset.Now.Date;
                _minDateTime = DateTimeOffset.Now.Date;
                GetConfirmationList();

                if(!_bookingParameter.IsEditBooking && !_bookingParameter.IsViewBooking)
                    _bookingDate = DateTimeOffset.Now.Date;

                if(_bookingParameter.BookingDetails != null)
                    SetBookingDetail(_bookingParameter.BookingDetails);

                OnPropertyChanged(() => BookedBy);
                OnPropertyChanged(() => MaxDate);
                OnPropertyChanged(() => MinDateTime);
                OnPropertyChanged(() => BookingDate);
            }
        }

        private void SetBookingDetail(BookingDetails bookingDetails)
        {
            _id = BookingParameter.BookingDetails.Id;
            _customerName = bookingDetails.CustomerName;
            _bookingDate = bookingDetails.BookingDate.ConvertToDateTime();
            _creationDate = bookingDetails.CreationDate.ConvertToDateTime();
            _totalRooms = bookingDetails.NumberOfRooms;
            _totalFlours = bookingDetails.NumberOfFlours;
            _amount = bookingDetails.TotalAmount;
            _advanced = bookingDetails.Advanced;
            _balance = bookingDetails.Balance;
            _selecteConfirmation = bookingDetails.Confirmation;
            _photoName = bookingDetails.PhotoName;

            OnPropertyChanged(() => CustomerName);
            OnPropertyChanged(() => BookingDate);
            OnPropertyChanged(() => TotalRooms);
            OnPropertyChanged(() => TotalFlours);
            OnPropertyChanged(() => TotalAmount);
            OnPropertyChanged(() => Balance);
            OnPropertyChanged(() => SelecteConfirmation);
        }

        public void OnSaveNewBooking()
        {
            if (ValidateProperties())
                _isValidationSuccess = true;
            else
                _isValidationSuccess = false;
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

        [Required(ErrorMessage = ApplicationConstants.NameRequired)]
        public string CustomerName
        {
            get => _customerName;
            set => _customerName = value;
        }

        public int Id => _id;

        public string PhotoName => _photoName;

        [DateRequiredAttribute(ErrorMessage = ApplicationConstants.DateRequired)]
        public DateTimeOffset BookingDate
        {
            get { return _bookingDate; }
            set { SetValue(() => BookingDate, ref _bookingDate, value); }
        }

         public DateTimeOffset CreationDate => _creationDate;

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

        public string TotalRooms
        {
            get => _totalRooms;
            set => _totalRooms = value;
        }

        public string TotalFlours
        {
            get => _totalFlours;
            set => _totalFlours = value;
        }

        [Required(ErrorMessage = ApplicationConstants.TotalAmountError)]
        public string TotalAmount
        {
            get => _amount;
            set => _amount = value;
        }

        [Required(ErrorMessage = ApplicationConstants.AdvanceAmountError)]
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

        public BookingParameter BookingParameter
        {
            get => _bookingParameter;
            set => _bookingParameter = value;
        }

        public string SelecteConfirmation
        {
            get => _selecteConfirmation;
            set => _selecteConfirmation = value;
        }

        public ICommand DialogLoadingCommand => _dialogLoadingCommand;

        public ICommand SaveCommand => _saveCommand;

        public bool IsValidationSuccess { get { return _isValidationSuccess; } }
    }
}
