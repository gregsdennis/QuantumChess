using System;
using System.Collections.Generic;

namespace QuantumChess.App.Framework
{
	internal class DialogManager : IDialogManager
	{
		private readonly Stack<IScreen> _dialogStack = new Stack<IScreen>();

		private IDialogHost _dialogHost;

		public void RegisterHost(IDialogHost dialogHost)
		{
			_dialogHost = dialogHost;
		}

		/// <summary>
		/// Shows a modal dialog for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <returns>The dialog result.</returns>
		public virtual void ShowDialog(IScreen rootModel)
		{
			if (_dialogHost == null)
				throw new InvalidOperationException("A dialog host must be registered before showing dialogs.");

			void OnRootModelOnDeactivated(object sender, DeactivationEventArgs args)
			{
				_dialogHost.Dialog = _dialogStack.Count == 0 ? null : _dialogStack.Pop();
				rootModel.Deactivated -= OnRootModelOnDeactivated;
			}

			rootModel.Deactivated += OnRootModelOnDeactivated;
			if (_dialogHost.Dialog != null)
				_dialogStack.Push(_dialogHost.Dialog);
			_dialogHost.Dialog = rootModel;
			rootModel.Activate();
		}
	}
}