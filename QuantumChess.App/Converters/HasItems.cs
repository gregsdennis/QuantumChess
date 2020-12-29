using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Converts collections to <see cref="Boolean"/> based on whether they have items.
	/// </summary>
	public class HasItems : IValueConverter
	{
		private static HasItems _itemsToTrue;
		private static HasItems _itemsToFalse;

		private readonly bool _isInverted;

		/// <summary>
		/// Converts collections with items to true.
		/// </summary>
		public static HasItems ItemsToTrue =>
			_itemsToTrue ?? (_itemsToTrue = new HasItems(false));
		/// <summary>
		/// Converts collections with items to false.
		/// </summary>
		public static HasItems ItemsToFalse =>
			_itemsToFalse ?? (_itemsToFalse = new HasItems(true));

		private HasItems(bool isInverted)
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
			var collection = value as IEnumerable<object>;
			return LogicInverter.InvertIfNecessary(collection?.Any() ?? false, _isInverted, true, false);
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