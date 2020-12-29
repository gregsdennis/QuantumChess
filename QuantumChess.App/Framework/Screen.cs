using System;
using System.Windows.Input;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// A base implementation of <see cref = "IScreen" />.
	/// </summary>
	public abstract class Screen : InitializableViewModel, IScreen, IChild
	{
		private bool _isActive;
		private object _parent;
		private bool _hasBeenActivated;

		public Screen()
		{
			Close = new RelayCommand(TryClose, () =>
			{
				var shouldClose = true;
				CanClose(b => shouldClose = b);

				return shouldClose;
			});
		}

		/// <summary>
		/// Gets or Sets the Parent <see cref = "IConductor" />
		/// </summary>
		public object Parent
		{
			get { return _parent; }
			set
			{
				_parent = value;
				NotifyOfPropertyChange();
			}
		}

		/// <summary>
		/// Indicates whether or not this instance is currently active.
		/// Virtualized in order to help with document oriented view models.
		/// </summary>
		public bool IsActive
		{
			get { return _isActive; }
			private set
			{
				_isActive = value;
				NotifyOfPropertyChange();
			}
		}

		public ICommand Close { get; }

		/// <summary>
		/// Raised after activation occurs.
		/// </summary>
		public event EventHandler<ActivationEventArgs> Activated = delegate { };

		/// <summary>
		/// Raised before deactivation.
		/// </summary>
		public event EventHandler<AttemptingDeactivationEventArgs> AttemptingDeactivation = delegate { };

		/// <summary>
		/// Raised after deactivation.
		/// </summary>
		public event EventHandler<DeactivationEventArgs> Deactivated = delegate { };

		/// <summary>
		/// Raised after initialization, just before the first activation.
		/// </summary>
		public event EventHandler FirstActivation = delegate { };

		void IActivate.Activate()
		{
			if (IsActive) return;

			var firstActivation = false;

			if (!_hasBeenActivated)
			{
				_hasBeenActivated = firstActivation = true;

				OnFirstActivation();
			}

			IsActive = true;
			OnActivate(firstActivation);
		}

		void IDeactivate.Deactivate(bool close)
		{
			if (close)
			{
				var shouldClose = true;
				CanClose(b => shouldClose = b);

				if (!shouldClose) return;
			}

			if (!IsActive) return;

			if (IsInitialized && close)
			{
				IsActive = false;
				OnDeactivate(close);
			}
		}

		/// <summary>
		/// Called to check whether or not this instance can close.
		/// </summary>
		/// <param name = "callback">The implementor calls this action with the result of the close check.</param>
		public virtual void CanClose(Action<bool> callback)
		{
			callback(OnAttemptingDeactivation());
		}

		/// <summary>
		/// Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to close.
		/// Also provides an opportunity to pass a dialog result to it's corresponding view.
		/// </summary>
		public virtual void TryClose()
		{
			var conductor = Parent as IConductor;
			if (conductor != null)
			{
				conductor.DeactivateItem(this, true);
				return;
			}

			var deactivate = (IDeactivate) this;
			deactivate.Deactivate(true);
		}

		/// <summary>
		/// Called when initializing.
		/// </summary>
		protected virtual void OnFirstActivation()
		{
			//this.Log().Info("Activating first time {0}.", this);
			RaiseEvent(FirstActivation);
		}

		/// <summary>
		/// Called when activating.
		/// </summary>
		protected virtual void OnActivate(bool firstActivation)
		{
			//this.Log().Info("Activating {0}.", this);
			RaiseEvent(Activated, new ActivationEventArgs {FirstActivation = firstActivation});
		}

		/// <summary>
		/// Called when attempting deactivating.
		/// </summary>
		/// <returns>true if deactivation may proceed; false otherwise.</returns>
		protected virtual bool OnAttemptingDeactivation()
		{
			var args = new AttemptingDeactivationEventArgs();
			RaiseEvent(AttemptingDeactivation, args);

			return !args.Cancel;
		}

		/// <summary>
		/// Called when deactivating.
		/// </summary>
		/// <param name = "close">Inidicates whether this instance will be closed.</param>
		protected virtual void OnDeactivate(bool close)
		{
			//this.Log().Info("Deactivating {0}.", this);
			RaiseEvent(Deactivated, new DeactivationEventArgs {WasClosed = close});
			//if (close)
				//this.Log().Info("Closed {0}.", this);
		}
	}
}