using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModelsAndValidation.Core;
using Windows.UI.Xaml.Navigation;

namespace ViewModelsAndValidation
{
    public abstract class ViewModelBase : NotificationObject, IViewModel
    {
        private ViewModelState _viewModelState = ViewModelState.Clean;

        /// <summary>
        /// Sets the value of a property on the view model. Raises an 
        /// OnPropertyChanged event if specified and changes the state of the view 
        /// model to Dirty if the value of the property has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expression">An expression returning the property's name</param>
        /// <param name="field">The backing field for the property.</param>
        /// <param name="value">The value that the property is to be set to.</param>
        /// <param name="raisePropertyChangeEvent">Indicates if an 'OnPropertyChangeEvent' should be raised regardless of whether the property has changed.</param>
        /// <exception cref="ArgumentNullException">if the expression is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#", Justification = "Need to pass by reference to allow update of backing field")]
        public void SetValue<T>(Expression<Func<T>> expression, ref T field, T value, bool raisePropertyChangeEvent)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (field == null || !field.Equals(value))
            {
                field = value;
                ViewModelState = ViewModelState.Dirty;
            }

            if (raisePropertyChangeEvent)
                OnPropertyChanged(PropertySupport.GetPropertyName(expression));
        }

        /// <summary>
        /// Sets the value of a property on the view model. Raises an 
        /// OnPropertyChanged event and changes the state of the view 
        /// model to Dirty if the value of the property has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expression">An expression returning the property's name</param>
        /// <param name="field">The backing field for the property.</param>
        /// <param name="value">The value that the property is to be set to.</param>
        /// <exception cref="ArgumentNullException">if the expression is null.</exception>
        public virtual void SetValue<T>(Expression<Func<T>> expression, ref T field, T value)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (field == null || !field.Equals(value))
            {
                field = value;
                ViewModelState = ViewModelState.Dirty;
                OnPropertyChanged(PropertySupport.GetPropertyName(expression));
            }
        }

        /// <summary>
        /// Gets or sets the state of the view model.
        /// </summary>
        /// <value>
        /// The state of the view model.
        /// </value>
        public ViewModelState ViewModelState
        {
            get { return _viewModelState; }
            set
            {
                if (_viewModelState != value)
                {
                    _viewModelState = value;
                    OnViewModelStateChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when the view model state value changes.
        /// </summary>
        public virtual void OnViewModelStateChanged()
        {
        }

        /// <summary>
        /// Called when navigation is performed to a page. You can use this method to load state if it is available.
        /// </summary>
        public virtual async Task OnNavigatedToAsync(object parameter, NavigationMode navigationMode)
        {
            await Task.FromResult<object>(null);
        }

        /// <summary>
        /// This method will be called when navigating away from a page. You can use this method to save your view model data in case of a suspension event.
        /// </summary>
        public virtual async Task OnNavigatingFromAsync(bool suspending)
        {
            await Task.FromResult<object>(null);
        }
    }
}
