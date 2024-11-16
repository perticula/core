// perticula - model - ICacheService.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model.Service;

/// <summary>
///   Interface ICacheService
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICacheService<T>
{
	/// <summary>
	///   Gets the value of the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>T.</returns>
	T Get(string key);

	/// <summary>
	///   Gets the value of the specified key (asynchronous).
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="token">
	///   The cancellation token that can be used by other objects or threads to receive notice of
	///   cancellation.
	/// </param>
	/// <returns>Task&lt;T&gt;.</returns>
	Task<T> GetAsync(string key, CancellationToken token = default);

	/// <summary>
	///   Refreshes the expiration for a key/value pair
	/// </summary>
	/// <param name="key">The key.</param>
	void Refresh(string key);

	/// <summary>
	///   Refreshes the expiration for a key/value pair (asynchronous).
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="token">
	///   The cancellation token that can be used by other objects or threads to receive notice of
	///   cancellation.
	/// </param>
	/// <returns>Task.</returns>
	Task RefreshAsync(string key, CancellationToken token = default);

	/// <summary>
	///   Removes a cache item.
	/// </summary>
	/// <param name="key">The key.</param>
	void Remove(string key);

	/// <summary>
	///   Removes a cache item (asynchronous).
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="token">
	///   The cancellation token that can be used by other objects or threads to receive notice of
	///   cancellation.
	/// </param>
	/// <returns>Task.</returns>
	Task RemoveAsync(string key, CancellationToken token = default);

	/// <summary>
	///   Sets the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	/// <param name="slidingExpiration">The sliding expiration.</param>
	void Set(string key, T value, TimeSpan? slidingExpiration = null);

	/// <summary>
	///   Sets the specified key/value pair (asynchronous).
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	/// <param name="slidingExpiration">The sliding expiration.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>Task.</returns>
	Task SetAsync(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);
}
