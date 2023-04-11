// perticula - core - INotificationHandler.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Notification;

/// <summary>
///   Interface INotificationHandler
///   defines a habdler for a notification.
/// </summary>
/// <typeparam name="TNotification">The type of the t notification.</typeparam>
public interface INotificationHandler<in TNotification>
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
	Task Handle(TNotification notification, CancellationToken cancellationToken);
}
