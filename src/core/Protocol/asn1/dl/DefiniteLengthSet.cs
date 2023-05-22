// perticula - core - DefiniteLengthSet.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1.dl;

/// <summary>
///   Class DefiniteLengthSet.
///   Implements the <see cref="DerSet" />
/// </summary>
/// <seealso cref="DerSet" />
public class DefiniteLengthSet : DerSet
{
	/// <summary>
	///   The empty
	/// </summary>
	internal new static readonly DefiniteLengthSet Empty = new();

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthSet" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	internal DefiniteLengthSet(Asn1Encodable element) : base(element) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthSet" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	internal DefiniteLengthSet(params Asn1Encodable[] elements) : base(elements, false) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthSet" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	internal DefiniteLengthSet(Asn1EncodableVector elementVector) : base(elementVector, false) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthSet" /> class.
	/// </summary>
	/// <param name="isSorted">if set to <c>true</c> [is sorted].</param>
	/// <param name="elements">The elements.</param>
	internal DefiniteLengthSet(bool isSorted, Asn1Encodable[] elements) : base(isSorted, elements) { }

	/// <summary>
	///   Froms the vector.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <returns>DefiniteLengthSet.</returns>
	internal new static DefiniteLengthSet FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new DefiniteLengthSet(elementVector);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncoding(encoding) : new ConstructedDefiniteLengthEncoding(Asn1Tags.Universal, Asn1Tags.Set, Asn1OutputStream.GetContentsEncodings(encoding, Elements));

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncodingImplicit(encoding, tagClass, tagNo) : new ConstructedDefiniteLengthEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, Elements));
}
