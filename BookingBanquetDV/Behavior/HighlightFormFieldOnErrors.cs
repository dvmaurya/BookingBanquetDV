using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookingBanquetDV.Behavior
{
    public class HighlightFormFieldOnErrors : Behavior<FrameworkElement>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104: Do not declare read only mutable reference types", Justification = "DependencyProperty must be readonly")]
        public static readonly DependencyProperty PropertyErrorsProperty =
            DependencyProperty.RegisterAttached(
                "PropertyErrors",
                typeof(object),
                typeof(HighlightFormFieldOnErrors),
                new PropertyMetadata(
                    new List<string>(),
                    OnPropertyErrorsChanged));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104: Do not declare read only mutable reference types", Justification = "DependencyProperty must be readonly")]
        public static readonly DependencyProperty HighlightStyleNameProperty =
            DependencyProperty.RegisterAttached(
                "HighlightStyleName",
                typeof(string),
                typeof(HighlightFormFieldOnErrors),
                new PropertyMetadata("HighlightTextBoxStyle"));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104: Do not declare read only mutable reference types", Justification = "DependencyProperty must be readonly")]
        protected static readonly DependencyProperty OriginalStyleNameProperty =
            DependencyProperty.RegisterAttached(
                "OriginalStyleName",
                typeof(Style),
                typeof(HighlightFormFieldOnErrors),
                new PropertyMetadata("BaseTextBoxStyle"));


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104: Do not declare read only mutable reference types", Justification = "DependencyProperty must be readonly")]
        public static readonly DependencyProperty IsHighlightBorderControlProperty =
            DependencyProperty.Register(
                "IsHighlightBorderControl",
                typeof(bool),
                typeof(HighlightFormFieldOnErrors),
                new PropertyMetadata(false));

        public object PropertyErrors
        {
            get { return (object)GetValue(PropertyErrorsProperty); }
            set { SetValue(PropertyErrorsProperty, value); }
        }

        public string HighlightStyleName
        {
            get { return (string)GetValue(HighlightStyleNameProperty); }
            set { SetValue(HighlightStyleNameProperty, value); }
        }

        public string OriginalStyleName
        {
            get { return (string)GetValue(OriginalStyleNameProperty); }
            set { SetValue(OriginalStyleNameProperty, value); }
        }

        public bool IsHighlightBorderControl
        {
            get { return (bool)GetValue(IsHighlightBorderControlProperty); }
            set { SetValue(IsHighlightBorderControlProperty, value); }
        }

        protected override void OnAttached()
        {
        }

        protected override void OnDetached()
        {
        }

        private static void OnPropertyErrorsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (args == null || args.NewValue == null)
                return;

            FrameworkElement control = ((Behavior<FrameworkElement>)dependencyObject).AssociatedObject;
            ICollection<string> propertyErrors = (ICollection<string>)args.NewValue;
            HighlightFormFieldOnErrors type = dependencyObject as HighlightFormFieldOnErrors;

            Style style =
                propertyErrors.Any()
                ?
                (Style)Application.Current.Resources[type.HighlightStyleName]
                :
                (Style)Application.Current.Resources[type.OriginalStyleName];

            if (control != null)
                control.Style = style;

            if (type.IsHighlightBorderControl &&
                control.Parent is Border)
            {
                ((Border)control.Parent).Style =
                    propertyErrors.Any()
                    ?
                    (Style)Application.Current.Resources["HighlightBorderStyle"]
                    :
                    (Style)Application.Current.Resources["OriginalBorderStyle"];
            }
        }
    }
}
