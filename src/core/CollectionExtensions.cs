// perticula - core - CollectionExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

public static class CollectionExtensions
{
	/// <summary>
	///   Conditionally adds the specified item to the collection.
	/// </summary>
	/// <typeparam name="TType">The type of the t type.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="item">The item.</param>
	/// <param name="predicate">The predicate.</param>
	/// <exception cref="ArgumentNullException">
	///   collection
	///   or
	///   predicate
	/// </exception>
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
	///   Removes a set of items from a collection
	/// </summary>
	/// <typeparam name="TType">The type of the t type.</typeparam>
	/// <param name="collection">The collection.</param>
	/// <param name="newItems">The items to merge.</param>
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
