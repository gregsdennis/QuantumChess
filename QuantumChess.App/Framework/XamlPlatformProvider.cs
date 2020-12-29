using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// A <see cref="IPlatformProvider"/> implementation for the XAML platfrom.
	/// </summary>
	internal class XamlPlatformProvider : IPlatformProvider
	{
		private readonly Dispatcher _dispatcher;
		private static bool? _inDesignMode;

		/// <summary>
		/// Indicates whether or not the framework is in design-time mode.
		/// </summary>
		public bool InDesignMode
		{
			get
			{
				if (_inDesignMode == null)
				{
					var descriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
					_inDesignMode = (bool)descriptor.Metadata.DefaultValue;
				}

				return _inDesignMode.GetValueOrDefault(false);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XamlPlatformProvider"/> class.
		/// </summary>
		public XamlPlatformProvider()
		{
			_dispatcher = Dispatcher.CurrentDispatcher;
		}

		private void ValidateDispatcher()
		{
			if (_dispatcher == null) throw new InvalidOperationException("Not initialized with dispatcher.");
		}

		/// <summary>
		/// Executes the action on the UI thread asynchronously.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		public Task BeginOnUiThread(Action action)
		{
			ValidateDispatcher();
			return _dispatcher.InvokeAsync(action).Task;
		}

		/// <summary>
		/// Executes the action on the UI thread.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <exception cref="TargetInvocationException"></exception>
		public void OnUiThread(Action action)
		{
			if (_CheckAccess())
				action();
			else
			{
				Exception exception = null;
				_dispatcher.Invoke(() =>
				{
					try
					{
						action();
					}
					catch (Exception ex)
					{
						exception = ex;
					}
				});
				if (exception != null)
					throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
			}
		}

		/// <summary>
		/// Occurs when the system detects conditions that might change the ability of a command to execute.
		/// </summary>
		public event EventHandler RequerySuggested
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		/// <summary>
		/// Forces the system to raise the <see cref="IPlatformProvider.RequerySuggested"/> event.
		/// </summary>
		public void InvalidateRequerySuggested()
		{
			OnUiThread(CommandManager.InvalidateRequerySuggested);
		}

		private bool _CheckAccess()
		{
			return _dispatcher == null || _dispatcher.CheckAccess();
		}
	}
}
