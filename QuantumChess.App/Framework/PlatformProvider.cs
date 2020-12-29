namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Access the current <see cref="IPlatformProvider"/>.
	/// </summary>
	public static class PlatformProvider
	{
		private static IPlatformProvider _current;
		/// <summary>
		/// Gets or sets the current <see cref="IPlatformProvider"/>.
		/// </summary>
		public static IPlatformProvider Current
		{
			get { return _current ?? (_current = new DeafultPlatformProvider()); }
			set { _current = value ?? new DeafultPlatformProvider(); }
		}
	}
}
