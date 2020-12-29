using System;

namespace QuantumChess.App.Framework
{

	/// <summary>
	/// EventArgs sent during activation.
	/// </summary>
	public class ActivationEventArgs : EventArgs
	{
		/// <summary>
		/// Indicates whether the sender was initialized in addition to being activated.
		/// </summary>
		public bool FirstActivation;
	}
}
