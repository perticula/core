// perticula - core - ConcurrentCache.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Caching;

/// <summary>
///   Class ConcurrentCache.
///   Implements the <see cref="CacheCommonBase{ConcurrentCache}" />
///   Implements the <see cref="IConcurrentCache" />
/// </summary>
/// <seealso cref="CacheCommonBase{ConcurrentCache}" />
/// <seealso cref="IConcurrentCache" />
internal class ConcurrentCache : CacheCommonBase<ConcurrentCache>, IConcurrentCache
{
	/// <summary>
	///   Initializes a new instance of the <see cref="ConcurrentCache" /> class.
	/// </summary>
	public ConcurrentCache() : base(true) { }

	/// <summary>
	///   Create or overwrite an entry in the cache.
	/// </summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="key">A string identifying the entry.</param>
	/// <param name="value">The value.</param>
	/// <returns>IConcurrentCache.</returns>
	public new IConcurrentCache Set<TValue>(string key, TValue value) => base.Set(key, value);

	/// <summary>
	///   Removes the object associated with the given key.
	/// </summary>
	/// <param name="key">A string identifying the entry.</param>
	/// <returns>IConcurrentCache.</returns>
	public new IConcurrentCache Remove(string key) => base.Remove(key);

	/// <summary>
	///   Flushes all values from the shared cache.
	/// </summary>
	/// <returns>IConcurrentCache.</returns>
	public new IConcurrentCache Flush() => base.Flush();

	/// <summary>
	///   Returns a cached instance by the given key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>IConcurrentCache.</returns>
	/// <exception cref="System.ArgumentNullException">key</exception>
	public IConcurrentCache ByName(string key)
	{
		if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
		var existing = Get<IConcurrentCache>(key);
		if (existing != null) return existing;
		existing = new ConcurrentCache();
		Set(key, existing);
		return existing;
	}
}
