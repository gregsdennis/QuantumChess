namespace QuantumChess.App.Model.MessageBox
{
	/// <summary>
	/// Enumerates various message box result types.
	/// </summary>
	public enum MessageBoxAction
	{
		/// <summary>
		/// Indicates no action was taken.
		/// </summary>
		None,
		/// <summary>
		/// Indicates the user chose to cancel.
		/// </summary>
		Cancel,
		/// <summary>
		/// Indicates the user confirmed.
		/// </summary>
		Confirm,
		/// <summary>
		/// Indicates the user declined.
		/// </summary>
		Decline
	}
}