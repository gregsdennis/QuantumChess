using System;
using System.Globalization;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Converts string values to <see cref="Boolean"/> based on its content.
	/// </summary>
	public class StringNullability : IValueConverter
	{
		private static StringNullability _isNull;
		private static StringNullability _isNullOrWhitespace;
		private static StringNullability _isNotNull;
		private static StringNullability _isNotNullOrWhitespace;

		private readonly bool _isInverted;
		private readonly Func<string, bool> _stringTest;

		/// <summary>
		/// Converts null to <b>true</b> and non-null to <b>false</b>.
		/// </summary>
		public static StringNullability IsNull =>
			_isNull ?? (_isNull = new StringNullability(false, s => s == null));
		/// <summary>
		/// Converts null or whitespace to <b>true</b> and non-null, non-whitespace to <b>false</b>.
		/// </summary>
		public static StringNullability IsNullOrWhitespace =>
			_isNullOrWhitespace ?? (_isNullOrWhitespace = new StringNullability(false, string.IsNullOrWhiteSpace));
		/// <summary>
		/// Converts null to <b>false</b> and non-null to <b>true</b>.
		/// </summary>
		public static StringNullability IsNotNull =>
			_isNotNull ?? (_isNotNull = new StringNullability(true, s => s == null));
		/// <summary>
		/// Converts null or whitespace to <b>false</b> and non-null, non-whitespace to <b>true</b>.
		/// </summary>
		public static StringNullability IsNotNullOrNotWhitespace =>
			_isNotNullOrWhitespace ?? (_isNotNullOrWhitespace = new StringNullability(true, string.IsNullOrWhiteSpace));

		private StringNullability(bool isInverted, Func<string, bool> stringTest)
		{
			_isInverted = isInverted;
			_stringTest = stringTest;
		}

		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && !(value is string)) throw new ArgumentException($"value must be of type '{typeof(string)}'");

			var retVal = LogicInverter.InvertIfNecessary(_stringTest((string) value), _isInverted, true, false);

			return retVal;
		}
		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("Cannot implement nullability -> string conversion.");
		}
	}
}
