namespace QuantumChess.App.Converters
{
	internal static class LogicInverter
	{
		public static T InvertIfNecessary<T>(bool condition, bool invert, T trueValue, T falseValue)
		{
			return (!invert && condition) || (invert && !condition) ? trueValue : falseValue;
		}
	}
}
