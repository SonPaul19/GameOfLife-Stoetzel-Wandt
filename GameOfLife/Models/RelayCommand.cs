using System;
using System.Windows.Input;

namespace GameOfLife.Models
{
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// hat eine Action zum Ausführen
        /// </summary>
        private readonly Action<object> _Ausfuehren;
        /// <summary>
        /// üguckt ob es ausführen kann
        /// </summary>
        private readonly Func<object, bool> _KannAusfuehren;

        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _Ausfuehren = execute ?? throw new ArgumentNullException(nameof(execute));
            _KannAusfuehren = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
           return _KannAusfuehren?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
          _Ausfuehren?.Invoke(parameter);
        }

        public void RaiseCanExecuteChange() 
        { 
        CommandManager.InvalidateRequerySuggested();
        } 
    
}
}
