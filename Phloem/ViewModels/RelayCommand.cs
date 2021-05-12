using System;
using System.Windows.Input;

namespace Phloem.ViewModels
{
    /// <summary>
    /// This class implements the <see cref="ICommand"/> Interface to pass events
    /// between views and viewmodel.
    /// </summary>
    class RelayCommand : ICommand
    {
        /// <summary>
        /// This <see cref="Action{T}"/> stores a method that is called when a
        /// binded input is control.
        /// </summary>
        private readonly Action<object> ExecuteAction;

        /// <summary>
        /// This <see cref="Func{T, TResult}"/> stores a method that returns a
        /// <see cref="Boolean"/> to enable or disable a binded control. Can be
        /// null.
        /// </summary>
        private readonly Func<object, bool>? CanExecuteFunc;

        /// <summary>
        /// This is the event that will trigger a binded control.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executeAction">The <see cref="Action{T}"/> that will
        /// trigger all binded controls.</param>
        /// <param name="canExecuteFunc">The <see cref="Func{T, TResult}"/> that
        /// will disable or enable all binded controles. Can be null.</param>
        public RelayCommand(Action<object> executeAction, Func<object, bool>? canExecuteFunc = null)
        {
            ExecuteAction = executeAction;
            CanExecuteFunc = canExecuteFunc;
        }

        /// <summary>
        /// Will call the stored <see cref="CanExecuteFunc"/> to enable or disable
        /// the binded control, return true if <see cref="CanExecuteFunc"/> is null.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunc != null)
            {
                return CanExecuteFunc(parameter);
            }
            return true;
        }

        /// <summary>
        /// Will call the stored <see cref="ExecuteAction"/> binded to the
        /// control.
        /// </summary>
        public void Execute(object parameter)
        {
            ExecuteAction(parameter);
        }
    }
}
