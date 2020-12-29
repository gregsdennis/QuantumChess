using System;
using System.Threading.Tasks;

namespace QuantumChess.App.Framework
{
	/// <summary>
	///   Enables easy marshalling of code to the UI thread.
	/// </summary>
	public static class Execute
	{
		/// <summary>
		///   Indicates whether or not the framework is in design-time mode.
		/// </summary>
		public static bool InDesignMode => PlatformProvider.Current.InDesignMode;

		/// <summary>
		///   Executes the action on the UI thread asynchronously.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		public static Task BeginOnUiThread(this Action action)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));

			return PlatformProvider.Current.BeginOnUiThread(action);
		}

		/// <summary>
		///   Executes the action on the UI thread.
		/// </summary>
		/// <param name = "action">The action to execute.</param>
		public static void OnUiThread(this Action action)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));

			PlatformProvider.Current.OnUiThread(action);
		}
	}
}
