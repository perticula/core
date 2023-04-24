// perticula - core - Asn1Sequence.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;

namespace core.Protocol.asn1;

public abstract class Asn1Sequence : Asn1Object, IEnumerable<Asn1Encodable>
{
	internal readonly Asn1Encodable[] Elements;

	protected internal Asn1Sequence() => Elements = Asn1EncodableVector.EmptyElements;

	protected internal Asn1Sequence(Asn1Encodable element)
	{
		if (null == element)
			throw new ArgumentNullException(nameof(element));

		Elements = new[] {element};
	}

	protected internal Asn1Sequence(Asn1Encodable element1, Asn1Encodable element2)
	{
		if (null == element1)
			throw new ArgumentNullException(nameof(element1));
		if (null == element2)
			throw new ArgumentNullException(nameof(element2));

		Elements = new[] {element1, element2};
	}

	protected internal Asn1Sequence(params Asn1Encodable?[]? elements)
	{
		if (Arrays.IsNullOrContainsNull(elements))
			throw new NullReferenceException("'elements' cannot be null, or contain null");

		Elements = Asn1EncodableVector.CloneElements(elements!);
	}

	internal Asn1Sequence(Asn1Encodable[] elements, bool clone) => Elements = clone ? Asn1EncodableVector.CloneElements(elements) : elements;

	protected internal Asn1Sequence(Asn1EncodableVector elementVector)
	{
		if (null == elementVector) throw new ArgumentNullException(nameof(elementVector));

		Elements = elementVector.TakeElements();
	}

	public virtual IAsn1SequenceParser Parser => new Asn1SequenceParser(this);

	public virtual Asn1Encodable this[int index] => Elements[index];

	public virtual int Count => Elements.Length;

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public virtual IEnumerator<Asn1Encodable> GetEnumerator()
	{
		IEnumerable<Asn1Encodable> e = Elements;
		return e.GetEnumerator();
	}

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
		var that = asn1Object as Asn1Sequence;
		if (null == that)
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

	internal DerBitString[] GetConstructedBitStrings() => MapElements(DerBitString.GetInstance);

	internal Asn1OctetString[] GetConstructedOctetStrings() => MapElements(Asn1OctetString.GetInstance);

	internal abstract DerBitString ToAsn1BitString();

	internal abstract DerExternal ToAsn1External();

	internal abstract Asn1OctetString ToAsn1OctetString();

	internal abstract Asn1Set ToAsn1Set();


	/// <summary>
	///   Gets an instance of Asn1Sequence from the passed object..
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>Asn1Sequence.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">failed to construct sequence from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
	public static Asn1Sequence GetInstance(object obj)
	{
		switch (obj)
		{
			case null:                      throw new ArgumentNullException(nameof(obj));
			case Asn1Sequence asn1Sequence: return asn1Sequence;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is Asn1Sequence converted) return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1Sequence) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct sequence from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	/// <summary>
	///   Return an ASN1 sequence from a tagged object.
	///   There is a special case here: if an object appears to have been explicitly tagged on
	///   reading but we were expecting it to be implicitly tagged in the
	///   normal course of events it indicates that we lost the surrounding
	///   sequence - so we need to add it back (this will happen if the tagged
	///   object is a sequence that contains other sequences). If you are
	///   dealing with implicitly tagged sequences you really <b>should</b>
	///   be using this method.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1Sequence.</returns>
	public static Asn1Sequence GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1Sequence) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	internal class Meta : Asn1UniversalType
	{
		internal static readonly Asn1UniversalType Instance = new Meta();

		private Meta() : base(typeof(Asn1Sequence), Asn1Tags.Sequence) { }

		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence;
	}
}
