// perticula - core - DefinteLengthSequence.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.ber;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1.dl;

/// <summary>
///   Class DefinteLengthSequence.
///   Implements the <see cref="DerSequence" />
/// </summary>
/// <seealso cref="DerSequence" />
public class DefinteLengthSequence : DerSequence
{
	/// <summary>
	///   The empty
	/// </summary>
	public new static readonly DefinteLengthSequence Empty = new();

	/// <summary>
	///   Initializes a new instance of the <see cref="DefinteLengthSequence" /> class.
	/// </summary>
	public DefinteLengthSequence() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefinteLengthSequence" /> class.
	/// </summary>
	/// <param name="element">The element.</param>
	public DefinteLengthSequence(Asn1Encodable element) : base(element) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefinteLengthSequence" /> class.
	/// </summary>
	/// <param name="element1">The element1.</param>
	/// <param name="element2">The element2.</param>
	public DefinteLengthSequence(Asn1Encodable element1, Asn1Encodable element2) : base(element1, element2) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefinteLengthSequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	public DefinteLengthSequence(params Asn1Encodable[] elements) : base(elements) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefinteLengthSequence" /> class.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	public DefinteLengthSequence(Asn1EncodableVector elementVector) : base(elementVector) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefinteLengthSequence" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	public DefinteLengthSequence(Asn1Encodable[] elements, bool clone) : base(elements, clone) { }

	/// <summary>
	///   Froms the vector.
	/// </summary>
	/// <param name="elementVector">The element vector.</param>
	/// <returns>DefinteLengthSequence.</returns>
	public new static DefinteLengthSequence FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new DefinteLengthSequence(elementVector);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncoding(encoding) : new ConstructedDefiniteLengthEncoding(Asn1Tags.Universal, Asn1Tags.Sequence, Asn1OutputStream.GetContentsEncodings(encoding, Elements));

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncodingImplicit(encoding, tagClass, tagNo) : new ConstructedDefiniteLengthEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, Elements));

	/// <summary>
	///   Converts to asn1bitstring.
	/// </summary>
	/// <returns>DerBitString.</returns>
	public override DerBitString ToAsn1BitString() => new DefinteLengthBitString(BerBitString.FlattenBitStrings(GetConstructedBitStrings()), false);

	/// <summary>
	///   Converts to asn1external.
	/// </summary>
	/// <returns>DerExternal.</returns>
	public override DerExternal ToAsn1External() => new DefiniteLengthExternal(this);

	/// <summary>
	///   Converts to asn1set.
	/// </summary>
	/// <returns>Asn1Set.</returns>
	public override Asn1Set ToAsn1Set() => new DefiniteLengthSet(false, Elements);
}
