// perticula - core - Asn1StreamParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;
using core.Protocol.asn1.ber;
using core.Protocol.asn1.der;
using core.Protocol.asn1.dl;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1StreamParser.
/// </summary>
public class Asn1StreamParser
{
	/// <summary>
	///   The base stream
	/// </summary>
	private readonly Stream _baseStream;

	/// <summary>
	///   The limit
	/// </summary>
	private readonly int _limit;

	/// <summary>
	///   The temporary buffers
	/// </summary>
	private readonly byte[][] _tmpBuffers;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1StreamParser" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	public Asn1StreamParser(Stream input) : this(input, Asn1InputStream.FindLimit(input)) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1StreamParser" /> class.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	public Asn1StreamParser(byte[] encoding) : this(new MemoryStream(encoding, false), encoding.Length) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1StreamParser" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="limit">The limit.</param>
	public Asn1StreamParser(Stream input, int limit) : this(input, limit, new byte[16][]) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1StreamParser" /> class.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="limit">The limit.</param>
	/// <param name="tmpBuffers">The temporary buffers.</param>
	/// <exception cref="ArgumentException">Expected stream to be readable, nameof(input)</exception>
	internal Asn1StreamParser(Stream input, int limit, byte[][] tmpBuffers)
	{
		if (!input.CanRead) throw new ArgumentException("Expected stream to be readable", nameof(input));

		_baseStream = input;
		_limit      = limit;
		_tmpBuffers = tmpBuffers;
	}

	/// <summary>
	///   Reads the object.
	/// </summary>
	/// <returns>core.Protocol.asn1.IAsn1Convertable?.</returns>
	public virtual IAsn1Convertable? ReadObject()
	{
		var tagHdr = _baseStream.ReadByte();
		return tagHdr < 0 ? null : ImplParseObject(tagHdr);
	}

	/// <summary>
	///   Implementations the parse object.
	/// </summary>
	/// <param name="tagHdr">The tag HDR.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	/// <exception cref="IOException">indefinite-length primitive encoding encountered</exception>
	internal IAsn1Convertable ImplParseObject(int tagHdr)
	{
		// turn off looking for "00" while we resolve the tag
		Set00Check(false);

		// calculate tag number
		var tagNo = Asn1InputStream.ReadTagNumber(_baseStream, tagHdr);

		// calculate length
		var length = Asn1InputStream.ReadLength(_baseStream, _limit, tagNo is Asn1Tags.BitString or Asn1Tags.OctetString or Asn1Tags.Sequence or Asn1Tags.Set or Asn1Tags.External);

		if (length < 0) // indefinite-length method
		{
			if (0 == (tagHdr & Asn1Tags.Constructed)) throw new IOException("indefinite-length primitive encoding encountered");

			var indIn = new IndefiniteLengthInputStream(_baseStream, _limit);
			var sp    = new Asn1StreamParser(indIn, _limit, _tmpBuffers);

			var tagClass = tagHdr & Asn1Tags.Private;
			return 0 != tagClass ? new BerTaggedObjectParser(tagClass, tagNo, sp) : sp.ParseImplicitConstructedIndefiniteLength(tagNo);
		}
		else
		{
			var defIn = new DefiniteLengthInputStream(_baseStream, length, _limit);

			if (0 == (tagHdr & Asn1Tags.Flags))
				return ParseImplicitPrimitive(tagNo, defIn);

			var sp = new Asn1StreamParser(defIn, defIn.Remaining, _tmpBuffers);

			var tagClass = tagHdr & Asn1Tags.Private;
			if (0 == tagClass) return sp.ParseImplicitConstructedDefiniteLength(tagNo);
			var isConstructed = (tagHdr & Asn1Tags.Constructed) != 0;

			return new DefiniteLengthTaggedObjectParser(tagClass, tagNo, isConstructed, sp);
		}
	}

	/// <summary>
	///   Loads the tagged dl.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="constructed">The constructed.</param>
	/// <returns>core.Protocol.asn1.Asn1Object.</returns>
	internal Asn1Object LoadTaggedDefiniteLength(int tagClass, int tagNo, bool constructed)
	{
		if (!constructed)
		{
			var contentsOctets = ((DefiniteLengthInputStream) _baseStream).ToArray();
			return Asn1TaggedObject.CreatePrimitive(tagClass, tagNo, contentsOctets);
		}

		var contentsElements = LoadVector();
		return Asn1TaggedObject.CreateConstructedDL(tagClass, tagNo, contentsElements);
	}

	/// <summary>
	///   Loads the tagged il.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1Object.</returns>
	internal Asn1Object LoadTaggedIndefiniteLength(int tagClass, int tagNo)
	{
		var contentsElements = LoadVector();
		return Asn1TaggedObject.CreateConstructedIL(tagClass, tagNo, contentsElements);
	}

	/// <summary>
	///   Parses the implicit constructed dl.
	/// </summary>
	/// <param name="univTagNo">The univ tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	internal IAsn1Convertable ParseImplicitConstructedDefiniteLength(int univTagNo)
		=> univTagNo switch
		   {
			   Asn1Tags.BitString   => new BerBitStringParser(this),
			   Asn1Tags.External    => new DerExternalParser(this),
			   Asn1Tags.OctetString => new BerOctetStringParser(this),
			   Asn1Tags.Set         => new DerSetParser(this),
			   Asn1Tags.Sequence    => new DerSequenceParser(this),
			   _                    => throw new Asn1Exception($"unknown DL object encountered: 0x{univTagNo:X}")
			   // TODO[asn1] DLExternalParser
			   // TODO[asn1] DLConstructedOctetStringParser
			   // TODO[asn1] DLSetParser
			   // TODO[asn1] DLSequenceParser
			   // TODO[asn1] DLConstructedBitStringParser
		   };

