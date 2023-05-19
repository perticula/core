// perticula - core - Asn1InputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers.Binary;
using core.IO;
using core.Protocol.asn1.ber;
using core.Protocol.asn1.der;
using core.Protocol.asn1.dl;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1InputStream.
///   a general purpose ASN.1 decoder - note: this class differs from the
///   others in that it returns null after it has read the last object in
///   the stream.If an ASN.1 Null is encountered a Der/BER Null object is returned.
///   Implements the <see cref="FilterStream" />
/// </summary>
/// <seealso cref="FilterStream" />
public class Asn1InputStream : FilterStream
{
	/// <summary>
	///   The leave open
	/// </summary>
	private readonly bool _leaveOpen;

	/// <summary>
	///   The limit
	/// </summary>
	private readonly int _limit;

	/// <summary>
	///   The temporary buffers
	/// </summary>
	internal byte[][]? TmpBuffers;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1InputStream" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	public Asn1InputStream(byte[] input) : this(new MemoryStream(input, false), input.Length) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1InputStream" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	public Asn1InputStream(Stream input) : this(input, FindLimit(input)) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1InputStream" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="limit">The limit.</param>
	public Asn1InputStream(Stream input, int limit) : this(input, limit, false) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1InputStream" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="limit">The limit.</param>
	/// <param name="leaveOpen">if set to <c>true</c> [leave open].</param>
	public Asn1InputStream(Stream input, int limit, bool leaveOpen) : this(input, limit, leaveOpen, new byte[16][]) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1InputStream" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="limit">The limit.</param>
	/// <param name="leaveOpen">if set to <c>true</c> [leave open].</param>
	/// <param name="tmpBuffers">The temporary buffers.</param>
	/// <exception cref="System.ArgumentException">Expected stream to be readable - input</exception>
	internal Asn1InputStream(Stream input, int limit, bool leaveOpen, byte[][] tmpBuffers) : base(input)
	{
		if (!input.CanRead) throw new ArgumentException("Expected stream to be readable", nameof(input));

		_limit     = limit;
		_leaveOpen = leaveOpen;
		TmpBuffers = tmpBuffers;
	}

	/// <summary>
	///   Finds the limit.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>System.Int32.</returns>
	internal static int FindLimit(Stream input) =>
		input switch
		{
			LimitedInputStream limited => limited.CurrentLimit,
			Asn1InputStream asn1       => asn1._limit,
			MemoryStream memory        => Convert.ToInt32(memory.Length - memory.Position),
			_                          => int.MaxValue
		};

	/// <summary>
	///   Builds the object.
	/// </summary>
	/// <param name="tagHdr">The tag HDR.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="length">The length.</param>
	/// <returns>Asn1Object.</returns>
	private Asn1Object BuildObject(int tagHdr, int tagNo, int length)
	{
		var defIn = new DefiniteLengthInputStream(BaseStream, length, _limit);

		if (0 == (tagHdr & Asn1Tags.Flags)) return CreatePrimitiveDerObject(tagNo, defIn, TmpBuffers ?? Array.Empty<byte[]>());

		var tagClass = tagHdr & Asn1Tags.Private;
		if (tagClass != 0)
		{
			var isConstructed = (tagHdr & Asn1Tags.Constructed) != 0;
			return ReadTaggedObjectDL(tagClass, tagNo, isConstructed, defIn);
		}

		return tagNo switch
		       {
			       Asn1Tags.BitString   => BuildConstructedBitString(ReadVector(defIn)),
			       Asn1Tags.OctetString => BuildConstructedOctetString(ReadVector(defIn)),
			       Asn1Tags.Sequence    => DLSequence.FromVector(ReadVector(defIn)),
			       Asn1Tags.Set         => DLSet.FromVector(ReadVector(defIn)),
			       Asn1Tags.External    => DLSequence.FromVector(ReadVector(defIn)).ToAsn1External(),
			       _                    => throw new IOException("unknown tag " + tagNo + " encountered")
		       };
	}

	/// <summary>
	///   Reads the tagged object dl.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="constructed">if set to <c>true</c> [constructed].</param>
	/// <param name="defIn">The definition in.</param>
	/// <returns>Asn1Object.</returns>
	internal Asn1Object ReadTaggedObjectDL(int tagClass, int tagNo, bool constructed, DefiniteLengthInputStream defIn)
	{
		if (!constructed)
		{
			var contentsOctets = defIn.ToArray();
			return Asn1TaggedObject.CreatePrimitive(tagClass, tagNo, contentsOctets);
		}

		var contentsElements = ReadVector(defIn);
		return Asn1TaggedObject.CreateConstructedDL(tagClass, tagNo, contentsElements);
	}

