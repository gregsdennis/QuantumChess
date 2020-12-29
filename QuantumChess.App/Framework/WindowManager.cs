using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

// ReSharper disable IdentifierTypo

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// A service that manages windows.
	/// </summary>
	public class WindowManager : IWindowManager
	{
		private class WindowConductor
		{
			private bool _deactivatingFromView;
			private bool _deactivateFromViewModel;
			private bool _actuallyClosing;
			private readonly Window _view;
			private readonly object _model;

			public WindowConductor(object model, Window view)
			{
				_model = model;
				_view = view;

				var activatable = model as IActivate;
				activatable?.Activate();

				if (model is IDeactivate deactivatable)
				{
					view.Closed += Closed;
					deactivatable.Deactivated += Deactivated;
				}

				if (model is IGuardClose)
					view.Closing += Closing;
			}

			private void Closed(object sender, EventArgs e)
			{
				_view.Closed -= Closed;
				_view.Closing -= Closing;

				if (_deactivateFromViewModel) return;

				var deactivatable = (IDeactivate)_model;

				_deactivatingFromView = true;
				deactivatable.Deactivate(true);
				_deactivatingFromView = false;
			}

			private void Deactivated(object sender, DeactivationEventArgs e)
			{
				if (!e.WasClosed) return;

				((IDeactivate)_model).Deactivated -= Deactivated;

				if (_deactivatingFromView) return;

				_deactivateFromViewModel = true;
				_actuallyClosing = true;
				Execute.OnUiThread(() => _view.Close());
				_actuallyClosing = false;
				_deactivateFromViewModel = false;
			}

			private void Closing(object sender, CancelEventArgs e)
			{
				if (e.Cancel) return;

				var guard = (IGuardClose)_model;

				if (_actuallyClosing)
				{
					_actuallyClosing = false;
					return;
				}

				bool runningAsync = false, shouldEnd = false;

				guard.CanClose(canClose =>
				{
					Execute.OnUiThread(() =>
					{
						// ReSharper disable once AccessToModifiedClosure
						if (runningAsync && canClose)
						{
							_actuallyClosing = true;
							_view.Close();
						}
						else
							e.Cancel = !canClose;

						shouldEnd = true;
					});
				});

				if (shouldEnd) return;

				runningAsync = e.Cancel = true;
			}
		}

		/// <summary>
		/// Shows a window for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		public virtual void ShowWindow(IScreen rootModel)
		{
			NavigationWindow navWindow = null;

			var application = Application.Current;
			if (application?.MainWindow != null)
				navWindow = application.MainWindow as NavigationWindow;

			if (navWindow != null)
			{
				var window = CreatePage(rootModel);
				navWindow.Navigate(window);
			}
			else
				Execute.OnUiThread(() => CreateWindow(rootModel, false).Show());
		}

		/// <summary>
		/// Creates a window.
		/// </summary>
		/// <param name="rootModel">The view model.</param>
		/// <param name="isDialog">Whether or not the window is being shown as a dialog.</param>
		/// <returns>The window.</returns>
		private static Window CreateWindow(object rootModel, bool isDialog)
		{
			Window view = EnsureWindow(rootModel, ViewLocator.LocateForModel(rootModel, null), isDialog);
			
			new WindowConductor(rootModel, view);
				
			return view;
		}

		/// <summary>
		/// Makes sure the view is a window is is wrapped by one.
		/// </summary>
		/// <param name="model">The view model.</param>
		/// <param name="view">The view.</param>
		/// <param name="isDialog">Whether or not the window is being shown as a dialog.</param>
		/// <returns>The window.</returns>
		private static Window EnsureWindow(object model, object view, bool isDialog)
		{
			var window = view as Window;

			if (window == null)
			{
				window = new Window
					{
						Content = view,
						SizeToContent = SizeToContent.WidthAndHeight,
						Title = model.GetType().Name
					};

				var owner = InferOwnerOf(window);
				if (owner != null)
				{
					window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
					window.Owner = owner;
				}
				else
					window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			}
			else
			{
				var owner = InferOwnerOf(window);
				if (owner != null && isDialog)
					window.Owner = owner;
			}

			window.DataContext = model;

			return window;
		}

		/// <summary>
		/// Infers the owner of the window.
		/// </summary>
		/// <param name="window">The window to whose owner needs to be determined.</param>
		/// <returns>The owner.</returns>
		private static Window InferOwnerOf(Window window)
		{
			var application = Application.Current;
			if (application == null) return null;

			var active = application.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
			// ReSharper disable once AssignNullToNotNullAttribute
			active = active ?? (PresentationSource.FromVisual(application.MainWindow) == null ? null : application.MainWindow);
			return Equals(active, window) ? null : active;
		}

		/// <summary>
		/// Creates the page.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <returns>The page.</returns>
		private static Page CreatePage(object rootModel)
		{
			var view = EnsurePage(rootModel, ViewLocator.LocateForModel(rootModel, null));

			var activatable = rootModel as IActivate;
			activatable?.Activate();

			if (rootModel is IDeactivate deactivatable)
				view.Unloaded += (s, e) => deactivatable.Deactivate(true);

			return view;
		}

		/// <summary>
		/// Ensures the view is a page or provides one.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="view">The view.</param>
		/// <returns>The page.</returns>
		private static Page EnsurePage(object model, object view)
		{
			var page = view as Page;

			if (page == null)
				page = new Page {Content = view};

			page.DataContext = model;

			return page;
		}
	}
}