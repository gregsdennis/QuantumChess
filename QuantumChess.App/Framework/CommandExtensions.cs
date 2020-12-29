using System.Windows.Input;

namespace QuantumChess.App.Framework
{
	public static class CommandExtensions
	{
		public static void Refresh(this ICommand command)
		{
			PlatformProvider.Current.InvalidateRequerySuggested();
		}
	}
}
