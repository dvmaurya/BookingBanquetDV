using CommonServices.ConstantData;
using CommonServices.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModelsAndValidation.Services;
using Windows.Foundation;
using Windows.UI.Popups;

namespace BookingBanquetDV.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        private IList<IAsyncOperation<IUICommand>> _commands;
        
        private const string ClosedMessageString = "Closed previously opened message dialog during auto log off - {0}";

        public MessageDialogService()
        {
            _commands = new List<IAsyncOperation<IUICommand>>();
        }

        private void DisposeMessageDialogs(object sender, EventArgs e)
        {
            foreach (var command in _commands)
                if (command != null)
                    command.Cancel();
        }

        public async Task<IUICommand> ShowMessageDialogAsync(string title, string content)
        {
            return await InvokeMessageDialog(new MessageDialog(content, title)).ConfigureAwait(false);
        }

        public async Task<IUICommand> ShowMessageDialogAsync(string title, string content, IReadOnlyCollection<IUICommand> commands)
        {
            MessageDialog messageDialog = new MessageDialog(content, title);

            foreach (IUICommand command in commands)
                messageDialog.Commands.Add(command);

            return await InvokeMessageDialog(messageDialog).ConfigureAwait(false);
        }

        private async Task<IUICommand> InvokeMessageDialog(MessageDialog messageDialog)
        {
            IUICommand invokedCommand = null;
            try
            {
                messageDialog.DefaultCommandIndex = ApplicationConstants.DefaultCommandIndex;
                messageDialog.CancelCommandIndex = ApplicationConstants.CancelCommandIndex;

                IAsyncOperation<IUICommand> showCommand = messageDialog.ShowAsync();

                _commands.Add(showCommand);

                invokedCommand = await showCommand;

                _commands.Remove(showCommand);

            }
            catch (TaskCanceledException canceledException)
            {
                App.Logger.Info(ClosedMessageString.FormatString(canceledException.Message));
            }

            return invokedCommand;
        }
    }
}
