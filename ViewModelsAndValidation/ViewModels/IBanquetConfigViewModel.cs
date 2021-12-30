using CommonServices.Model;
using System.Windows.Input;

namespace ViewModelsAndValidation.ViewModels
{
    public interface IBanquetConfigViewModel
    {
        string AvailableRooms { get; set; }
        string AvailableFlours { get; set; }
        string AvailableParkings { get; set; }
        bool IsYesSelected { get; set; }
        bool IsNoSelected { get; set; }
        BanquetDetail BanquetDetail { get; set; }
        LoginDetail LoginDetail { get; set; }
        ICommand SaveDetailCommand { get; }
        ICommand DialogLoadingCommand { get; }
    }
}
