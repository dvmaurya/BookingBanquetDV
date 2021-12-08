using BookingBanquetDV.Source.CommonControls;
using CommonServices.ConstantData;
using CommonServices.Model.City;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModelsAndValidation;
using ViewModelsAndValidation.Services;

namespace BookingBanquetDV.Utility
{
    public static class SingleSelectionUtility
    {
        public static async Task<SingleSelection> OnSelectSingleSelectionAsync(
            IReadOnlyCollection<SingleSelection> organisationList,
            string header)
        {
            SingleSelectorDialog dialog =
                new SingleSelectorDialog(organisationList, header);

            App.ShowLoadingIndicator(ApplicationConstants.Loading);
            await DIContainer.GetInstance<IContentDialogService>().ShowDialogAsync(dialog);
            SingleSelection result = dialog.SelectedCode;
            App.HideLoadingIndicator();


            return result;
        }
    }
}
