// perticula - core - DlBitString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1.dl;

/// <summary>
///   Class DLBitString.
///   Implements the <see cref="DerBitString" />
/// </summary>
/// <seealso cref="DerBitString" />
public class DLBitString : DerBitString
{
	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	public DLBitString(byte data, int padBits) : base(data, padBits) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	public DLBitString(byte[] data) : this(data, 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	public DLBitString(byte[] data, int padBits) : base(data, padBits) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	public DLBitString(ReadOnlySpan<byte> data) : this(data, 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	public DLBitString(ReadOnlySpan<byte> data, int padBits) : base(data, padBits) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="namedBits">The named bits.</param>
	public DLBitString(int namedBits) : base(namedBits) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="obj">The object.</param>
	public DLBitString(Asn1Encodable obj) : this(obj.GetDerEncoded(), 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DLBitString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="check">if set to <c>true</c> [check].</param>
	internal DLBitString(byte[] contents, bool check) : base(contents, check) { }

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncoding(encoding) : new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.BitString, Contents);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => Asn1OutputStream.EncodingDer == encoding ? base.GetEncodingImplicit(encoding, tagClass, tagNo) : new Asn1Encoding(tagClass, tagNo, Contents);
}
