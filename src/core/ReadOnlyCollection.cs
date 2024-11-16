// perticula - core - ReadOnlyCollection.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;

namespace core;

/// <summary>
///   Class ReadOnlyCollection. Differs from the System.Collections.ObjectModel.ReadOnlyCollection{T} in that it does not
///   require a concrete collection to be passed in.
///   Implements the <see cref="System.Collections.Generic.ICollection{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Collections.Generic.ICollection{T}" />
public abstract class ReadOnlyCollection<T> : ICollection<T>
{
	/// <summary>
	///   Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
	/// </summary>
	/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
	public bool IsReadOnly => true;

	/// <summary>
	///   Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public abstract IEnumerator<T> GetEnumerator();

	/// <summary>
	///   Returns an enumerator that iterates through a collection.
	/// </summary>
	/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public void Add(T item) => throw new NotSupportedException();

	/// <summary>
	///   Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <exception cref="System.NotSupportedException"></exception>
	public void Clear() => throw new NotSupportedException();

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
	///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
	/// </summary>
	/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <returns>
	///   <see langword="true" /> if <paramref name="item" /> is found in the
	///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.
	/// </returns>
	public abstract bool Contains(T item);

	/// <summary>
	///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <value>The count.</value>
	public abstract int Count { get; }

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
}
