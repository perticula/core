namespace core;

/// <summary>
/// Class ConcurrentCache.
/// Implements the <see cref="Internal.CacheCommonBase{Internal.ConcurrentCache}" />
/// Implements the <see cref="IConcurrentCache" />
/// </summary>
/// <seealso cref="Internal.CacheCommonBase{Internal.ConcurrentCache}" />
/// <seealso cref="IConcurrentCache" />
internal class ConcurrentCache : CacheCommonBase<ConcurrentCache>, IConcurrentCache
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ConcurrentCache"/> class.
  /// </summary>
  public ConcurrentCache() : base(true) { }

  /// <summary>
  /// Create or overwrite an entry in the cache.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <param name="value">The value.</param>
  /// <returns>IConcurrentCache.</returns>
  public new IConcurrentCache Set<TValue>(string key, TValue value) => base.Set(key, value);

  /// <summary>
  /// Removes the object associated with the given key.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>IConcurrentCache.</returns>
  public new IConcurrentCache Remove(string key) => base.Remove(key);

  /// <summary>
  /// Flushes all values from the shared cache.
  /// </summary>
  /// <returns>IConcurrentCache.</returns>
  public new IConcurrentCache Flush() => base.Flush();

  /// <summary>
  /// Bies the name.
  /// </summary>
  /// <param name="key">The key.</param>
  /// <returns>IConcurrentCache.</returns>
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