	/// <summary>
	///   Reads the vector.
	/// </summary>
	/// <returns>Asn1EncodableVector.</returns>
	private Asn1EncodableVector ReadVector()
	{
		var o = ReadObject();
		if (null == o)
			return new Asn1EncodableVector(0);

		var v = new Asn1EncodableVector();
		do
			v.Add(o);
		while ((o = ReadObject()) != null);
		return v;
	}

	/// <summary>
	///   Reads the vector.
	/// </summary>
	/// <param name="defIn">The definition in.</param>
	/// <returns>Asn1EncodableVector.</returns>
	private Asn1EncodableVector ReadVector(DefiniteLengthInputStream defIn)
	{
		var remaining = defIn.Remaining;
		if (remaining < 1) return new Asn1EncodableVector(0);

		using var sub = new Asn1InputStream(defIn, remaining, true, TmpBuffers ?? Array.Empty<byte[]>());
		return sub.ReadVector();
	}

	/// <summary>
	///   Reads the object.
	/// </summary>
	/// <returns>System.Nullable&lt;Asn1Object&gt;.</returns>
	/// <exception cref="System.IO.IOException">unexpected end-of-contents marker</exception>
	/// <exception cref="System.IO.IOException">indefinite-length primitive encoding encountered</exception>
	/// <exception cref="core.Protocol.asn1.Asn1Exception">corrupted stream detected</exception>
	public Asn1Object? ReadObject()
	{
		var tagHdr = BaseStream.ReadByte();
		if (tagHdr <= 0)
		{
			if (tagHdr == 0) throw new IOException("unexpected end-of-contents marker");

			return null;
		}

		var tagNo  = ReadTagNumber(BaseStream, tagHdr);
		var length = ReadLength(BaseStream, _limit, false);

		if (length >= 0)
			// definite-length
			try
			{
				return BuildObject(tagHdr, tagNo, length);
			}
			catch (ArgumentException e)
			{
				throw new Asn1Exception("corrupted stream detected", e);
			}

		// indefinite-length

		if (0 == (tagHdr & Asn1Tags.Constructed)) throw new IOException("indefinite-length primitive encoding encountered");

		var indIn = new IndefiniteLengthInputStream(BaseStream, _limit);
		var sp    = new Asn1StreamParser(indIn, _limit, TmpBuffers!);

		var tagClass = tagHdr & Asn1Tags.Private;
		if (0 != tagClass)
			return sp.LoadTaggedIndefiniteLength(tagClass, tagNo);

		return tagNo switch
		       {
			       Asn1Tags.BitString   => BerBitStringParser.Parse(sp),
			       Asn1Tags.OctetString => BerOctetStringParser.Parse(sp),
			       Asn1Tags.Sequence    => BerSequenceParser.Parse(sp),
			       Asn1Tags.Set         => BerSetParser.Parse(sp),
			       Asn1Tags.External    => DerExternalParser.Parse(sp),
			       _                    => throw new IOException("unknown BER object encountered")
		       };
	}

	/// <summary>
	///   Builds the constructed bit string.
	/// </summary>
	/// <param name="contentsElements">The contents elements.</param>
	/// <returns>DerBitString.</returns>
	/// <exception cref="core.Protocol.asn1.Asn1Exception">
	///   unknown object encountered in constructed BIT STRING:
	///   {contentsElements[i].GetTypeName()}
	/// </exception>
	private DerBitString BuildConstructedBitString(Asn1EncodableVector contentsElements)
	{
		var bitStrings = new DerBitString[contentsElements.Count];

		for (var i = 0; i != bitStrings.Length; i++)
		{
			if (contentsElements[i] is not DerBitString bitString)
				throw new Asn1Exception($"unknown object encountered in constructed BIT STRING: {contentsElements[i].GetTypeName()}");
			bitStrings[i] = bitString;
		}

		return new DLBitString(BerBitString.FlattenBitStrings(bitStrings), false);
	}

	/// <summary>
	///   Builds the constructed octet string.
	/// </summary>
	/// <param name="contentsElements">The contents elements.</param>
	/// <returns>Asn1OctetString.</returns>
	/// <exception cref="core.Protocol.asn1.Asn1Exception">
	///   unknown object encountered in constructed OCTET STRING:
	///   {contentsElements[i].GetTypeName()}
	/// </exception>
	private Asn1OctetString BuildConstructedOctetString(Asn1EncodableVector contentsElements)
	{
		var octetStrings = new Asn1OctetString[contentsElements.Count];

		for (var i = 0; i != octetStrings.Length; i++)
		{
			if (contentsElements[i] is not Asn1OctetString octetString)
				throw new Asn1Exception($"unknown object encountered in constructed OCTET STRING (pos {i}): {contentsElements[i].GetTypeName()}");

			octetStrings[i] = octetString;
		}

		// Note: No DLOctetString available
		return new DerOctetString(BerOctetString.FlattenOctetStrings(octetStrings));
	}

