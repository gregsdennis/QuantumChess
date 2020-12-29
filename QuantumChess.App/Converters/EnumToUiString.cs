using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace QuantumChess.App.Converters
{
	/// <summary>
	/// Converts enumeration values into display-ready strings.
	/// </summary>
	public class EnumToUiString : IValueConverter
	{
		private static readonly Dictionary<Type, Dictionary<object, string>> _registry = 
			new Dictionary<Type, Dictionary<object, string>>();

		/// <summary>
		/// Gets the default instance.
		/// </summary>
		public static EnumToUiString Instance { get; }

		static EnumToUiString()
		{
			Instance = new EnumToUiString();
		}
		private EnumToUiString() { }

		/// <summary>
		/// Registers an enumeration type mapping.  All mappings should be registered 
		/// as part of the application bootstrapping process.
		/// </summary>
		/// <typeparam name="T">The enumeration type</typeparam>
		/// <param name="map"></param>
		public static void RegisterType<T>(Dictionary<T, string> map)
			where T : struct
		{
			if (!typeof(Enum).IsAssignableFrom(typeof(T))) throw new ArgumentException("Only enumeration types can be registered.");

			_registry[typeof(T)] = map.ToDictionary(kvp => (object) kvp.Key, kvp => kvp.Value);
		}

		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return null;

			if (_registry.TryGetValue(value.GetType(), out Dictionary<object, string> map))
			{
				if (map.TryGetValue(value, out string mappedValue)) return mappedValue;
			}
			return value.ToString();
		}
		/// <summary>Converts a value. </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return null;

			if (value is string text && _registry.TryGetValue(targetType, out Dictionary<object, string> map))
			{
				// must do this check b/c KeyValuePair<> is a struct.
				if (map.Any(kvp => kvp.Value == text))
				{
					var entry = map.FirstOrDefault(kvp => kvp.Value == text);
					return entry.Key;
				}
			}
			return null;
		}
	}
}
