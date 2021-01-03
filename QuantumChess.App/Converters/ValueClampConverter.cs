using System;
using System.Globalization;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	public class ValueClampConverter : IValueConverter
	{
		public static ValueClampConverter Instance { get; } = new ValueClampConverter();

		private ValueClampConverter(){ }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var range = ((string) parameter).Split("..");
			var lower = double.Parse(range[0]);
			var upper = double.Parse(range[1]);
			var span = upper - lower;

			return System.Convert.ToDouble(value) * span + lower;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
