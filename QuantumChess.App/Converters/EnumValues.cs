using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Converts enumeration types to collections of their values.
	/// </summary>
	public class EnumValues : IValueConverter
	{
		/// <summary>
		/// Returns the typed values.
		/// </summary>
		public static EnumValues RawValues { get; }
		/// <summary>
		/// Returns the UI strings for each value via the <see cref="EnumToUiString"/>.
		/// </summary>
		public static EnumValues UiStrings { get; }

		private readonly bool _returnUiStrings;

		static EnumValues()
		{
			RawValues = new EnumValues(false);
			UiStrings = new EnumValues(true);
		}
		private EnumValues(bool returnUiStrings)
		{
			_returnUiStrings = returnUiStrings;
		}

		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var enumType = (value ?? parameter) as Type;
			if (!enumType.IsEnum) return value;

			var values = Enum.GetValues(enumType).Cast<object>().ToList();

			return _returnUiStrings
				       ? values.Select(v => EnumToUiString.Instance.Convert(v, targetType, parameter, culture))
				       : values;
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
