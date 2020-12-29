using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Provides various boolean combinatorial functions.
	/// </summary>
	public class BooleanLogic : IMultiValueConverter
	{
		/// <summary>
		/// Implements AND logic.  Returns true if all values are true.
		/// </summary>
		public static BooleanLogic And { get; }
		/// <summary>
		/// Implements NAND (not-AND) logic.  Returns true if any value is false.
		/// </summary>
		public static BooleanLogic Nand { get; }
		/// <summary>
		/// Implements OR logic.  Returns true if any value is true.
		/// </summary>
		public static BooleanLogic Or { get; }
		/// <summary>
		/// Implements NOR (not-OR) logic.  Returns true if all values are false.
		/// </summary>
		public static BooleanLogic Nor { get; }
		/// <summary>
		/// Implements XOR (exclusive-or) logic using an "inversion" algorithm.  Returns true if an odd number of imputs are true.
		/// </summary>
		public static BooleanLogic XorOdd { get; }
		/// <summary>
		/// Implements XNOR (not-XOR) logic using an "inversion" algorithm.  Returns true if an even number of inputs are true.
		/// </summary>
		public static BooleanLogic XnorOdd { get; }
		/// <summary>
		/// Implements XOR (exclusive-or) logic using a "pure exclusive" algorithm.  Returns true if only a single value is true.
		/// </summary>
		public static BooleanLogic XorSingle { get; }
		/// <summary>
		/// Implements XNOR (not-XOR) logic using a "pure exclusive" algorithm.  Return true if more than one value is true.
		/// </summary>
		public static BooleanLogic XnorSingle { get; }

		private readonly Func<IEnumerable<bool>, bool> _combine;
		private readonly bool _isInverted;

		static BooleanLogic()
		{
			And = new BooleanLogic(_And, false);
			Nand = new BooleanLogic(_And, true);
			Or = new BooleanLogic(_Or, false);
			Nor = new BooleanLogic(_Or, true);
			XorOdd = new BooleanLogic(_XorOdd, false);
			XnorOdd = new BooleanLogic(_XorOdd, true);
			XorSingle = new BooleanLogic(_XorSingle, false);
			XnorSingle = new BooleanLogic(_XorSingle, true);
		}
		private BooleanLogic(Func<IEnumerable<bool>, bool> combine, bool isInverted)
		{
			_combine = combine;
			_isInverted = isInverted;
		}


		/// <summary>Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
		/// <returns>A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty" />.<see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing" /> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
		/// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding" /> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue" /> indicates that the source binding has no value to provide for conversion.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Any(x => x == DependencyProperty.UnsetValue)) return values;
			
			if (!values.Any())
				throw new ArgumentException($"'{nameof(values)}' must contain atleast one boolean.");
			if (values.Any(v => !(v is bool)))
				throw new ArgumentException($"'{nameof(values)}' must contain only booleans.");

			var bools = values.Cast<bool>();
			return LogicInverter.InvertIfNecessary(_combine(bools), _isInverted, true, false);
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

		private static bool _And(IEnumerable<bool> values)
		{
			return values.All(v => v);
		}
		private static bool _Or(IEnumerable<bool> values)
		{
			return values.Any(v => v);
		}
		private static bool _XorOdd(IEnumerable<bool> values)
		{
			return  values.Count(v => v) % 2 == 1;
		}
		private static bool _XorSingle(IEnumerable<bool> values)
		{
			return  values.Count(v => v) == 1;
		}
	}
}