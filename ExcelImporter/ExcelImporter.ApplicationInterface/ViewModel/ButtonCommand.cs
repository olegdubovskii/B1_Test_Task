using System;
using System.Windows.Input;

namespace ExcelImporter.ApplicationInterface.ViewModel
{
    /// <summary>
    /// Class used in ViewModel by buttons as commands. Implements 2 main methods from ICommand interface: CanExecute and Execute
    /// </summary>
    public class ButtonCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ButtonCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        /// <summary>
        /// Determines if the command can be executed
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns>true if this command can be executed; otherwise, false</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Execute command logic
        /// </summary>
        /// <param name="parameter">Data used by the command</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
