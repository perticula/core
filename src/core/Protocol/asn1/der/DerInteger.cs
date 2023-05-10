// perticula - core - DerInteger.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Math;

namespace core.Protocol.asn1.der;

public class DerInteger : Asn1Object
{
	public const string AllowUnsafeProperty = "Asn1.AllowUnsafeInteger";

	internal const int SignExtSigned   = -1;
	internal const int SignExtUnsigned = 0xFF;

	private readonly byte[] _bytes;
	private readonly int    _start;

	public DerInteger(int value)
	{
		_bytes = BigInteger.ValueOf(value).ToByteArray();
		_start = 0;
	}

	public DerInteger(long value)
	{
		_bytes = BigInteger.ValueOf(value).ToByteArray();
		_start = 0;
	}

	public DerInteger(BigInteger value)
	{
		if (value == null)
			throw new ArgumentNullException(nameof(value));

		_bytes = value.ToByteArray();
		_start = 0;
	}

	public DerInteger(byte[] bytes)
		: this(bytes, true) { }

	internal DerInteger(byte[] bytes, bool clone)
	{
		if (IsMalformed(bytes))
			throw new ArgumentException("malformed integer", nameof(bytes));

		_bytes = clone ? Arrays.Clone(bytes) : bytes;
		_start = SignBytesToSkip(bytes);
	}

	public BigInteger PositiveValue => new(1, _bytes);

	public BigInteger Value => new(_bytes);

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

	internal static bool AllowUnsafe()
	{
		var allowUnsafeValue = Environment.GetEnvironmentVariable(AllowUnsafeProperty);
		return allowUnsafeValue != null && "true".EqualsIgnoreCase(allowUnsafeValue);
	}


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

	public static DerInteger GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerInteger) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	public bool HasValue(int x) =>
		_bytes.Length - _start                     <= 4
		&& IntValue(_bytes, _start, SignExtSigned) == x;

	public bool HasValue(long x) =>
		_bytes.Length - _start                      <= 8
		&& LongValue(_bytes, _start, SignExtSigned) == x;

	public bool HasValue(BigInteger x) =>
		// Fast check to avoid allocation
		IntValue(_bytes, _start, SignExtSigned) == x.IntValue
		&& Value.Equals(x);

	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.Integer, _bytes);

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, _bytes);

	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.Integer, _bytes);

	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, _bytes);

	protected override int Asn1GetHashCode() => Arrays.GetHashCode(_bytes);

	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerInteger other && Arrays.AreEqual(_bytes, other._bytes);

	public override string ToString() => Value.ToString();

	internal static DerInteger CreatePrimitive(byte[] contents) => new(contents, false);

	internal static int GetEncodingLength(BigInteger x) => Asn1OutputStream.GetLengthOfEncodingDL(Asn1Tags.Integer, BigIntegers.GetByteLength(x));

	internal static int IntValue(byte[] bytes, int start, int signExt)
	{
		var length = bytes.Length;
		var pos    = System.Math.Max(start, length - 4);

		var val                    = (sbyte) bytes[pos] & signExt;
		while (++pos < length) val = (val << 8) | bytes[pos];
		return val;
	}

	internal static long LongValue(byte[] bytes, int start, int signExt)
	{
		var length = bytes.Length;
		var pos    = System.Math.Max(start, length - 8);

		long val                   = (sbyte) bytes[pos] & signExt;
		while (++pos < length) val = (val << 8) | bytes[pos];
		return val;
	}

	internal static bool IsMalformed(byte[] bytes)
		=> bytes.Length switch
		   {
			   0 => true,
			   1 => false,
			   _ => (sbyte) bytes[0] == (sbyte) bytes[1] >> 7 && !AllowUnsafe()
		   };

	internal static int SignBytesToSkip(byte[] bytes)
	{
		int pos = 0, last = bytes.Length - 1;
		while (pos                   < last
		       && (sbyte) bytes[pos] == (sbyte) bytes[pos + 1] >> 7)
			++pos;
		return pos;
	}

	internal class Meta : Asn1UniversalType
	{
		internal static readonly Asn1UniversalType Instance = new Meta();

		private Meta() : base(typeof(DerInteger), Asn1Tags.Integer) { }

		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
