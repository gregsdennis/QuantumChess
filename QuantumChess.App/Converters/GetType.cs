using System;
using System.Globalization;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	internal class GetType : IValueConverter
	{
		public static GetType Instance { get; } = new GetType();

		private GetType() { }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value?.GetType().Name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
