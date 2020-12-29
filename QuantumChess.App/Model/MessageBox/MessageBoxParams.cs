using System;
using System.Collections.Generic;
using System.Linq;
using QuantumChess.App.Properties;

namespace QuantumChess.App.Model.MessageBox
{
	/// <summary>
	/// Defines the parameters for a message box.
	/// </summary>
	public class MessageBoxParams
	{
		/// <summary>
		/// Gets or sets the text which appears in the title bar.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the text which appears as the main body.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets an optional icon.
		/// </summary>
		public MessageBoxIcon Icon { get; set; }

		/// <summary>
		/// Gets or sets whether to show a confirm button.
		/// </summary>
		public bool ShowConfirm { get; set; }

		/// <summary>
		/// Gets or sets the text to show in the confirm button.
		/// </summary>
		public string ConfirmText { get; set; }

		/// <summary>
		/// Gets or sets whether to show a decline button.
		/// </summary>
		public bool ShowDecline { get; set; }

		/// <summary>
		/// Gets or sets the text to show in the decline button.
		/// </summary>
		public string DeclineText { get; set; }

		/// <summary>
		/// Gets or sets whether to show a cancel button.
		/// </summary>
		public bool ShowCancel { get; set; }

		/// <summary>
		/// Gets or sets the text to show in the cancel button.
		/// </summary>
		public string CancelText { get; set; }

		/// <summary>
		/// Gets or sets the default action to be executed when pressing the ENTER key.
		/// </summary>
		public MessageBoxAction? DefaultAction { get; set; }

		/// <summary>
		/// Gets or sets whether the user is allowed to close the window using the
		/// red X button in the upper right corner of the window.
		/// The default is true.
		/// </summary>
		public bool AllowNonResponse { get; set; } = true;

		/// <summary>
		/// Creates a new <see cref="MessageBoxParams"/> instance that shows only an OK button.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <param name="icon">(optional) The icon.  Default is <see cref="MessageBoxIcon.None"/></param>
		public static MessageBoxParams Ok(string title, string message, MessageBoxIcon icon = MessageBoxIcon.None)
		{
			return new MessageBoxParams
				{
					Title = title,
					Message = message,
					Icon = icon,
					ShowConfirm = true,
					ConfirmText = Resources.Ok,
					DefaultAction = MessageBoxAction.Confirm
				};
		}

		/// <summary>
		/// Creates a new <see cref="MessageBoxParams"/> instance that shows OK and Cancel buttons.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <param name="icon">(optional) The icon.  Default is <see cref="MessageBoxIcon.None"/></param>
		public static MessageBoxParams OkCancel(string title, string message, MessageBoxIcon icon = MessageBoxIcon.None)
		{
			return new MessageBoxParams
				{
					Title = title,
					Message = message,
					Icon = icon,
					ShowConfirm = true,
					ConfirmText = Resources.Ok,
					ShowCancel = true,
					CancelText = Resources.Cancel,
					DefaultAction = MessageBoxAction.Confirm
				};
		}

		/// <summary>
		/// Creates a new <see cref="MessageBoxParams"/> instance that shows Yes and No buttons.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <param name="icon">(optional) The icon.  Default is <see cref="MessageBoxIcon.None"/></param>
		public static MessageBoxParams YesNo(string title, string message, MessageBoxIcon icon = MessageBoxIcon.None)
		{
			return new MessageBoxParams
				{
					Title = title,
					Message = message,
					Icon = icon,
					ShowConfirm = true,
					ConfirmText = Resources.Yes,
					ShowDecline = true,
					DeclineText = Resources.No,
					AllowNonResponse = false,
					DefaultAction = MessageBoxAction.Confirm
				};
		}

		/// <summary>
		/// Creates a new <see cref="MessageBoxParams"/> instance that shows Yes, No, and Cancel buttons.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <param name="icon">(optional) The icon.  Default is <see cref="MessageBoxIcon.None"/></param>
		public static MessageBoxParams YesNoCancel(string title, string message, MessageBoxIcon icon = MessageBoxIcon.None)
		{
			return new MessageBoxParams
				{
					Title = title,
					Message = message,
					Icon = icon,
					ShowConfirm = true,
					ConfirmText = Resources.Yes,
					ShowDecline = true,
					DeclineText = Resources.No,
					ShowCancel = true,
					CancelText = Resources.Cancel,
					DefaultAction = MessageBoxAction.Confirm
				};
		}

		/// <summary>
		/// Creates a new <see cref="MessageBoxParams"/> instance that shows only an OK button and extracts inner exception details.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		/// <param name="maxDepth">The max depth to go for loading inner exception messages.</param>
		public static MessageBoxParams Exception(string title, string message, Exception exception, int maxDepth)
		{
			int depth = 0;

			List<string> messages = new List<string>();

			if (!string.IsNullOrWhiteSpace(message))
				messages.Add(message);

			messages.Add(exception.Message);

			messages.AddRange(exception.FromHierarchy(ex => ex.InnerException,
			                                          ex => !string.IsNullOrEmpty(ex.InnerException?.Message) && depth++ < maxDepth)
			                           .Select(ex => ex.InnerException.Message));

			string n = Environment.NewLine;

			string errorMessage = string.Join($"{n}{n}", messages);

			return new MessageBoxParams
				{
					Title = title,
					Message = errorMessage,
					Icon = MessageBoxIcon.Error,
					ShowConfirm = true,
					ConfirmText = Resources.Ok
				};
		}
	}
}