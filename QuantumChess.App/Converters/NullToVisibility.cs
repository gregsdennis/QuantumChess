using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Converts boolean values to and from <see cref="Visibility"/>.
	/// </summary>
	public class NullToVisibility : IValueConverter
	{
		private static NullToVisibility _trueToCollapsed;
		private static NullToVisibility _trueToHidden;
		private static NullToVisibility _falseToCollapsed;
		private static NullToVisibility _falseToHidden;

		private readonly bool _isInverted;
		private readonly Visibility _nonVisibleState;

		/// <summary>
		/// Converts true to <see cref="Visibility.Collapsed"/> and
		/// false to <see cref="Visibility.Visible"/>.
		/// </summary>
		public static NullToVisibility NullToCollapsed =>
			_trueToCollapsed ??= new NullToVisibility(true, Visibility.Collapsed);
		/// <summary>
		/// Converts true to <see cref="Visibility.Hidden"/> and
		/// false to <see cref="Visibility.Visible"/>.
		/// </summary>
		public static NullToVisibility NullToHidden =>
			_trueToHidden ??= new NullToVisibility(true, Visibility.Hidden);
		/// <summary>
		/// Converts true to <see cref="Visibility.Visible"/> and
		/// false to <see cref="Visibility.Collapsed"/>.
		/// </summary>
		public static NullToVisibility NotNullToCollapsed =>
			_falseToCollapsed ??= new NullToVisibility(false, Visibility.Collapsed);
		/// <summary>
		/// Converts true to <see cref="Visibility.Visible"/> and
		/// false to <see cref="Visibility.Hidden"/>.
		/// </summary>
		public static NullToVisibility NotNullToHidden =>
			_falseToHidden ??= new NullToVisibility(false, Visibility.Hidden);

		private NullToVisibility(bool isInverted, Visibility nonVisibleState)
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
			return LogicInverter.InvertIfNecessary(value == null, _isInverted, Visibility.Visible, _nonVisibleState);
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
