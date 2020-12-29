using System;
using System.Globalization;
using System.Windows.Data;
using QuantumChess.App.Model;

namespace QuantumChess.App.Converters
{
	public class PieceProbabilitySummaryConverter : IValueConverter
	{
		public static PieceProbabilitySummaryConverter Instance { get; } = new PieceProbabilitySummaryConverter();

		private PieceProbabilitySummaryConverter(){ }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var piece = value as PieceProbability;
			if (piece == null) return null;

			if ((string) parameter == "Name")
				return $"{piece.Piece.Color} {piece.Piece.Kind}";

			if ((string) parameter == "Stats")
				return piece.Probability.ToString("P2");

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
