// perticula - core - ICacheCommon.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Caching;

/// <summary>
///   Interface ICacheCommon
/// </summary>
/// <typeparam name="TChild">The type of the t child.</typeparam>
public interface ICacheCommon<out TChild>
{
  /// <summary>
  ///   Returns the object with the specified key. If the object does not exist,
  ///   the value function will be used to insert the new object and return the evaluated
  ///   value
  /// </summary>
  /// <typeparam name="TValue">The type of the value object.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="value">The value.</param>
  /// <returns>TValue.</returns>
  TValue FindOrSet<TValue>(string key, Func<string, TValue> value);

  /// <summary>
  ///   Returns the object with the specified key.
  ///   if the object does not exist, a default value will be returned
  /// </summary>
  /// <typeparam name="TValue">The type of the value object.</typeparam>
  /// <param name="key">The key.</param>
  /// <param name="default">The default value to return</param>
  /// <returns>TValue.</returns>
  TValue FindOrDefault<TValue>(string key, TValue @default);

  /// <summary>
  ///   Gets the item associated with this key if present.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>System.Object.</returns>
  object? Get(string key);

  /// <summary>
  ///   Gets the item associated with this key if present.
  ///   Note: An item can exist with the same key but of a different type.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>TValue.</returns>
  TValue? Get<TValue>(string key);

  /// <summary>
  ///   Create or overwrite an entry in the cache.
  /// </summary>
  /// <typeparam name="TValue">The type of the value.</typeparam>
  /// <param name="key">A string identifying the entry.</param>
  /// <param name="value">The value.</param>
  /// <returns>TChild.</returns>
  TChild Set<TValue>(string key, TValue value);

  /// <summary>
  ///   Removes the object associated with the given key.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns>TChild.</returns>
  TChild Remove(string key);

  /// <summary>
  ///   Determines whether the cache contains an item the specified key, and the item is not expired.
  /// </summary>
  /// <param name="key">A string identifying the entry.</param>
  /// <returns><c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.</returns>
  bool Contains(string key);

  /// <summary>
  ///   Flushes all values from the shared cache.
  /// </summary>
  /// <returns>TChild.</returns>
  TChild Flush();

  /// <summary>
  ///   Generates a cache key from a base key name and supplied argument list.
  /// </summary>
  /// <param name="baseKey">The base key.</param>
  /// <param name="args">The arguments.</param>
  /// <returns>the generated cache key</returns>
  string Key(string baseKey, params object[] args);
}
