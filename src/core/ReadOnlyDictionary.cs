// perticula - core - ReadOnlyDictionary.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;

namespace core;

/// <summary>
///   Class ReadOnlyDictionary.
///   Differs from <see cref="System.Collections.ObjectModel.ReadOnlyDictionary{TK, TV}" /> in that it does not require a
///   <see cref="System.Collections.Generic.IDictionary{TK, TV}" /> to be passed in.
///   Implements the <see cref="System.Collections.Generic.IDictionary{TK, TV}" />
/// </summary>
/// <typeparam name="TK">The type of the tk.</typeparam>
/// <typeparam name="TV">The type of the tv.</typeparam>
/// <seealso cref="System.Collections.Generic.IDictionary{TK, TV}" />
public abstract class ReadOnlyDictionary<TK, TV> : IDictionary<TK, TV>
{
	/// <summary>
	///   Gets or sets the <see cref="TV" /> with the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>TV.</returns>
	/// <exception cref="System.NotSupportedException"></exception>
	public TV this[TK key]
	{
		get => Lookup(key);
		set => throw new NotSupportedException();
	}

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
	///   Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
	/// </summary>
	/// <param name="key">The object to use as the key of the element to add.</param>
	/// <param name="value">The object to use as the value of the element to add.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public void Add(TK key, TV value) => throw new NotSupportedException();

	/// <summary>
	///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public void Add(KeyValuePair<TK, TV> item) => throw new NotSupportedException();

	/// <summary>
	///   Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <exception cref="System.NotSupportedException"></exception>
	public void Clear() => throw new NotSupportedException();

	/// <summary>
	///   Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
	/// </summary>
	/// <param name="key">The key of the element to remove.</param>
	/// <returns>
	///   <see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This
	///   method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original
	///   <see cref="T:System.Collections.Generic.IDictionary`2" />.
	/// </returns>
	/// <exception cref="System.NotSupportedException"></exception>
	public bool Remove(TK key) => throw new NotSupportedException();

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
	public bool Remove(KeyValuePair<TK, TV> item) => throw new NotSupportedException();

	/// <summary>
	///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
	/// </summary>
	/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <returns>
	///   <see langword="true" /> if <paramref name="item" /> is found in the
	///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.
	/// </returns>
	public abstract bool Contains(KeyValuePair<TK, TV> item);

	/// <summary>
	///   Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the
	///   specified key.
	/// </summary>
	/// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
	/// <returns>
	///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element
	///   with the key; otherwise, <see langword="false" />.
	/// </returns>
	public abstract bool ContainsKey(TK key);

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
	public abstract void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex);

	/// <summary>
	///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <value>The count.</value>
	public abstract int Count { get; }

	/// <summary>
	///   Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public abstract IEnumerator<KeyValuePair<TK, TV>> GetEnumerator();

	/// <summary>
	///   Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the
	///   <see cref="T:System.Collections.Generic.IDictionary`2" />.
	/// </summary>
	/// <value>The keys.</value>
	public abstract ICollection<TK> Keys { get; }

	/// <summary>
	///   Gets the value associated with the specified key.
	/// </summary>
	/// <param name="key">The key whose value to get.</param>
	/// <param name="value">
	///   When this method returns, the value associated with the specified key, if the key is found;
	///   otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed
	///   uninitialized.
	/// </param>
	/// <returns>
	///   <see langword="true" /> if the object that implements
	///   <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public abstract bool TryGetValue(TK key, out TV value);

	/// <summary>
	///   Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the
	///   <see cref="T:System.Collections.Generic.IDictionary`2" />.
	/// </summary>
	/// <value>The values.</value>
	public abstract ICollection<TV> Values { get; }

	/// <summary>
	///   Lookups the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>TV.</returns>
	protected abstract TV Lookup(TK key);
}
