using System;
using System.Windows.Input;
using QuantumChess.App.Framework;

namespace QuantumChess.App.Model.MessageBox
{
	/// <summary>
	/// Represents a message box.
	/// </summary>
	public class MessageBoxViewModel : Screen
	{
		private string _cancelText;
		private bool _showCancel;
		private string _declineText;
		private bool _showDecline;
		private string _confirmText;
		private bool _showConfirm;
		private MessageBoxIcon _icon;
		private string _message;
		private string _title;
		private MessageBoxAction? _defaultAction;
		private bool _allowNonResponse;

		/// <summary>
		/// Gets or sets the text which appears in the title bar.
		/// </summary>
		public string Title
		{
			get { return _title; }
			set
			{
				if (value == _title) return;
				_title = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets the text which appears as the main body.
		/// </summary>
		public string Message
		{
			get { return _message; }
			set
			{
				if (value == _message) return;
				_message = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets an optional icon.
		/// </summary>
		public MessageBoxIcon Icon
		{
			get { return _icon; }
			set
			{
				if (value == _icon) return;
				_icon = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets whether to show a confirm button.
		/// </summary>
		public bool ShowConfirm
		{
			get { return _showConfirm; }
			set
			{
				if (value == _showConfirm) return;
				_showConfirm = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets the text to show in the confirm button.
		/// </summary>
		public string ConfirmText
		{
			get { return _confirmText; }
			set
			{
				if (value == _confirmText) return;
				_confirmText = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets whether to show a decline button.
		/// </summary>
		public bool ShowDecline
		{
			get { return _showDecline; }
			set
			{
				if (value == _showDecline) return;
				_showDecline = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets the text to show in the decline button.
		/// </summary>
		public string DeclineText
		{
			get { return _declineText; }
			set
			{
				if (value == _declineText) return;
				_declineText = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets whether to show a cancel button.
		/// </summary>
		public bool ShowCancel
		{
			get { return _showCancel; }
			set
			{
				if (value == _showCancel) return;
				_showCancel = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets the text to show in the cancel button.
		/// </summary>
		public string CancelText
		{
			get { return _cancelText; }
			set
			{
				if (value == _cancelText) return;
				_cancelText = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets the default action to be executed when pressing the ENTER key.
		/// </summary>
		public MessageBoxAction? DefaultAction
		{
			get { return _defaultAction; }
			set
			{
				if (value == _defaultAction) return;
				_defaultAction = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets or sets whether the user is allowed to close the window using the
		/// red X button in the upper right corner of the window.
		/// </summary>
		public bool AllowNonResponse
		{
			get { return _allowNonResponse; }
			set
			{
				if (value == _allowNonResponse) return;
				_allowNonResponse = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Gets the user-provided response.
		/// </summary>
		public MessageBoxAction Result { get; private set; }

		/// <summary>
		/// Implements the confirm button action.
		/// </summary>
		public ICommand Confirm { get; }

		/// <summary>
		/// Implements the decline button action.
		/// </summary>
		public ICommand Decline { get; }

		/// <summary>
		/// Implements the cancel button action.
		/// </summary>
		public ICommand Cancel { get; }

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public MessageBoxViewModel()
		{
			Confirm = new RelayCommand(() =>
				                           {
					                           Result = MessageBoxAction.Confirm;
					                           TryClose();
				                           });
			Decline = new RelayCommand(() =>
				                           {
					                           Result = MessageBoxAction.Decline;
					                           TryClose();
				                           });
			Cancel = new RelayCommand(() =>
				                          {
					                          Result = MessageBoxAction.Cancel;
					                          TryClose();
				                          });
		}

		public static MessageBoxViewModel FromParams(MessageBoxParams context)
		{
			return new MessageBoxViewModel
				{
					Title = context.Title,
					Message = context.Message,
					Icon = context.Icon,
					ShowConfirm = context.ShowConfirm,
					ConfirmText = context.ConfirmText,
					ShowDecline = context.ShowDecline,
					DeclineText = context.DeclineText,
					ShowCancel = context.ShowCancel,
					CancelText = context.CancelText,
					AllowNonResponse = context.AllowNonResponse,
					DefaultAction = context.DefaultAction
				};
		}

		public override void CanClose(Action<bool> callback)
		{
			callback(Result != MessageBoxAction.None || AllowNonResponse);
		}
	}
}
