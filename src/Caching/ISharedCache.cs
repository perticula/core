namespace core;

/// <summary>
///     Interface ISharedCache
///     Implements the <see cref="ICacheCommon{ISharedCache}" />
/// </summary>
/// <seealso cref="ICacheCommon{ISharedCache}" />
public interface ISharedCache : ICacheCommon<ISharedCache>
{
  /// <summary>
  ///     Returns the object with the specified key. If the object does not exist,
  ///     the value function will be used to insert the new object and return the evaluated
  ///     value
  /// </summary>
  /// <typeparam name="TValue">The type of the value object.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="expires">How long should this item stay in the cache? (in seconds)</param>
  /// <param name="value">The value.</param>
  /// <returns>TValue.</returns>
  TValue FindOrSet<TValue>(string key, int expires, Func<string, TValue> value);

  /// <summary>
  ///     Create or overwrite an entry in the cache.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <param name="value">The value.</param>
  /// <param name="expires">How long should this item stay in the cache? (in seconds)</param>
  /// <returns>ISharedCache.</returns>
  ISharedCache Set<TValue>(string key, TValue value, int expires);

  /// <summary>
  ///     References a cache of values by name. Useful to isolate one group of values
  ///     from another. Note, the name used here will collide with other values using
  ///     the same name
  /// </summary>
  /// <param name="key">The key.</param>
  /// <param name="expires">How long should this item stay in the cache? (in seconds)</param>
  /// <returns>ISimpleCache.</returns>
  IConcurrentCache ByName(string key, int expires = -1);

  /// <summary>
  ///     Clear the cache of all entries.
  /// </summary>
  void Reset();
}
