﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace QuantumChess.App.Framework
{
	public abstract partial class Conductor<T>
	{
		/// <summary>
		/// An implementation of <see cref="IConductor"/> that holds on many items.
		/// </summary>
		public abstract partial class Collection
		{
			/// <summary>
			/// An implementation of <see cref="IConductor"/> that holds on to many items wich are all activated.
			/// </summary>
			public abstract class AllActive : ConductorBase<T>
			{
				private readonly BindableCollection<T> _items = new BindableCollection<T>();
				private readonly bool _openPublicItems;

				/// <summary>
				/// Gets the items that are currently being conducted.
				/// </summary>
				public IObservableCollection<T> Items => _items;

				/// <summary>
				/// Initializes a new instance of the <see cref="Conductor&lt;T&gt;.Collection.AllActive"/> class.
				/// </summary>
				/// <param name="openPublicItems">if set to <c>true</c> opens public items that are properties of this class.</param>
				protected AllActive(bool openPublicItems)
					: this()
				{
					_openPublicItems = openPublicItems;
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="Conductor&lt;T&gt;.Collection.AllActive"/> class.
				/// </summary>
				public AllActive()
				{
					_items.CollectionChanged += (s, e) =>
						{
							switch (e.Action)
							{
								case NotifyCollectionChangedAction.Add:
									e.NewItems.OfType<IChild>().Apply(x => x.Parent = this);
									break;
								case NotifyCollectionChangedAction.Remove:
									e.OldItems.OfType<IChild>().Apply(x => x.Parent = null);
									break;
								case NotifyCollectionChangedAction.Replace:
									e.NewItems.OfType<IChild>().Apply(x => x.Parent = this);
									e.OldItems.OfType<IChild>().Apply(x => x.Parent = null);
									break;
								case NotifyCollectionChangedAction.Reset:
									_items.OfType<IChild>().Apply(x => x.Parent = this);
									break;
							}
						};
				}

				/// <summary>
				/// Called to check whether or not this instance can close.
				/// </summary>
				/// <param name="callback">The implementor calls this action with the result of the close check.</param>
				public override void CanClose(Action<bool> callback)
				{
					CloseStrategy.Execute(_items.ToList(), (canClose, closable) =>
						                      {
							                      if (!canClose && closable.Any())
							                      {
								                      closable.OfType<IDeactivate>().Apply(x => x.Deactivate(true));
								                      _items.RemoveRange(closable);
							                      }

							                      callback(canClose);
						                      });
				}

				/// <summary>
				/// Activates the specified item.
				/// </summary>
				/// <param name="item">The item to activate.</param>
				public override void ActivateItem(T item)
				{
					if (item == null) return;

					item = EnsureItem(item);

					if (IsActive)
						ScreenExtensions.TryActivate(item);

					OnActivationProcessed(item, true);
				}

				/// <summary>
				/// Deactivates the specified item.
				/// </summary>
				/// <param name="item">The item to close.</param>
				/// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
				public override void DeactivateItem(T item, bool close)
				{
					if (item == null) return;

					if (close)
						CloseStrategy.Execute(new[] {item}, (canClose, closable) =>
							                      {
								                      if (canClose)
									                      CloseItemCore(item);
							                      });
					else
						ScreenExtensions.TryDeactivate(item, false);
				}

				/// <summary>
				/// Gets the children.
				/// </summary>
				/// <returns>The collection of children.</returns>
				public override IEnumerable<T> GetChildren()
				{
					return _items;
				}

				/// <summary>
				/// Called when activating.
				/// </summary>
				protected override void OnActivate(bool initialized)
				{
					_items.OfType<IActivate>().Apply(x => x.Activate());
				}

				/// <summary>
				/// Called when deactivating.
				/// </summary>
				/// <param name="close">Inidicates whether this instance will be closed.</param>
				protected override void OnDeactivate(bool close)
				{
					_items.OfType<IDeactivate>().Apply(x => x.Deactivate(close));
					if (close)
						_items.Clear();
					if (!IsActive)
						base.OnDeactivate(close);
				}

				/// <summary>
				/// Called when initializing.
				/// </summary>
				protected override void OnFirstActivation()
				{
					if (_openPublicItems)
						GetType().GetProperties()
								 .Where(x => x.Name != "Parent" && typeof(T).IsAssignableFrom(x.PropertyType))
								 .Select(x => x.GetValue(this, null))
								 .Cast<T>()
								 .Apply(ActivateItem);
				}

				/// <summary>
				/// Ensures that an item is ready to be activated.
				/// </summary>
				/// <param name="newItem">The item that is about to be activated.</param>
				/// <returns>The item to be activated.</returns>
				protected override T EnsureItem(T newItem)
				{
					var index = _items.IndexOf(newItem);

					if (index == -1)
						_items.Add(newItem);
					else
						newItem = _items[index];

					return base.EnsureItem(newItem);
				}

				private void CloseItemCore(T item)
				{
					ScreenExtensions.TryDeactivate(item, true);
					_items.Remove(item);
				}
			}
		}
	}
}
