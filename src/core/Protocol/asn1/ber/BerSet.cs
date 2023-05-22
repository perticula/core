// perticula - core - BerSet.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerSet.
///   Implements the <see cref="DerSet" />
/// </summary>
/// <seealso cref="DerSet" />
public class BerSet : DerSet
{
	/// <summary>
	///   The empty
	/// </summary>
	public new static readonly BerSet Empty = new();

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSet" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	public BerSet(Asn1Encodable element) : base(element) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSet" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	public BerSet(params Asn1Encodable[] elements) : base(elements, false) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSet" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	public BerSet(Asn1EncodableVector elementVector) : base(elementVector, false) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSet" /> class.
	/// </summary>
	/// <param name="isSorted">if set to <c>true</c> [is sorted].</param>
	/// <param name="elements">The elements.</param>
	internal BerSet(bool isSorted, Asn1Encodable[] elements) : base(isSorted, elements) { }

	/// <summary>
	///   Froms the vector.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <returns>BerSet.</returns>
	public new static BerSet FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new BerSet(elementVector);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncoding(encoding)
			   : new ConstructedIndefiniteLengthEncoding(Asn1Tags.Universal, Asn1Tags.Set, Asn1OutputStream.GetContentsEncodings(encoding, Elements));

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncodingImplicit(encoding, tagClass, tagNo)
			   : new ConstructedIndefiniteLengthEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, Elements));
}
