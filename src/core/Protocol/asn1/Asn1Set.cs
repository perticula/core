// perticula - core - Asn1Set.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;
using System.Diagnostics;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Set.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
///   Implements the <see cref="System.Collections.Generic.IEnumerable{core.Protocol.asn1.Asn1Encodable}" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
/// <seealso cref="System.Collections.Generic.IEnumerable{core.Protocol.asn1.Asn1Encodable}" />
public abstract class Asn1Set : Asn1Object, IEnumerable<Asn1Encodable>
{
	/// <summary>
	///   The elements
	/// </summary>
	internal readonly Asn1Encodable[] Elements;

	/// <summary>
	///   The is sorted
	/// </summary>
	protected readonly bool IsSorted;

	/// <summary>
	///   The sorted der encodings
	/// </summary>
	internal DerEncoding[]? SortedDerEncodings;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Set" /> class.
	/// </summary>
	protected internal Asn1Set()
	{
		Elements           = Asn1EncodableVector.EmptyElements;
		SortedDerEncodings = null;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Set" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	/// <exception cref="System.ArgumentNullException">element</exception>
	protected internal Asn1Set(Asn1Encodable element)
	{
		if (null == element) throw new ArgumentNullException(nameof(element));

		Elements           = new[] {element};
		SortedDerEncodings = null;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Set" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <param name="doSort">if set to <c>true</c> [do sort].</param>
	/// <exception cref="System.NullReferenceException">'elements' cannot be null, or contain null</exception>
	protected internal Asn1Set(Asn1Encodable[] elements, bool doSort)
	{
		if (Arrays.IsNullOrContainsNull(elements)) throw new NullReferenceException("'elements' cannot be null, or contain null");

		elements = Asn1EncodableVector.CloneElements(elements);
		DerEncoding[]? sortedDerEncodings = null;

		if (doSort && elements.Length > 1) sortedDerEncodings = SortElements(elements);

		Elements           = elements;
		SortedDerEncodings = sortedDerEncodings;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Set" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <param name="doSort">if set to <c>true</c> [do sort].</param>
	/// <exception cref="System.ArgumentNullException">elementVector</exception>
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

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Set" /> class.
	/// </summary>
	/// <param name="isSorted">if set to <c>true</c> [is sorted].</param>
	/// <param name="elements">The elements.</param>
	protected internal Asn1Set(bool isSorted, Asn1Encodable[] elements)
	{
		Debug.Assert(!isSorted);
		IsSorted           = isSorted;
		Elements           = elements;
		SortedDerEncodings = null;
	}

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
	///   Gets the parser.
	/// </summary>
	/// <value>The parser.</value>
	public IAsn1SetParser Parser => new Asn1SetParser(this);

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
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;Asn1Set&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct set from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
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

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1Set.</returns>
	public static Asn1Set GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1Set) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Maps the elements.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="func">The function.</param>
	/// <returns>T[].</returns>
	public virtual T[] MapElements<T>(Func<Asn1Encodable, T> func)
	{
		var count                                 = Count;
		var result                                = new T[count];
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

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => Elements.AsString();

	/// <summary>
	///   Sorts the elements.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <returns>DerEncoding[].</returns>
	private static DerEncoding[] SortElements(Asn1Encodable[] elements)
	{
		var derEncodings = Asn1OutputStream.GetContentsEncodingsDer(elements);
		Array.Sort(derEncodings, elements);
		return derEncodings;
	}

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
		private Meta() : base(typeof(Asn1Set), Asn1Tags.Set) { }

		/// <summary>
		///   Froms the implicit constructed.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence.ToAsn1Set();
	}
}
