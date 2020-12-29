using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace QuantumChess.App.Framework
{
	/// <summary>
	///   A strategy for determining which view to use for a given model.
	/// </summary>
	internal static class ViewLocator
	{
		/// <summary>
		///   Locates the view for the specified model instance.
		/// </summary>
		/// <returns>The view.</returns>
		/// <remarks>
		///   Pass the model instance, display location (or null) and the context (or null) as parameters and receive a view instance.
		/// </remarks>
		public static readonly Func<object, DependencyObject, UIElement> LocateForModel =
			(model, displayLocation) => LocateForModelType(model.GetType(), displayLocation);

		/// <summary>
		///   Retrieves the view from the IoC container or tries to create it if not found.
		/// </summary>
		/// <remarks>
		///   Pass the type of view as a parameter and recieve an instance of the view.
		/// </remarks>
		private static readonly Func<Type, UIElement> GetOrCreateViewType =
			viewType =>
				{
					if (viewType.IsInterface || viewType.IsAbstract || !typeof(UIElement).IsAssignableFrom(viewType))
						return new TextBlock {Text = $"Cannot create {viewType.FullName}."};
					try
					{
						var view = (UIElement) Activator.CreateInstance(viewType);

						InitializeComponent(view);

						return view;
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						return null;
					}
				};

		/// <summary>
		/// Modifies the name of the type to be used at design time.
		/// </summary>
		private static readonly Func<string, string> ModifyModelTypeAtDesignTime =
			modelTypeName =>
				{
					if (modelTypeName.StartsWith("_"))
					{
						var index = modelTypeName.IndexOf('.');
						modelTypeName = modelTypeName.Substring(index + 1);
						index = modelTypeName.IndexOf('.');
						modelTypeName = modelTypeName.Substring(index + 1);
					}

					return modelTypeName;
				};

		/// <summary>
		///   Locates the view type based on the specified model type.
		/// </summary>
		/// <returns>The view.</returns>
		/// <remarks>
		///   Pass the model type, display location (or null) and the context instance (or null) as parameters and receive a view type.
		/// </remarks>
		private static readonly Func<Type, DependencyObject, Type> LocateTypeForModelType =
			(modelType, displayLocation) =>
				{
					var viewTypeName = modelType.Name;

					if (Execute.InDesignMode)
						viewTypeName = ModifyModelTypeAtDesignTime(viewTypeName);

					viewTypeName = viewTypeName.Substring(0, viewTypeName.IndexOf('`') < 0 ? viewTypeName.Length : viewTypeName.IndexOf('`'))
					                           .Replace("Model", string.Empty);

					var viewType = AssemblySource.FindTypeByNames(new[] {viewTypeName});

					//if (viewType == null)
					//	typeof(ViewLocator).Log().Warn($"View not found. Searched: {viewTypeName}.");

					return viewType;
				};

		/// <summary>
		///   Locates the view for the specified model type.
		/// </summary>
		/// <returns>The view.</returns>
		/// <remarks>
		///   Pass the model type, display location (or null) and the context instance (or null) as parameters and receive a view instance.
		/// </remarks>
		private static readonly Func<Type, DependencyObject, UIElement> LocateForModelType =
			(modelType, displayLocation) =>
				{
					var viewType = LocateTypeForModelType(modelType, displayLocation);

					return viewType == null
						       ? new TextBlock {Text = $"Cannot find view for {modelType}."}
						       : GetOrCreateViewType(viewType);
				};

		/// <summary>
		///   When a view does not contain a code-behind file, we need to automatically call InitializeCompoent.
		/// </summary>
		/// <param name = "element">The element to initialize</param>
		private static void InitializeComponent(object element)
		{
			var method = element.GetType()
			                    .GetMethod("InitializeComponent", BindingFlags.Public | BindingFlags.Instance);

			if (method == null) return;

			method.Invoke(element, null);
		}
	}
}
