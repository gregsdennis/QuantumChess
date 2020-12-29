using System;
using System.Windows.Input;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// To register commands in MVVM pattern
	/// </summary>
	public class RelayCommand : ICommand
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		/// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				PlatformProvider.Current.RequerySuggested += value;
			}
			remove
			{
				PlatformProvider.Current.RequerySuggested -= value;
			}
		}

		/// <summary>
		/// Constructer takes Execute and CanExcecute events to register in CommandManager.
		/// </summary>
		/// <param name="execute">Execute method as action.</param>
		/// <param name="canExecute">CanExecute method as return bool type.</param>
		public RelayCommand(Action execute, Func<bool> canExecute = null)
		{
			if (execute == null) throw new ArgumentNullException(nameof(execute));

			_execute = execute;
			_canExecute = canExecute;
		}

		/// <summary>Defines the method to be called when the command is invoked.</summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			_execute();
		}

		/// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute();
		}
	}

	/// <summary>
	/// To register commands in MVVM pattern
	/// </summary>
	/// <typeparam name="T">The type of input for the command.</typeparam>
	public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _execute;
		private readonly Predicate<T> _canExecute;

		/// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
		public event EventHandler CanExecuteChanged
		{
			add
			{
				PlatformProvider.Current.RequerySuggested += value;
			}
			remove
			{
				PlatformProvider.Current.RequerySuggested -= value;
			}
		}

		/// <summary>
		/// Constructor takes Execute and CanExecute events to register in CommandManager.
		/// </summary>
		/// <param name="execute">Execute method as action.</param>
		/// <param name="canExecute">CanExecute method as return bool type.</param>
		public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute;
		}

		/// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute((T) (parameter ?? default(T)));
		}

		/// <summary>Defines the method to be called when the command is invoked.</summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			_execute((T) parameter);
		}
	}

}
