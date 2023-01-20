using System.Collections.Concurrent;
using System.Diagnostics;

namespace core;

/// <summary>
/// Class CacheCommonBase.
/// Implements the <see cref="ICacheCommon{TChild}" />
/// </summary>
/// <typeparam name="TChild">The type of the t child.</typeparam>
/// <seealso cref="ICacheCommon{TChild}" />
[DebuggerDisplay("CacheItems: {_cache.Count,nq}")]
internal abstract class CacheCommonBase<TChild> : ICacheCommon<TChild>
    where TChild : CacheCommonBase<TChild>
{
  /// <summary>
  /// The cache
  /// </summary>
  private readonly IDictionary<string, object> _cache;

  /// <summary>
  /// Initializes a new instance of the <see cref="CacheCommonBase{TChild}"/> class.
  /// </summary>
  /// <param name="threadSafe">if set to <c>true</c> [thread safe].</param>
  internal CacheCommonBase(bool threadSafe) => _cache = threadSafe
                   ? (IDictionary<string, object>)new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                   : new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

  /// <summary>
  /// Determines whether the cache contains an item the specified key, and the item is not expired.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
  public virtual bool Contains(string key)
  {
    if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
    return _cache.ContainsKey(key);
  }

  /// <summary>
  /// Finds the or default.
  /// </summary>
  /// <typeparam name="TValue">The type of the t value.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="def">The definition.</param>
  /// <returns>TValue.</returns>
  public virtual TValue FindOrDefault<TValue>(string key, TValue def)
  {
    if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
    return _cache.ContainsKey(key) ? Get<TValue>(key) : def;
  }

  /// <summary>
  /// Returns the object with the specified key. If the object does not exist,
  /// the value function will be used to insert the new object and return the evaluated
  /// value
  /// </summary>
  /// <typeparam name="TValue">The type of the value object.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="value">The value.</param>
  /// <returns>TValue.</returns>
  /// <exception cref="ArgumentNullException">value</exception>
  public virtual TValue FindOrSet<TValue>(string key, Func<string, TValue> value)
  {
    if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
    if (value == null) throw new ArgumentNullException(nameof(value));
    if (_cache.ContainsKey(key))
      return Get<TValue>(key);

    var val = value(key);
    if (val != null)
      _cache.Add(key, val);

    return Get<TValue>(key);
  }

  /// <summary>
  /// Flushes all values from the shared cache.
  /// </summary>
  /// <returns>TChild.</returns>
  public virtual TChild Flush()
  {
    _cache.Clear();
    return (TChild)this;
  }

  /// <summary>
  /// Gets the item associated with this key if present.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>System.Object.</returns>
  public virtual object Get(string key)
  {
    if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
    return !_cache.ContainsKey(key) ? null : _cache[key];
  }

  /// <summary>
  /// Gets the item associated with this key if present.
  /// Note: An item can exist with the same key but of a different type.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>TValue.</returns>
  public virtual TValue Get<TValue>(string key)
  {
    if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
    if (!_cache.ContainsKey(key)) return default;
    return _cache[key] is TValue value ? value : default;
  }

  /// <summary>
  /// Generates a cache key from a base key name and supplied argument list.
  /// </summary>
  /// <param name="baseKey">The base key.</param>
  /// <param name="args">The arguments.</param>
  /// <returns>the generated cache key</returns>
  public virtual string Key(string baseKey, params object[] args)
  {
    if (string.IsNullOrEmpty(baseKey)) throw new ArgumentNullException(nameof(baseKey));
    return args == null
               ? baseKey
               : baseKey + "-" + string.Join("-", args.Select(arg => arg ?? "NULL"));
  }

  /// <summary>
  /// Removes the object associated with the given key.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>TChild.</returns>
  public virtual TChild Remove(string key)
  {
    if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
    if (_cache.ContainsKey(key))
      _cache.Remove(key);
    return (TChild)this;
  }

  /// <summary>
  /// Sets the specified key.
  /// </summary>
  /// <typeparam name="TValue">The type of the t value.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="value">The value.</param>
  /// <returns>TChild.</returns>
  public virtual TChild Set<TValue>(string key, TValue value)
  {
    if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
    if (value == null) throw new ArgumentNullException(nameof(value));
    if (_cache.ContainsKey(key))
      _cache[key] = value;
    else
      _cache.Add(key, value);
    return (TChild)this;
  }
}
