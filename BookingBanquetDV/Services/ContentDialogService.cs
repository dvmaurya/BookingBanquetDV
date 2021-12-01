using System;
using System.Threading.Tasks;
using ViewModelsAndValidation.Services;
using Windows.UI.Xaml.Controls;

namespace BookingBanquetDV.Services
{
    public class ContentDialogService : IContentDialogService
    {
        public async Task ShowDialogAsync(ContentDialog dialog)
        {
            if (dialog == null) return;
            
            await dialog.ShowAsync();
        }
    }
}
