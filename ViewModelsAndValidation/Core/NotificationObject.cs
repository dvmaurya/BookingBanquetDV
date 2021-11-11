using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;

namespace ViewModelsAndValidation.Core
{
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs after a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expression">An expression returning the property's name</param>
        /// <exception cref="ArgumentNullException">if the expression is null.</exception>
        protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            OnPropertyChanged(PropertySupport.GetPropertyName(expression));
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Verifies the name of the property exists.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void VerifyPropertyName(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Invalid property name: {0}", propertyName));
            }
        }
    }
}
