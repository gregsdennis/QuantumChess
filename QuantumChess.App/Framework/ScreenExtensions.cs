namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Hosts extension methods for <see cref="IScreen"/> classes.
	/// </summary>
	internal static class ScreenExtensions
	{
		/// <summary>
		/// Activates the item if it implements <see cref="IActivate"/>, otherwise does nothing.
		/// </summary>
		/// <param name="potentialActivatable">The potential activatable.</param>
		public static void TryActivate(object potentialActivatable)
		{
			var activator = potentialActivatable as IActivate;
			activator?.Activate();
		}

		/// <summary>
		/// Deactivates the item if it implements <see cref="IDeactivate"/>, otherwise does nothing.
		/// </summary>
		/// <param name="potentialDeactivatable">The potential deactivatable.</param>
		/// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
		public static void TryDeactivate(object potentialDeactivatable, bool close)
		{
			var deactivator = potentialDeactivatable as IDeactivate;
			deactivator?.Deactivate(close);
		}
	}
}
