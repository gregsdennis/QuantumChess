using System;
using System.ComponentModel;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Extends <see cref = "INotifyPropertyChanged" /> such that the change event can be raised by external parties.
	/// </summary>
	public interface INotifyPropertyChangedEx : INotifyPropertyChanged
	{
		/// <summary>
		/// Enables/Disables property change notification.
		/// </summary>
		bool IsNotifying { get; set; }

		/// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <param name = "propertyName">Name of the property.</param>
		void NotifyOfPropertyChange(string propertyName = null);

		/// <summary>
		/// Raises a change notification indicating that all bindings should be refreshed.
		/// </summary>
		void Refresh();

		void RunWithSingleUpdate(Action action);
	}
}
