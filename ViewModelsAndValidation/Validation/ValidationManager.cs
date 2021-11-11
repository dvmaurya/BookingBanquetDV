using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ViewModelsAndValidation.Validation
{
    internal class ValidationManager : IValidationManager
    {
        private ConcurrentDictionary<string, IReadOnlyCollection<string>> _errors = new ConcurrentDictionary<string, IReadOnlyCollection<string>>();

        private readonly IViewModel _viewModelToValidate;

        public ValidationManager(IViewModel viewModelToValidate)
        {
            _viewModelToValidate = viewModelToValidate ?? throw new ArgumentNullException("viewModelToValidate");
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyCollection<string> this[string propertyName]
        {
            get
            {
                return
                    _errors.ContainsKey(propertyName)
                    ?
                    _errors[propertyName]
                    :
                    new List<string>();
            }
        }

        public ReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors => new ReadOnlyDictionary<string, IReadOnlyCollection<string>>(_errors);

        public bool ValidateProperties()
        {
            var propertiesWithChangedErrors = new List<string>();

            var propertiesToValidate =
                _viewModelToValidate
                .GetType()
                .GetRuntimeProperties()
                .Where(property => property.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (PropertyInfo propertyInfo in propertiesToValidate)
            {
                var propertyErrors = new List<string>();
                TryValidateProperty(propertyInfo, propertyErrors);

                bool errorsChanged = SetPropertyErrors(propertyInfo.Name, propertyErrors);
                if (errorsChanged && !propertiesWithChangedErrors.Contains(propertyInfo.Name))
                    propertiesWithChangedErrors.Add(propertyInfo.Name);
            }

            foreach (string propertyName in propertiesWithChangedErrors)
                Notify(propertyName);

            return _errors.Values.Count == 0;
        }

        public bool ValidateProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName");


            var propertyInfo = _viewModelToValidate.GetType().GetRuntimeProperty(propertyName);

            if (propertyInfo == null)
                throw new ArgumentException("InvalidPropertyNameException", propertyName);

            var propertyErrors = new List<string>();
            bool isValid = TryValidateProperty(propertyInfo, propertyErrors);

            if (SetPropertyErrors(propertyInfo.Name, propertyErrors))
                Notify(propertyName);

            return isValid;
        }

        public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors)
        {
            if (entityErrors == null)
                throw new ArgumentNullException("entityErrors");

            _errors.Clear();

            foreach (var item in entityErrors)
                SetPropertyErrors(item.Key, item.Value);

            Notify(string.Empty);
        }

        private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_viewModelToValidate) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue(_viewModelToValidate);

            bool isValid = Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
                propertyErrors.AddRange(results.Select(c => c.ErrorMessage));

            return isValid;
        }

        private bool SetPropertyErrors(string propertyName, IReadOnlyCollection<string> propertyNewErrors)
        {
            bool errorsChanged = false;

            if (!_errors.ContainsKey(propertyName))
            {
                if (propertyNewErrors.Count > 0)
                    errorsChanged = _errors.TryAdd(propertyName, propertyNewErrors);
            }
            else
            {
                if (propertyNewErrors.Count != _errors[propertyName].Count ||
                    _errors[propertyName].Intersect(propertyNewErrors).Count() != propertyNewErrors.Count())
                {
                    if (propertyNewErrors.Count > 0)
                        _errors[propertyName] = propertyNewErrors;
                    else
                        _errors.TryRemove(propertyName, out IReadOnlyCollection<string> removedList);

                    errorsChanged = true;
                }
            }

            return errorsChanged;
        }

        private void Notify(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
    }
}
