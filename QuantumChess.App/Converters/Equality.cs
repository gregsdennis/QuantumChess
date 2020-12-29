using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	public class Equality : IValueConverter, IMultiValueConverter
	{
		public static Equality AreEqual { get; }
		public static Equality AreNotEqual { get; }

		private readonly bool _invert;

		static Equality()
		{
			AreEqual = new Equality(false);
			AreNotEqual = new Equality(true);
		}
		private Equality(bool invert)
		{
			_invert = invert;
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length == 1)
				values = new[] {values[0], parameter};

			var distinct = values.Distinct().Count();
			return _invert ? distinct != 1 : distinct == 1;
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert(new[] {value, parameter}, targetType, null, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
