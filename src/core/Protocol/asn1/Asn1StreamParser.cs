// perticula - core - Asn1StreamParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1;

public class Asn1StreamParser
{
	private readonly Stream _baseStream;
	private readonly int    _limit;

	private readonly byte[][] _tmpBuffers;

	public Asn1StreamParser(Stream input) : this(input, Asn1InputStream.FindLimit(input)) { }

	public Asn1StreamParser(byte[] encoding) : this(new MemoryStream(encoding, false), encoding.Length) { }

	public Asn1StreamParser(Stream input, int limit) : this(input, limit, new byte[16][]) { }

	internal Asn1StreamParser(Stream input, int limit, byte[][] tmpBuffers)
	{
		if (!input.CanRead) throw new ArgumentException("Expected stream to be readable", nameof(input));

		_baseStream = input;
		_limit      = limit;
		_tmpBuffers = tmpBuffers;
	}

	public virtual IAsn1Convertable? ReadObject()
	{
		var tagHdr = _baseStream.ReadByte();
		return tagHdr < 0 ? null : ImplParseObject(tagHdr);
	}

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
			if (0 != tagClass) return new BerTaggedObjectParser(tagClass, tagNo, sp);

			return sp.ParseImplicitConstructedIL(tagNo);
		}
		else
		{
			var defIn = new DefiniteLengthInputStream(_baseStream, length, _limit);

			if (0 == (tagHdr & Asn1Tags.Flags))
				return ParseImplicitPrimitive(tagNo, defIn);

			var sp = new Asn1StreamParser(defIn, defIn.Remaining, _tmpBuffers);

			var tagClass = tagHdr & Asn1Tags.Private;
			if (0 != tagClass)
			{
				var isConstructed = (tagHdr & Asn1Tags.Constructed) != 0;

				return new DLTaggedObjectParser(tagClass, tagNo, isConstructed, sp);
			}

			return sp.ParseImplicitConstructedDL(tagNo);
		}
	}

	internal Asn1Object LoadTaggedDL(int tagClass, int tagNo, bool constructed)
	{
		if (!constructed)
		{
			var contentsOctets = ((DefiniteLengthInputStream) _baseStream).ToArray();
			return Asn1TaggedObject.CreatePrimitive(tagClass, tagNo, contentsOctets);
		}

		var contentsElements = LoadVector();
		return Asn1TaggedObject.CreateConstructedDL(tagClass, tagNo, contentsElements);
	}

	internal Asn1Object LoadTaggedIL(int tagClass, int tagNo)
	{
		var contentsElements = LoadVector();
		return Asn1TaggedObject.CreateConstructedIL(tagClass, tagNo, contentsElements);
	}

	internal IAsn1Convertable ParseImplicitConstructedDL(int univTagNo)
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

	internal IAsn1Convertable ParseImplicitConstructedIL(int univTagNo)
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


	internal IAsn1Convertable ParseImplicitPrimitive(int univTagNo) => ParseImplicitPrimitive(univTagNo, (DefiniteLengthInputStream) _baseStream);

	internal IAsn1Convertable ParseImplicitPrimitive(int univTagNo, DefiniteLengthInputStream defIn)
	{
		// Some primitive encodings can be handled by parsers too...
		switch (univTagNo)
		{
			case Asn1Tags.BitString:   return new DLBitStringParser(defIn);
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

	internal IAsn1Convertable? ParseObject(int univTagNo)
	{
		if (univTagNo is < 0 or > 30) throw new ArgumentException($"invalid universal tag number: {univTagNo}", nameof(univTagNo));

		var tagHdr = _baseStream.ReadByte();
		if (tagHdr < 0) return null;

		if ((tagHdr & ~Asn1Tags.Constructed) != univTagNo) throw new IOException($"unexpected identifier encountered: {tagHdr}");

		return ImplParseObject(tagHdr);
	}

	internal IAsn1TaggedObjectParser? ParseTaggedObject()
	{
		var tagHdr = _baseStream.ReadByte();
		if (tagHdr < 0) return null;

		var tagClass = tagHdr & Asn1Tags.Private;
		if (0 == tagClass) throw new Asn1Exception("no tagged object found");

		return (IAsn1TaggedObjectParser) ImplParseObject(tagHdr);
	}

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
