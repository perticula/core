// perticula - core - BerBitString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

public class BerBitString : DerBitString
{
	private readonly DerBitString[]? _elements;

	public BerBitString(byte data, int padBits) : base(data, padBits) => _elements = null;

	public BerBitString(byte[] data) : this(data, 0) { }

	public BerBitString(byte[] data, int padBits) : base(data, padBits) => _elements = null;

	public BerBitString(ReadOnlySpan<byte> data) : this(data, 0) { }

	public BerBitString(ReadOnlySpan<byte> data, int padBits) : base(data, padBits) => _elements = null;

	public BerBitString(int namedBits) : base(namedBits) => _elements = null;

	public BerBitString(Asn1Encodable obj) : this(obj.GetDerEncoded(), 0) { }

	public BerBitString(DerBitString[] elements) : base(FlattenBitStrings(elements), false) => _elements = elements;

	internal BerBitString(byte[] contents, bool check) : base(contents, check) => _elements = null;

	public static BerBitString FromSequence(Asn1Sequence seq) => new(seq.MapElements(GetInstance));

	internal static byte[] FlattenBitStrings(DerBitString[] bitStrings)
	{
		var count = bitStrings.Length;
		switch (count)
		{
			case 0:
				// No bits
				return new byte[] {0};
			case 1:
				return bitStrings[0].Contents;
			default:
			{
				int last = count - 1, totalLength = 0;
				for (var i = 0; i < last; ++i)
				{
					var elementContents = bitStrings[i].Contents;
					if (elementContents[0] != 0)
						throw new ArgumentException("only the last nested bitstring can have padding", nameof(bitStrings));

					totalLength += elementContents.Length - 1;
				}

				// Last one can have padding
				var lastElementContents = bitStrings[last].Contents;
				var padBits             = lastElementContents[0];
				totalLength += lastElementContents.Length;

				var contents = new byte[totalLength];
				contents[0] = padBits;

				var pos = 1;
				for (var i = 0; i < count; ++i)
				{
					var elementContents = bitStrings[i].Contents;
					var length          = elementContents.Length - 1;
					Array.Copy(elementContents, 1, contents, pos, length);
					pos += length;
				}

				Debug.Assert(pos == totalLength);
				return contents;
			}
		}
	}

	internal override IAsn1Encoding GetEncoding(int encoding)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncoding(encoding)
			   : null == _elements
				   ? new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.BitString, Contents)
				   : (IAsn1Encoding) new ConstructedILEncoding(Asn1Tags.Universal, Asn1Tags.BitString, Asn1OutputStream.GetContentsEncodings(encoding, _elements));

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncodingImplicit(encoding, tagClass, tagNo)
			   : null == _elements
				   ? new Asn1Encoding(tagClass, tagNo, Contents)
				   : (IAsn1Encoding) new ConstructedILEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, _elements));
}
