// perticula - core - BerSet.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

public class BerSet : DerSet
{
	public new static readonly BerSet Empty = new();

	public BerSet(Asn1Encodable element) : base(element) { }

	public BerSet(params Asn1Encodable[] elements) : base(elements, false) { }

	public BerSet(Asn1EncodableVector elementVector) : base(elementVector, false) { }

	internal BerSet(bool isSorted, Asn1Encodable[] elements) : base(isSorted, elements) { }

	public new static BerSet FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new BerSet(elementVector);

	internal override IAsn1Encoding GetEncoding(int encoding)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncoding(encoding)
			   : (IAsn1Encoding) new ConstructedILEncoding(Asn1Tags.Universal, Asn1Tags.Set, Asn1OutputStream.GetContentsEncodings(encoding, Elements));

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncodingImplicit(encoding, tagClass, tagNo)
			   : (IAsn1Encoding) new ConstructedILEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, Elements));
}
