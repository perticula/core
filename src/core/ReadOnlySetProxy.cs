// perticula - core - ReadOnlySetProxy.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class ReadOnlySetProxy.
///   Implements the <see cref="core.ReadOnlySet{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="core.ReadOnlySet{T}" />
public class ReadOnlySetProxy<T> : ReadOnlySet<T>, IProxy<ISet<T>>
{
	/// <summary>
	///   The target
	/// </summary>
	private readonly ISet<T> _target;

	/// <summary>
	///   Initializes a new instance of the <see cref="ReadOnlySetProxy{T}" /> class.
	/// </summary>
	/// <param name="target">The target.</param>
	/// <exception cref="System.ArgumentNullException">target</exception>
	internal ReadOnlySetProxy(ISet<T> target) => _target = target ?? throw new ArgumentNullException(nameof(target));

	/// <summary>
	///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
	/// </summary>
	/// <value>The count.</value>
	public override int Count => Target.Count;

	/// <summary>
	///   Gets the target.
	/// </summary>
	/// <value>The target.</value>
	/// <exception cref="System.NotSupportedException"></exception>
	public ISet<T> Target
	{
		get => _target;
		set => throw new NotSupportedException();
	}

	/// <summary>
	///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
	/// </summary>
	/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
	/// <returns>
	///   <see langword="true" /> if <paramref name="item" /> is found in the
	///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.
	/// </returns>
	public override bool Contains(T item) => Target.Contains(item);

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
	public override void CopyTo(T[] array, int arrayIndex) => Target.CopyTo(array, arrayIndex);

	/// <summary>
	///   Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public override IEnumerator<T> GetEnumerator() => Target.GetEnumerator();

	/// <summary>
	///   Determines whether the current set is a proper (strict) subset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a proper subset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public override bool IsProperSubsetOf(IEnumerable<T> other) => Target.IsProperSubsetOf(other);

	/// <summary>
	///   Determines whether the current set is a proper (strict) superset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a proper superset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public override bool IsProperSupersetOf(IEnumerable<T> other) => Target.IsProperSupersetOf(other);

	/// <summary>
	///   Determines whether a set is a subset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a subset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public override bool IsSubsetOf(IEnumerable<T> other) => Target.IsSubsetOf(other);

	/// <summary>
	///   Determines whether the current set is a superset of a specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set is a superset of <paramref name="other" />; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public override bool IsSupersetOf(IEnumerable<T> other) => Target.IsSupersetOf(other);

	/// <summary>
	///   Determines whether the current set overlaps with the specified collection.
	/// </summary>
	/// <param name="other">The collection to compare to the current set.</param>
	/// <returns>
	///   <see langword="true" /> if the current set and <paramref name="other" /> share at least one common element;
	///   otherwise, <see langword="false" />.
	/// </returns>
	public override bool Overlaps(IEnumerable<T> other) => Target.Overlaps(other);
}
