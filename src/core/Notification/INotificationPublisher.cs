// perticula - core - INotificationPublisher.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Notification;

/// <summary>
///   Interface INotificationPublisher
/// </summary>
public interface INotificationPublisher
{
	/// <summary>
	///   Publishes the notification using the specified executor.
	/// </summary>
	/// <param name="executors">The executor.</param>
	/// <param name="notification">The notification.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>Task.</returns>
	Task Publish(IEnumerable<NotificationHandlerExecutor> executors, INotification notification, CancellationToken cancellationToken);
}
