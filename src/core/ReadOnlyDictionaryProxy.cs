// perticula - core - ReadOnlyDictionaryProxy.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class ReadOnlyDictionaryProxy.
///   Implements the <see cref="core.ReadOnlyDictionary{TK, TV}" />
/// </summary>
/// <typeparam name="TK">The type of the tk.</typeparam>
/// <typeparam name="TV">The type of the tv.</typeparam>
/// <seealso cref="core.ReadOnlyDictionary{TK, TV}" />
public class ReadOnlyDictionaryProxy<TK, TV> : ReadOnlyDictionary<TK, TV>, IProxy<IDictionary<TK, TV>>
{
	/// <summary>
	///   The target
	/// </summary>
	private readonly IDictionary<TK, TV> _target;

	/// <summary>
	///   Initializes a new instance of the <see cref="ReadOnlyDictionaryProxy{TK, TV}" /> class.
	/// </summary>
	/// <param name="target">The target.</param>
	/// <exception cref="System.ArgumentNullException">target</exception>
	internal ReadOnlyDictionaryProxy(IDictionary<TK, TV> target) => _target = target ?? throw new ArgumentNullException(nameof(target));

	/// <summary>
	///   Gets the count.
	/// </summary>
	/// <value>The count.</value>
	public override int Count => Target.Count;

	/// <summary>
	///   Gets the keys.
	/// </summary>
	/// <value>The keys.</value>
	public override ICollection<TK> Keys => new ReadOnlyCollectionProxy<TK>(Target.Keys);

	/// <summary>
	///   Gets the values.
	/// </summary>
	/// <value>The values.</value>
	public override ICollection<TV> Values => new ReadOnlyCollectionProxy<TV>(Target.Values);

	/// <summary>
	///   The target
	/// </summary>
	/// <value>The target.</value>
	/// <exception cref="System.NotSupportedException"></exception>
	public IDictionary<TK, TV> Target
	{
		get => _target;
		set => throw new NotSupportedException();
	}

	/// <summary>
	///   Determines whether this instance contains the object.
	/// </summary>
	/// <param name="item">The item.</param>
	/// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
	public override bool Contains(KeyValuePair<TK, TV> item) => Target.Contains(item);

	/// <summary>
	///   Determines whether the specified key contains key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns><c>true</c> if the specified key contains key; otherwise, <c>false</c>.</returns>
	public override bool ContainsKey(TK key) => Target.ContainsKey(key);

	/// <summary>
	///   Copies to.
	/// </summary>
	/// <param name="array">The array.</param>
	/// <param name="arrayIndex">Index of the array.</param>
	public override void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) => Target.CopyTo(array, arrayIndex);

	/// <summary>
	///   Gets the enumerator.
	/// </summary>
	/// <returns>IEnumerator&lt;KeyValuePair&lt;TK, TV&gt;&gt;.</returns>
	public override IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => Target.GetEnumerator();

	/// <summary>
	///   Tries the get value.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public override bool TryGetValue(TK key, out TV value) => Target.TryGetValue(key, out value!);

	/// <summary>
	///   Lookups the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>TV.</returns>
	protected override TV Lookup(TK key) => Target[key];
}