	/// <summary>
	///   Reads the tag number.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="tagHdr">The tag HDR.</param>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.IO.EndOfStreamException">EOF found inside tag value.</exception>
	/// <exception cref="System.IO.IOException">corrupted stream - high tag number less than 31 found</exception>
	/// <exception cref="System.IO.IOException">corrupted stream - invalid high tag number found</exception>
	/// <exception cref="System.IO.IOException">Tag number more than 31 bits</exception>
	internal static int ReadTagNumber(Stream s, int tagHdr)
	{
		var tagNo = tagHdr & 0x1f;

		//
		// with tagged object tag number is bottom 5 bits, or stored at the start of the content
		//
		if (tagNo != 0x1f) return tagNo;
		var b = s.ReadByte();
		if (b < 31)
		{
			if (b < 0)
				throw new EndOfStreamException("EOF found inside tag value.");

			throw new IOException("corrupted stream - high tag number < 31 found");
		}

		tagNo = b & 0x7f;

		// X.690-0207 8.1.2.4.2
		// "c) bits 7 to 1 of the first subsequent octet shall not all be zero."
		if (0 == tagNo)
			throw new IOException("corrupted stream - invalid high tag number found");

		while ((b & 0x80) != 0)
		{
			if ((uint) tagNo >> 24 != 0U)
				throw new IOException("Tag number more than 31 bits");

			tagNo <<= 7;

			b = s.ReadByte();
			if (b < 0)
				throw new EndOfStreamException("EOF found inside tag value.");

			tagNo |= b & 0x7f;
		}

		return tagNo;
	}

	/// <summary>
	///   Reads the length.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="limit">The limit.</param>
	/// <param name="isParsing">if set to <c>true</c> [is parsing].</param>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.IO.EndOfStreamException">EOF found when length expected</exception>
	/// <exception cref="System.IO.EndOfStreamException">EOF found reading length</exception>
	/// <exception cref="System.IO.IOException">invalid long form definite-length 0xFF</exception>
	/// <exception cref="System.IO.IOException">long form definite-length more than 31 bits</exception>
	/// <exception cref="System.IO.IOException">corrupted stream - out of bounds length found: {length} >= {limit}</exception>
	internal static int ReadLength(Stream s, int limit, bool isParsing)
	{
		var length = s.ReadByte();

		// definite-length short form  
		if ((uint) length >> 7 == 0U) return length;
		switch (length)
		{
			case 0x80: // indefinite-length
				return -1;
			case < 0:
				throw new EndOfStreamException("EOF found when length expected");
			case 0xFF:
				throw new IOException("invalid long form definite-length 0xFF");
		}

		int octetsCount = length & 0x7F, octetsPos = 0;

		length = 0;
		do
		{
			var octet = s.ReadByte();
			if (octet < 0) throw new EndOfStreamException("EOF found reading length");

			if ((uint) length >> 23 != 0U) throw new IOException("long form definite-length more than 31 bits");

			length = (length << 8) + octet;
		} while (++octetsPos < octetsCount);

		if (length >= limit && !isParsing) // we must have read at least 1 byte
			throw new IOException($"corrupted stream - out of bounds length found: {length} >= {limit}");

		return length;
	}

	/// <summary>
	///   Gets the buffer.
	/// </summary>
	/// <param name="defIn">The definition in.</param>
	/// <param name="tmpBuffers">The temporary buffers.</param>
	/// <param name="contents">The contents.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private static bool GetBuffer(DefiniteLengthInputStream defIn, IReadOnlyList<byte[]> tmpBuffers, out byte[] contents)
	{
		var len = defIn.Remaining;
		if (len >= tmpBuffers.Count)
		{
			contents = defIn.ToArray();
			return false;
		}

		var buf = tmpBuffers[len];

		defIn.ReadAllIntoByteArray(buf);

		contents = buf;
		return true;
	}

