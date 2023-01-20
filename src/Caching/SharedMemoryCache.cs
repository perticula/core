using System.Runtime.Caching;

namespace core;

/// <summary>
///     Class SharedMemoryCache.
///     Implements the <see cref="ISharedCache" />
/// </summary>
/// <seealso cref="ISharedCache" />
internal class SharedMemoryCache : ISharedCache
{
  /// <summary>
  ///     The cache lock
  /// </summary>
  private readonly object _cacheLock = new();

  /// <summary>
  ///     The perm policy
  /// </summary>
  private readonly CacheItemPolicy _permPolicy;

  /// <summary>
  ///     Initializes a new instance of the <see cref="SharedMemoryCache" /> class.
  /// </summary>
  public SharedMemoryCache() => _permPolicy = new CacheItemPolicy
  {
    AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
    Priority = CacheItemPriority.NotRemovable
  };

  /// <summary>
  ///     Finds the or default.
  /// </summary>
  /// <typeparam name="TValue">The type of the t value.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="def">The definition.</param>
  /// <returns>TValue.</returns>
  public TValue FindOrDefault<TValue>(string key, TValue def)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock)
    {
      if (!MemoryCache.Default.Contains(key)) return def;
      return MemoryCache.Default.Get(key) is TValue ? (TValue)MemoryCache.Default.Get(key) : def;
    }
  }

  /// <summary>
  ///     Gets the item associated with this key if present.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>System.Object.</returns>
  public object Get(string key)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock) return MemoryCache.Default.Get(key);
  }

  /// <summary>
  ///     Gets the item associated with this key if present.
  ///     Note: An item can exist with the same key but of a different type.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>TValue.</returns>
  public TValue Get<TValue>(string key)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock)
    {
      var value = MemoryCache.Default.Get(key);
      if (value == null) throw new KeyNotFoundException($"Key {key} not found in cache");
      if (value is TValue) return (TValue)value;
      throw new InvalidCastException($"Key {key} is not of type {typeof(TValue)}");
    }
  }

  /// <summary>
  ///     Removes the object associated with the given key.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>ISharedCache.</returns>
  public ISharedCache Remove(string key)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock)
    {
      if (MemoryCache.Default.Contains(key))
      {
        MemoryCache.Default.Remove(key);
      }
    }

    return this;
  }

  /// <summary>
  ///     Determines whether the cache contains an item the specified key, and the item is not expired.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
  public bool Contains(string key)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock) return MemoryCache.Default.Contains(key);
  }

  /// <summary>
  ///     Flushes all values from the shared cache.
  /// </summary>
  /// <returns>ISharedCache.</returns>
  /// <exception cref="NotSupportedException">Shared Memory Caches Cannot Be Flushed</exception>
  public ISharedCache Flush() => throw new NotSupportedException("Shared Memory Caches Cannot Be Flushed");

  /// <summary>
  ///     Generates a cache key from a base key name and supplied argument list.
  /// </summary>
  /// <param name="baseKey">The base key.</param>
  /// <param name="args">The arguments.</param>
  /// <returns>the generated cache key</returns>
  public string Key(string baseKey, params object[] args)
  {
    if (baseKey == null) throw new ArgumentNullException(nameof(baseKey));
    if (string.IsNullOrWhiteSpace(baseKey)) throw new ArgumentNullException(nameof(baseKey));
    return args == null ? baseKey : baseKey + "-" + string.Join("-", args.Select(arg => arg ?? "NULL"));
  }

  /// <summary>
  ///     Returns the object with the specified key. If the object does not exist,
  ///     the value function will be used to insert the new object and return the evaluated
  ///     value
  /// </summary>
  /// <typeparam name="TValue">The type of the value object.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="value">The value.</param>
  /// <returns>TValue.</returns>
  public TValue FindOrSet<TValue>(string key, Func<string, TValue> value) => FindOrSet(key, -1, value);

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
  public TValue FindOrSet<TValue>(string key, int expires, Func<string, TValue> value)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (value == null) throw new ArgumentNullException(nameof(value));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock)
    {
      if (!MemoryCache.Default.Contains(key))
      {
        var set = value(key);
        if (set == null)
        {
          throw new ArgumentNullException(nameof(set));
        }
        MemoryCache.Default.Add(key, set, GetExpirePolicy(expires));
      }
      var val = MemoryCache.Default.Get(key);
      if (val == null) throw new KeyNotFoundException($"Key {key} not found in cache");
      if (val is TValue) return (TValue)val;
    }
    return FindOrSet(key, expires, value);
  }

  /// <summary>
  ///     Create or overwrite an entry in the cache.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <param name="value">The value.</param>
  /// <returns>ISharedCache.</returns>
  public ISharedCache Set<TValue>(string key, TValue value) => Set(key, value, 0);

  /// <summary>
  ///     Create or overwrite an entry in the cache.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <param name="value">The value.</param>
  /// <param name="expires">How long should this item stay in the cache? (in seconds)</param>
  /// <returns>ISharedCache.</returns>
  public ISharedCache Set<TValue>(string key, TValue value, int expires)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (value == null) throw new ArgumentNullException(nameof(value));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock)
    {
      if (MemoryCache.Default.Contains(key)) MemoryCache.Default.Remove(key);
      MemoryCache.Default.Add(key, value, GetExpirePolicy(expires));
    }

    return this;
  }

  /// <summary>
  ///     References a cache of values by name. Useful to isolate one group of values
  ///     from another. Note, the name used here will collide with other values using
  ///     the same name
  /// </summary>
  /// <param name="key">The key.</param>
  /// <param name="expires">How long should this item stay in the cache? (in seconds)</param>
  /// <returns>IConcurrentCache.</returns>
  public IConcurrentCache ByName(string key, int expires = -1)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
    key = NotCaseSensitive(key);
    lock (_cacheLock)
    {
      if (MemoryCache.Default.Contains(key))
      {
        var exists = MemoryCache.Default.Get(key) is IConcurrentCache cachedValue ? cachedValue : null;
        if (exists != null) return exists;
        MemoryCache.Default.Remove(key);
      }

      MemoryCache.Default.Add(key, new ConcurrentCache(), GetExpirePolicy(expires));
      return (IConcurrentCache)MemoryCache.Default.Get(key);
    }
  }

  /// <summary>
  ///     Clear the cache of all entries.
  /// </summary>
  public void Reset()
  {
    foreach (var item in MemoryCache.Default)
    {
      lock (_cacheLock)
        MemoryCache.Default.Remove(item.Key);
    }
  }

  /// <summary>
  ///     Ensures that the specified key is always uppercase invariant
  /// </summary>
  /// <param name="key">The key.</param>
  /// <returns>System.String.</returns>
  private static string NotCaseSensitive(string key)
  {
    if (key == null) throw new ArgumentNullException(nameof(key));
    return key.ToUpperInvariant();
  }

  /// <summary>
  ///     Returns the expiration policy to be used for the cached item
  /// </summary>
  /// <param name="expires">The expires.</param>
  /// <returns>CacheItemPolicy.</returns>
  private CacheItemPolicy GetExpirePolicy(int expires) => expires < 1
    ? _permPolicy
    : new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(expires) };
}
