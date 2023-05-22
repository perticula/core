// perticula - core - Asn1Sequence.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Sequence.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
///   Implements the <see cref="System.Collections.Generic.IEnumerable{core.Protocol.asn1.Asn1Encodable}" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
/// <seealso cref="System.Collections.Generic.IEnumerable{core.Protocol.asn1.Asn1Encodable}" />
public abstract class Asn1Sequence : Asn1Object, IEnumerable<Asn1Encodable>
{
	/// <summary>
	///   The elements
	/// </summary>
	internal readonly Asn1Encodable[] Elements;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Sequence" /> class.
	/// </summary>
	protected internal Asn1Sequence() => Elements = Asn1EncodableVector.EmptyElements;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Sequence" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	/// <exception cref="System.ArgumentNullException">element</exception>
	protected internal Asn1Sequence(Asn1Encodable element)
	{
		if (null == element) throw new ArgumentNullException(nameof(element));

		Elements = new[] {element};
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Sequence" /> class.
	/// </summary>
	/// <param name="element1">The element1.</param>
	/// <param name="element2">The element2.</param>
	/// <exception cref="System.ArgumentNullException">element1</exception>
	/// <exception cref="System.ArgumentNullException">element2</exception>
	protected internal Asn1Sequence(Asn1Encodable element1, Asn1Encodable element2)
	{
		if (null == element1) throw new ArgumentNullException(nameof(element1));
		if (null == element2) throw new ArgumentNullException(nameof(element2));

		Elements = new[] {element1, element2};
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Sequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <exception cref="System.NullReferenceException">'elements' cannot be null, or contain null</exception>
	protected internal Asn1Sequence(params Asn1Encodable?[]? elements)
	{
		if (Arrays.IsNullOrContainsNull(elements)) throw new NullReferenceException("'elements' cannot be null, or contain null");

		Elements = Asn1EncodableVector.CloneElements(elements!);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Sequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	internal Asn1Sequence(Asn1Encodable[] elements, bool clone) => Elements = clone ? Asn1EncodableVector.CloneElements(elements) : elements;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Sequence" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <exception cref="System.ArgumentNullException">elementVector</exception>
	protected internal Asn1Sequence(Asn1EncodableVector elementVector)
	{
		if (null == elementVector) throw new ArgumentNullException(nameof(elementVector));

		Elements = elementVector.TakeElements();
	}

	/// <summary>
	///   Gets the parser.
	/// </summary>
	/// <value>The parser.</value>
	public virtual IAsn1SequenceParser Parser => new Asn1SequenceParser(this);

	/// <summary>
	///   Gets the <see cref="Asn1Encodable" /> at the specified index.
	/// </summary>
	/// <param name="index">The index.</param>
	/// <returns>Asn1Encodable.</returns>
	public virtual Asn1Encodable this[int index] => Elements[index];

	/// <summary>
	///   Gets the count.
	/// </summary>
	/// <value>The count.</value>
	public virtual int Count => Elements.Length;

	/// <summary>
	///   Returns an enumerator that iterates through a collection.
	/// </summary>
	/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <summary>
	///   Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public virtual IEnumerator<Asn1Encodable> GetEnumerator()
	{
		IEnumerable<Asn1Encodable> e = Elements;
		return e.GetEnumerator();
	}

	/// <summary>
	///   Maps the elements.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="func">The function.</param>
	/// <returns>T[].</returns>
	public virtual T[] MapElements<T>(Func<Asn1Encodable, T> func)
	{
		var count  = Count;
		var result = new T[count];

		for (var i = 0; i < count; ++i) result[i] = func(Elements[i]);
		return result;
	}

	/// <summary>
	///   Converts to array.
	/// </summary>
	/// <returns>Asn1Encodable[].</returns>
	public virtual Asn1Encodable[] ToArray() => Asn1EncodableVector.CloneElements(Elements);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
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

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object)
	{
		if (asn1Object is not Asn1Sequence that)
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

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => Elements.AsString();

	/// <summary>
	///   Gets the constructed bit strings.
	/// </summary>
	/// <returns>System.Nullable&lt;DerBitString&gt;[].</returns>
	public DerBitString[] GetConstructedBitStrings() => MapElements(DerBitString.GetInstance);

	/// <summary>
	///   Gets the constructed octet strings.
	/// </summary>
	/// <returns>System.Nullable&lt;Asn1OctetString&gt;[].</returns>
	public Asn1OctetString[] GetConstructedOctetStrings() => MapElements(Asn1OctetString.GetInstance);

	/// <summary>
	///   Converts to asn1bitstring.
	/// </summary>
	/// <returns>DerBitString.</returns>
	public abstract DerBitString ToAsn1BitString();

	/// <summary>
	///   Converts to asn1external.
	/// </summary>
	/// <returns>DerExternal.</returns>
	public abstract DerExternal ToAsn1External();

	/// <summary>
	///   Converts to asn1octetstring.
	/// </summary>
	/// <returns>Asn1OctetString.</returns>
	public abstract Asn1OctetString ToAsn1OctetString();

	/// <summary>
	///   Converts to asn1set.
	/// </summary>
	/// <returns>Asn1Set.</returns>
	public abstract Asn1Set ToAsn1Set();


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

	/// <summary>
	///   Class Meta.
	///   Implements the <see cref="core.Protocol.asn1.Asn1UniversalType" />
	/// </summary>
	/// <seealso cref="core.Protocol.asn1.Asn1UniversalType" />
	internal class Meta : Asn1UniversalType
	{
		/// <summary>
		///   The instance
		/// </summary>
		internal static readonly Asn1UniversalType Instance = new Meta();

		/// <summary>
		///   Prevents a default instance of the <see cref="Meta" /> class from being created.
		/// </summary>
		private Meta() : base(typeof(Asn1Sequence), Asn1Tags.Sequence) { }

		/// <summary>
		///   Froms the implicit constructed.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence;
	}
}
