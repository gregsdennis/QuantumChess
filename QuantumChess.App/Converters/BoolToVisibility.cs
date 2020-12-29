using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Converts boolean values to and from <see cref="Visibility"/>.
	/// </summary>
	public class BoolToVisibility : IValueConverter
	{
		private static BoolToVisibility _trueToCollapsed;
		private static BoolToVisibility _trueToHidden;
		private static BoolToVisibility _falseToCollapsed;
		private static BoolToVisibility _falseToHidden;

		private readonly bool _isInverted;
		private readonly Visibility _nonVisibleState;

		/// <summary>
		/// Converts true to <see cref="Visibility.Collapsed"/> and
		/// false to <see cref="Visibility.Visible"/>.
		/// </summary>
		public static BoolToVisibility TrueToCollapsed =>
			_trueToCollapsed ?? (_trueToCollapsed = new BoolToVisibility(true, Visibility.Collapsed));
		/// <summary>
		/// Converts true to <see cref="Visibility.Hidden"/> and
		/// false to <see cref="Visibility.Visible"/>.
		/// </summary>
		public static BoolToVisibility TrueToHidden =>
			_trueToHidden ?? (_trueToHidden = new BoolToVisibility(true, Visibility.Hidden));
		/// <summary>
		/// Converts true to <see cref="Visibility.Visible"/> and
		/// false to <see cref="Visibility.Collapsed"/>.
		/// </summary>
		public static BoolToVisibility FalseToCollapsed =>
			_falseToCollapsed ?? (_falseToCollapsed = new BoolToVisibility(false, Visibility.Collapsed));
		/// <summary>
		/// Converts true to <see cref="Visibility.Visible"/> and
		/// false to <see cref="Visibility.Hidden"/>.
		/// </summary>
		public static BoolToVisibility FalseToHidden =>
			_falseToHidden ?? (_falseToHidden = new BoolToVisibility(false, Visibility.Hidden));

		private BoolToVisibility(bool isInverted, Visibility nonVisibleState)
		{
			_isInverted = isInverted;
			_nonVisibleState = nonVisibleState;
		}

		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool)) return value;

			return LogicInverter.InvertIfNecessary((bool) value, _isInverted, Visibility.Visible, _nonVisibleState);
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
