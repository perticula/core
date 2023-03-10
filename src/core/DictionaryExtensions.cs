// perticula - core - DictionaryExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

public static class DictionaryExtensions
{
	/// <summary>Adds a value a dictionary only if it is not already set. If it is already set no action is performed.</summary>
	/// <typeparam name="TKey">The type of the attribute key.</typeparam>
	/// <typeparam name="TValue">The type of the attribute value.</typeparam>
	/// <param name="dictionary">The dictionary.</param>
	/// <param name="key">The key.</param>
	/// <param name="value">Lambda to get the value (not called if the key exists)</param>
	/// <exception cref="ArgumentNullException">dictionary</exception>
	public static void AddOnce<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> value)
	{
		if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
		if (dictionary.ContainsKey(key)) return;
		dictionary.Add(key, value(key));
	}

	/// <summary>
	///   Attempts to add a value to a dictionary, if the key being added already exists the value will be updated
	///   instead.
	/// </summary>
	/// <typeparam name="TKey">The dictionary key type</typeparam>
	/// <typeparam name="TValue">The dictionary value type</typeparam>
	/// <param name="dictionary">The dictionary.</param>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	/// <returns>TValue.</returns>
	/// <exception cref="ArgumentNullException">dictionary</exception>
	public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
		if (dictionary.ContainsKey(key))
			dictionary[key] = value;
		else
			dictionary.Add(key, value);

		return value;
	}

	/// <summary>Deconstructs the dictionary into a key/value tuple. used as a workaround becuase .NET is kind of broken</summary>
	/// <typeparam name="T1">The type of the t1.</typeparam>
	/// <typeparam name="T2">The type of the t2.</typeparam>
	/// <param name="tuple">The tuple.</param>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
	{
		key   = tuple.Key;
		value = tuple.Value;
	}

	/// <summary>If a dictionary contains a value, the value is returns. Otherwise a default is returned.</summary>
	/// <typeparam name="TKey">The type of the dictionary key</typeparam>
	/// <typeparam name="TValue">The type of the dictionary value</typeparam>
	/// <param name="dictionary">The dictionary to search</param>
	/// <param name="key">The key to find</param>
	/// <param name="def">The default to return if the key is not set</param>
	/// <returns>TValue.</returns>
	public static TValue? FindOrDefault<TKey, TValue>(this IDictionary<TKey, TValue?>? dictionary, TKey key, TValue? def = default)
		=> dictionary?.ContainsKey(key) ?? false ? dictionary[key] : def;


	/// <summary>
	///   Finds an existing value in a dictionary and returns it OR calls a function to generate the value and then
	///   saves and returns that same value
	/// </summary>
	/// <typeparam name="TKey">The dictionary key type</typeparam>
	/// <typeparam name="TValue">The dictionary value type</typeparam>
	/// <param name="dictionary">The dictionary</param>
	/// <param name="key">The key to find or set</param>
	/// <param name="value">The function used to create the value if it was not found</param>
	/// <returns>TValue.</returns>
	/// <exception cref="ArgumentNullException">dictionary or key or value</exception>
	public static TValue FindOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> value)
	{
		if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
		if (key        == null) throw new ArgumentNullException(nameof(key));
		if (value      == null) throw new ArgumentNullException(nameof(value));

		if (dictionary.TryGetValue(key, out var current)) return current;

		var val = value(key);
		if (val != null) dictionary.Add(key, val);

		return dictionary[key];
	}

	/// <summary>
	///   Spreads the specified dictionary into a key/value tuple.
	/// </summary>
	/// <typeparam name="T1">The type of the t1.</typeparam>
	/// <typeparam name="T2">The type of the t2.</typeparam>
	/// <param name="value">The value.</param>
	/// <returns>System.Collections.Generic.IEnumerable&lt;(T1 key, T2 value)&gt;.</returns>
	public static IEnumerable<(T1 key, T2 value)> Spread<T1, T2>(this IDictionary<T1, T2> value)
	{
		if (value == null) throw new ArgumentNullException(nameof(value));
		foreach (var (k, v) in value) yield return (k, v);
	}
}
