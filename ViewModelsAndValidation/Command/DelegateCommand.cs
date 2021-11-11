using System;
using System.Windows.Input;

namespace ViewModelsAndValidation.Command
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecutePredicate;

        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, null)
        {
        }

        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecutePredicate)
        {
            _executeAction = executeAction ?? throw new ArgumentNullException("executeAction");
            _canExecutePredicate = canExecutePredicate;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecutePredicate == null || _canExecutePredicate(parameter);
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}
