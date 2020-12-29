using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using QuantumChess.App.Model;

namespace QuantumChess.App.Converters
{
	public class PieceColorConverter : IValueConverter
	{
		public static PieceColorConverter Instance { get; } = new PieceColorConverter();

		private PieceColorConverter(){ }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((PieceColor)value)
			{
				case PieceColor.White:
					return Colors.White;
				case PieceColor.Black:
					return Colors.Black;
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
