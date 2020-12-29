namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Denotes an instance which implements <see cref="IActivate"/>, 
	/// <see cref="IDeactivate"/>, <see cref="IGuardClose"/> and <see cref="INotifyPropertyChangedEx"/>
	/// </summary>
	public interface IScreen : IActivate, IDeactivate, IGuardClose, IInitialize, INotifyPropertyChangedEx {}
}
