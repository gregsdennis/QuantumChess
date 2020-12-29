using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuantumChess.App.Framework
{
	public class AsyncCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private bool _isExecuting;
		private readonly Func<Task> _execute;
		private readonly Func<bool> _canExecute;
		private readonly Action<Exception> _errorHandler;

		public AsyncCommand(Func<Task> execute,
		                    Func<bool> canExecute = null,
		                    Action<Exception> errorHandler = null)
		{
			_execute = execute;
			_canExecute = canExecute;
			_errorHandler = errorHandler;
		}

		private bool CanExecute()
		{
			return !_isExecuting && (_canExecute?.Invoke() ?? true);
		}

		private async Task ExecuteAsync()
		{
			if (CanExecute())
			{
				try
				{
					_isExecuting = true;
					await _execute();
				}
				finally
				{
					_isExecuting = false;
				}
			}

			RaiseCanExecuteChanged();
		}

		private void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		#region Explicit implementations
		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute();
		}

		async void ICommand.Execute(object parameter)
		{
			try
			{
				await ExecuteAsync();
			}
			catch (Exception e)
			{
				_errorHandler?.Invoke(e);
			}
		}
		#endregion
	}

	public class AsyncCommand<T> : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private bool _isExecuting;
		private readonly Func<T, Task> _execute;
		private readonly Func<T, bool> _canExecute;
		private readonly Action<Exception> _errorHandler;

		public AsyncCommand(Func<T, Task> execute,
		                    Func<T, bool> canExecute = null,
		                    Action<Exception> errorHandler = null)
		{
			_execute = execute;
			_canExecute = canExecute;
			_errorHandler = errorHandler;
		}

		private bool CanExecute(T parameter)
		{
			return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
		}

		private async Task ExecuteAsync(T parameter)
		{
			if (CanExecute(parameter))
			{
				try
				{
					_isExecuting = true;
					await _execute(parameter);
				}
				finally
				{
					_isExecuting = false;
				}
			}

			RaiseCanExecuteChanged();
		}

		private void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		#region Explicit implementations

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T) parameter);
		}

		async void ICommand.Execute(object parameter)
		{
			try
			{
				await ExecuteAsync((T) parameter);
			}
			catch (Exception e)
			{
				_errorHandler?.Invoke(e);
			}
		}

		#endregion
	}
}