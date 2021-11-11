using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ViewModelsAndValidation.Validation
{
    public interface IValidationManager : INotifyPropertyChanged
    {
        IReadOnlyCollection<string> this[string propertyName] { get; }

        bool ValidateProperty(string propertyName);

        bool ValidateProperties();

        void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors);

        ReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors { get; }

        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
