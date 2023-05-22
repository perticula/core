// perticula - core - DefiniteLengthExternal.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1.dl;

/// <summary>
///   Class DefiniteLengthExternal.
///   Implements the <see cref="DerExternal" />
/// </summary>
/// <seealso cref="DerExternal" />
public class DefiniteLengthExternal : DerExternal
{
	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthExternal" /> class.
	/// </summary>
	/// <param name="vector">The vector.</param>
	internal DefiniteLengthExternal(Asn1EncodableVector vector) : base(vector) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthExternal" /> class.
	/// </summary>
	/// <param name="sequence">The sequence.</param>
	internal DefiniteLengthExternal(Asn1Sequence sequence) : base(sequence) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthExternal" /> class.
	/// </summary>
	/// <param name="directReference">The direct reference.</param>
	/// <param name="indirectReference">The indirect reference.</param>
	/// <param name="dataValueDescriptor">The data value descriptor.</param>
	/// <param name="externalData">The external data.</param>
	internal DefiniteLengthExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1ObjectDescriptor dataValueDescriptor, Asn1TaggedObject externalData) : base(directReference, indirectReference, dataValueDescriptor, externalData) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthExternal" /> class.
	/// </summary>
	/// <param name="directReference">The direct reference.</param>
	/// <param name="indirectReference">The indirect reference.</param>
	/// <param name="dataValueDescriptor">The data value descriptor.</param>
	/// <param name="encoding">The encoding.</param>
	/// <param name="externalData">The external data.</param>
	internal DefiniteLengthExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1ObjectDescriptor dataValueDescriptor, int encoding, Asn1Object externalData) : base(directReference, indirectReference, dataValueDescriptor, encoding, externalData) { }

	/// <summary>
	///   Builds the sequence.
	/// </summary>
	/// <returns>Asn1Sequence.</returns>
	internal override Asn1Sequence BuildSequence()
	{
		var v = new Asn1EncodableVector(4);
		v.AddOptional(DirectReference, IndirectReference, DataValueDescriptor);
		v.Add(new DefiniteLengthTaggedObject(0 == BaseEncoding, BaseEncoding, ExternalContent!));
		return new DefinteLengthSequence(v);
	}

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncoding(encoding) : BuildSequence().GetEncodingImplicit(encoding, Asn1Tags.Universal, Asn1Tags.External);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncodingImplicit(encoding, tagClass, tagNo) : BuildSequence().GetEncodingImplicit(encoding, tagClass, tagNo);
}
