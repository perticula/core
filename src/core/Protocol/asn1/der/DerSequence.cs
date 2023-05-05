// perticula - core - DerSequence.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.ber;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerSequence.
///   Implements the <see cref="core.Protocol.asn1.Asn1Sequence" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Sequence" />
public class DerSequence : Asn1Sequence
{
	/// <summary>
	///   The empty
	/// </summary>
	public static readonly DerSequence Empty = new();

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequence" /> class.
	/// </summary>
	public DerSequence() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequence" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	public DerSequence(Asn1Encodable element) : base(element) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequence" /> class.
	/// </summary>
	/// <param name="element1">The element1.</param>
	/// <param name="element2">The element2.</param>
	public DerSequence(Asn1Encodable element1, Asn1Encodable element2) : base(element1, element2) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	public DerSequence(params Asn1Encodable[] elements) : base(elements) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequence" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	public DerSequence(Asn1EncodableVector elementVector) : base(elementVector) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	internal DerSequence(Asn1Encodable[] elements, bool clone) : base(elements, clone) { }

	/// <summary>
	///   Froms the vector.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <returns>DerSequence.</returns>
	public static DerSequence FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new DerSequence(elementVector);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new ConstructedDLEncoding(Asn1Tags.Universal, Asn1Tags.Sequence, Asn1OutputStream.GetContentsEncodings(Asn1OutputStream.EncodingDer, Elements));

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new ConstructedDLEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(Asn1OutputStream.EncodingDer, Elements));

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new ConstructedDerEncoding(Asn1Tags.Universal, Asn1Tags.Sequence, Asn1OutputStream.GetContentsEncodingsDer(Elements));

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new ConstructedDerEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodingsDer(Elements));

	/// <summary>
	///   Converts to asn1bitstring.
	/// </summary>
	/// <returns>DerBitString.</returns>
	public override DerBitString ToAsn1BitString() => new(BerBitString.FlattenBitStrings(GetConstructedBitStrings()), false);

	/// <summary>
	///   Converts to asn1external.
	/// </summary>
	/// <returns>DerExternal.</returns>
	public override DerExternal ToAsn1External() => new DerExternal(this);

	/// <summary>
	///   Converts to asn1octetstring.
	/// </summary>
	/// <returns>Asn1OctetString.</returns>
	public override Asn1OctetString ToAsn1OctetString() => new DerOctetString(BerOctetString.FlattenOctetStrings(GetConstructedOctetStrings()));

	/// <summary>
	///   Converts to asn1set.
	/// </summary>
	/// <returns>Asn1Set.</returns>
	public override Asn1Set ToAsn1Set() => new DLSet(false, Elements); // NOTE: DLSet is intentional, we don't want sorting

	/// <summary>
	///   Gets the length of the encoding.
	/// </summary>
	/// <param name="contentsLength">Length of the contents.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetEncodingLength(int contentsLength) => Asn1OutputStream.GetLengthOfEncodingDL(Asn1Tags.Sequence, contentsLength);
}
