using System;
using System.Collections.Generic;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Extension methods for <see cref="IEnumerable&lt;T&gt;"/>
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Applies the action to each element in the list.
		/// </summary>
		/// <typeparam name="T">The enumerable item's type.</typeparam>
		/// <param name="enumerable">The elements to enumerate.</param>
		/// <param name="action">The action to apply to each item in the list.</param>
		public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
			if (action == null) throw new ArgumentNullException(nameof(action));

			foreach (var item in enumerable)
			{
				action(item);
			}
		}
	}
}
