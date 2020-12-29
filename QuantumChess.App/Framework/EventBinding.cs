using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Allows binding events to ICommand properties.
	/// </summary>
	public class EventBinding : MarkupExtension
	{
		// ReSharper disable InconsistentNaming
		private static ICommand GetCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(CommandProperty);
		}
		private static void SetCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(CommandProperty, value);
		}
		private static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EventBinding), new PropertyMetadata(null));
		// ReSharper restore InconsistentNaming

		private class BindingTarget : FrameworkElement
		{
			// ReSharper disable once MemberHidesStaticFromOuterClass
			public static readonly DependencyProperty CommandProperty =
				DependencyProperty.Register("Command", typeof(ICommand), typeof(BindingTarget), new PropertyMetadata(null));

			public ICommand Command
			{
				get { return (ICommand)GetValue(CommandProperty); }
				set { SetValue(CommandProperty, value); }
			}
		}

		private class EventCommandHandler
		{
			private readonly ICommand _command;

			public EventCommandHandler(ICommand command)
			{
				_command = command ?? throw new ArgumentNullException(nameof(command), "This may have occurred because either the data context or the command property is null.");
			}

			public void Handle<T>(object sender, T e)
			{
				if (_command.CanExecute(e))
					_command.Execute(e);
			}
		}

		//
		// Summary:
		//     Gets or sets a value that determines the timing of binding source updates.
		//
		// Returns:
		//     One of the System.Windows.Data.UpdateSourceTrigger values. The default is System.Windows.Data.UpdateSourceTrigger.Default,
		//     which returns the default System.Windows.Data.UpdateSourceTrigger value of the
		//     target dependency property. However, the default value for most dependency properties
		//     is System.Windows.Data.UpdateSourceTrigger.PropertyChanged, while the System.Windows.Controls.TextBox.Text
		//     property has a default value of System.Windows.Data.UpdateSourceTrigger.LostFocus.A
		//     programmatic way to determine the default System.Windows.Data.Binding.UpdateSourceTrigger
		//     value of a dependency property is to get the property metadata of the property
		//     using System.Windows.DependencyProperty.GetMetadata(System.Type) and then check
		//     the value of the System.Windows.FrameworkPropertyMetadata.DefaultUpdateSourceTrigger
		//     property.
		[DefaultValue(UpdateSourceTrigger.Default)]
		public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether to raise the System.Windows.Data.Binding.SourceUpdated
		//     event when a value is transferred from the binding target to the binding source.
		//
		// Returns:
		//     true if the System.Windows.Data.Binding.SourceUpdated event should be raised
		//     when the binding source value is updated; otherwise, false. The default is false.
		[DefaultValue(false)]
		public bool NotifyOnSourceUpdated { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether to raise the System.Windows.Data.Binding.TargetUpdated
		//     event when a value is transferred from the binding source to the binding target.
		//
		// Returns:
		//     true if the System.Windows.Data.Binding.TargetUpdated event should be raised
		//     when the binding target value is updated; otherwise, false. The default is false.
		[DefaultValue(false)]
		public bool NotifyOnTargetUpdated { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether to raise the System.Windows.Controls.Validation.Error
		//     attached event on the bound object.
		//
		// Returns:
		//     true if the System.Windows.Controls.Validation.Error attached event should be
		//     raised on the bound object when there is a validation error during source updates;
		//     otherwise, false. The default is false.
		[DefaultValue(false)]
		public bool NotifyOnValidationError { get; set; }
		//
		// Summary:
		//     Gets or sets the converter to use.
		//
		// Returns:
		//     A value of type System.Windows.Data.IValueConverter. The default is null.
		[DefaultValue(null)]
		public IValueConverter Converter { get; set; }
		//
		// Summary:
		//     Gets or sets the parameter to pass to the System.Windows.Data.Binding.Converter.
		//
		// Returns:
		//     The parameter to pass to the System.Windows.Data.Binding.Converter. The default
		//     is null.
		[DefaultValue(null)]
		public object ConverterParameter { get; set; }
		//
		// Summary:
		//     Gets or sets the culture in which to evaluate the converter.
		//
		// Returns:
		//     The default is null.
		[DefaultValue(null)]
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo ConverterCulture { get; set; }
		//
		// Summary:
		//     Gets or sets the object to use as the binding source.
		//
		// Returns:
		//     The object to use as the binding source.
		public object Source { get; set; }
		//
		// Summary:
		//     Gets or sets the binding source by specifying its location relative to the position
		//     of the binding target.
		//
		// Returns:
		//     A System.Windows.Data.RelativeSource object specifying the relative location
		//     of the binding source to use. The default is null.
		[DefaultValue(null)]
		public RelativeSource RelativeSource { get; set; }
		//
		// Summary:
		//     Gets or sets the name of the element to use as the binding source object.
		//
		// Returns:
		//     The value of the Name property or x:Name Directive of the element of interest.
		//     You can refer to elements in code only if they are registered to the appropriate
		//     System.Windows.NameScope through RegisterName. For more information, see WPF
		//     XAML Namescopes.The default is null.
		[DefaultValue(null)]
		public string ElementName { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether the System.Windows.Data.Binding should
		//     get and set values asynchronously.
		//
		// Returns:
		//     The default is null.
		[DefaultValue(false)]
		public bool IsAsync { get; set; }
		//
		// Summary:
		//     Gets or sets opaque data passed to the asynchronous data dispatcher.
		//
		// Returns:
		//     Data passed to the asynchronous data dispatcher.
		[DefaultValue(null)]
		public object AsyncState { get; set; }
		//
		// Summary:
		//     Gets or sets an XPath query that returns the value on the XML binding source
		//     to use.
		//
		// Returns:
		//     The XPath query. The default is null.
		[DefaultValue(null)]
		public string XPath { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether to include the System.Windows.Controls.DataErrorValidationRule.
		//
		// Returns:
		//     true to include the System.Windows.Controls.DataErrorValidationRule; otherwise,
		//     false.
		[DefaultValue(false)]
		public bool ValidatesOnDataErrors { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether to include the System.Windows.Controls.NotifyDataErrorValidationRule.
		//
		// Returns:
		//     true to include the System.Windows.Controls.NotifyDataErrorValidationRule; otherwise,
		//     false. The default is true.
		[DefaultValue(true)]
		public bool ValidatesOnNotifyDataErrors { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether to evaluate the System.Windows.Data.Binding.Path
		//     relative to the data item or the System.Windows.Data.DataSourceProvider object.
		//
		// Returns:
		//     false to evaluate the path relative to the data item itself; otherwise, true.
		//     The default is false.
		[DefaultValue(false)]
		public bool BindsDirectlyToSource { get; set; }
		//
		// Summary:
		//     Gets or sets a value that indicates whether to include the System.Windows.Controls.ExceptionValidationRule.
		//
		// Returns:
		//     true to include the System.Windows.Controls.ExceptionValidationRule; otherwise,
		//     false.
		[DefaultValue(false)]
		public bool ValidatesOnExceptions { get; set; }
		//
		// Summary:
		//     Gets a collection of rules that check the validity of the user input.
		//
		// Returns:
		//     A collection of System.Windows.Controls.ValidationRule objects.
		public Collection<ValidationRule> ValidationRules { get; }
		//
		// Summary:
		//     Gets or sets the path to the binding source property.
		//
		// Returns:
		//     The path to the binding source. The default is null.
		public PropertyPath Path { get; set; }
		//
		// Summary:
		//     Gets or sets a handler you can use to provide custom logic for handling exceptions
		//     that the binding engine encounters during the update of the binding source value.
		//     This is only applicable if you have associated an System.Windows.Controls.ExceptionValidationRule
		//     with your binding.
		//
		// Returns:
		//     A method that provides custom logic for handling exceptions that the binding
		//     engine encounters during the update of the binding source value.
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="EventBinding"/> class.
		/// </summary>
		public EventBinding() { }
		/// <summary>
		/// Creates a new instance of the <see cref="EventBinding"/> class.
		/// </summary>
		/// <param name="path"></param>
		public EventBinding(string path)
		{
			Path = new PropertyPath(path);
		}

		/// <summary>When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension. </summary>
		/// <returns>The object value to set on the property where the extension is applied. </returns>
		/// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var provider = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));

			if (!(provider.TargetObject is FrameworkElement target))
				throw new ArgumentException("Event bindings can only be set on types derived from FrameworkElement.");

			var handledEvent = provider.TargetProperty as EventInfo;
			Type eventType;
			if (handledEvent == null)
			{
				// Some events are handled via Add*Handler methods on other classes.
				// Examples of this are MouseEnter (Mouse.AddMouseEnterHandler) and LostFocus (FocusManager.AddLostFocusHandler).
				// In these cases, the event handler type is the second parameter of that method.
				var handledMethod = provider.TargetProperty as MethodInfo;
				if (handledMethod == null)
					throw new ArgumentException("Event bindings can only be set on events.");
				eventType = handledMethod.GetParameters()
				                         .Last()
				                         .ParameterType;
			}
			else
				eventType = handledEvent.EventHandlerType;
			var eventArgsType = eventType.GetMethod("Invoke")
			                             .GetParameters()
			                             .Last()
			                             .ParameterType;

			var tempTarget = new BindingTarget();
			var useTarget = false;

			var binding = new Binding();
			if (RelativeSource != null)
			{
				binding.RelativeSource = RelativeSource;
				useTarget = true;
			}
			else if (Source != null)
			{
				binding.Source = Source;
				useTarget = true;
			}
			else if (ElementName != null)
			{
				binding.ElementName = ElementName;
				useTarget = true;
			}
			else
				tempTarget.DataContext = target.DataContext;
			binding.UpdateSourceTrigger = UpdateSourceTrigger;
			binding.NotifyOnSourceUpdated = NotifyOnSourceUpdated;
			binding.NotifyOnTargetUpdated = NotifyOnTargetUpdated;
			binding.NotifyOnValidationError = NotifyOnValidationError;
			binding.Converter = Converter;
			binding.ConverterParameter = ConverterParameter;
			binding.ConverterCulture = ConverterCulture;
			binding.IsAsync = IsAsync;
			binding.AsyncState = AsyncState;
			binding.Mode = BindingMode.OneWay;
			binding.XPath = XPath;
			binding.ValidatesOnDataErrors = ValidatesOnDataErrors;
			binding.ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors;
			binding.BindsDirectlyToSource = BindsDirectlyToSource;
			binding.ValidatesOnExceptions = ValidatesOnExceptions;
			binding.Path = Path;
			binding.UpdateSourceExceptionFilter = UpdateSourceExceptionFilter;
			if (ValidationRules != null)
			{
				foreach (var validationRule in ValidationRules)
				{
					binding.ValidationRules.Add(validationRule);
				}
			}
			ICommand command;
			if (useTarget)
			{
				BindingOperations.SetBinding(target, CommandProperty, binding);
				command = GetCommand(target);
			}
			else
			{
				BindingOperations.SetBinding(tempTarget, BindingTarget.CommandProperty, binding);
				command = tempTarget.Command;
			}

			var handler = new EventCommandHandler(command);

			// TODO: add "update" logic for if/when the binding updates
			var handlerMethod = typeof(EventCommandHandler).GetMethod(nameof(handler.Handle))
			                                               .MakeGenericMethod(eventArgsType);

			return Delegate.CreateDelegate(eventType, handler, handlerMethod);
		}
	}
}
