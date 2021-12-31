using BookingServices.Interfaces;
using CommonServices.Analytcis;
using CommonServices.Extensions;
using CommonServices.Model;
using System;
using System.Windows.Input;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Command;
using ViewModelsAndValidation.ViewModels;

namespace BookingBanquetDV.Source.Welcome.Controls
{
    public class BanquetConfigViewModel : ValidationViewModelBase, IBanquetConfigViewModel
    {
        private string _availableRooms;
        private string _availableFlours;
        private string _availableParkings;
        private bool _isYesSelected;

        private BanquetDetail _banquetDetail;
        private LoginDetail _loginDetail;
        private bool _isNoSelected;
        private readonly ICommand _saveDetailCommand;
        private readonly ICommand _dialogLoadingCommand;

        private readonly IBanquetDataProvider _banquetDataProvider;
        private readonly IAnalyticsManager _analyticsManager;

        public BanquetConfigViewModel(
            IBanquetDataProvider banquetDataProvider,
            IAnalyticsManager analyticsManager)
        {
            _banquetDataProvider = banquetDataProvider ?? throw new ArgumentNullException("banquetDataProvider");
            _analyticsManager = analyticsManager ?? throw new ArgumentNullException("analyticsManager");

            _saveDetailCommand = new DelegateCommand(
                parameter => OnSaveDetails());

            _dialogLoadingCommand = new DelegateCommand(
                parameter => OnDialogLoading());
        }

        private void OnDialogLoading()
        {
            _analyticsManager.TraceScreenView(
                $"{AnalyticConstants.Banquet} " +
                $"{AnalyticConstants.Config} " +
                $"{AnalyticConstants.Screen}");

            if (_banquetDetail != null)
            {
                _availableFlours = _banquetDetail.AvailableFlours;
                _availableParkings= _banquetDetail.AvailableParkings;
                _availableRooms = _banquetDetail.AvailableRooms;

                if(!string.IsNullOrEmpty(_banquetDetail.AvailableMandap))
                {
                    if (_banquetDetail.AvailableMandap.FormatStringEquals("true"))
                        _isYesSelected = true;
                    else
                        _isNoSelected = true;
                }
            }
            else
            {
                _isYesSelected = true;
            }

            OnPropertyChanged(() => AvailableFlours);
            OnPropertyChanged(() => IsYesSelected);
            OnPropertyChanged(() => IsNoSelected);
            OnPropertyChanged(() => AvailableParkings);
            OnPropertyChanged(() => AvailableRooms);
        }

        private void OnSaveDetails()
        {
            if(ValidateProperties())
            {
                if (_banquetDetail == null)
                    _banquetDetail = new BanquetDetail();

                _banquetDetail.AvailableFlours = _availableFlours;
                _banquetDetail.AvailableMandap = _isYesSelected ? "true" : "false";
                _banquetDetail.AvailableParkings = _availableParkings;
                _banquetDetail.AvailableRooms = _availableRooms;
                _banquetDetail.EmailId = _loginDetail?.EmailId;

                _banquetDataProvider.AddOrUpdateBanquet(_banquetDetail);
            }
        }

        public string AvailableRooms
        {
            get { return _availableRooms; }
            set { SetValue(() => AvailableRooms, ref _availableRooms, value); }
        }

        public string AvailableFlours
        {
            get { return _availableFlours; }
            set { SetValue(() => AvailableFlours, ref _availableFlours, value); }
        }

        public string AvailableParkings
        {
            get { return _availableParkings; }
            set { SetValue(() => AvailableParkings, ref _availableParkings, value); }
        }

        public bool IsYesSelected
        {
            get { return _isYesSelected; }
            set { SetValue(() => IsYesSelected, ref _isYesSelected, value); }
        }

        public bool IsNoSelected
        {
            get { return _isNoSelected; }
            set { SetValue(() => IsNoSelected, ref _isNoSelected, value); }
        }

        public LoginDetail LoginDetail
        {
            get => _loginDetail;
            set => _loginDetail = value;
        }

        public BanquetDetail BanquetDetail
        {
            get => _banquetDetail;
            set => _banquetDetail = value;
        }

        public ICommand SaveDetailCommand => _saveDetailCommand;

        public ICommand DialogLoadingCommand => _dialogLoadingCommand;
    }
}
