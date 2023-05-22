// perticula - core - BerBitString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerBitString.
///   Implements the <see cref="DerBitString" />
/// </summary>
/// <seealso cref="DerBitString" />
public class BerBitString : DerBitString
{
	/// <summary>
	///   The elements
	/// </summary>
	private readonly DerBitString[]? _elements;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	public BerBitString(byte data, int padBits) : base(data, padBits) => _elements = null;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	public BerBitString(byte[] data) : this(data, 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	public BerBitString(byte[] data, int padBits) : base(data, padBits) => _elements = null;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	public BerBitString(ReadOnlySpan<byte> data) : this(data, 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	public BerBitString(ReadOnlySpan<byte> data, int padBits) : base(data, padBits) => _elements = null;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="namedBits">The named bits.</param>
	public BerBitString(int namedBits) : base(namedBits) => _elements = null;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="obj">The object.</param>
	public BerBitString(Asn1Encodable obj) : this(obj.GetDerEncoded(), 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="elements">The elements.</param>
	public BerBitString(DerBitString[] elements) : base(FlattenBitStrings(elements), false) => _elements = elements;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="check">if set to <c>true</c> [check].</param>
	internal BerBitString(byte[] contents, bool check) : base(contents, check) => _elements = null;

	/// <summary>
	///   Froms the sequence.
	/// </summary>
	/// <param name="seq">The seq.</param>
	/// <returns>BerBitString.</returns>
	public static BerBitString FromSequence(Asn1Sequence seq) => new(seq.MapElements(GetInstance));

	/// <summary>
	///   Flattens the bit strings.
	/// </summary>
	/// <param name="bitStrings">The bit strings.</param>
	/// <returns>System.Byte[].</returns>
	/// <exception cref="System.ArgumentException">only the last nested bitstring can have padding - bitStrings</exception>
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

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding)
		=> Asn1OutputStream.EncodingBer != encoding
			   ? base.GetEncoding(encoding)
			   : null == _elements
				   ? new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.BitString, Contents)
				   : new ConstructedIndefiniteLengthEncoding(Asn1Tags.Universal, Asn1Tags.BitString, Asn1OutputStream.GetContentsEncodings(encoding, _elements));

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
			   : null == _elements
				   ? new Asn1Encoding(tagClass, tagNo, Contents)
				   : new ConstructedIndefiniteLengthEncoding(tagClass, tagNo, Asn1OutputStream.GetContentsEncodings(encoding, _elements));
}
