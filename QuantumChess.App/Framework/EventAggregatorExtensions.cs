using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuantumChess.App.Framework
{
	/// <summary>
	/// Extensions for <see cref="IEventAggregator"/>.
	/// </summary>
	public static class EventAggregatorExtensions
	{
		/// <summary>
		/// Publishes a message on a background thread (async).
		/// </summary>
		/// <param name="eventAggregator">The event aggregator.</param>
		/// <param name="message">The message instance.</param>
		public static void PublishOnBackgroundThread(this IEventAggregator eventAggregator, object message)
		{
			if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));

			eventAggregator.Publish(message, action => Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default));
		}

		/// <summary>
		/// Publishes a message on the UI thread.
		/// </summary>
		/// <param name="eventAggregator">The event aggregator.</param>
		/// <param name="message">The message instance.</param>
		public static void PublishOnUiThread(this IEventAggregator eventAggregator, object message)
		{
			if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));

			eventAggregator.Publish(message, Execute.OnUiThread);
		}

		/// <summary>
		/// Publishes a message on the UI thread asynchrone.
		/// </summary>
		/// <param name="eventAggregator">The event aggregator.</param>
		/// <param name="message">The message instance.</param>
		public static Task BeginPublishOnUiThread(this IEventAggregator eventAggregator, object message)
		{
			if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));

			Task task = null;
			eventAggregator.Publish(message, action => task = action.BeginOnUiThread());
			return task;
		}
	}
}
