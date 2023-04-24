// perticula - core - CollectionExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class CollectionExtensions.
/// </summary>
public static class CollectionExtensions
{
	/// <summary>
	///   Conditionally adds the specified item to the collection.
	/// </summary>
	/// <typeparam name="TType">The type of the t type.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="item">The item.</param>
	/// <param name="predicate">The predicate.</param>
	/// <exception cref="System.ArgumentNullException">collection</exception>
	/// <exception cref="System.ArgumentNullException">predicate</exception>
	public static void ConditionalAdd<TType>(this ICollection<TType> collection, TType item, Func<ICollection<TType>, TType, bool> predicate)
	{
		if (collection == null) throw new ArgumentNullException(nameof(collection));
		if (predicate  == null) throw new ArgumentNullException(nameof(predicate));
		if (predicate(collection, item))
			collection.Add(item);
	}

	/// <summary>
	///   Merges a set of items into a collection
	/// </summary>
	/// <typeparam name="TType">The type of the t type.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="newItems">The items to merge.</param>
	/// <exception cref="System.ArgumentNullException">collection</exception>
	public static void Merge<TType>(this ICollection<TType> collection, IEnumerable<TType> newItems)
	{
		if (collection == null) throw new ArgumentNullException(nameof(collection));
		foreach (var item in newItems)
		{
			if (item == null) continue;
			if (!collection.Contains(item)) collection.Add(item);
		}
	}

	/// <summary>
	///   Proxies the specified e.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="e">The e.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	public static IEnumerable<T> Proxy<T>(this IEnumerable<T> e) => new EnumerableProxy<T>(e);

	/// <summary>
	///   Reads the only.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="c">The c.</param>
	/// <returns>ICollection&lt;T&gt;.</returns>
	public static ICollection<T> ReadOnly<T>(this ICollection<T> c) => new ReadOnlyCollectionProxy<T>(c);

	/// <summary>
	///   Reads the only.
	/// </summary>
	/// <typeparam name="TK">The type of the tk.</typeparam>
	/// <typeparam name="TV">The type of the tv.</typeparam>
	/// <param name="d">The d.</param>
	/// <returns>IDictionary&lt;TK, TV&gt;.</returns>
	public static IDictionary<TK, TV> ReadOnly<TK, TV>(IDictionary<TK, TV> d) => new ReadOnlyDictionaryProxy<TK, TV>(d);

	/// <summary>
	///   Reads the only.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="l">The l.</param>
	/// <returns>IList&lt;T&gt;.</returns>
	public static IList<T> ReadOnly<T>(IList<T> l) => new ReadOnlyListProxy<T>(l);

	/// <summary>
	///   Reads the only.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="s">The s.</param>
	/// <returns>ISet&lt;T&gt;.</returns>
	public static ISet<T> ReadOnly<T>(ISet<T> s) => new ReadOnlySetProxy<T>(s);

	/// <summary>
	///   Removes a set of items from a collection
	/// </summary>
	/// <typeparam name="TType">The type of the t type.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="newItems">The items to merge.</param>
	/// <exception cref="System.ArgumentNullException">collection</exception>
	public static void Purge<TType>(this ICollection<TType> collection, IEnumerable<TType> newItems)
	{
		if (collection == null) throw new ArgumentNullException(nameof(collection));
		foreach (var item in newItems)
		{
			if (item == null) continue;
			if (collection.Contains(item)) collection.Remove(item);
		}
	}
}
