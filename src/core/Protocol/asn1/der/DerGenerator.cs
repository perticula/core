// perticula - core - DerGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerGenerator.
///   Implements the <see cref="core.Protocol.asn1.Asn1Generator" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Generator" />
public abstract class DerGenerator : Asn1Generator
{
	/// <summary>
	///   The is explicit
	/// </summary>
	private readonly bool _isExplicit;

	/// <summary>
	///   The tagged
	/// </summary>
	private readonly bool _tagged;

	/// <summary>
	///   The tag no
	/// </summary>
	public readonly int TagNo;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	protected DerGenerator(Stream outStream) : base(outStream) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	protected DerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
	{
		_tagged     = true;
		_isExplicit = isExplicit;
		TagNo       = tagNo;
	}

	/// <summary>
	///   Writes the der encoded.
	/// </summary>
	/// <param name="tag">The tag.</param>
	/// <param name="bytes">The bytes.</param>
	protected void WriteDerEncoded(int tag, byte[] bytes)
	{
		switch (_tagged)
		{
			case true:
			{
				var tagNum = TagNo | Asn1Tags.ContextSpecific;

				if (_isExplicit)
				{
					var newTag = TagNo | Asn1Tags.Constructed | Asn1Tags.ContextSpecific;
					var bOut   = new MemoryStream();
					WriteDerEncoded(bOut,      tag,    bytes);
					WriteDerEncoded(OutStream, newTag, bOut.ToArray());
				}
				else
				{
					if ((tag & Asn1Tags.Constructed) != 0) tagNum |= Asn1Tags.Constructed;

					WriteDerEncoded(OutStream, tagNum, bytes);
				}

				break;
			}
			default:
				WriteDerEncoded(OutStream, tag, bytes);
				break;
		}
	}

	/// <summary>
	///   Writes the der encoded.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tag">The tag.</param>
	/// <param name="bytes">The bytes.</param>
	protected static void WriteDerEncoded(Stream outStream, int tag, byte[] bytes)
	{
		outStream.WriteByte((byte) tag);
		WriteLength(outStream, bytes.Length);
		outStream.Write(bytes, 0, bytes.Length);
	}

	/// <summary>
	///   Writes the der encoded.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tag">The tag.</param>
	/// <param name="inStream">The in stream.</param>
	protected static void WriteDerEncoded(Stream outStream, int tag, Stream inStream) => WriteDerEncoded(outStream, tag, Streams.ReadAll(inStream));

	/// <summary>
	///   Writes the length.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="length">The length.</param>
	private static void WriteLength(Stream outStream, int length)
	{
		switch (length)
		{
			case > 127:
			{
				var size = 1;
				var val  = length;

				while ((val >>= 8) != 0) size++;

				outStream.WriteByte((byte) (size | 0x80));

				for (var i = (size - 1) * 8; i >= 0; i -= 8) outStream.WriteByte((byte) (length >> i));
				break;
			}
			default:
				outStream.WriteByte((byte) length);
				break;
		}
	}
}
