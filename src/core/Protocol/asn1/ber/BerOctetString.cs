// perticula - core - BerOctetString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerOctetString.
///   Implements the <see cref="DerOctetString" />
/// </summary>
/// <seealso cref="DerOctetString" />
public class BerOctetString : DerOctetString
{
	/// <summary>
	///   The elements
	/// </summary>
	private readonly Asn1OctetString[] _elements;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerOctetString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	public BerOctetString(byte[] contents) : this(contents, Array.Empty<Asn1OctetString>()) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerOctetString" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	public BerOctetString(Asn1OctetString[] elements) : this(FlattenOctetStrings(elements), elements) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerOctetString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="elements">The elements.</param>
	private BerOctetString(byte[] contents, Asn1OctetString[] elements) : base(contents) => _elements = elements;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerOctetString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	internal BerOctetString(ReadOnlySpan<byte> contents) : base(contents) => _elements = Array.Empty<Asn1OctetString>();

	/// <summary>
	///   Froms the sequence.
	/// </summary>
	/// <param name="seq">The seq.</param>
	/// <returns>BerOctetString.</returns>
	public static BerOctetString FromSequence(Asn1Sequence seq) => new(seq.MapElements(GetInstance));

	/// <summary>
	///   Flattens the octet strings.
	/// </summary>
	/// <param name="octetStrings">The octet strings.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] FlattenOctetStrings(Asn1OctetString[] octetStrings)
	{
		var count = octetStrings.Length;
		switch (count)
		{
			case 0:
				return EmptyOctets;
			case 1:
				return octetStrings[0].Contents;
			default:
			{
				var totalOctets                             = 0;
				for (var i = 0; i < count; ++i) totalOctets += octetStrings[i].Contents.Length;

				var str = new byte[totalOctets];
				var pos = 0;
				for (var i = 0; i < count; ++i)
				{
					var octets = octetStrings[i].Contents;
					Array.Copy(octets, 0, str, pos, octets.Length);
					pos += octets.Length;
				}

				Debug.Assert(pos == totalOctets);
				return str;
			}
		}
	}

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncoding(encoding)
			   : (IAsn1Encoding) new ConstructedILEncoding(Asn1Tags.Universal, Asn1Tags.OctetString, Asn1OutputStream.GetContentsEncodings(encoding, _elements));

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
			   : (IAsn1Encoding) new ConstructedILEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, _elements));
}
