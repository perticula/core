// perticula - core - DerBitString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Text;

namespace core.Protocol.asn1.der;

public class DerBitString : DerString, IAsn1BitStringParser
{
	private static readonly char[] Table = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
	internal readonly       byte[] Contents;

	public DerBitString(byte data, int padBits)
	{
		if (padBits is > 7 or < 0) throw new ArgumentException("pad bits cannot be greater than 7 or less than 0", nameof(padBits));

		Contents = new[] {(byte) padBits, data};
	}

	public DerBitString(byte[] data) : this(data, 0) { }

	public DerBitString(byte[] data, int padBits)
	{
		if (data == null) throw new ArgumentNullException(nameof(data));
		if (padBits is < 0 or > 7) throw new ArgumentException("must be in the range 0 to 7", nameof(padBits));
		if (data.Length == 0 && padBits != 0) throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");

		Contents = Arrays.Prepend(data, (byte) padBits);
	}

	public DerBitString(Asn1Encodable obj) : this(obj.GetDerEncoded() ?? new ReadOnlySpan<byte>(0)) { }

	public DerBitString(int namedBits)
	{
		if (namedBits == 0)
		{
			Contents = new byte[] {0};
			return;
		}

		var bits  = 32 - Integers.NumberOfLeadingZeros(namedBits);
		var bytes = (bits + 7) / 8;
		Debug.Assert(bytes is > 0 and <= 4);

		var data = new byte[1 + bytes];

		for (var i = 1; i < bytes; i++)
		{
			data[i]   =   (byte) namedBits;
			namedBits >>= 8;
		}

		Debug.Assert((namedBits & 0xFF) != 0);
		data[bytes] = (byte) namedBits;

		var padBits = 0;
		while ((namedBits & (1 << padBits)) == 0) ++padBits;

		Debug.Assert(padBits < 8);
		data[0] = (byte) padBits;

		Contents = data;
	}

	public DerBitString(ReadOnlySpan<byte> data) : this(data, 0) { }

	public DerBitString(ReadOnlySpan<byte> data, int padBits)
	{
		if (padBits is < 0 or > 7) throw new ArgumentException("must be in the range 0 to 7", nameof(padBits));
		if (data.IsEmpty && padBits != 0) throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");

		Contents = Arrays.Prepend(data.ToArray(), (byte) padBits);
	}

	internal DerBitString(byte[] contents, bool check)
	{
		if (check)
		{
			if (contents        == null) throw new ArgumentNullException(nameof(contents));
			if (contents.Length < 1) throw new ArgumentException("cannot be empty", nameof(contents));

			int padBits = contents[0];
			if (padBits > 0)
			{
				if (contents.Length < 2) throw new ArgumentException("zero length data with non-zero pad bits",          nameof(contents));
				if (padBits         > 7) throw new ArgumentException("pad bits cannot be greater than 7 or less than 0", nameof(contents));
			}
		}

		Contents = contents;
	}

	public virtual int IntValue
	{
		get
		{
			int value = 0, end = System.Math.Min(5, Contents.Length - 1);

			for (var i = 1; i < end; ++i) value |= Contents[i] << (8 * (i - 1));
			if (end is >= 1 and < 5)
			{
				int padBits = Contents[0];
				var der     = (byte) (Contents[end] & (0xFF << padBits));
				value |= der << (8 * (end - 1));
			}

			return value;
		}
	}

	public IAsn1BitStringParser Parser => this;

	public virtual int PadBits => Contents[0];

	public Stream GetBitStream() => new MemoryStream(Contents, 1, Contents.Length - 1, false);

	public Stream GetOctetStream()
	{
		var padBits = Contents[0] & 0xFF;
		if (0 != padBits)
			throw new IOException("expected octet-aligned bitstring, but found padBits: " + padBits);

		return GetBitStream();
	}

