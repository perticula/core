// perticula - core - DerOctetString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

public class DerOctetString : Asn1OctetString
{
	public DerOctetString(byte[] contents) : base(contents) { }

	public DerOctetString(IAsn1Convertable obj) : this(obj.ToAsn1Object()) { }

	public DerOctetString(Asn1Encodable obj) : base(obj.GetEncoded(Der)) { }

	internal DerOctetString(ReadOnlySpan<byte> contents) : base(contents) { }

	internal override IAsn1Encoding GetEncoding(int encoding) => new DerEncoding(Asn1Tags.Universal, Asn1Tags.OctetString, Contents);

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, Contents);

	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.OctetString, Contents);

	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, Contents);

	internal static void Encode(Asn1OutputStream asn1Out, byte[] buf, int off, int len)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Universal, Asn1Tags.OctetString);
		asn1Out.WriteDL(len);
		asn1Out.Write(buf, off, len);
	}

	internal static void Encode(Asn1OutputStream asn1Out, ReadOnlySpan<byte> buf)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Universal, Asn1Tags.OctetString);
		asn1Out.WriteDL(buf.Length);
		asn1Out.Write(buf);
	}

	internal static void Encode(Asn1OutputStream asn1Out, ReadOnlySpan<byte> buf1, ReadOnlySpan<byte> buf2)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Universal, Asn1Tags.OctetString);
		asn1Out.WriteDL(buf1.Length + buf2.Length);
		asn1Out.Write(buf1);
		asn1Out.Write(buf2);
	}
}
