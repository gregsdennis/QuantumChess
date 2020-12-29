using System;
using System.Windows;
using System.Windows.Threading;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Inherit from this class in order to customize the configuration of the framework.
	/// </summary>
	public abstract class BootstrapperBase
	{
		private readonly bool _useApplication;
		private bool _isInitialized;

		/// <summary>
		/// The application.
		/// </summary>
		protected Application Application { get; set; }

		/// <summary>
		/// Creates an instance of the bootstrapper.
		/// </summary>
		/// <param name="useApplication">Set this to false when hosting Caliburn.Micro inside and Office or WinForms application. The default is true.</param>
		protected BootstrapperBase(bool useApplication = true)
		{
			_useApplication = useApplication;
		}

		/// <summary>
		/// Initialize the framework.
		/// </summary>
		public void Initialize()
		{
			if (_isInitialized) return;

			_isInitialized = true;

			PlatformProvider.Current = new XamlPlatformProvider();

			if (Execute.InDesignMode)
			{
				try
				{
					StartDesignTime();
				}
				catch
				{
					//if something fails at design-time, there's really nothing we can do...
					_isInitialized = false;
					throw;
				}
			}
			else
			{
				StartRuntime();
			}
		}

		/// <summary>
		/// Implement to configure the framework and setup your IoC container.
		/// </summary>
		protected abstract void Configure();

		/// <summary>
		/// Implement to add custom behavior to execute after the application starts.
		/// Typically this is where the primary window will be shown.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The args.</param>
		protected abstract void OnStartup(object sender, StartupEventArgs e);

		/// <summary>
		/// Override this to add custom behavior on exit.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event args.</param>
		protected virtual void OnExit(object sender, EventArgs e) {}

		/// <summary>
		/// Override this to add custom behavior for unhandled exceptions.  The base implementation
		/// simply shows a <see cref="MessageBox"/> containing the exception message.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event args.</param>
		protected virtual void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var temp = new Window {Visibility = Visibility.Hidden};
			temp.Show();
			MessageBox.Show(temp, $"An error occurred: {e.Exception.Message}", "Unhandled Error");
		}

		/// <summary>
		/// Called by the bootstrapper's constructor at design time to start the framework.
		/// </summary>
		private void StartDesignTime()
		{
			AssemblySource.Instance.Clear();

			Configure();
		}

		/// <summary>
		/// Called by the bootstrapper's constructor at runtime to start the framework.
		/// </summary>
		private void StartRuntime()
		{
			AssemblySourceCache.Install();

			if (_useApplication)
			{
				Application = Application.Current;
				PrepareApplication();
			}

			Configure();
		}

		/// <summary>
		/// Provides an opportunity to hook into the application object.
		/// </summary>
		private void PrepareApplication()
		{
			Application.Startup += OnStartup;
			Application.DispatcherUnhandledException += OnUnhandledException;
			Application.Exit += OnExit;
		}
	}
}
