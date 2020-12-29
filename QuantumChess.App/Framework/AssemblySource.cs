using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// A source of assemblies that are inspectable by the framework.
	/// </summary>
	internal static class AssemblySource
	{
		/// <summary>
		/// The singleton instance of the AssemblySource used by the framework.
		/// </summary>
		public static readonly IObservableCollection<Assembly> Instance = new BindableCollection<Assembly>();

		/// <summary>
		/// Finds a type which matches one of the elements in the sequence of names.
		/// </summary>
		public static Func<IEnumerable<string>, Type> FindTypeByNames = names =>
			{
				var type = names?.Join(Instance.SelectMany(a => a.GetExportedTypes()),
				                       n => n,
				                       t => t.Name,
				                       (n, t) => t)
				                .FirstOrDefault();
				return type;
			};
	}
}
