// perticula - core - DerSet.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerSet.
///   Implements the <see cref="core.Protocol.asn1.Asn1Set" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Set" />
public class DerSet : Asn1Set
{
	/// <summary>
	///   The empty
	/// </summary>
	public static readonly DerSet Empty = new();

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSet" /> class.
	/// </summary>
	public DerSet() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSet" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	public DerSet(Asn1Encodable element) : base(element) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSet" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	public DerSet(params Asn1Encodable[] elements) : base(elements, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSet" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <param name="doSort">if set to <c>true</c> [do sort].</param>
	internal DerSet(Asn1Encodable[] elements, bool doSort) : base(elements, doSort) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSet" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	public DerSet(Asn1EncodableVector elementVector) : base(elementVector, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSet" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <param name="doSort">if set to <c>true</c> [do sort].</param>
	internal DerSet(Asn1EncodableVector elementVector, bool doSort) : base(elementVector, doSort) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSet" /> class.
	/// </summary>
	/// <param name="isSorted">if set to <c>true</c> [is sorted].</param>
	/// <param name="elements">The elements.</param>
	internal DerSet(bool isSorted, Asn1Encodable[] elements) : base(isSorted, elements) { }

	/// <summary>
	///   Froms the vector.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <returns>DerSet.</returns>
	public static DerSet FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new DerSet(elementVector);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new ConstructedDefiniteLengthEncoding(Asn1Tags.Universal, Asn1Tags.Set, GetSortedDerEncodings());

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new ConstructedDefiniteLengthEncoding(tagClass, tagNo, GetSortedDerEncodings());

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new ConstructedDerEncoding(Asn1Tags.Universal, Asn1Tags.Set, GetSortedDerEncodings());

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new ConstructedDerEncoding(tagClass, tagNo, GetSortedDerEncodings());

	/// <summary>
	///   Gets the sorted der encodings.
	/// </summary>
	/// <returns>DerEncoding[].</returns>
	private DerEncoding[] GetSortedDerEncodings() => Objects.EnsureSingleton(ref SortedDerEncodings, Elements, CreateSortedDerEncodings);

	/// <summary>
	///   Creates the sorted der encodings.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <returns>DerEncoding[].</returns>
	private static DerEncoding[] CreateSortedDerEncodings(Asn1Encodable[] elements)
	{
		var derEncodings = Asn1OutputStream.GetContentsEncodingsDer(elements);
		if (derEncodings.Length > 1) Array.Sort(derEncodings);
		return derEncodings;
	}
}
