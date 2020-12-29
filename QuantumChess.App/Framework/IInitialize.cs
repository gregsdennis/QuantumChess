namespace QuantumChess.App.Framework
{
	public interface IInitialize
	{
		/// <summary>
		/// Gets whether the view model is in the process of being initialized.
		/// </summary>
		bool IsInitializing { get; }
		/// <summary>
		/// Gets whether the view model has completed initialization.
		/// </summary>
		bool IsInitialized { get; }

		/// <summary>
		/// Sets properties to indicate initialization has begun.
		/// </summary>
		void BeginInitialization();

		/// <summary>
		/// Sets properties to indicate initialization is complete.
		/// </summary>
		void CompleteInitialization();
	}
}