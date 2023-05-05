// perticula - core - BerSequence.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerSequence.
///   Implements the <see cref="DerSequence" />
/// </summary>
/// <seealso cref="DerSequence" />
public class BerSequence : DerSequence
{
	/// <summary>
	///   The empty
	/// </summary>
	public new static readonly BerSequence Empty = new();

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequence" /> class.
	/// </summary>
	public BerSequence() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequence" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	public BerSequence(Asn1Encodable element) : base(element) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequence" /> class.
	/// </summary>
	/// <param name="element1">The element1.</param>
	/// <param name="element2">The element2.</param>
	public BerSequence(Asn1Encodable element1, Asn1Encodable element2) : base(element1, element2) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	public BerSequence(params Asn1Encodable[] elements) : base(elements) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequence" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	public BerSequence(Asn1EncodableVector elementVector) : base(elementVector) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	internal BerSequence(Asn1Encodable[] elements, bool clone) : base(elements, clone) { }

	/// <summary>
	///   Froms the vector.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <returns>BerSequence.</returns>
	public new static BerSequence FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new BerSequence(elementVector);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncoding(encoding)
			   : (IAsn1Encoding) new ConstructedILEncoding(Asn1Tags.Universal, Asn1Tags.Sequence, Asn1OutputStream.GetContentsEncodings(encoding, Elements));

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
			   : (IAsn1Encoding) new ConstructedILEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, Elements));

	/// <summary>
	///   Converts to asn1bitstring.
	/// </summary>
	/// <returns>DerBitString.</returns>
	public override DerBitString ToAsn1BitString() => new BerBitString(GetConstructedBitStrings());

	/// <summary>
	///   Converts to asn1external.
	/// </summary>
	/// <returns>DerExternal.</returns>
	public override DerExternal ToAsn1External() => new DLExternal(this);

	/// <summary>
	///   Converts to asn1octetstring.
	/// </summary>
	/// <returns>Asn1OctetString.</returns>
	public override Asn1OctetString ToAsn1OctetString() => new BerOctetString(GetConstructedOctetStrings());

	/// <summary>
	///   Converts to asn1set.
	/// </summary>
	/// <returns>Asn1Set.</returns>
	public override Asn1Set ToAsn1Set() => new BerSet(false, Elements);
}
