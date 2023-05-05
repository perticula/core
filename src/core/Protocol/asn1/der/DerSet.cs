// perticula - core - DerSet.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

public class DerSet : Asn1Set
{
	public static readonly DerSet Empty = new();

	public DerSet() { }

	public DerSet(Asn1Encodable element) : base(element) { }

	public DerSet(params Asn1Encodable[] elements) : base(elements, true) { }

	internal DerSet(Asn1Encodable[] elements, bool doSort) : base(elements, doSort) { }

	public DerSet(Asn1EncodableVector elementVector) : base(elementVector, true) { }

	internal DerSet(Asn1EncodableVector elementVector, bool doSort) : base(elementVector, doSort) { }

	internal DerSet(bool isSorted, Asn1Encodable[] elements) : base(isSorted, elements) { }

	public static DerSet FromVector(Asn1EncodableVector elementVector) => elementVector.Count < 1 ? Empty : new DerSet(elementVector);

	internal override IAsn1Encoding GetEncoding(int encoding) => new ConstructedDLEncoding(Asn1Tags.Universal, Asn1Tags.Set, GetSortedDerEncodings());

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new ConstructedDLEncoding(tagClass, tagNo, GetSortedDerEncodings());

	internal sealed override DerEncoding GetEncodingDer() => new ConstructedDerEncoding(Asn1Tags.Universal, Asn1Tags.Set, GetSortedDerEncodings());

	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new ConstructedDerEncoding(tagClass, tagNo, GetSortedDerEncodings());

	private DerEncoding[] GetSortedDerEncodings() => Objects.EnsureSingleton(ref SortedDerEncodings, Elements, CreateSortedDerEncodings);

	private static DerEncoding[] CreateSortedDerEncodings(Asn1Encodable[] elements)
	{
		var derEncodings = Asn1OutputStream.GetContentsEncodingsDer(elements);
		if (derEncodings.Length > 1) Array.Sort(derEncodings);
		return derEncodings;
	}
}
