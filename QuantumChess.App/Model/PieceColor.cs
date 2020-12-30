namespace QuantumChess.App.Model
{
	public enum PieceColor
	{
		White = -1,
		Black = 1,
	}

	public static class Extensions
	{
		public static PieceColor Other(this PieceColor color)
		{
			return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
		}
	}
}