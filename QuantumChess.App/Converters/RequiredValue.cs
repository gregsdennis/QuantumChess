namespace QuantumChess.App.Converters
{
	//internal class RequiredValue : IValueConverter
	//{
	//	private readonly bool _invert;

	//	public static RequiredValue Yes { get; }
	//	public static RequiredValue No { get; }

	//	static RequiredValue()
	//	{
	//		Yes = new RequiredValue(false);
	//		No = new RequiredValue(true);
	//	}
	//	private RequiredValue(bool invert)
	//	{
	//		_invert = invert;
	//	}

	//	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		if (!(value is IEnumerable collection)) return false;

	//		var hasRequiredRule = collection.Cast<object>().Any(o => o.GetType().InheritsOrImplements(typeof(RequiredValueRule<>)));

	//		return _invert ? !hasRequiredRule : hasRequiredRule;
	//	}
	//	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
}
