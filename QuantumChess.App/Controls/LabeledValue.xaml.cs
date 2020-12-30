using System.Windows;
using System.Windows.Controls;

namespace QuantumChess.App.Controls
{
	/// <summary>
	/// Interaction logic for LabeledValue.xaml
	/// </summary>
	public partial class LabeledValue : UserControl
	{
		public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
			"Label", typeof(string), typeof(LabeledValue), new PropertyMetadata(default(string)));

		public string Label
		{
			get { return (string) GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			"Value", typeof(object), typeof(LabeledValue), new PropertyMetadata(default(object)));

		public object Value
		{
			get { return (object) GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public LabeledValue()
		{
			InitializeComponent();
		}
	}
}
