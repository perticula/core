// perticula - core - TestableCache.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Caching;

/// <summary>
///   Class TestableCache.
///   Implements the <see cref="ISimpleCache" />
/// </summary>
/// <seealso cref="ISimpleCache" />
[Obsolete("Used ony in unit tests, otherwise prefer the injected ISimpleCache class")]
public class TestableCache : ISimpleCache
{
  /// <summary>
  ///   The internal
  /// </summary>
  private readonly ISimpleCache _internal = new SimpleCache();

  /// <summary>
  ///   Returns the object with the specified key. If the object does not exist,
  ///   the value function will be used to insert the new object and return the evaluated
  ///   value
  /// </summary>
  /// <typeparam name="TValue">The type of the value object.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="value">The value.</param>
  /// <returns>TValue.</returns>
  public TValue FindOrSet<TValue>(string key, Func<string, TValue> value) => _internal.FindOrSet(key, value);

  /// <summary>
  ///   Returns the object with the specified key.
  ///   if the object does not exist, a default value will be returned
  /// </summary>
  /// <typeparam name="TValue">The type of the value object.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="default">The default value to return</param>
  /// <returns>TValue.</returns>
  public TValue FindOrDefault<TValue>(string key, TValue @default) => _internal.FindOrDefault(key, @default);

  /// <summary>
  ///   Gets the item associated with this key if present.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>System.Object.</returns>
  /// <exception cref="System.Collections.Generic.KeyNotFoundException">key</exception>
  public object Get(string key) => _internal.Get(key) ?? throw new KeyNotFoundException(nameof(key));

  /// <summary>
  ///   Gets the item associated with this key if present.
  ///   Note: An item can exist with the same key but of a different type.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>TValue.</returns>
  /// <exception cref="System.Collections.Generic.KeyNotFoundException">key</exception>
  public TValue Get<TValue>(string key) => _internal.Get<TValue>(key) ?? throw new KeyNotFoundException(nameof(key));

  /// <summary>
  ///   Create or overwrite an entry in the cache.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <param name="value">The value.</param>
  /// <returns>TChild.</returns>
  public ISimpleCache Set<TValue>(string key, TValue value) => _internal.Set(key, value);

  /// <summary>
  ///   Removes the object associated with the given key.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>TChild.</returns>
  public ISimpleCache Remove(string key) => _internal.Remove(key);

  /// <summary>
  ///   Determines whether the cache contains an item the specified key, and the item is not expired.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
  public bool Contains(string key) => _internal.Contains(key);

  /// <summary>
  ///   Flushes all values from the shared cache.
  /// </summary>
  /// <returns>TChild.</returns>
  public ISimpleCache Flush() => _internal.Flush();

  /// <summary>
  ///   Generates a cache key from a base key name and supplied argument list.
  /// </summary>
  /// <param name="baseKey">The base key.</param>
  /// <param name="args">The arguments.</param>
  /// <returns>the generated cache key</returns>
  public string Key(string baseKey, params object[] args) => _internal.Key(baseKey, args);

  /// <summary>
  ///   References a cache of values by name. Useful to isolate one group of values
  ///   from another. Note, the name used here will collide with other values using
  ///   the same name
  /// </summary>
  /// <param name="key">The key.</param>
  /// <returns>ISimpleCache.</returns>
  public ISimpleCache ByName(string key) => _internal.ByName(key);
}
