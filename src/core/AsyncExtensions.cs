// perticula - core - AsyncExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class AsyncExtensions.
/// </summary>
public static class AsyncExtensions
{
	/// <summary>
	///   Fire off an async task, but don't wait for results.
	///   Used for things such as logging, where you only need one way communication.
	/// </summary>
	/// <param name="task">The task.</param>
	/// <returns>Task.</returns>
	public static Task FireAndForget(this Task task) => task.ContinueWith(t => t, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);

	/// <summary>
	///   Fire off an async task, but don't wait for results.
	///   Used for things such as logging, where you only need one way communication.
	/// </summary>
	/// <param name="task">The task.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>Task.</returns>
	public static Task FireAndForget(this Task task, CancellationToken cancellationToken) => task.ContinueWith(t => t, cancellationToken, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
}