	public static DerBitString GetInstance(object? obj)
	{
		switch (obj)
		{
			case null: throw new ArgumentNullException(nameof(obj));

			case DerBitString derBitString: return derBitString;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerBitString converted) return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return GetInstance(FromByteArray(bytes));
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct BIT STRING from byte[]: {e.Message}");
				}
			default:
				throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}");
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}");
	}

	public static DerBitString? GetInstance(Asn1TaggedObject obj, bool isExplicit)
	{
		var o = obj.GetObject();

		if (isExplicit || o is DerBitString) return GetInstance(o);

		// Not copied because assumed to be a tagged implicit primitive from the parser
		return CreatePrimitive(((Asn1OctetString) o).GetOctets());
	}

	public virtual byte[] GetOctets()
	{
		if (Contents[0] != 0) throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
		return Arrays.CopyOfRange(Contents, 1, Contents.Length);
	}

	internal ReadOnlyMemory<byte> GetOctetsMemory()
	{
		if (Contents[0] != 0) throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
		return Contents.AsMemory(1);
	}

	internal ReadOnlySpan<byte> GetOctetsSpan()
	{
		if (Contents[0] != 0) throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
		return Contents.AsSpan(1);
	}

	public virtual byte[] GetBytes()
	{
		if (Contents.Length == 1) return Asn1OctetString.EmptyOctets;

		int padBits = Contents[0];
		var rv      = Arrays.CopyOfRange(Contents, 1, Contents.Length);
		// DER requires pad bits to be zero
		rv[^1] &= (byte) (0xFF << padBits);
		return rv;
	}

	internal override IAsn1Encoding GetEncoding(int encoding)
	{
		int padBits = Contents[0];
		if (padBits != 0)
		{
			var last    = Contents.Length - 1;
			var lastBer = Contents[last];
			var lastDer = (byte) (lastBer & (0xFF << padBits));

			if (lastBer != lastDer)
				return new Asn1EncodingSuffixed(Asn1Tags.Universal, Asn1Tags.BitString, Contents, lastDer);
		}

		return new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.BitString, Contents);
	}

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
	{
		int padBits = Contents[0];
		if (padBits != 0)
		{
			var last    = Contents.Length - 1;
			var lastBer = Contents[last];
			var lastDer = (byte) (lastBer & (0xFF << padBits));

			if (lastBer != lastDer)
				return new Asn1EncodingSuffixed(tagClass, tagNo, Contents, lastDer);
		}

		return new Asn1Encoding(tagClass, tagNo, Contents);
	}

	internal sealed override DerEncoding GetEncodingDer()
	{
		int padBits = Contents[0];
		if (padBits != 0)
		{
			var last    = Contents.Length - 1;
			var lastBer = Contents[last];
			var lastDer = (byte) (lastBer & (0xFF << padBits));

			if (lastBer != lastDer)
				return new DerEncodingSuffixed(Asn1Tags.Universal, Asn1Tags.BitString, Contents, lastDer);
		}

		return new DerEncoding(Asn1Tags.Universal, Asn1Tags.BitString, Contents);
	}

	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
	{
		int padBits = Contents[0];
		if (padBits != 0)
		{
			var last    = Contents.Length - 1;
			var lastBer = Contents[last];
			var lastDer = (byte) (lastBer & (0xFF << padBits));

			if (lastBer != lastDer)
				return new DerEncodingSuffixed(tagClass, tagNo, Contents, lastDer);
		}

		return new DerEncoding(tagClass, tagNo, Contents);
	}

	protected override int Asn1GetHashCode()
	{
		if (Contents.Length < 2)
			return 1;

		int padBits = Contents[0];
		var last    = Contents.Length - 1;

		var lastDer = (byte) (Contents[last] & (0xFF << padBits));

		var hc = Arrays.GetHashCode(Contents, 0, last);
		hc *= 257;
		hc ^= lastDer;
		return hc;
	}

	protected override bool Asn1Equals(Asn1Object asn1Object)
	{
		if (asn1Object is not DerBitString that) return false;

		byte[] thisContents = Contents, thatContents = that.Contents;

		var length = thisContents.Length;
		if (thatContents.Length != length)
			return false;
		if (length == 1)
			return true;

		var last = length - 1;
		for (var i = 0; i < last; ++i)
		{
			if (thisContents[i] != thatContents[i])
				return false;
		}

		int padBits     = thisContents[0];
		var thisLastDer = (byte) (thisContents[last] & (0xFF << padBits));
		var thatLastDer = (byte) (thatContents[last] & (0xFF << padBits));

		return thisLastDer == thatLastDer;
	}

	public override string GetString()
	{
		var str = GetDerEncoded();

		var buffer = new StringBuilder(1 + str.Length * 2);
		buffer.Append('#');

		for (var i = 0; i != str.Length; i++)
		{
			uint u8 = str[i];
			buffer.Append(Table[u8 >> 4]);
			buffer.Append(Table[u8 & 0xF]);
		}

		return buffer.ToString();
	}

	internal static DerBitString CreatePrimitive(byte[] contents)
	{
		var length = contents.Length;
		if (length < 1) throw new ArgumentException("truncated BIT STRING detected", nameof(contents));

		int padBits = contents[0];
		if (padBits > 0)
		{
			if (padBits > 7 || length < 2) throw new ArgumentException("invalid pad bits detected", nameof(contents));

			var finalOctet = contents[length - 1];
			if (finalOctet != (byte) (finalOctet & (0xFF << padBits)))
				return new DLBitString(contents, false);
		}

		return new DerBitString(contents, false);
	}

	internal class Meta : Asn1UniversalType
	{
		internal static readonly Asn1UniversalType Instance = new Meta();

		private Meta() : base(typeof(DerBitString), Asn1Tags.BitString) { }

		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());

		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence.ToAsn1BitString();
	}
}
