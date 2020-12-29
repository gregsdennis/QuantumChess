using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Aggregates a series of converters to produce a single value.
	/// </summary>
	[DefaultProperty(nameof(Converters))]
	[ContentProperty(nameof(Converters))]
	public class Aggregator : MarkupExtension, IValueConverter, IMultiValueConverter
	{
		private List<object> _converters;

		/// <summary>
		/// Gets or sets the child converters.
		/// </summary>
		public IList Converters
		{
			get { return _converters; }
			set
			{
				var values = value.Cast<object>().ToList();
				if (values.Any(v => !(v is IValueConverter) && !(v is IMultiValueConverter)))
					throw new ArgumentException("Only IValueConverters and IMultiValueConverters are allowed.");
				_converters = values;
			}
		}

		/// <summary>
		/// Creates an empty instance of <see cref="Aggregator"/>.
		/// </summary>
		public Aggregator()
		{
			Converters = new List<object>();
		}
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public Aggregator(IValueConverter converter1, IValueConverter converter2)
		{
			Converters = new object[] {converter1, converter2};
		}
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public Aggregator(IValueConverter converter1, IValueConverter converter2, IValueConverter converter3)
		{
			Converters = new object[] {converter1, converter2, converter3};
		}
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public Aggregator(IValueConverter converter1, IValueConverter converter2, IValueConverter converter3, IValueConverter converter4)
		{
			Converters = new object[] {converter1, converter2, converter3, converter4};
		}
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public Aggregator(IValueConverter converter1, IValueConverter converter2, IValueConverter converter3, IValueConverter converter4, IValueConverter converter5)
		{
			Converters = new object[] {converter1, converter2, converter3, converter4, converter5};
		}

		/// <summary>When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension. </summary>
		/// <returns>The object value to set on the property where the extension is applied. </returns>
		/// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = _converters?.Aggregate(new[] {value}, (v, c) => _PerformConversion(c, v, targetType, parameter, culture));
			return result?[0];
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
		/// <summary>Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
		/// <returns>A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
		/// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var result = _converters?.Aggregate(values, (v, c) => _PerformConversion(c, v, targetType, parameter, culture));
			return result?[0];
		}
		/// <summary>Converts a binding target value to the source binding values.</summary>
		/// <returns>An array of values that have been converted from the target value back to the source values.</returns>
		/// <param name="value">The value that the binding target produces.</param>
		/// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private static object[] _PerformConversion(object converter, object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			switch (converter)
			{
				case IValueConverter single:
					return new[] {single.Convert(values[0], targetType, parameter, culture)};
				case IMultiValueConverter multi:
					return new[] {multi.Convert(values, targetType, parameter, culture)};
				default:
					throw new ArgumentOutOfRangeException(nameof(converter));
			}
		}
	}

	public class MultiAggregator : Aggregator
	{
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public MultiAggregator(IMultiValueConverter converter1, IValueConverter converter2)
		{
			Converters = new object[] { converter1, converter2 };
		}
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public MultiAggregator(IMultiValueConverter converter1, IValueConverter converter2, IValueConverter converter3)
		{
			Converters = new object[] { converter1, converter2, converter3 };
		}
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public MultiAggregator(IMultiValueConverter converter1, IValueConverter converter2, IValueConverter converter3, IValueConverter converter4)
		{
			Converters = new object[] { converter1, converter2, converter3, converter4 };
		}
		/// <summary>
		/// Creates a new instance of <see cref="Aggregator"/> and initializes with the passed converters.
		/// </summary>
		public MultiAggregator(IMultiValueConverter converter1, IValueConverter converter2, IValueConverter converter3, IValueConverter converter4, IValueConverter converter5)
		{
			Converters = new object[] { converter1, converter2, converter3, converter4, converter5 };
		}
	}
}
