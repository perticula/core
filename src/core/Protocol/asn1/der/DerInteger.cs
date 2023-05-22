// perticula - core - DerInteger.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Math;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerInteger.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class DerInteger : Asn1Object
{
	/// <summary>
	///   The allow unsafe property
	/// </summary>
	public const string AllowUnsafeProperty = "Asn1.AllowUnsafeInteger";

	/// <summary>
	///   The sign ext signed
	/// </summary>
	internal const int SignExtSigned = -1;

	/// <summary>
	///   The sign ext unsigned
	/// </summary>
	internal const int SignExtUnsigned = 0xFF;

	/// <summary>
	///   The bytes
	/// </summary>
	private readonly byte[] _bytes;

	/// <summary>
	///   The start
	/// </summary>
	private readonly int _start;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerInteger" /> class.
	/// </summary>
	/// <param name="value">The value.</param>
	public DerInteger(int value)
	{
		_bytes = BigInteger.ValueOf(value).ToByteArray();
		_start = 0;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerInteger" /> class.
	/// </summary>
	/// <param name="value">The value.</param>
	public DerInteger(long value)
	{
		_bytes = BigInteger.ValueOf(value).ToByteArray();
		_start = 0;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerInteger" /> class.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <exception cref="System.ArgumentNullException">value</exception>
	public DerInteger(BigInteger value)
	{
		if (value == null)
			throw new ArgumentNullException(nameof(value));

		_bytes = value.ToByteArray();
		_start = 0;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerInteger" /> class.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public DerInteger(byte[] bytes)
		: this(bytes, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerInteger" /> class.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	/// <exception cref="System.ArgumentException">malformed integer - bytes</exception>
	internal DerInteger(byte[] bytes, bool clone)
	{
		if (IsMalformed(bytes))
			throw new ArgumentException("malformed integer", nameof(bytes));

		_bytes = clone ? Arrays.Clone(bytes) : bytes;
		_start = SignBytesToSkip(bytes);
	}

	/// <summary>
	///   Gets the positive value.
	/// </summary>
	/// <value>The positive value.</value>
	public BigInteger PositiveValue => new(1, _bytes);

	/// <summary>
	///   Gets the value.
	/// </summary>
	/// <value>The value.</value>
	public BigInteger Value => new(_bytes);

	/// <summary>
	///   Gets the int positive value exact.
	/// </summary>
	/// <value>The int positive value exact.</value>
	/// <exception cref="System.ArithmeticException">ASN.1 Integer out of positive int range</exception>
	public int IntPositiveValueExact
	{
		get
		{
			var count = _bytes.Length - _start;
			if (count > 4 || (count == 4 && 0 != (_bytes[_start] & 0x80)))
				throw new ArithmeticException("ASN.1 Integer out of positive int range");

			return IntValue(_bytes, _start, SignExtUnsigned);
		}
	}

	/// <summary>
	///   Gets the int value exact.
	/// </summary>
	/// <value>The int value exact.</value>
	/// <exception cref="System.ArithmeticException">ASN.1 Integer out of int range</exception>
	public int IntValueExact
	{
		get
		{
			var count = _bytes.Length - _start;
			if (count > 4)
				throw new ArithmeticException("ASN.1 Integer out of int range");

			return IntValue(_bytes, _start, SignExtSigned);
		}
	}

	/// <summary>
	///   Gets the long value exact.
	/// </summary>
	/// <value>The long value exact.</value>
	/// <exception cref="System.ArithmeticException">ASN.1 Integer out of long range</exception>
	public long LongValueExact
	{
		get
		{
			var count = _bytes.Length - _start;
			if (count > 8)
				throw new ArithmeticException("ASN.1 Integer out of long range");

			return LongValue(_bytes, _start, SignExtSigned);
		}
	}

	/// <summary>
	///   Allows the unsafe.
	/// </summary>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	internal static bool AllowUnsafe()
	{
		var allowUnsafeValue = Environment.GetEnvironmentVariable(AllowUnsafeProperty);
		return allowUnsafeValue != null && "true".EqualsIgnoreCase(allowUnsafeValue);
	}


	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>DerInteger.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">failed to construct integer from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in {nameof(GetInstance)}: " + obj.GetTypeName()</exception>
	public static DerInteger GetInstance(object obj)
	{
		switch (obj)
		{
			case null:
				throw new ArgumentNullException(nameof(obj));
			case DerInteger derInteger:
				return derInteger;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerInteger converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerInteger) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct integer from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in {nameof(GetInstance)}: " + obj.GetTypeName());
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>DerInteger.</returns>
	public static DerInteger GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerInteger) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Determines whether the specified x has value.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <returns><c>true</c> if the specified x has value; otherwise, <c>false</c>.</returns>
	public bool HasValue(int x) =>
		_bytes.Length - _start                     <= 4
		&& IntValue(_bytes, _start, SignExtSigned) == x;

	/// <summary>
	///   Determines whether the specified x has value.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <returns><c>true</c> if the specified x has value; otherwise, <c>false</c>.</returns>
	public bool HasValue(long x) =>
		_bytes.Length - _start                      <= 8
		&& LongValue(_bytes, _start, SignExtSigned) == x;

	/// <summary>
	///   Determines whether the specified x has value.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <returns><c>true</c> if the specified x has value; otherwise, <c>false</c>.</returns>
	public bool HasValue(BigInteger x) =>
		// Fast check to avoid allocation
		IntValue(_bytes, _start, SignExtSigned) == x.IntValue
		&& Value.Equals(x);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.Integer, _bytes);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, _bytes);

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.Integer, _bytes);

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, _bytes);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Arrays.GetHashCode(_bytes);

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerInteger other && Arrays.AreEqual(_bytes, other._bytes);

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => Value.ToString();

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>DerInteger.</returns>
	internal static DerInteger CreatePrimitive(byte[] contents) => new(contents, false);

	/// <summary>
	///   Gets the length of the encoding.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetEncodingLength(BigInteger x) => Asn1OutputStream.GetLengthOfEncodingDefiniteLength(Asn1Tags.Integer, BigIntegers.GetByteLength(x));

	/// <summary>
	///   Ints the value.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="start">The start.</param>
	/// <param name="signExt">The sign ext.</param>
	/// <returns>System.Int32.</returns>
	internal static int IntValue(byte[] bytes, int start, int signExt)
	{
		var length = bytes.Length;
		var pos    = System.Math.Max(start, length - 4);

		var val                    = (sbyte) bytes[pos] & signExt;
		while (++pos < length) val = (val << 8) | bytes[pos];
		return val;
	}

	/// <summary>
	///   Longs the value.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="start">The start.</param>
	/// <param name="signExt">The sign ext.</param>
	/// <returns>System.Int64.</returns>
	internal static long LongValue(byte[] bytes, int start, int signExt)
	{
		var length = bytes.Length;
		var pos    = System.Math.Max(start, length - 8);

		long val                   = (sbyte) bytes[pos] & signExt;
		while (++pos < length) val = (val << 8) | bytes[pos];
		return val;
	}

	/// <summary>
	///   Determines whether the specified bytes is malformed.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns><c>true</c> if the specified bytes is malformed; otherwise, <c>false</c>.</returns>
	internal static bool IsMalformed(byte[] bytes)
		=> bytes.Length switch
		   {
			   0 => true,
			   1 => false,
			   _ => (sbyte) bytes[0] == (sbyte) bytes[1] >> 7 && !AllowUnsafe()
		   };

	/// <summary>
	///   Signs the bytes to skip.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>System.Int32.</returns>
	internal static int SignBytesToSkip(byte[] bytes)
	{
		int pos = 0, last = bytes.Length - 1;
		while (pos                   < last
		       && (sbyte) bytes[pos] == (sbyte) bytes[pos + 1] >> 7)
			++pos;
		return pos;
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
		private Meta() : base(typeof(DerInteger), Asn1Tags.Integer) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
