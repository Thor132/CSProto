// -----------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.WPF
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    /// <summary>
    /// Relay command class.
    /// http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// Action to execute.
        /// </summary>
        private readonly Action<object> executeAction;

        /// <summary>
        /// Predicate to determine whether action is able to execute.
        /// </summary>
        private readonly Predicate<object> canExecutePredicate;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Predicate to test whether the action can execute.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.executeAction = execute;
            this.canExecutePredicate = canExecute;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Event handler for when the predicate result changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Returns whether the action can be executed.
        /// </summary>
        /// <param name="parameter">Parameter to pass to the predicate.</param>
        /// <returns>Whether the action can execute.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return this.canExecutePredicate == null || this.canExecutePredicate(parameter);
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="parameter">Parameter to pass into the action.</param>
        public void Execute(object parameter)
        {
            this.executeAction(parameter);
        }

        #endregion
    }
}