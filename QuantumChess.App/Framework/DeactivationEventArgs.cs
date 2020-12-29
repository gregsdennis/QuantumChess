﻿using System;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// EventArgs sent during deactivation.
	/// </summary>
	public class DeactivationEventArgs : EventArgs
	{
		/// <summary>
		/// Indicates whether the sender was closed in addition to being deactivated.
		/// </summary>
		public bool WasClosed;
	}
}
