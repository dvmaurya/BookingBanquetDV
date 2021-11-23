using System;

namespace CommonServices.Utilities
{
    public class DisposeDialogEventArgs : EventArgs
    {
        private readonly bool _isDialogDisposed;

        public DisposeDialogEventArgs(bool isDialogDisposed)
        {
            _isDialogDisposed = isDialogDisposed;
        }

        public bool IsDialogDisposed => _isDialogDisposed;
    }
}
