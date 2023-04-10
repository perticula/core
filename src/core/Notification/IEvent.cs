// perticula - model - IEvent.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Notification;

/// <summary>
///   Class IEvent.
///   Implements the <see cref="INotification" />
/// </summary>
/// <seealso cref="INotification" />
public abstract class NotificationEvent : INotification
{
	public string EventType => GetType().FullName!;

	/// <summary>
	///   Gets the time (UTC) this event was created.
	/// </summary>
	/// <value>The created at UTC.</value>
	public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;

	/// <summary>
	///   Gets the correlation identifier.
	/// </summary>
	/// <value>The correlation identifier.</value>
	public string CorrelationId { get; init; } = ShortGuid.NewGuid().ToString();

	/// <summary>
	///   Gets the meta data for this event.
	/// </summary>
	/// <value>The meta data.</value>
	public IDictionary<string, object> MetaData { get; init; } = new Dictionary<string, object>();

	/// <summary>
	///   Flattens this instance.
	/// </summary>
	public abstract void Flatten();
}
