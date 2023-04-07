// perticula - core - ISimpleCache.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Caching;

/// <summary>
///   Interface ISimpleCache
///   Implements the <see cref="ICacheCommon{ISimpleCache}" />
/// </summary>
/// <seealso cref="ICacheCommon{ISimpleCache}" />
public interface ISimpleCache : ICacheCommon<ISimpleCache>
{
	/// <summary>
	///   References a cache of values by name. Useful to isolate one group of values
	///   from another. Note, the name used here will collide with other values using
	///   the same name
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>ISimpleCache.</returns>
	ISimpleCache ByName(string key);
}