	/// <summary>
	///   Parses the implicit constructed il.
	/// </summary>
	/// <param name="univTagNo">The univ tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	internal IAsn1Convertable ParseImplicitConstructedIndefiniteLength(int univTagNo)
		=> univTagNo switch
		   {
			   Asn1Tags.BitString   => new BerBitStringParser(this),
			   Asn1Tags.External    => new DerExternalParser(this),
			   Asn1Tags.OctetString => new BerOctetStringParser(this),
			   Asn1Tags.Sequence    => new BerSequenceParser(this),
			   Asn1Tags.Set         => new BerSetParser(this),
			   _                    => throw new Asn1Exception($"unknown BER object encountered: 0x{univTagNo:X}")
			   // TODO[asn1] BERExternalParser
		   };


	/// <summary>
	///   Parses the implicit primitive.
	/// </summary>
	/// <param name="univTagNo">The univ tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	internal IAsn1Convertable ParseImplicitPrimitive(int univTagNo) => ParseImplicitPrimitive(univTagNo, (DefiniteLengthInputStream) _baseStream);

	/// <summary>
	///   Parses the implicit primitive.
	/// </summary>
	/// <param name="univTagNo">The univ tag no.</param>
	/// <param name="defIn">The definition in.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	/// <exception cref="Asn1Exception">externals must use constructed encoding (see X.690 8.18)</exception>
	/// <exception cref="Asn1Exception">sequences must use constructed encoding (see X.690 8.9.1/8.10.1)</exception>
	/// <exception cref="Asn1Exception">sets must use constructed encoding (see X.690 8.11.1/8.12.1)</exception>
	/// <exception cref="Asn1Exception">corrupted stream detected, e</exception>
	internal IAsn1Convertable ParseImplicitPrimitive(int univTagNo, DefiniteLengthInputStream defIn)
	{
		// Some primitive encodings can be handled by parsers too...
		switch (univTagNo)
		{
			case Asn1Tags.BitString:   return new DefiniteLengthBitStringParser(defIn);
			case Asn1Tags.External:    throw new Asn1Exception("externals must use constructed encoding (see X.690 8.18)");
			case Asn1Tags.OctetString: return new DerOctetStringParser(defIn);
			case Asn1Tags.Set:         throw new Asn1Exception("sequences must use constructed encoding (see X.690 8.9.1/8.10.1)");
			case Asn1Tags.Sequence:    throw new Asn1Exception("sets must use constructed encoding (see X.690 8.11.1/8.12.1)");
		}

		try
		{
			return Asn1InputStream.CreatePrimitiveDerObject(univTagNo, defIn, _tmpBuffers);
		}
		catch (ArgumentException e)
		{
			throw new Asn1Exception("corrupted stream detected", e);
		}
	}

	/// <summary>
	///   Parses the object.
	/// </summary>
	/// <param name="univTagNo">The univ tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable?.</returns>
	/// <exception cref="ArgumentException">$"invalid universal tag number: {univTagNo}, nameof(univTagNo)</exception>
	/// <exception cref="IOException">$"unexpected identifier encountered: {tagHdr}</exception>
	internal IAsn1Convertable? ParseObject(int univTagNo)
	{
		if (univTagNo is < 0 or > 30) throw new ArgumentException($"invalid universal tag number: {univTagNo}", nameof(univTagNo));

		var tagHdr = _baseStream.ReadByte();
		if (tagHdr < 0) return null;

		if ((tagHdr & ~Asn1Tags.Constructed) != univTagNo) throw new IOException($"unexpected identifier encountered: {tagHdr}");

		return ImplParseObject(tagHdr);
	}

	/// <summary>
	///   Parses the tagged object.
	/// </summary>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser?.</returns>
	/// <exception cref="Asn1Exception">no tagged object found</exception>
	internal IAsn1TaggedObjectParser? ParseTaggedObject()
	{
		var tagHdr = _baseStream.ReadByte();
		if (tagHdr < 0) return null;

		var tagClass = tagHdr & Asn1Tags.Private;
		if (0 == tagClass) throw new Asn1Exception("no tagged object found");

		return (IAsn1TaggedObjectParser) ImplParseObject(tagHdr);
	}

	/// <summary>
	///   Loads the vector.
	/// </summary>
	/// <returns>core.Protocol.asn1.Asn1EncodableVector.</returns>
	internal Asn1EncodableVector LoadVector()
	{
		var tagHdr = _baseStream.ReadByte();
		if (tagHdr < 0) return new Asn1EncodableVector(0);

		var v = new Asn1EncodableVector();
		do
		{
			var obj = ImplParseObject(tagHdr);

			v.Add(obj.ToAsn1Object());
		} while ((tagHdr = _baseStream.ReadByte()) >= 0);

		return v;
	}

	/// <summary>
	///   Set00s the check.
	/// </summary>
	/// <param name="enabled">The enabled.</param>
	private void Set00Check(bool enabled)
	{
		switch (_baseStream)
		{
			case IndefiniteLengthInputStream indef:
				indef.SetEofOn00(enabled);
				break;
		}
	}
}
