using System;
using System.Windows.Input;

namespace SoundVisualizer3D.Utilities
{
    sealed class DelegateCommand
        : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action _execute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    sealed class DelegateCommand<TParameter>
        : ICommand
            where TParameter : class
    {
        private readonly Predicate<TParameter> _canExecute;
        private readonly Action<TParameter> _execute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<TParameter> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<TParameter> execute, Predicate<TParameter> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter as TParameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter as TParameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}