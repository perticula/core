// perticula - core - Asn1EncodableVector.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;

namespace core.Protocol.asn1;

public class Asn1EncodableVector : IEnumerable<Asn1Encodable>
{
	private const int DefaultCapacity = 10;

	internal static readonly Asn1Encodable[] EmptyElements = Array.Empty<Asn1Encodable>();

	private bool _copyOnWrite;

	private Asn1Encodable[] _elements;

	public Asn1EncodableVector() : this(DefaultCapacity) { }

	public Asn1EncodableVector(int initialCapacity)
	{
		if (initialCapacity < 0) throw new ArgumentException("must not be negative", nameof(initialCapacity));

		_elements    = initialCapacity == 0 ? EmptyElements : new Asn1Encodable[initialCapacity];
		Count        = 0;
		_copyOnWrite = false;
	}

	public Asn1EncodableVector(Asn1Encodable element) : this() => Add(element);

	public Asn1EncodableVector(Asn1Encodable element1, Asn1Encodable element2) : this()
	{
		Add(element1);
		Add(element2);
	}

	public Asn1EncodableVector(params Asn1Encodable[] v) : this() => Add(v);

	public Asn1Encodable this[int index]
	{
		get
		{
			if (index >= Count)
				throw new IndexOutOfRangeException(index + " >= " + Count);

			return _elements[index];
		}
	}

	public int Count { get; private set; }

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<Asn1Encodable> GetEnumerator()
	{
		IEnumerable<Asn1Encodable> e = CopyElements();
		return e.GetEnumerator();
	}

	public static Asn1EncodableVector FromEnumerable(IEnumerable<Asn1Encodable> e)
	{
		var v = new Asn1EncodableVector();
		foreach (var obj in e) v.Add(obj);
		return v;
	}

	public void Add(Asn1Encodable element)
	{
		var capacity    = _elements.Length;
		var minCapacity = Count + 1;
		if ((minCapacity > capacity) | _copyOnWrite) Reallocate(minCapacity);

		_elements[Count] = element ?? throw new ArgumentNullException(nameof(element));
		Count            = minCapacity;
	}

	public void Add(Asn1Encodable element1, Asn1Encodable element2)
	{
		Add(element1);
		Add(element2);
	}

	public void Add(params Asn1Encodable[] objs)
	{
		foreach (var obj in objs) Add(obj);
	}

	public void AddOptional(Asn1Encodable? element)
	{
		if (element != null) Add(element);
	}

	public void AddOptional(Asn1Encodable? element1, Asn1Encodable? element2)
	{
		if (element1 != null) Add(element1);
		if (element2 != null) Add(element2);
	}

	public void AddOptional(params Asn1Encodable?[]? optionalElements)
	{
		if (optionalElements == null) return;
		foreach (var element in optionalElements)
		{
			if (element != null)
				Add(element);
		}
	}

	public void AddOptionalTagged(bool isExplicit, int tagNo, Asn1Encodable? obj)
	{
		if (null != obj) Add(new DerTaggedObject(isExplicit, tagNo, obj));
	}

	public void AddOptionalTagged(bool isExplicit, int tagClass, int tagNo, Asn1Encodable? obj)
	{
		if (null != obj) Add(new DerTaggedObject(isExplicit, tagClass, tagNo, obj));
	}

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

	internal Asn1Encodable[] CopyElements()
	{
		if (0 == Count)
			return EmptyElements;

		var copy = new Asn1Encodable[Count];
		Array.Copy(_elements, 0, copy, 0, Count);
		return copy;
	}

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

	private void Reallocate(int minCapacity)
	{
		var oldCapacity = _elements.Length;
		var newCapacity = System.Math.Max(oldCapacity, minCapacity + (minCapacity >> 1));

		var copy = new Asn1Encodable[newCapacity];
		Array.Copy(_elements, 0, copy, 0, Count);

		_elements    = copy;
		_copyOnWrite = false;
	}

	internal static Asn1Encodable[] CloneElements(Asn1Encodable[] elements) => elements.Length < 1 ? EmptyElements : (Asn1Encodable[]) elements.Clone();
}
