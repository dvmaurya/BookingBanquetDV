using System;
using System.Text.RegularExpressions;
using CommonServices.Extensions;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace BookingBanquetDV.Behavior
{
    public sealed class NumericTextBoxBehavior
            : DependencyObject, IBehavior
    {
        private string _lastText;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty AllowDecimalProperty = DependencyProperty.Register(
            "AllowDecimal",
            typeof(bool),
            typeof(NumericTextBoxBehavior),
            new PropertyMetadata(false));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty NumberOfDecimalsProperty = DependencyProperty.Register(
            "NumberOfDecimals",
            typeof(int),
            typeof(NumericTextBoxBehavior),
            new PropertyMetadata(7));

        public bool AllowDecimal
        {
            get
            {
                return (bool)base.GetValue(AllowDecimalProperty);
            }

            set
            {
                base.SetValue(AllowDecimalProperty, value);
            }
        }

        public int NumberOfDecimals
        {
            get
            {
                return (int)base.GetValue(NumberOfDecimalsProperty);
            }
            set
            {
                base.SetValue(NumberOfDecimalsProperty, value);
            }
        }

        public void Attach(DependencyObject associatedObject)
        {
            if (!(associatedObject is TextBox tb))
                throw new ArgumentException("NumericTextBoxBehavior can only be used with a TextBox.");

            AssociatedObject = associatedObject;

            _lastText = tb.Text;
            tb.TextChanging += Tb_TextChanging;

            if (tb.InputScope == null)
            {
                InputScope inputScope = new InputScope();
                inputScope.Names.Add(new InputScopeName(InputScopeNameValue.Number));
                tb.InputScope = inputScope;
            }
        }

        private void Tb_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (AssociatedObject is TextBox tb)
            {
                if (AllowDecimal)
                {
                    string decimalPattern = @"^\d*(\.\d{0," + NumberOfDecimals.ConvertToString() + "})?$";
                    if (string.IsNullOrEmpty(tb.Text) ||
                         Regex.IsMatch(tb.Text, decimalPattern))
                    {
                        _lastText = tb.Text;
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(tb.Text) ||
                        Regex.IsMatch(tb.Text, @"^\d*$"))
                    {
                        _lastText = tb.Text;
                        return;
                    }
                }

                tb.Text = _lastText;
                tb.SelectionStart = tb.Text.Length;
            }
        }

        public void Detach()
        {
            if (AssociatedObject is TextBox tb)
                tb.TextChanging -= Tb_TextChanging;
        }

        public DependencyObject AssociatedObject { get; private set; }
    }
}
