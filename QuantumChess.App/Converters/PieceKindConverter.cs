using System;
using System.Globalization;
using System.Windows.Data;
using QuantumChess.App.Model;

namespace QuantumChess.App.Converters
{
	public class PieceKindConverter : IValueConverter
	{
		public static PieceKindConverter Instance { get; } = new PieceKindConverter();

		private PieceKindConverter(){ }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((PieceKind)value)
			{
				case PieceKind.Pawn:
					return MaterialDesignThemes.Wpf.PackIconKind.ChessPawn;
				case PieceKind.Rook:
					return MaterialDesignThemes.Wpf.PackIconKind.ChessRook;
				case PieceKind.Knight:
					return MaterialDesignThemes.Wpf.PackIconKind.ChessKnight;
				case PieceKind.Bishop:
					return MaterialDesignThemes.Wpf.PackIconKind.ChessBishop;
				case PieceKind.Queen:
					return MaterialDesignThemes.Wpf.PackIconKind.ChessQueen;
				case PieceKind.King:
					return MaterialDesignThemes.Wpf.PackIconKind.ChessKing;
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
