// perticula - core - Asn1Set.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;
using System.Diagnostics;

namespace core.Protocol.asn1;

public abstract class Asn1Set : Asn1Object, IEnumerable<Asn1Encodable>
{
	private readonly  bool            _isSorted;
	internal readonly Asn1Encodable[] Elements;
	internal          DerEncoding[]?  SortedDerEncodings;

	protected internal Asn1Set()
	{
		Elements           = Asn1EncodableVector.EmptyElements;
		SortedDerEncodings = null;
	}

	protected internal Asn1Set(Asn1Encodable element)
	{
		if (null == element) throw new ArgumentNullException(nameof(element));

		Elements           = new[] {element};
		SortedDerEncodings = null;
	}

	protected internal Asn1Set(Asn1Encodable[] elements, bool doSort)
	{
		if (Arrays.IsNullOrContainsNull(elements)) throw new NullReferenceException("'elements' cannot be null, or contain null");

		elements = Asn1EncodableVector.CloneElements(elements);
		DerEncoding[]? sortedDerEncodings = null;

		if (doSort && elements.Length > 1) sortedDerEncodings = SortElements(elements);

		Elements           = elements;
		SortedDerEncodings = sortedDerEncodings;
	}

	protected internal Asn1Set(Asn1EncodableVector elementVector, bool doSort)
	{
		if (null == elementVector) throw new ArgumentNullException(nameof(elementVector));

		Asn1Encodable[] elements;
		DerEncoding[]?  sortedDerEncodings;

		if (doSort && elementVector.Count > 1)
		{
			elements           = elementVector.CopyElements();
			sortedDerEncodings = SortElements(elements);
		}
		else
		{
			elements           = elementVector.TakeElements();
			sortedDerEncodings = null;
		}

		Elements           = elements;
		SortedDerEncodings = sortedDerEncodings;
	}

	protected internal Asn1Set(bool isSorted, Asn1Encodable[] elements)
	{
		Debug.Assert(!isSorted);
		_isSorted          = isSorted;
		Elements           = elements;
		SortedDerEncodings = null;
	}

	public virtual Asn1Encodable this[int index] => Elements[index];

	public virtual int Count => Elements.Length;

	public IAsn1SetParser Parser => new Asn1SetParser(this);

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public virtual IEnumerator<Asn1Encodable> GetEnumerator()
	{
		IEnumerable<Asn1Encodable> e = Elements;
		return e.GetEnumerator();
	}

	public static Asn1Set? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:            return null;
			case Asn1Set asn1Set: return asn1Set;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is Asn1Set converted) return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1Set) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct set from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	public static Asn1Set GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1Set) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	public virtual T[] MapElements<T>(Func<Asn1Encodable, T> func)
	{
		var count                                 = Count;
		var result                                = new T[count];
		for (var i = 0; i < count; ++i) result[i] = func(Elements[i]);
		return result;
	}

	public virtual Asn1Encodable[] ToArray() => Asn1EncodableVector.CloneElements(Elements);

	protected override int Asn1GetHashCode()
	{
		var i  = Count;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= Elements[i].ToAsn1Object().CallAsn1GetHashCode();
		}

		return hc;
	}

	protected override bool Asn1Equals(Asn1Object asn1Object)
	{
		if (asn1Object is not Asn1Set that)
			return false;

		var count = Count;
		if (that.Count != count)
			return false;

		for (var i = 0; i < count; ++i)
		{
			var o1 = Elements[i].ToAsn1Object();
			var o2 = that.Elements[i].ToAsn1Object();

			if (!o1.Equals(o2))
				return false;
		}

		return true;
	}

	public override string ToString() => Elements.AsString();

	private static DerEncoding[] SortElements(Asn1Encodable[] elements)
	{
		var derEncodings = Asn1OutputStream.GetContentsEncodingsDer(elements);
		Array.Sort(derEncodings, elements);
		return derEncodings;
	}

	internal class Meta : Asn1UniversalType
	{
		internal static readonly Asn1UniversalType Instance = new Meta();
		private Meta() : base(typeof(Asn1Set), Asn1Tags.Set) { }
		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence.ToAsn1Set();
	}
}
