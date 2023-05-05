// perticula - core - BerGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerGenerator.
///   Implements the <see cref="core.Protocol.asn1.Asn1Generator" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Generator" />
public class BerGenerator : Asn1Generator
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
	private readonly int _tagNo;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	protected BerGenerator(Stream outStream) : base(outStream) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="isExplicit">The is explicit.</param>
	protected BerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
	{
		_tagged     = true;
		_isExplicit = isExplicit;
		_tagNo      = tagNo;
	}

	/// <summary>
	///   Finishes this instance.
	/// </summary>
	protected override void Finish() => WriteBerEnd();

	/// <summary>
	///   Adds the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	public override void AddObject(Asn1Encodable obj) => obj.EncodeTo(OutStream);

	/// <summary>
	///   Adds the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	public override void AddObject(Asn1Object obj) => obj.EncodeTo(OutStream);

	/// <summary>
	///   Gets the raw output stream.
	/// </summary>
	/// <returns>System.IO.Stream.</returns>
	public override Stream GetRawOutputStream() => OutStream;

	/// <summary>
	///   Writes the HDR.
	/// </summary>
	/// <param name="tag">The tag.</param>
	private void WriteHdr(int tag)
	{
		OutStream.WriteByte((byte) tag);
		OutStream.WriteByte(0x80);
	}

	/// <summary>
	///   Writes the ber header.
	/// </summary>
	/// <param name="tag">The tag.</param>
	protected void WriteBerHeader(int tag)
	{
		if (_tagged)
		{
			var tagNum = _tagNo | Asn1Tags.ContextSpecific;

			if (_isExplicit)
			{
				WriteHdr(tagNum | Asn1Tags.Constructed);
				WriteHdr(tag);
			}
			else
			{
				if ((tag & Asn1Tags.Constructed) != 0)
					WriteHdr(tagNum | Asn1Tags.Constructed);
				else
					WriteHdr(tagNum);
			}
		}
		else
		{
			WriteHdr(tag);
		}
	}

	/// <summary>
	///   Writes the ber body.
	/// </summary>
	/// <param name="contentStream">The content stream.</param>
	protected void WriteBerBody(Stream contentStream) => Streams.PipeAll(contentStream, OutStream);

	/// <summary>
	///   Writes the ber end.
	/// </summary>
	protected void WriteBerEnd()
	{
		Span<byte> data = stackalloc byte[4] {0x00, 0x00, 0x00, 0x00};
		if (_tagged && _isExplicit) // write extra end for tag header
			OutStream.Write(data[..4]);
		else
			OutStream.Write(data[..2]);
	}
}
