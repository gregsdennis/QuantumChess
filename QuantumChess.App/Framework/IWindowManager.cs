namespace QuantumChess.App.Framework
{
	/// <summary>
	/// A service that manages windows.
	/// </summary>
	public interface IWindowManager
	{
		/// <summary>
		/// Shows a non-modal window for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		void ShowWindow(IScreen rootModel);
	}
}