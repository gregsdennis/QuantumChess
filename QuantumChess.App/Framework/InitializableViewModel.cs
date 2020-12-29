using System;
using System.Threading.Tasks;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// <see cref="ViewModelBase"/> implementation that provides indication that a viewmodel
	/// requires initialization.  The default state is equivalent to "initialization complete."
	/// </summary>
	public abstract class InitializableViewModel : ViewModelBase, IInitialize
	{
		private bool _isInitialized;
		private bool _isInitializing;
		private bool _hasError;
		private string _errorMessage;

		/// <summary>
		/// Indicates whether or not this instance is currently initialized.
		/// Virtualized in order to help with document oriented view models.
		/// </summary>
		public bool IsInitialized
		{
			get { return _isInitialized; }
			private set
			{
				_isInitialized = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets whether the view model is in the process of being initialized.
		/// </summary>
		public bool IsInitializing
		{
			get { return _isInitializing; }
			private set
			{
				if (value == _isInitializing) return;
				_isInitializing = value;
				NotifyOfPropertyChange();
			}
		}

		public bool HasError
		{
			get { return _hasError; }
			private set
			{
				if (value == _hasError) return;

				_hasError = value;
				NotifyOfPropertyChange();
			}
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				if (value == _errorMessage) return;
				_errorMessage = value;
				NotifyOfPropertyChange();
				HasError = value != null;
			}
		}

		protected InitializableViewModel()
		{
			_isInitialized = true;
		}

		/// <summary>
		/// Sets properties to indicate initialization has begun.
		/// </summary>
		public void BeginInitialization()
		{
			IsInitialized = false;
			IsInitializing = true;
			HasError = false;
			ErrorMessage = null;
		}

		/// <summary>
		/// Sets properties to indicate initialization is complete.
		/// </summary>
		public void CompleteInitialization()
		{
			IsInitialized = true;
			IsInitializing = false;
		}

		protected async Task Initialize(Func<Task> initialize)
		{
			BeginInitialization();

			try
			{
				await initialize();
			}
			catch (Exception e)
			{
				ErrorMessage = $"An error occurred while attempting to initialize {GetType().Name}";
				HasError = true;
				//this.Log().Error(() => ErrorMessage, e);
			}
			finally
			{
				CompleteInitialization();
			}
		}
	}
}