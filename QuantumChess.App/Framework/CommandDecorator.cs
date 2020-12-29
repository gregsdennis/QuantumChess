using System;
using System.Windows.Input;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Decorates an <see cref="ICommand"/> with optional actions to run before and after command execution.
	/// </summary>
	public class CommandDecorator : ICommand
	{
		private readonly ICommand _inner;

		/// <summary>
		/// Gets or sets the <see cref="Action"/> to run before command execution.
		/// </summary>
		public Action BeforeExecuteAction { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Action"/> to run after command execution.
		/// </summary>
		public Action AfterExecuteAction { get; set; }

		/// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
		public event EventHandler CanExecuteChanged
		{
			add { _inner.CanExecuteChanged += value; }
			remove { _inner.CanExecuteChanged -= value; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CommandDecorator(ICommand inner)
		{
			if (inner == null) throw new ArgumentNullException(nameof(inner));

			_inner = inner;
		}

		/// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			return _inner.CanExecute(parameter);
		}

		/// <summary>Defines the method to be called when the command is invoked.</summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			BeforeExecuteAction?.Invoke();
			_inner.Execute(parameter);
			AfterExecuteAction?.Invoke();
		}
	}

	/// <summary>
	/// Decorates an <see cref="ICommand"/> with optional actions to run before and after command execution.
	/// </summary>
	public class CommandDecorator<T> : ICommand
	{
		private readonly ICommand _inner;

		/// <summary>
		/// Gets or sets the <see cref="Action"/> to run before command execution.
		/// </summary>
		public Action<T> BeforeExecuteAction { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Action"/> to run after command execution.
		/// </summary>
		public Action<T> AfterExecuteAction { get; set; }

		/// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
		public event EventHandler CanExecuteChanged
		{
			add { _inner.CanExecuteChanged += value; }
			remove { _inner.CanExecuteChanged -= value; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CommandDecorator(ICommand inner)
		{
			if (inner == null) throw new ArgumentNullException(nameof(inner));

			_inner = inner;
		}

		/// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter)
		{
			return _inner.CanExecute(parameter);
		}

		/// <summary>Defines the method to be called when the command is invoked.</summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public void Execute(object parameter)
		{
			BeforeExecuteAction?.Invoke((T) parameter);
			_inner.Execute(parameter);
			AfterExecuteAction?.Invoke((T) parameter);
		}
	}
}