	/// <summary>
	///   Creates the primitive der object.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="defIn">The definition in.</param>
	/// <param name="tmpBuffers">The temporary buffers.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.IO.IOException">unsupported tag {tagNo} encountered</exception>
	internal static Asn1Object CreatePrimitiveDerObject(int tagNo, DefiniteLengthInputStream defIn, byte[][] tmpBuffers)
	{
		switch (tagNo)
		{
			case Asn1Tags.BmpString: return CreateDerBmpString(defIn);
			case Asn1Tags.Boolean:
			{
				GetBuffer(defIn, tmpBuffers, out var contents);
				return DerBoolean.CreatePrimitive(contents);
			}
			case Asn1Tags.Enumerated:
			{
				var usedBuffer = GetBuffer(defIn, tmpBuffers, out var contents);
				return DerEnumerated.CreatePrimitive(contents, usedBuffer);
			}
			case Asn1Tags.ObjectIdentifier:
			{
				var usedBuffer = GetBuffer(defIn, tmpBuffers, out var contents);
				return DerObjectIdentifier.CreatePrimitive(contents, usedBuffer);
			}
		}

		var bytes = defIn.ToArray();

		return tagNo switch
		       {
			       Asn1Tags.BitString           => DerBitString.CreatePrimitive(bytes),
			       Asn1Tags.GeneralizedTime     => Asn1GeneralizedTime.CreatePrimitive(bytes),
			       Asn1Tags.GeneralString       => DerGeneralString.CreatePrimitive(bytes),
			       Asn1Tags.GraphicString       => DerGraphicString.CreatePrimitive(bytes),
			       Asn1Tags.Ia5String           => DerIa5String.CreatePrimitive(bytes),
			       Asn1Tags.Integer             => DerInteger.CreatePrimitive(bytes),
			       Asn1Tags.Null                => Asn1Null.CreatePrimitive(bytes),
			       Asn1Tags.NumericString       => DerNumericString.CreatePrimitive(bytes),
			       Asn1Tags.ObjectDescriptor    => Asn1ObjectDescriptor.CreatePrimitive(bytes),
			       Asn1Tags.OctetString         => Asn1OctetString.CreatePrimitive(bytes),
			       Asn1Tags.PrintableString     => DerPrintableString.CreatePrimitive(bytes),
			       Asn1Tags.RelativeOid         => Asn1RelativeOid.CreatePrimitive(bytes, false),
			       Asn1Tags.T61String           => DerT61String.CreatePrimitive(bytes),
			       Asn1Tags.UniversalString     => DerUniversalString.CreatePrimitive(bytes),
			       Asn1Tags.UtcTime             => Asn1UtcTime.CreatePrimitive(bytes),
			       Asn1Tags.Utf8String          => DerUtf8String.CreatePrimitive(bytes),
			       Asn1Tags.VideoTexString      => DerVideoTexString.CreatePrimitive(bytes),
			       Asn1Tags.VisibleString       => DerVisibleString.CreatePrimitive(bytes),
			       Asn1Tags.Real                => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.EmbeddedPdv         => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.Time                => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.UnrestrictedString  => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.Date                => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.TimeOfDay           => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.DateTime            => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.Duration            => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.ObjectIdentifierIri => throw new IOException($"unsupported tag {tagNo} encountered"),
			       Asn1Tags.RelativeOidIri      => throw new IOException($"unsupported tag {tagNo} encountered"),
			       _                            => throw new IOException($"unknown tag {tagNo} encountered")
		       };
	}

	/// <summary>
	///   Creates the der BMP string.
	/// </summary>
	/// <param name="defIn">The definition in.</param>
	/// <returns>DerBmpString.</returns>
	/// <exception cref="System.IO.IOException">malformed BMPString encoding encountered</exception>
	private static DerBmpString CreateDerBmpString(DefiniteLengthInputStream defIn)
	{
		var remainingBytes = defIn.Remaining;
		if (0 != (remainingBytes & 1)) throw new IOException("malformed BMPString encoding encountered");

		var length = remainingBytes / 2;

		return DerBmpString.CreatePrimitive(length, defIn, (str, inStr) =>
		{
			var stringPos = 0;

			Span<byte> buf = stackalloc byte[8];
			while (remainingBytes >= 8)
			{
				if (Streams.ReadFully(inStr, buf) != 8) throw new EndOfStreamException("EOF encountered in middle of BMPString");

				str[stringPos]     =  (char) BinaryPrimitives.ReadUInt16BigEndian(buf[..]);
				str[stringPos + 1] =  (char) BinaryPrimitives.ReadUInt16BigEndian(buf[2..]);
				str[stringPos + 2] =  (char) BinaryPrimitives.ReadUInt16BigEndian(buf[4..]);
				str[stringPos + 3] =  (char) BinaryPrimitives.ReadUInt16BigEndian(buf[6..]);
				stringPos          += 4;
				remainingBytes     -= 8;
			}

			if (remainingBytes > 0)
			{
				if (Streams.ReadFully(inStr, buf) != remainingBytes) throw new EndOfStreamException("EOF encountered in middle of BMPString");

				var bufPos = 0;
				do
				{
					var b1 = buf[bufPos++] << 8;
					var b2 = buf[bufPos++] & 0xFF;
					str[stringPos++] = (char) (b1 | b2);
				} while (bufPos < remainingBytes);
			}

			if (0 != inStr.Remaining || str.Length != stringPos)
				throw new InvalidOperationException();
		});
	}

	/// <summary>
	///   Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed
	///   resources.
	/// </summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources;
	///   <see langword="false" /> to release only unmanaged resources.
	/// </param>
	protected override void Dispose(bool disposing)
	{
		TmpBuffers = null;

		if (_leaveOpen) Detach(disposing);
		else base.Dispose(disposing);
	}
}
