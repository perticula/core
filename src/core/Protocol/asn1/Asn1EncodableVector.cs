// perticula - core - Asn1EncodableVector.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1EncodableVector.
///   Implements the <see cref="Asn1Encodable" />
/// </summary>
/// <seealso cref="Asn1Encodable" />
public class Asn1EncodableVector : IEnumerable<Asn1Encodable>
{
	/// <summary>
	///   The default capacity
	/// </summary>
	private const int DefaultCapacity = 10;

	/// <summary>
	///   The empty elements
	/// </summary>
	internal static readonly Asn1Encodable[] EmptyElements = Array.Empty<Asn1Encodable>();

	/// <summary>
	///   The copy on write
	/// </summary>
	private bool _copyOnWrite;

	/// <summary>
	///   The elements
	/// </summary>
	private Asn1Encodable[] _elements;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1EncodableVector" /> class.
	/// </summary>
	public Asn1EncodableVector() : this(DefaultCapacity) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1EncodableVector" /> class.
	/// </summary>
	/// <param name="initialCapacity">The initial capacity.</param>
	/// <exception cref="System.ArgumentException">must not be negative - initialCapacity</exception>
	public Asn1EncodableVector(int initialCapacity)
	{
		if (initialCapacity < 0) throw new ArgumentException("must not be negative", nameof(initialCapacity));

		_elements    = initialCapacity == 0 ? EmptyElements : new Asn1Encodable[initialCapacity];
		Count        = 0;
		_copyOnWrite = false;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1EncodableVector" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	public Asn1EncodableVector(Asn1Encodable element) : this() => Add(element);

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1EncodableVector" /> class.
	/// </summary>
	/// <param name="element1">The element1.</param>
	/// <param name="element2">The element2.</param>
	public Asn1EncodableVector(Asn1Encodable element1, Asn1Encodable element2) : this()
	{
		Add(element1);
		Add(element2);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1EncodableVector" /> class.
	/// </summary>
	/// <param name="v">The v.</param>
	public Asn1EncodableVector(params Asn1Encodable[] v) : this() => Add(v);

	/// <summary>
	///   Gets the <see cref="Asn1Encodable" /> at the specified index.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <returns>Asn1Encodable.</returns>
	/// <exception cref="System.IndexOutOfRangeException"></exception>
	public Asn1Encodable this[int index]
	{
		get
		{
			if (index >= Count)
				throw new IndexOutOfRangeException(index + " >= " + Count);

			return _elements[index];
		}
	}

	/// <summary>
	///   Gets the count.
	/// </summary>
	/// <value>The count.</value>
	public int Count { get; private set; }

	/// <summary>
	///   Returns an enumerator that iterates through a collection.
	/// </summary>
	/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///   Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public IEnumerator<Asn1Encodable> GetEnumerator()
	{
		IEnumerable<Asn1Encodable> e = CopyElements();
		return e.GetEnumerator();
	}

	/// <summary>
	///   Froms the enumerable.
	/// </summary>
	/// <param name="e">The e.</param>
	/// <returns>Asn1EncodableVector.</returns>
	public static Asn1EncodableVector FromEnumerable(IEnumerable<Asn1Encodable> e)
	{
		var v = new Asn1EncodableVector();
		foreach (var obj in e) v.Add(obj);
		return v;
	}

	/// <summary>
	///   Adds the specified element.
	/// </summary>
	/// <param name="element">The element.</param>
	/// <exception cref="System.ArgumentNullException">element</exception>
	public void Add(Asn1Encodable element)
	{
		var capacity    = _elements.Length;
		var minCapacity = Count + 1;
		if ((minCapacity > capacity) | _copyOnWrite) Reallocate(minCapacity);

		_elements[Count] = element ?? throw new ArgumentNullException(nameof(element));
		Count            = minCapacity;
	}

	/// <summary>
	///   Adds the specified element1.
	/// </summary>
	/// <param name="element1">The element1.</param>
	/// <param name="element2">The element2.</param>
	public void Add(Asn1Encodable element1, Asn1Encodable element2)
	{
		Add(element1);
		Add(element2);
	}

	/// <summary>
	///   Adds the specified objs.
	/// </summary>
	/// <param name="objs">The objs.</param>
	public void Add(params Asn1Encodable[] objs)
	{
		foreach (var obj in objs) Add(obj);
	}

	/// <summary>
	///   Adds the optional.
	/// </summary>
	/// <param name="element">The element.</param>
	public void AddOptional(Asn1Encodable? element)
	{
		if (element != null) Add(element);
	}

	/// <summary>
	///   Adds the optional.
	/// </summary>
	/// <param name="element1">The element1.</param>
	/// <param name="element2">The element2.</param>
	public void AddOptional(Asn1Encodable? element1, Asn1Encodable? element2)
	{
		if (element1 != null) Add(element1);
		if (element2 != null) Add(element2);
	}

	/// <summary>
	///   Adds the optional.
	/// </summary>
	/// <param name="optionalElements">The optional elements.</param>
	public void AddOptional(params Asn1Encodable?[]? optionalElements)
	{
		if (optionalElements == null) return;
		foreach (var element in optionalElements)
		{
			if (element != null)
				Add(element);
		}
	}

	/// <summary>
	///   Adds the optional tagged.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public void AddOptionalTagged(bool isExplicit, int tagNo, Asn1Encodable? obj)
	{
		if (null != obj) Add(new DerTaggedObject(isExplicit, tagNo, obj));
	}

	/// <summary>
	///   Adds the optional tagged.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public void AddOptionalTagged(bool isExplicit, int tagClass, int tagNo, Asn1Encodable? obj)
	{
		if (null != obj) Add(new DerTaggedObject(isExplicit, tagClass, tagNo, obj));
	}

	/// <summary>
	///   Adds all.
	/// </summary>
	/// <param name="other">The other.</param>
	/// <exception cref="System.ArgumentNullException">other</exception>
	/// <exception cref="System.NullReferenceException">'other' elements cannot be null</exception>
	public void AddAll(Asn1EncodableVector other)
	{
		if (null == other)
			throw new ArgumentNullException(nameof(other));

		var otherElementCount = other.Count;
		if (otherElementCount < 1)
			return;

		var capacity    = _elements.Length;
		var minCapacity = Count + otherElementCount;
		if ((minCapacity > capacity) | _copyOnWrite) Reallocate(minCapacity);

		var i = 0;
		do
		{
			var otherElement = other[i];

			_elements[Count + i] = otherElement ?? throw new NullReferenceException("'other' elements cannot be null");
		} while (++i < otherElementCount);

		Count = minCapacity;
	}

	/// <summary>
	///   Copies the elements.
	/// </summary>
	/// <returns>Asn1Encodable[].</returns>
	internal Asn1Encodable[] CopyElements()
	{
		if (0 == Count)
			return EmptyElements;

		var copy = new Asn1Encodable[Count];
		Array.Copy(_elements, 0, copy, 0, Count);
		return copy;
	}

	/// <summary>
	///   Takes the elements.
	/// </summary>
	/// <returns>Asn1Encodable[].</returns>
	internal Asn1Encodable[] TakeElements()
	{
		if (0 == Count)
			return EmptyElements;

		if (_elements.Length == Count)
		{
			_copyOnWrite = true;
			return _elements;
		}

		var copy = new Asn1Encodable[Count];
		Array.Copy(_elements, 0, copy, 0, Count);
		return copy;
	}

	/// <summary>
	///   Reallocates the specified minimum capacity.
	/// </summary>
	/// <param name="minCapacity">The minimum capacity.</param>
	private void Reallocate(int minCapacity)
	{
		var oldCapacity = _elements.Length;
		var newCapacity = System.Math.Max(oldCapacity, minCapacity + (minCapacity >> 1));

		var copy = new Asn1Encodable[newCapacity];
		Array.Copy(_elements, 0, copy, 0, Count);

		_elements    = copy;
		_copyOnWrite = false;
	}

	/// <summary>
	///   Clones the elements.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <returns>Asn1Encodable[].</returns>
	internal static Asn1Encodable[] CloneElements(Asn1Encodable[] elements) => elements.Length < 1 ? EmptyElements : (Asn1Encodable[]) elements.Clone();
}
