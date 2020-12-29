namespace QuantumChess.App.Framework
{
	public interface IDialogManager
	{
		/// <summary>
		/// Shows a modal dialog for the specified model.
		/// </summary>
		/// <param name="rootModel">The root model.</param>
		/// <returns>The dialog result.</returns>
		void ShowDialog(IScreen rootModel);

		void RegisterHost(IDialogHost dialogHost);
	}
}