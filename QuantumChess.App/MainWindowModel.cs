using QuantumChess.App.Framework;
using QuantumChess.App.Model;

namespace QuantumChess.App
{
	public class MainWindowModel : Screen
	{
		public BoardSpace BoardSpace { get; set; }

		public MainWindowModel()
		{
			BoardSpace = new BoardSpace();
		}
	}
}