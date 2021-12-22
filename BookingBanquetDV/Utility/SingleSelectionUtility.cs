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
            SingleSelection selectedValue,
            string header)
        {
            SingleSelectorDialog dialog =
                new SingleSelectorDialog(organisationList, header);

            App.ShowLoadingIndicator(ApplicationConstants.Loading);
            await DIContainer.GetInstance<IContentDialogService>().ShowDialogAsync(dialog);
            selectedValue = dialog.SelectedCode ?? selectedValue;
            App.HideLoadingIndicator();


            return selectedValue;
        }
    }
}
