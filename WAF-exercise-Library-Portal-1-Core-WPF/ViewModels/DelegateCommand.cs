using System;
using System.Windows.Input;

namespace WAF_exercise_Library_Portal_1_Core_WPF.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<Object> _execute;
        private readonly Func<Object, Boolean> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<Object> execute) : this(null, execute)
        {
        }

        public DelegateCommand(Func<Object, Boolean> canExecute, Action<Object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(Object parameter)
        {
            if (CanExecute(parameter) == false)
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }

            _execute(parameter);
        }
    }
}
