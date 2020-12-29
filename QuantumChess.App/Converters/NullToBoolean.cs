using System;
using System.Globalization;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Converts values to <see cref="Boolean"/>.
	/// </summary>
	public class NullToBoolean : IValueConverter
	{
		private static NullToBoolean _nullToTrue;

		private readonly bool _isInverted;

		/// <summary>
		/// Converts null values to true.
		/// </summary>
		public static NullToBoolean NullToTrue =>
			_nullToTrue ?? (_nullToTrue = new NullToBoolean(false));
		/// <summary>
		/// Converts null values to false.
		/// </summary>
		public static NullToBoolean NullToFalse =>
			_nullToTrue ?? (_nullToTrue = new NullToBoolean(true));

		private NullToBoolean(bool isInverted)
		{
			_isInverted = isInverted;
		}

		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return LogicInverter.InvertIfNecessary(value == null, _isInverted, true, false);
		}
		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
