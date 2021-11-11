using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using ViewModelsAndValidation.Core;
using ViewModelsAndValidation.Validation;

namespace ViewModelsAndValidation
{
    public abstract class ValidationViewModelBase : ViewModelBase
    {
        private bool _isValidationEnabled;

        private readonly IValidationManager _validationManager;

        protected ValidationViewModelBase()
        {
            _validationManager = new ValidationManager(this);
            _isValidationEnabled = true;
        }

        protected ValidationViewModelBase(IValidationManager validationManager)
        {
            _validationManager = validationManager ?? throw new ArgumentNullException("validationManager");
            _isValidationEnabled = true;
        }

        public bool IsValidationEnabled
        {
            get => _isValidationEnabled;
            set => _isValidationEnabled = value;
        }

        public bool HasErrors => _validationManager.Errors.Count > 0;

        public IValidationManager Errors => _validationManager;

        public ReadOnlyDictionary<string, IReadOnlyCollection<string>> GetAllErrors => _validationManager.Errors;

        public bool ValidateProperties() => _validationManager.ValidateProperties();

        public bool ValidateProperty(string propertyName) => _validationManager.ValidateProperty(propertyName);

        public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors)
        {
            _validationManager.SetAllErrors(entityErrors);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { _validationManager.ErrorsChanged += value; }

            remove { _validationManager.ErrorsChanged -= value; }
        }

        public override void SetValue<T>(Expression<Func<T>> expression, ref T field, T value)
        {
            base.SetValue<T>(expression, ref field, value);

            string propertyName = PropertySupport.GetPropertyName(expression);

            if (_isValidationEnabled &&
                !String.IsNullOrEmpty(propertyName))
                _validationManager.ValidateProperty(propertyName);
        }
    }
}
