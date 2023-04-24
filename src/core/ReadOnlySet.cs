// perticula - core - ReadOnlySet.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;

namespace core;

/// <summary>
///   Class ReadOnlySet.
///   Implements the <see cref="System.Collections.Generic.ISet{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Collections.Generic.ISet{T}" />
public abstract class ReadOnlySet<T> : ISet<T>
{
	/// <summary>
	///   Returns an enumerator that iterates through a collection.
	/// </summary>
	/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///   Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
	/// </summary>
	/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
	public bool IsReadOnly => true;

	/// <summary>
	///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	void ICollection<T>.Add(T item) => throw new NotSupportedException();

	/// <summary>
	///   Adds an element to the current set and returns a value to indicate if the element was successfully added.
	/// </summary>
	/// <param name="item">The element to add to the set.</param>
	/// <returns>
	///   <see langword="true" /> if the element is added to the set; <see langword="false" /> if the element is already
	///   in the set.
	/// </returns>
	/// <exception cref="System.NotSupportedException"></exception>
	public bool Add(T item) => throw new NotSupportedException();

	/// <summary>
	///   Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <exception cref="System.NotSupportedException"></exception>
	public void Clear() => throw new NotSupportedException();

	/// <summary>
	///   Removes all elements in the specified collection from the current set.
	/// </summary>
	/// <param name="other">The collection of items to remove from the set.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public void ExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

	/// <summary>
	///   Modifies the current set so that it contains only elements that are also in a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public void IntersectWith(IEnumerable<T> other) => throw new NotSupportedException();

	/// <summary>
	///   Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <returns>
	///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the
	///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also
	///   returns <see langword="false" /> if <paramref name="item" /> is not found in the original
	///   <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </returns>
	/// <exception cref="System.NotSupportedException"></exception>
	public bool Remove(T item) => throw new NotSupportedException();

	/// <summary>
	///   Determines whether the current set and the specified collection contain the same elements.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is equal to <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	/// <exception cref="System.NotSupportedException"></exception>
	public bool SetEquals(IEnumerable<T> other) => throw new NotSupportedException();

	/// <summary>
	///   Modifies the current set so that it contains only elements that are present either in the current set or in the
	///   specified collection, but not both.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public void SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

	/// <summary>
	///   Modifies the current set so that it contains all elements that are present in the current set, in the specified
	///   collection, or in both.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public void UnionWith(IEnumerable<T> other) => throw new NotSupportedException();

	/// <summary>
	///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
	/// </summary>
	/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <returns>
	///   <see langword="true" /> if <paramref name="item" /> is found in the
	///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.
	/// </returns>
	public abstract bool Contains(T item);

	/// <summary>
	///   Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
	///   <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
	/// </summary>
	/// <param name="array">
	///   The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
	///   from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have
	///   zero-based indexing.
	/// </param>
	/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
	public abstract void CopyTo(T[] array, int arrayIndex);

	/// <summary>
	///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <value>The count.</value>
	public abstract int Count { get; }

	/// <summary>
	///   Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public abstract IEnumerator<T> GetEnumerator();

	/// <summary>
	///   Determines whether the current set is a proper (strict) subset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a proper subset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public abstract bool IsProperSubsetOf(IEnumerable<T> other);

	/// <summary>
	///   Determines whether the current set is a proper (strict) superset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a proper superset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public abstract bool IsProperSupersetOf(IEnumerable<T> other);

	/// <summary>
	///   Determines whether a set is a subset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a subset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public abstract bool IsSubsetOf(IEnumerable<T> other);

	/// <summary>
	///   Determines whether the current set is a superset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a superset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public abstract bool IsSupersetOf(IEnumerable<T> other);

	/// <summary>
	///   Determines whether the current set overlaps with the specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set and <paramref name="other" /> share at least one common element;
	///   otherwise, <see langword="false" />.
	/// </returns>
	public abstract bool Overlaps(IEnumerable<T> other);
}
