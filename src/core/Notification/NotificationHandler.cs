// perticula - core - NotificationHandler.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Notification;

/// <summary>
///   Wrapper class for a synchronous notification handler
/// </summary>
/// <typeparam name="TNotification">The notification type</typeparam>
public abstract class NotificationHandler<TNotification> : INotificationHandler<TNotification>
	where TNotification : INotification
{
	/// <summary>
	///   Handles the specified notification.
	/// </summary>
	/// <param name="notification">The notification.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>Task.</returns>
	Task INotificationHandler<TNotification>.Handle(TNotification notification, CancellationToken cancellationToken)
	{
		Handle(notification);
		return Task.CompletedTask;
	}

	/// <summary>
	///   Override in a derived class for the handler logic
	/// </summary>
	/// <param name="notification">Notification</param>
	protected abstract void Handle(TNotification notification);
}
