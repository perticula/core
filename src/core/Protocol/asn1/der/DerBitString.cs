// perticula - core - DerBitString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Text;
using core.Protocol.asn1.dl;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerBitString.
///   Implements the <see cref="core.Protocol.asn1.der.DerString" />
///   Implements the <see cref="core.Protocol.asn1.IAsn1BitStringParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.der.DerString" />
/// <seealso cref="core.Protocol.asn1.IAsn1BitStringParser" />
public class DerBitString : DerString, IAsn1BitStringParser
{
	/// <summary>
	///   The table
	/// </summary>
	private static readonly char[] Table = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

	/// <summary>
	///   The contents
	/// </summary>
	internal readonly byte[] Contents;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	/// <exception cref="System.ArgumentException">pad bits cannot be greater than 7 or less than 0 - padBits</exception>
	public DerBitString(byte data, int padBits)
	{
		if (padBits is > 7 or < 0) throw new ArgumentException("pad bits cannot be greater than 7 or less than 0", nameof(padBits));

		Contents = new[] {(byte) padBits, data};
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	public DerBitString(byte[] data) : this(data, 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	/// <exception cref="System.ArgumentNullException">data</exception>
	/// <exception cref="System.ArgumentException">must be in the range 0 to 7 - padBits</exception>
	/// <exception cref="System.ArgumentException">if 'data' is empty, 'padBits' must be 0</exception>
	public DerBitString(byte[] data, int padBits)
	{
		if (data == null) throw new ArgumentNullException(nameof(data));
		if (padBits is < 0 or > 7) throw new ArgumentException("must be in the range 0 to 7", nameof(padBits));
		if (data.Length == 0 && padBits != 0) throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");

		Contents = Arrays.Prepend(data, (byte) padBits);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="obj">The object.</param>
	public DerBitString(Asn1Encodable obj) : this(obj.GetDerEncoded() ?? new ReadOnlySpan<byte>(0)) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="namedBits">The named bits.</param>
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

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	public DerBitString(ReadOnlySpan<byte> data) : this(data, 0) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="padBits">The pad bits.</param>
	/// <exception cref="System.ArgumentException">must be in the range 0 to 7 - padBits</exception>
	/// <exception cref="System.ArgumentException">if 'data' is empty, 'padBits' must be 0</exception>
	public DerBitString(ReadOnlySpan<byte> data, int padBits)
	{
		if (padBits is < 0 or > 7) throw new ArgumentException("must be in the range 0 to 7", nameof(padBits));
		if (data.IsEmpty && padBits != 0) throw new ArgumentException("if 'data' is empty, 'padBits' must be 0");

		Contents = Arrays.Prepend(data.ToArray(), (byte) padBits);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBitString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="check">if set to <c>true</c> [check].</param>
	/// <exception cref="System.ArgumentNullException">contents</exception>
	/// <exception cref="System.ArgumentException">cannot be empty - contents</exception>
	/// <exception cref="System.ArgumentException">zero length data with non-zero pad bits - contents</exception>
	/// <exception cref="System.ArgumentException">pad bits cannot be greater than 7 or less than 0 - contents</exception>
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

	/// <summary>
	///   Gets the int value.
	/// </summary>
	/// <value>The int value.</value>
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

	/// <summary>
	///   Gets the parser.
	/// </summary>
	/// <value>The parser.</value>
	public IAsn1BitStringParser Parser => this;

	/// <summary>
	///   Gets the pad bits.
	///   Return the number of pad bits, if any, in the final byte, if any, read from <see cref="GetBitStream" />.
	/// </summary>
	/// <value>The number of pad bits. In the range zero to seven.</value>
	/// <remarks>
	///   This number is in the range zero to seven. That number of the least significant bits of the final byte, if
	///   any, are not part of the contents and should be ignored.
	///   NOTE: Must be called AFTER the stream has been fully processed.
	///   Does not need to be called if <see cref="GetOctetStream" /> was used instead of   <see cref="GetBitStream" />.
	/// </remarks>
	public virtual int PadBits => Contents[0];

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	public Stream GetBitStream() => new MemoryStream(Contents, 1, Contents.Length - 1, false);

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	/// <exception cref="System.IO.IOException">expected octet-aligned bitstring, but found padBits: " + padBits</exception>
	public Stream GetOctetStream()
	{
		var padBits = Contents[0] & 0xFF;
		if (0 != padBits)
			throw new IOException("expected octet-aligned bitstring, but found padBits: " + padBits);

		return GetBitStream();
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>DerBitString.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">failed to construct BIT STRING from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()}</exception>
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

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <returns>System.Nullable&lt;DerBitString&gt;.</returns>
	public static DerBitString? GetInstance(Asn1TaggedObject obj, bool isExplicit)
	{
		var o = obj.GetObject();

		if (isExplicit || o is DerBitString) return GetInstance(o);

		// Not copied because assumed to be a tagged implicit primitive from the parser
		return CreatePrimitive(((Asn1OctetString) o).GetOctets());
	}

	/// <summary>
	///   Gets the octets.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	/// <exception cref="System.InvalidOperationException">attempt to get non-octet aligned data from BIT STRING</exception>
	public virtual byte[] GetOctets()
	{
		if (Contents[0] != 0) throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
		return Arrays.CopyOfRange(Contents, 1, Contents.Length);
	}

	/// <summary>
	///   Gets the octets memory.
	/// </summary>
	/// <returns>ReadOnlyMemory&lt;System.Byte&gt;.</returns>
	/// <exception cref="System.InvalidOperationException">attempt to get non-octet aligned data from BIT STRING</exception>
	internal ReadOnlyMemory<byte> GetOctetsMemory()
	{
		if (Contents[0] != 0) throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
		return Contents.AsMemory(1);
	}

	/// <summary>
	///   Gets the octets span.
	/// </summary>
	/// <returns>ReadOnlySpan&lt;System.Byte&gt;.</returns>
	/// <exception cref="System.InvalidOperationException">attempt to get non-octet aligned data from BIT STRING</exception>
	internal ReadOnlySpan<byte> GetOctetsSpan()
	{
		if (Contents[0] != 0) throw new InvalidOperationException("attempt to get non-octet aligned data from BIT STRING");
		return Contents.AsSpan(1);
	}

	/// <summary>
	///   Gets the bytes.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	public virtual byte[] GetBytes()
	{
		if (Contents.Length == 1) return Asn1OctetString.EmptyOctets;

		int padBits = Contents[0];
		var rv      = Arrays.CopyOfRange(Contents, 1, Contents.Length);
		// DER requires pad bits to be zero
		rv[^1] &= (byte) (0xFF << padBits);
		return rv;
	}

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
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

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
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

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
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

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
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

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
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

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

	/// <summary>
	///   Gets the current string value.
	/// </summary>
	/// <returns>System.String.</returns>
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

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>DerBitString.</returns>
	/// <exception cref="System.ArgumentException">truncated BIT STRING detected - contents</exception>
	/// <exception cref="System.ArgumentException">invalid pad bits detected - contents</exception>
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
				return new DefinteLengthBitString(contents, false);
		}

		return new DerBitString(contents, false);
	}

	/// <summary>
	///   Class Meta.
	///   Implements the <see cref="core.Protocol.asn1.Asn1UniversalType" />
	/// </summary>
	/// <seealso cref="core.Protocol.asn1.Asn1UniversalType" />
	internal class Meta : Asn1UniversalType
	{
		/// <summary>
		///   The instance
		/// </summary>
		internal static readonly Asn1UniversalType Instance = new Meta();

		/// <summary>
		///   Prevents a default instance of the <see cref="Meta" /> class from being created.
		/// </summary>
		private Meta() : base(typeof(DerBitString), Asn1Tags.BitString) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());

		/// <summary>
		///   Froms the implicit constructed.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence.ToAsn1BitString();
	}
}
