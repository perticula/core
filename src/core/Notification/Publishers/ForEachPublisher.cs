// perticula - core - ForEachPublisher.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Notification.Publishers;

/// <summary>
///   Class ForEachPublisher.
///   Implements the <see cref="core.Notification.INotificationPublisher" />
/// </summary>
/// <seealso cref="core.Notification.INotificationPublisher" />
public class ForEachPublisher : INotificationPublisher
{
	/// <summary>
	///   publishes a notification using the specified executors.
	///   awaits for
	/// </summary>
	/// <param name="executors">The executor.</param>
	/// <param name="notification">The notification.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>Task.</returns>
	public async Task Publish(IEnumerable<NotificationHandlerExecutor> executors, INotification notification, CancellationToken cancellationToken)
	{
		foreach (var executor in executors)
		{
			await executor.Handle(notification, cancellationToken)
			              .ConfigureAwait(false);
		}
	}
}
