// perticula - core - DerOctetString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerOctetString.
///   Implements the <see cref="core.Protocol.asn1.Asn1OctetString" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1OctetString" />
public class DerOctetString : Asn1OctetString
{
	/// <summary>
	///   Initializes a new instance of the <see cref="DerOctetString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	public DerOctetString(byte[] contents) : base(contents) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerOctetString" /> class.
	/// </summary>
	/// <param name="obj">The object.</param>
	public DerOctetString(IAsn1Convertable obj) : this(obj.ToAsn1Object()) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerOctetString" /> class.
	/// </summary>
	/// <param name="obj">The object.</param>
	public DerOctetString(Asn1Encodable obj) : base(obj.GetEncoded(Der)) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerOctetString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	internal DerOctetString(ReadOnlySpan<byte> contents) : base(contents) { }

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new DerEncoding(Asn1Tags.Universal, Asn1Tags.OctetString, Contents);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, Contents);

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.OctetString, Contents);

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, Contents);

	/// <summary>
	///   Encodes the specified asn1 out.
	/// </summary>
	/// <param name="asn1Out">The asn1 out.</param>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	internal static void Encode(Asn1OutputStream asn1Out, byte[] buf, int off, int len)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Universal, Asn1Tags.OctetString);
		asn1Out.WriteDefiniteLength(len);
		asn1Out.Write(buf, off, len);
	}

	/// <summary>
	///   Encodes the specified asn1 out.
	/// </summary>
	/// <param name="asn1Out">The asn1 out.</param>
	/// <param name="buf">The buf.</param>
	internal static void Encode(Asn1OutputStream asn1Out, ReadOnlySpan<byte> buf)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Universal, Asn1Tags.OctetString);
		asn1Out.WriteDefiniteLength(buf.Length);
		asn1Out.Write(buf);
	}

	/// <summary>
	///   Encodes the specified asn1 out.
	/// </summary>
	/// <param name="asn1Out">The asn1 out.</param>
	/// <param name="buf1">The buf1.</param>
	/// <param name="buf2">The buf2.</param>
	internal static void Encode(Asn1OutputStream asn1Out, ReadOnlySpan<byte> buf1, ReadOnlySpan<byte> buf2)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Universal, Asn1Tags.OctetString);
		asn1Out.WriteDefiniteLength(buf1.Length + buf2.Length);
		asn1Out.Write(buf1);
		asn1Out.Write(buf2);
	}
}
