// perticula - core - SimpleCache.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Caching;

/// <summary>
///   Class SimpleCache.
///   Implements the <see cref="CacheCommonBase{SimpleCache}" />
///   Implements the <see cref="ISimpleCache" />
/// </summary>
/// <seealso cref="CacheCommonBase{SimpleCache}" />
/// <seealso cref="ISimpleCache" />
internal class SimpleCache : CacheCommonBase<SimpleCache>, ISimpleCache
{
	/// <summary>
	///   Initializes a new instance of the <see cref="SimpleCache" /> class.
	/// </summary>
	public SimpleCache() : base(false)
	{
	}

	/// <summary>
	///   Create or overwrite an entry in the cache.
	/// </summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="key">A string identifying the entry.</param>
	/// <param name="value">The value.</param>
	/// <returns>ISimpleCache.</returns>
	public new ISimpleCache Set<TValue>(string key, TValue value) => base.Set(key, value);

	/// <summary>
	///   Removes the object associated with the given key.
	/// </summary>
	/// <param name="key">A string identifying the entry.</param>
	/// <returns>ISimpleCache.</returns>
	public new ISimpleCache Remove(string key) => base.Remove(key);

	/// <summary>
	///   Flushes all values from the shared cache.
	/// </summary>
	/// <returns>ISimpleCache.</returns>
	public new ISimpleCache Flush() => base.Flush();

	/// <summary>
	///   Bies the name.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>ISimpleCache.</returns>
	/// <exception cref="System.ArgumentNullException">key</exception>
	public ISimpleCache ByName(string key)
	{
		if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
		var existing = Get<ISimpleCache>(key);
		if (existing != null) return existing;
		existing = new SimpleCache();
		Set(key, existing);
		return existing;
	}
}
