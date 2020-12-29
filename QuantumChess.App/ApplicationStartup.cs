using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using QuantumChess.App.Framework;
using QuantumChess.App.Model.MessageBox;

namespace QuantumChess.App
{
	internal class ApplicationStartup : BootstrapperBase
	{
		private IContainer _container;

		protected override void Configure()
		{
			var builder = new ContainerBuilder();

			builder.RegisterModule<AppModule>();
			builder.RegisterModule<WpfFrameworkModule>();

			_container = builder.Build();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			//var shell = _container.Resolve<MainWindowModel>();
			//var windowManager = _container.Resolve<IWindowManager>();
			//windowManager.ShowWindow(shell);
		}

		protected override void OnExit(object sender, EventArgs e)
		{
			base.OnExit(sender, e);
		}

		protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;

			var dialogManager = _container.Resolve<IDialogManager>();
			var parameters = MessageBoxParams.Ok("Error", "Something remarkably horrible has happened.  " +
			                                              "Please note the time, tell Greg that POS is a P.O.S., and restart the app.",
			                                     MessageBoxIcon.Error);
			var msgBox = MessageBoxViewModel.FromParams(parameters);
			dialogManager.ShowDialog(msgBox);
		}
	}
}
