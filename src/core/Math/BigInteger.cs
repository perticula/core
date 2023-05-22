// perticula - core - BigInteger.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using core.Cryptography;
using core.Random;

namespace core.Math;

/// <summary>
///   Class BigInteger. This class cannot be inherited.
///   Implements the <see cref="System.IComparable" />
///   Implements the <see cref="System.IComparable`1" />
///   Implements the <see cref="System.IEquatable`1" />
/// </summary>
/// <seealso cref="System.IComparable" />
/// <seealso cref="System.IComparable`1" />
/// <seealso cref="System.IEquatable`1" />
[Serializable]
public sealed class BigInteger : IComparable, IComparable<BigInteger>, IEquatable<BigInteger>
{
	/// <summary>
	///   The integer mask
	/// </summary>
	private const long IntegerMask = 0xFFFFFFFFL;

	/// <summary>
	///   The unsigned integer mask
	/// </summary>
	private const ulong UnsignedIntegerMask = 0xFFFFFFFFUL;

	/// <summary>
	///   The chunk2
	/// </summary>
	private const int Chunk2 = 1;

	/// <summary>
	///   The chunk8
	/// </summary>
	private const int Chunk8 = 1;

	/// <summary>
	///   The chunk10
	/// </summary>
	private const int Chunk10 = 19;

	/// <summary>
	///   The chunk16
	/// </summary>
	private const int Chunk16 = 16;

	/// <summary>
	///   The bits per byte
	/// </summary>
	private const int BitsPerByte = 8;

	/// <summary>
	///   The bits per int
	/// </summary>
	private const int BitsPerInt = 32;

	/// <summary>
	///   The bytes per int
	/// </summary>
	private const int BytesPerInt = 4;

	/// <summary>
	///   The prime products
	/// </summary>
	internal static readonly int[] PrimeProducts;

	/// <summary>
	///   The zero magnitude
	/// </summary>
	private static readonly uint[] ZeroMagnitude = Array.Empty<uint>();

	/// <summary>
	///   The zero encoding
	/// </summary>
	private static readonly byte[] ZeroEncoding = Array.Empty<byte>();

	/// <summary>
	///   The small constants
	/// </summary>
	private static readonly BigInteger[] SmallConstants = new BigInteger[17];

	/// <summary>
	///   The zero
	/// </summary>
	public static readonly BigInteger Zero;

	/// <summary>
	///   The one
	/// </summary>
	public static readonly BigInteger One;

	/// <summary>
	///   The two
	/// </summary>
	public static readonly BigInteger Two;

	/// <summary>
	///   The three
	/// </summary>
	public static readonly BigInteger Three;

	/// <summary>
	///   The four
	/// </summary>
	public static readonly BigInteger Four;

	/// <summary>
	///   The ten
	/// </summary>
	public static readonly BigInteger Ten;

	/// <summary>
	///   The bit length table
	/// </summary>
	private static readonly byte[] BitLengthTable =
	{
		0, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4,
		5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
		6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
		6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
		7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
		7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
		7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
		7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
		8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8
	};

	/// <summary>
	///   The radix2
	/// </summary>
	private static readonly BigInteger Radix2;

	/// <summary>
	///   The radix2 e
	/// </summary>
	private static readonly BigInteger Radix2E;

	/// <summary>
	///   The radix8
	/// </summary>
	private static readonly BigInteger Radix8;

	/// <summary>
	///   The radix8 e
	/// </summary>
	private static readonly BigInteger Radix8E;

	/// <summary>
	///   The radix10
	/// </summary>
	private static readonly BigInteger Radix10;

	/// <summary>
	///   The radix10 e
	/// </summary>
	private static readonly BigInteger Radix10E;

	/// <summary>
	///   The radix16
	/// </summary>
	private static readonly BigInteger Radix16;

	/// <summary>
	///   The radix16 e
	/// </summary>
	private static readonly BigInteger Radix16E;

	/*
   * These are the threshold bit-lengths (of an exponent) where we increase the window size.
   * They are calculated according to the expected savings in multiplications.
   * Some squares will also be saved on average, but we offset these against the extra storage costs.
   */
	/// <summary>
	///   The exponent window thresholds
	/// </summary>
	private static readonly int[] ExponentWindowThresholds = {7, 25, 81, 241, 673, 1793, 4609, int.MaxValue};

	/// <summary>
	///   The prime lists
	/// </summary>
	internal static readonly int[][] PrimeLists =
	{
		new[] {3, 5, 7, 11, 13, 17, 19, 23},
		new[] {29, 31, 37, 41, 43},
		new[] {47, 53, 59, 61, 67},
		new[] {71, 73, 79, 83},
		new[] {89, 97, 101, 103},

		new[] {107, 109, 113, 127},
		new[] {131, 137, 139, 149},
		new[] {151, 157, 163, 167},
		new[] {173, 179, 181, 191},
		new[] {193, 197, 199, 211},

		new[] {223, 227, 229},
		new[] {233, 239, 241},
		new[] {251, 257, 263},
		new[] {269, 271, 277},
		new[] {281, 283, 293},

		new[] {307, 311, 313},
		new[] {317, 331, 337},
		new[] {347, 349, 353},
		new[] {359, 367, 373},
		new[] {379, 383, 389},

		new[] {397, 401, 409},
		new[] {419, 421, 431},
		new[] {433, 439, 443},
		new[] {449, 457, 461},
		new[] {463, 467, 479},

		new[] {487, 491, 499},
		new[] {503, 509, 521},
		new[] {523, 541, 547},
		new[] {557, 563, 569},
		new[] {571, 577, 587},

		new[] {593, 599, 601},
		new[] {607, 613, 617},
		new[] {619, 631, 641},
		new[] {643, 647, 653},
		new[] {659, 661, 673},

		new[] {677, 683, 691},
		new[] {701, 709, 719},
		new[] {727, 733, 739},
		new[] {743, 751, 757},
		new[] {761, 769, 773},

		new[] {787, 797, 809},
		new[] {811, 821, 823},
		new[] {827, 829, 839},
		new[] {853, 857, 859},
		new[] {863, 877, 881},

		new[] {883, 887, 907},
		new[] {911, 919, 929},
		new[] {937, 941, 947},
		new[] {953, 967, 971},
		new[] {977, 983, 991},

		new[] {997, 1009, 1013},
		new[] {1019, 1021, 1031},
		new[] {1033, 1039, 1049},
		new[] {1051, 1061, 1063},
		new[] {1069, 1087, 1091},

		new[] {1093, 1097, 1103},
		new[] {1109, 1117, 1123},
		new[] {1129, 1151, 1153},
		new[] {1163, 1171, 1181},
		new[] {1187, 1193, 1201},

		new[] {1213, 1217, 1223},
		new[] {1229, 1231, 1237},
		new[] {1249, 1259, 1277},
		new[] {1279, 1283, 1289}
	};

	/// <summary>
	///   The magnitude
	/// </summary>
	private readonly uint[] _magnitude; // array of uints with [0] being the most significant

	/// <summary>
	///   The sign
	/// </summary>
	private readonly int _sign; // -1 means -ve; +1 means +ve; 0 means 0;

	/// <summary>
	///   The n bit length
	/// </summary>
	[NonSerialized] private int _nBitLength = -1; // cache BitLength() value

	/// <summary>
	///   The n bits
	/// </summary>
	[NonSerialized] private int _nBits = -1; // cache BitCount() value

	/// <summary>
	///   Initializes static members of the <see cref="BigInteger" /> class.
	/// </summary>
	static BigInteger()
	{
		Zero = new BigInteger(0, ZeroMagnitude, false)
		{
			_nBits      = 0,
			_nBitLength = 0
		};

		SmallConstants[0] = Zero;
		for (uint i = 1; i < SmallConstants.Length; ++i) SmallConstants[i] = CreateUValueOf(i);

		One   = SmallConstants[1];
		Two   = SmallConstants[2];
		Three = SmallConstants[3];
		Four  = SmallConstants[4];
		Ten   = SmallConstants[10];

		Radix2  = ValueOf(2);
		Radix2E = Radix2.Pow(Chunk2);

		Radix8  = ValueOf(8);
		Radix8E = Radix8.Pow(Chunk8);

		Radix10  = ValueOf(10);
		Radix10E = Radix10.Pow(Chunk10);

		Radix16  = ValueOf(16);
		Radix16E = Radix16.Pow(Chunk16);

		PrimeProducts = new int[PrimeLists.Length];

		for (var i = 0; i < PrimeLists.Length; ++i)
		{
			var primeList                                      = PrimeLists[i];
			var product                                        = primeList[0];
			for (var j = 1; j < primeList.Length; ++j) product *= primeList[j];
			PrimeProducts[i] = product;
		}
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="signum">The signum.</param>
	/// <param name="mag">The mag.</param>
	/// <param name="checkMag">The check mag.</param>
	private BigInteger(int signum, uint[] mag, bool checkMag)
	{
		if (!checkMag)
		{
			_sign      = signum;
			_magnitude = mag;
			return;
		}

		var i = 0;
		while (i < mag.Length && mag[i] == 0) ++i;

		if (i == mag.Length)
		{
			_sign      = 0;
			_magnitude = ZeroMagnitude;
		}
		else
		{
			_sign = signum;

			if (i == 0)
			{
				_magnitude = mag;
			}
			else
			{
				// strip leading 0 words
				_magnitude = new uint[mag.Length - i];
				Array.Copy(mag, i, _magnitude, 0, _magnitude.Length);
			}
		}
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="value">The value.</param>
	public BigInteger(string? value) : this(value, 10) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="str">The string.</param>
	/// <param name="radix">The radix.</param>
	/// <exception cref="ArgumentNullException">nameof(str)</exception>
	/// <exception cref="FormatException">Zero length BigInteger</exception>
	/// <exception cref="FormatException">Only bases 2, 8, 10, or 16 allowed</exception>
	/// <exception cref="FormatException">Bad character in radix 2 string: " + s</exception>
	/// <exception cref="FormatException">Bad character in radix 8 string: " + s</exception>
	public BigInteger(string? str, int radix)
	{
		if (str == null)
			throw new ArgumentNullException(nameof(str));
		if (str.Length == 0)
			throw new FormatException("Zero length BigInteger");

		NumberStyles style;
		int          chunk;
		BigInteger   r;
		BigInteger   rE;

		switch (radix)
		{
			case 2:
				// Is there anyway to restrict to binary digits?
				style = NumberStyles.Integer;
				chunk = Chunk2;
				r     = Radix2;
				rE    = Radix2E;
				break;
			case 8:
				// Is there anyway to restrict to octal digits?
				style = NumberStyles.Integer;
				chunk = Chunk8;
				r     = Radix8;
				rE    = Radix8E;
				break;
			case 10:
				// This style seems to handle spaces and minus sign already (our processing redundant?)
				style = NumberStyles.Integer;
				chunk = Chunk10;
				r     = Radix10;
				rE    = Radix10E;
				break;
			case 16:
				// TODO Should this be HexNumber?
				style = NumberStyles.AllowHexSpecifier;
				chunk = Chunk16;
				r     = Radix16;
				rE    = Radix16E;
				break;
			default:
				throw new FormatException("Only bases 2, 8, 10, or 16 allowed");
		}


		var index = 0;
		_sign = 1;

		if (str[0] == '-')
		{
			if (str.Length == 1)
				throw new FormatException("Zero length BigInteger");

			_sign = -1;
			index = 1;
		}

		// strip leading zeros from the string str
		while (index < str.Length && int.Parse(str[index].ToString(), style) == 0) index++;

		if (index >= str.Length)
		{
			// zero value - we're done
			_sign      = 0;
			_magnitude = ZeroMagnitude;
			return;
		}

		//////
		// could we work out the max number of ints required to store
		// str.Length digits in the given base, then allocate that
		// storage in one hit?, then Generate the magnitude in one hit too?
		//////

		var b = Zero;


		var next = index + chunk;

		if (next <= str.Length)
			do
			{
				var s  = str.Substring(index, chunk);
				var i  = ulong.Parse(s, style);
				var bi = CreateUValueOf(i);

				switch (radix)
				{
					case 2:
						if (i >= 2)
							throw new FormatException("Bad character in radix 2 string: " + s);

						b = b.ShiftLeft(1);
						break;
					case 8:
						if (i >= 8)
							throw new FormatException("Bad character in radix 8 string: " + s);

						b = b.ShiftLeft(3);
						break;
					case 16:
						b = b.ShiftLeft(64);
						break;
					default:
						b = b.Multiply(rE);
						break;
				}

				b = b.Add(bi);

				index =  next;
				next  += chunk;
			} while (next <= str.Length);

		if (index < str.Length)
		{
			var s  = str[index..];
			var i  = ulong.Parse(s, style);
			var bi = CreateUValueOf(i);

			if (b._sign > 0)
			{
				if (radix == 2)
					// NB: Can't reach here since we are parsing one char at a time
					Debug.Assert(false);
				// TODO Parse all bits at once
				//						b = b.ShiftLeft(s.Length);
				else if (radix == 8)
					// NB: Can't reach here since we are parsing one char at a time
					Debug.Assert(false);
				// TODO Parse all bits at once
				//						b = b.ShiftLeft(s.Length * 3);
				else if (radix == 16)
					b = b.ShiftLeft(s.Length << 2);
				else
					b = b.Multiply(r.Pow(s.Length));

				b = b.Add(bi);
			}
			else
			{
				b = bi;
			}
		}

		_magnitude = b._magnitude;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public BigInteger(byte[] bytes) => _magnitude = InitBigEndian(bytes, 0, bytes.Length, out _sign);

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="bigEndian">The big endian.</param>
	public BigInteger(byte[] bytes, bool bigEndian) =>
		_magnitude = bigEndian
			             ? InitBigEndian(bytes, 0, bytes.Length, out _sign)
			             : InitLittleEndian(bytes, 0, bytes.Length, out _sign);

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <exception cref="FormatException">Zero length BigInteger</exception>
	public BigInteger(byte[] bytes, int offset, int length)
	{
		if (length == 0) throw new FormatException("Zero length BigInteger");

		_magnitude = InitBigEndian(bytes, offset, length, out _sign);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <param name="bigEndian">The big endian.</param>
	/// <exception cref="FormatException">Zero length BigInteger</exception>
	public BigInteger(byte[] bytes, int offset, int length, bool bigEndian)
	{
		if (length <= 0) throw new FormatException("Zero length BigInteger");

		_magnitude = bigEndian
			             ? InitBigEndian(bytes, offset, length, out _sign)
			             : InitLittleEndian(bytes, offset, length, out _sign);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="sign">The sign.</param>
	/// <param name="bytes">The bytes.</param>
	public BigInteger(int sign, byte[] bytes) : this(sign, bytes, 0, bytes.Length, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="sign">The sign.</param>
	/// <param name="bytes">The bytes.</param>
	/// <param name="bigEndian">The big endian.</param>
	public BigInteger(int sign, byte[] bytes, bool bigEndian) : this(sign, bytes, 0, bytes.Length, bigEndian) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="sign">The sign.</param>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	public BigInteger(int sign, byte[] bytes, int offset, int length) : this(sign, bytes, offset, length, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="sign">The sign.</param>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <param name="bigEndian">The big endian.</param>
	/// <exception cref="FormatException">Invalid sign value</exception>
	public BigInteger(int sign, byte[] bytes, int offset, int length, bool bigEndian)
	{
		switch (sign)
		{
			case < -1 or > 1:
				throw new FormatException("Invalid sign value");
			case 0:
				_sign      = 0;
				_magnitude = ZeroMagnitude;
				break;
			default:
				// copy bytes
				_magnitude = bigEndian
					             ? MakeMagnitudeBigEndian(bytes, offset, length)
					             : MakeMagnitudeLittleEndian(bytes, offset, length);
				_sign = _magnitude.Length < 1 ? 0 : sign;
				break;
		}
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="sign">The sign.</param>
	/// <param name="bytes">The bytes.</param>
	public BigInteger(int sign, ReadOnlySpan<byte> bytes) : this(sign, bytes, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="sign">The sign.</param>
	/// <param name="bytes">The bytes.</param>
	/// <param name="bigEndian">The big endian.</param>
	/// <exception cref="FormatException">Invalid sign value</exception>
	public BigInteger(int sign, ReadOnlySpan<byte> bytes, bool bigEndian)
	{
		switch (sign)
		{
			case < -1 or > 1:
				throw new FormatException("Invalid sign value");
			case 0:
				_sign      = 0;
				_magnitude = ZeroMagnitude;
				break;
			default:
				// copy bytes
				_magnitude = bigEndian
					             ? MakeMagnitudeBigEndian(bytes)
					             : MakeMagnitudeLittleEndian(bytes);
				_sign = _magnitude.Length < 1 ? 0 : sign;
				break;
		}
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="sizeInBits">The size in bits.</param>
	/// <param name="random">The random.</param>
	/// <exception cref="ArgumentException">sizeInBits must be non-negative</exception>
	public BigInteger(int sizeInBits, System.Random random)
	{
		if (sizeInBits < 0) throw new ArgumentException("sizeInBits must be non-negative");

		_nBits      = -1;
		_nBitLength = -1;

		if (sizeInBits == 0)
		{
			_sign      = 0;
			_magnitude = ZeroMagnitude;
			return;
		}

		var nBytes = GetBytesLength(sizeInBits);

		var b = nBytes <= 512
			        ? stackalloc byte[nBytes]
			        : new byte[nBytes];

		random.NextBytes(b);

		// strip off any excess bits in the MSB
		var xBits = BitsPerByte * nBytes - sizeInBits;
		b[0] &= (byte) (255U >> xBits);

		_magnitude = MakeMagnitudeBigEndian(b);
		_sign      = _magnitude.Length < 1 ? 0 : 1;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BigInteger" /> class.
	/// </summary>
	/// <param name="bitLength">Length of the bit.</param>
	/// <param name="certainty">The certainty.</param>
	/// <param name="random">The random.</param>
	/// <exception cref="ArithmeticException">
	///   bitLength < 2</exception>
	public BigInteger(int bitLength, int certainty, System.Random random)
	{
		if (bitLength < 2)
			throw new ArithmeticException("bitLength < 2");

		_sign       = 1;
		_nBitLength = bitLength;

		if (bitLength == 2)
		{
			_magnitude = random.Next(2) == 0
				             ? Two._magnitude
				             : Three._magnitude;
			return;
		}

		var nBytes = GetBytesLength(bitLength);

		var b = nBytes <= 512
			        ? stackalloc byte[nBytes]
			        : new byte[nBytes];


		var xBits = BitsPerByte * nBytes - bitLength;
		var mask  = (byte) (255U >> xBits);
		var lead  = (byte) (1    << (7 - xBits));

		for (;;)
		{
			random.NextBytes(b);

			// strip off any excess bits in the MSB
			b[0] &= mask;

			// ensure the leading bit is 1 (to meet the strength requirement)
			b[0] |= lead;

			// ensure the trailing bit is 1 (i.e. must be odd)
			b[nBytes - 1] |= 1;

			_magnitude = MakeMagnitudeBigEndian(b);
			_nBits     = -1;

			if (certainty < 1)
				break;

			if (CheckProbablePrime(certainty, random, true))
				break;

			for (var j = 1; j < _magnitude.Length - 1; ++j)
			{
				_magnitude[j] ^= (uint) random.Next();

				if (CheckProbablePrime(certainty, random, true))
					return;
			}
		}
	}

	/// <summary>
	///   Gets the bit count.
	/// </summary>
	/// <value>The bit count.</value>
	public int BitCount
	{
		get
		{
			switch (_nBits)
			{
				case -1 when _sign < 0:
					// TODO Optimise this case
					_nBits = Not().BitCount;
					break;
				case -1:
				{
					var sum = 0;
					for (var i = 0; i < _magnitude.Length; ++i)
						sum += Integers.PopCount(i);
					_nBits = sum;
					break;
				}
			}

			return _nBits;
		}
	}

	/// <summary>
	///   Gets the length of the bit.
	/// </summary>
	/// <value>The length of the bit.</value>
	public int BitLength
	{
		get
		{
			if (_nBitLength == -1)
				_nBitLength = _sign == 0 ? 0 : CalcBitLength(_sign, 0, _magnitude);

			return _nBitLength;
		}
	}

	/// <summary>
	///   Gets the sign value.
	/// </summary>
	/// <value>The sign value.</value>
	public int SignValue => _sign;

	/// <summary>
	///   Gets the int value.
	/// </summary>
	/// <value>The int value.</value>
	public int IntValue
	{
		get
		{
			if (_sign == 0) return 0;

			var n = _magnitude.Length;
			var v = (int) _magnitude[n - 1];

			return _sign < 0 ? -v : v;
		}
	}

	/// <summary>
	///   Gets the int value exact.
	/// </summary>
	/// <value>The int value exact.</value>
	/// <exception cref="ArithmeticException">BigInteger out of int range</exception>
	public int IntValueExact
	{
		get
		{
			if (BitLength > 31) throw new ArithmeticException("BigInteger out of int range");
			return IntValue;
		}
	}

	/// <summary>
	///   Gets the long value.
	/// </summary>
	/// <value>The long value.</value>
	public long LongValue
	{
		get
		{
			if (_sign == 0)
				return 0;

			var n = _magnitude.Length;

			var v        = _magnitude[n - 1] & IntegerMask;
			if (n > 1) v |= (_magnitude[n - 2] & IntegerMask) << 32;

			return _sign < 0 ? -v : v;
		}
	}

	/// <summary>
	///   Gets the long value exact.
	/// </summary>
	/// <value>The long value exact.</value>
	/// <exception cref="ArithmeticException">BigInteger out of long range</exception>
	public long LongValueExact
	{
		get
		{
			if (BitLength > 63)
				throw new ArithmeticException("BigInteger out of long range");

			return LongValue;
		}
	}

	/// <summary>
	///   Compares to.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>int.</returns>
	public int CompareTo(object? obj)
	{
		if (obj == null)
			return 1;

		return obj is not BigInteger other ? throw new ArgumentException("Object is not a BigInteger", nameof(obj)) : CompareTo(other);
	}

	/// <summary>
	///   Compares to.
	/// </summary>
	/// <param name="other">The other.</param>
	/// <returns>int.</returns>
	public int CompareTo(BigInteger? other)
	{
		if (other == null) return 1;

		return _sign < other._sign
			       ? -1
			       : _sign > other._sign
				       ? 1
				       : _sign == 0
					       ? 0
					       : _sign * CompareNoLeadingZeroes(0, _magnitude, 0, other._magnitude);
	}

	/// <summary>
	///   Equalses the specified other.
	/// </summary>
	/// <param name="other">The other.</param>
	/// <returns>bool.</returns>
	public bool Equals(BigInteger? other)
	{
		if (other == null) return false;
		if (other.Equals(this)) return true;
		return _sign == other._sign && IsEqualMagnitude(other);
	}

	/// <summary>
	///   Determines whetherthis value is probably prime within the specified certainty (probability of 1 - (1/2)).
	/// </summary>
	/// <param name="certainty">The certainty.</param>
	/// <returns><c>true</c> if is probably prime ; otherwise, <c>false</c>.</returns>
	/// <remarks>
	///   <p>From Knuth Vol 2, pg 395.</p>
	/// </remarks>
	public bool IsProbablePrime(int certainty) => IsProbablePrime(certainty, false);

	/// <summary>
	///   Determines whether [is probable prime] [the specified certainty].
	/// </summary>
	/// <param name="certainty">The certainty.</param>
	/// <param name="randomlySelected">The randomly selected.</param>
	/// <returns>bool.</returns>
	internal bool IsProbablePrime(int certainty, bool randomlySelected)
	{
		if (certainty <= 0) return true;

		var n = Abs();

		if (!n.TestBit(0)) return n.Equals(Two);

		if (n.Equals(One)) return false;

		return n.CheckProbablePrime(certainty, SecureRandom.ArbitraryRandom, randomlySelected);
	}

	/// <summary>
	///   Checks the probable prime.
	/// </summary>
	/// <param name="certainty">The certainty.</param>
	/// <param name="random">The random.</param>
	/// <param name="randomlySelected">The randomly selected.</param>
	/// <returns>bool.</returns>
	private bool CheckProbablePrime(int certainty, System.Random random, bool randomlySelected)
	{
		Debug.Assert(certainty      > 0);
		Debug.Assert(CompareTo(Two) > 0);
		Debug.Assert(TestBit(0));


		// Try to reduce the penalty for really small numbers
		var numLists = System.Math.Min(BitLength - 1, PrimeLists.Length);

		for (var i = 0; i < numLists; ++i)
		{
			var test = Remainder(PrimeProducts[i]);

			var primeList = PrimeLists[i];
			for (var j = 0; j < primeList.Length; ++j)
			{
				var prime = primeList[j];
				var qRem  = test % prime;
				if (qRem == 0)
					// We may find small numbers in the list
					return BitLength < 16 && IntValue == prime;
			}
		}


		// TODO Special case for < 10^16 (RabinMiller fixed list)
		//			if (BitLength < 30)
		//			{
		//				RabinMiller against 2, 3, 5, 7, 11, 13, 23 is sufficient
		//			}


		// TODO Is it worth trying to create a hybrid of these two?
		return RabinMillerTest(certainty, random, randomlySelected);
		//			return SolovayStrassenTest(certainty, random);

		//			bool rbTest = RabinMillerTest(certainty, random);
		//			bool ssTest = SolovayStrassenTest(certainty, random);
		//
		//			Debug.Assert(rbTest == ssTest);
		//
		//			return rbTest;
	}

	/// <summary>
	///   Rabins the miller test.
	/// </summary>
	/// <param name="certainty">The certainty.</param>
	/// <param name="random">The random.</param>
	/// <returns>bool.</returns>
	public bool RabinMillerTest(int certainty, System.Random random) => RabinMillerTest(certainty, random, false);

	/// <summary>
	///   Rabins the miller test.
	/// </summary>
	/// <param name="certainty">The certainty.</param>
	/// <param name="random">The random.</param>
	/// <param name="randomlySelected">The randomly selected.</param>
	/// <returns>bool.</returns>
	internal bool RabinMillerTest(int certainty, System.Random random, bool randomlySelected)
	{
		var bits = BitLength;

		Debug.Assert(certainty > 0);
		Debug.Assert(bits      > 2);
		Debug.Assert(TestBit(0));

		var iterations = (certainty - 1) / 2 + 1;
		if (randomlySelected)
		{
			var itersFor100Cert = bits   >= 1024 ? 4
			                      : bits >= 512  ? 8
			                      : bits >= 256  ? 16
			                                       : 50;

			if (certainty < 100)
			{
				iterations = System.Math.Min(itersFor100Cert, iterations);
			}
			else
			{
				iterations -= 50;
				iterations += itersFor100Cert;
			}
		}

		// let n = 1 + d . 2^s
		var n = this;
		var s = n.GetLowestSetBitMaskFirst(uint.MaxValue << 1);
		Debug.Assert(s >= 1);
		var r = n.ShiftRight(s);

		// NOTE: Avoid conversion to/from Montgomery form and check for R/-R as result instead

		var montRadix      = One.ShiftLeft(32 * n._magnitude.Length).Remainder(n);
		var minusMontRadix = n.Subtract(montRadix);

		do
		{
			BigInteger a;
			do
				a = new BigInteger(n.BitLength, random);

			while (a._sign == 0 || a.CompareTo(n) >= 0 || a.IsEqualMagnitude(montRadix) || a.IsEqualMagnitude(minusMontRadix));

			var y = ModPowMonty(a, r, n, false);

			if (!y.Equals(montRadix))
			{
				var j = 0;
				while (!y.Equals(minusMontRadix))
				{
					if (++j == s)
						return false;

					y = ModPowMonty(y, Two, n, false);

					if (y.Equals(montRadix))
						return false;
				}
			}
		} while (--iterations > 0);

		return true;
	}

	/// <summary>
	///   Determines the maximum of the parameters.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Max(BigInteger value) => CompareTo(value) > 0 ? this : value;

	/// <summary>
	///   Determines the minimum of the parameters.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Min(BigInteger value) => CompareTo(value) < 0 ? this : value;

	/// <summary>
	///   Mods the specified m.
	/// </summary>
	/// <param name="m">The m.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Modulus must be positive</exception>
	public BigInteger Mod(BigInteger m)
	{
		if (m._sign < 1) throw new ArithmeticException("Modulus must be positive");

		var biggie = Remainder(m);

		return biggie._sign >= 0 ? biggie : biggie.Add(m);
	}


	/// <summary>
	///   Bits the length.
	/// </summary>
	/// <param name="b">The b.</param>
	/// <returns>int.</returns>
	private static int BitLen(byte b) => 32 - BitOperations.LeadingZeroCount(b);

	/// <summary>
	///   Bits the length.
	/// </summary>
	/// <param name="v">The v.</param>
	/// <returns>int.</returns>
	private static int BitLen(uint v) => 32 - BitOperations.LeadingZeroCount(v);

	/// <summary>
	///   Quicks the pow2 check.
	/// </summary>
	/// <returns>bool.</returns>
	private bool QuickPow2Check() => _sign > 0 && _nBits == 1;

	/// <summary>
	///   Mods the inverse.
	/// </summary>
	/// <param name="m">The m.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Modulus must be positive</exception>
	/// <exception cref="ArithmeticException">Numbers not relatively prime.</exception>
	public BigInteger ModInverse(BigInteger m)
	{
		if (m._sign < 1)
			throw new ArithmeticException("Modulus must be positive");

		// TODO Too slow at the moment
		// "Fast Key Exchange with Elliptic Curve Systems" R.Schoeppel


		if (m.QuickPow2Check()) return ModInversePow2(m);

		var d   = Remainder(m);
		var gcd = ExtEuclid(d, m, out var x);

		if (!gcd.Equals(One))
			throw new ArithmeticException("Numbers not relatively prime.");

		if (x._sign < 0) x = x.Add(m);

		return x;
	}

	/// <summary>
	///   Mods the inverse pow2.
	/// </summary>
	/// <param name="m">The m.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Numbers not relatively prime.</exception>
	private BigInteger ModInversePow2(BigInteger m)
	{
		Debug.Assert(m.SignValue > 0);
		Debug.Assert(m.BitCount  == 1);

		if (!TestBit(0)) throw new ArithmeticException("Numbers not relatively prime.");

		var pow = m.BitLength - 1;

		var inv64 = (long) Raw.Mod.Inverse64((ulong) LongValue);

		if (pow < 64) inv64 &= (1L << pow) - 1;

		var x = ValueOf(inv64);

		if (pow > 64)
		{
			var d = Remainder(m);

			var bitsCorrect = 64;

			do
			{
				var t = x.Multiply(d).Remainder(m);
				x           =   x.Multiply(Two.Subtract(t)).Remainder(m);
				bitsCorrect <<= 1;
			} while (bitsCorrect < pow);
		}

		if (x._sign < 0) x = x.Add(m);

		return x;
	}

	/// <summary>
	///   Calculate the numbers u1, u2, and u3 such that:
	///   u1 * a + u2 * b = u3
	///   where u3 is the greatest common divider of a and b.
	/// </summary>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <param name="u1Out">The u1 out.</param>
	/// <returns>BigInteger.</returns>
	/// <remarks>
	///   <p>using the extended Euclid algorithm (refer p. 323 of The Art of Computer Programming vol 2, 2nd ed).</p>
	///   <p>This also seems to have the side effect of calculating some form of multiplicative inverse.</p>
	/// </remarks>
	private static BigInteger ExtEuclid(BigInteger a, BigInteger b, out BigInteger u1Out)
	{
		BigInteger u1 = One, v1 = Zero;
		BigInteger u3 = a,   v3 = b;

		if (v3._sign > 0)
			for (;;)
			{
				var q = u3.DivideAndRemainder(v3);
				u3 = v3;
				v3 = q[1];

				var oldU1 = u1;
				u1 = v1;

				if (v3._sign <= 0)
					break;

				v1 = oldU1.Subtract(v1.Multiply(q[0]));
			}

		u1Out = u1;

		return u3;
	}

	/// <summary>
	///   Zeroes the out.
	/// </summary>
	/// <param name="x">The x.</param>
	private static void ZeroOut(int[] x) => Array.Clear(x, 0, x.Length);

	/// <summary>
	///   Mods the pow.
	/// </summary>
	/// <param name="e">The e.</param>
	/// <param name="m">The m.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Modulus must be positive</exception>
	public BigInteger ModPow(BigInteger e, BigInteger m)
	{
		if (m._sign < 1) throw new ArithmeticException("Modulus must be positive");
		if (m.Equals(One)) return Zero;
		if (e._sign == 0) return One;
		if (_sign   == 0) return Zero;

		var negExp    = e._sign < 0;
		if (negExp) e = e.Negate();

		var result                 = Mod(m);
		if (!e.Equals(One)) result = (m._magnitude[^1] & 1U) == 0U ? ModPowBarrett(result, e, m) : ModPowMonty(result, e, m, true);
		if (negExp) result         = result.ModInverse(m);

		return result;
	}

	/// <summary>
	///   Mods the pow monty.
	/// </summary>
	/// <param name="b">The b.</param>
	/// <param name="e">The e.</param>
	/// <param name="m">The m.</param>
	/// <param name="convert">The convert.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private static BigInteger ModPowMonty(BigInteger b, BigInteger e, BigInteger m, bool convert)
	{
		var n                 = m._magnitude.Length;
		var powR              = 32 * n;
		var smallMontyModulus = m.BitLength + 2 <= powR;
		var mDash             = m.GetMQuote();

		// tmp = this * R mod m
		if (convert) b = b.ShiftLeft(powR).Remainder(m);

		var yAccum = new uint[n + 1];

		var zVal = b._magnitude;
		Debug.Assert(zVal.Length <= n);
		if (zVal.Length < n)
		{
			var tmp = new uint[n];
			zVal.CopyTo(tmp, n - zVal.Length);
			zVal = tmp;
		}

		// Sliding window from MSW to LSW

		var extraBits = 0;

		// Filter the common case of small RSA exponents with few bits set
		if (e._magnitude.Length > 1 || e.BitCount > 2)
		{
			var expLength = e.BitLength;
			while (expLength > ExponentWindowThresholds[extraBits]) ++extraBits;
		}

		var numPowers = 1 << extraBits;
		var oddPowers = new uint[numPowers][];
		oddPowers[0] = zVal;

		var zSquared = Arrays.Clone(zVal);
		SquareMonty(yAccum, zSquared, m._magnitude, mDash, smallMontyModulus);

		for (var i = 1; i < numPowers; ++i)
		{
			oddPowers[i] = Arrays.Clone(oddPowers[i - 1]);
			MultiplyMonty(yAccum, oddPowers[i], zSquared, m._magnitude, mDash, smallMontyModulus);
		}

		var windowList = GetWindowList(e._magnitude, extraBits);
		Debug.Assert(windowList.Length > 1);

		var window = windowList[0];
		int mult   = window & 0xFF, lastZeroes = window >> 8;

		uint[] yVal;
		if (mult == 1)
		{
			yVal = zSquared;
			--lastZeroes;
		}
		else
		{
			yVal = Arrays.Clone(oddPowers[mult >> 1]);
		}

		var windowPos = 1;
		while ((window = windowList[windowPos++]) != -1)
		{
			mult = window & 0xFF;

			var bits = lastZeroes + BitLen((byte) mult);
			for (var j = 0; j < bits; ++j) SquareMonty(yAccum, yVal, m._magnitude, mDash, smallMontyModulus);

			MultiplyMonty(yAccum, yVal, oddPowers[mult >> 1], m._magnitude, mDash, smallMontyModulus);

			lastZeroes = window >> 8;
		}

		for (var i = 0; i < lastZeroes; ++i) SquareMonty(yAccum, yVal, m._magnitude, mDash, smallMontyModulus);

		if (convert)
			// Return y * R^(-1) mod m
			MontgomeryReduce(yVal, m._magnitude, mDash);
		else if (smallMontyModulus && CompareTo(0, yVal, 0, m._magnitude) >= 0) Subtract(0, yVal, 0, m._magnitude);

		return new BigInteger(1, yVal, true);
	}

	/// <summary>
	///   Mods the pow barrett.
	/// </summary>
	/// <param name="b">The b.</param>
	/// <param name="e">The e.</param>
	/// <param name="m">The m.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private static BigInteger ModPowBarrett(BigInteger b, BigInteger e, BigInteger m)
	{
		var k  = m._magnitude.Length;
		var mr = One.ShiftLeft((k + 1) << 5);
		var yu = One.ShiftLeft(k       << 6).Divide(m);

		// Sliding window from MSW to LSW
		int extraBits = 0, expLength = e.BitLength;
		while (expLength > ExponentWindowThresholds[extraBits]) ++extraBits;

		var numPowers = 1 << extraBits;
		var oddPowers = new BigInteger[numPowers];
		oddPowers[0] = b;

		var b2 = ReduceBarrett(b.Square(), m, mr, yu);

		for (var i = 1; i < numPowers; ++i) oddPowers[i] = ReduceBarrett(oddPowers[i - 1].Multiply(b2), m, mr, yu);

		var windowList = GetWindowList(e._magnitude, extraBits);
		Debug.Assert(windowList.Length > 0);

		var window = windowList[0];
		int mult   = window & 0xFF, lastZeroes = window >> 8;

		BigInteger y;
		if (mult == 1)
		{
			y = b2;
			--lastZeroes;
		}
		else
		{
			y = oddPowers[mult >> 1];
		}

		var windowPos = 1;
		while ((window = windowList[windowPos++]) != -1)
		{
			mult = window & 0xFF;

			var bits                         = lastZeroes + BitLen((byte) mult);
			for (var j = 0; j < bits; ++j) y = ReduceBarrett(y.Square(), m, mr, yu);

			y = ReduceBarrett(y.Multiply(oddPowers[mult >> 1]), m, mr, yu);

			lastZeroes = window >> 8;
		}

		for (var i = 0; i < lastZeroes; ++i) y = ReduceBarrett(y.Square(), m, mr, yu);

		return y;
	}

	/// <summary>
	///   Reduces the barrett.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="m">The m.</param>
	/// <param name="mr">The mr.</param>
	/// <param name="yu">The yu.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private static BigInteger ReduceBarrett(BigInteger x, BigInteger m, BigInteger mr, BigInteger yu)
	{
		int xLen = x.BitLength, mLen = m.BitLength;
		if (xLen < mLen)
			return x;

		if (xLen - mLen > 1)
		{
			var k = m._magnitude.Length;

			var q1 = x.DivideWords(k - 1);
			var q2 = q1.Multiply(yu); // TODO Only need partial multiplication here
			var q3 = q2.DivideWords(k + 1);

			var r1 = x.RemainderWords(k + 1);
			var r2 = q3.Multiply(m); // TODO Only need partial multiplication here
			var r3 = r2.RemainderWords(k + 1);

			x = r1.Subtract(r3);
			if (x._sign < 0) x = x.Add(mr);
		}

		while (x.CompareTo(m) >= 0) x = x.Subtract(m);

		return x;
	}

	/// <summary>
	///   Multiplies the specified x.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <param name="z">The z.</param>
	/// <returns>uint[].</returns>
	private static uint[] Multiply(uint[] x, IReadOnlyList<uint> y, IReadOnlyList<uint> z)
	{
		var i = z.Count;

		if (i < 1)
			return x;

		var xBase = x.Length - y.Count;

		do
		{
			var  a   = z[--i] & IntegerMask;
			long val = 0;

			if (a != 0)
				for (var j = y.Count - 1; j >= 0; j--)
				{
					val += a * (y[j] & IntegerMask) + (x[xBase + j] & IntegerMask);

					x[xBase + j] = (uint) val;

					val >>>= 32;
				}

			--xBase;

			if (xBase >= 0)
				x[xBase] = (uint) val;
			else
				Debug.Assert(val == 0L);
		} while (i > 0);

		return x;
	}

	/// <summary>
	///   Gets the m quote.
	/// </summary>
	/// <returns>uint.</returns>
	private uint GetMQuote()
	{
		Debug.Assert(_sign > 0);

		var d = 0U - _magnitude[^1];

		Debug.Assert((d & 1U) != 0U);

		return Raw.Mod.Inverse32(d);
	}

	/// <summary>
	///   return w with w = x * x - w is assumed to have enough space.
	/// </summary>
	/// <param name="w">The w.</param>
	/// <param name="x">The x.</param>
	/// <returns>System.UInt32[].</returns>
	private static uint[] Square(uint[] w, IReadOnlyList<uint> x)
	{
		// Note: this method allows w to be only (2 * x.Length - 1) words if result will fit
		//			if (w.Length != 2 * x.Length)
		//				throw new ArgumentException("no I don't think so...");

		ulong c;

		var wBase = w.Length - 1;

		for (var i = x.Count - 1; i > 0; --i)
		{
			ulong v = x[i];

			c        =   v * v + w[wBase];
			w[wBase] =   (uint) c;
			c        >>= 32;

			for (var j = i - 1; j >= 0; --j)
			{
				var prod = v * x[j];

				c        += (w[--wBase] & UnsignedIntegerMask) + ((uint) prod << 1);
				w[wBase] =  (uint) c;
				c        =  (c >> 32) + (prod >> 31);
			}

			c        += w[--wBase];
			w[wBase] =  (uint) c;

			if (--wBase >= 0)
				w[wBase] = (uint) (c >> 32);
			else
				Debug.Assert(c >> 32 == 0);

			wBase += i;
		}

		c = x[0];

		c        = c * c + w[wBase];
		w[wBase] = (uint) c;

		if (--wBase >= 0)
			w[wBase] += (uint) (c >> 32);
		else
			Debug.Assert(c >> 32 == 0);

		return w;
	}

	/// <summary>
	///   Gets the window list.
	/// </summary>
	/// <param name="mag">The mag.</param>
	/// <param name="extraBits">The extra bits.</param>
	/// <returns>int[].</returns>
	private static int[] GetWindowList(IReadOnlyList<uint> mag, int extraBits)
	{
		var v = (int) mag[0];
		Debug.Assert(v != 0);

		var leadingBits = BitLen((uint) v);

		var resultSize = (((mag.Count - 1) << 5) + leadingBits) / (1 + extraBits) + 2;
		var result     = new int[resultSize];
		var resultPos  = 0;

		var bitPos = 33 - leadingBits;
		v <<= bitPos;

		int mult   = 1, multLimit = 1 << extraBits;
		var zeroes = 0;

		var i = 0;
		for (;;)
		{
			for (; bitPos < 32; ++bitPos)
			{
				if (mult < multLimit)
				{
					mult = (mult << 1) | (v >>> 31);
				}
				else if (v < 0)
				{
					result[resultPos++] = CreateWindowEntry(mult, zeroes);
					mult                = 1;
					zeroes              = 0;
				}
				else
				{
					++zeroes;
				}

				v <<= 1;
			}

			if (++i == mag.Count)
			{
				result[resultPos++] = CreateWindowEntry(mult, zeroes);
				break;
			}

			v      = (int) mag[i];
			bitPos = 0;
		}

		result[resultPos] = -1;
		return result;
	}

	/// <summary>
	///   Creates the window entry.
	/// </summary>
	/// <param name="mult">The mult.</param>
	/// <param name="zeroes">The zeroes.</param>
	/// <returns>int.</returns>
	private static int CreateWindowEntry(int mult, int zeroes)
	{
		Debug.Assert(mult > 0);

		var tz = BitOperations.TrailingZeroCount(mult);
		mult   >>= tz;
		zeroes +=  tz;

		return mult | (zeroes << 8);
	}

	/// <summary>
	///   Calculates the length of the bit.
	/// </summary>
	/// <param name="sign">The sign.</param>
	/// <param name="indx">The indx.</param>
	/// <param name="mag">The mag.</param>
	/// <returns>int.</returns>
	private static int CalcBitLength(int sign, int indx, IReadOnlyList<uint> mag)
	{
		for (;;)
		{
			if (indx >= mag.Count)
				return 0;

			if (mag[indx] != 0U)
				break;

			++indx;
		}

		// bit length for everything after the first int
		var bitLength = 32 * (mag.Count - indx - 1);

		// and determine bitlength of first int
		var firstMag = mag[indx];
		bitLength += BitLen(firstMag);

		// Check for negative powers of two
		if (sign < 0 && (firstMag & -firstMag) == firstMag)
			do
			{
				if (++indx >= mag.Count)
				{
					--bitLength;
					break;
				}
			} while (mag[indx] == 0U);

		return bitLength;
	}

	/// <summary>
	///   Initializes the big endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <param name="sign">The sign.</param>
	/// <returns>uint[].</returns>
	private static uint[] InitBigEndian(byte[] bytes, int offset, int length, out int sign)
	{
		// TODO Move this processing into MakeMagnitudeBigEndian (provide sign argument)
		if ((sbyte) bytes[offset] >= 0)
		{
			var magnitude = MakeMagnitudeBigEndian(bytes, offset, length);
			sign = magnitude.Length > 0 ? 1 : 0;
			return magnitude;
		}

		sign = -1;

		var end = offset + length;

		int iBval;
		// strip leading sign bytes
		for (iBval = offset; iBval < end && (sbyte) bytes[iBval] == -1; iBval++) { }

		if (iBval >= end)
			return One._magnitude;

		var numBytes = end - iBval;


		var inverse = numBytes <= 512
			              ? stackalloc byte[numBytes]
			              : new byte[numBytes];


		var index                                 = 0;
		while (index < numBytes) inverse[index++] = (byte) ~bytes[iBval++];

		Debug.Assert(iBval == end);

		while (inverse[--index] == byte.MaxValue) inverse[index] = byte.MinValue;

		inverse[index]++;

		return MakeMagnitudeBigEndian(inverse);
	}

	/// <summary>
	///   Abses this instance.
	/// </summary>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Abs() => _sign >= 0 ? this : Negate();

	/// <summary>
	///   Adds the magnitudes.
	/// </summary>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns>uint[].</returns>
	private static uint[] AddMagnitudes(uint[] a, IReadOnlyList<uint> b)
	{
		var   tI = a.Length - 1;
		var   vI = b.Count  - 1;
		ulong m  = 0;

		while (vI >= 0)
		{
			m       +=  (ulong) a[tI] + b[vI--];
			a[tI--] =   (uint) m;
			m       >>= 32;
		}

		if (m != 0)
			while (tI >= 0 && ++a[tI--] == uint.MinValue) { }

		return a;
	}

	/// <summary>
	///   Adds the specified value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Add(BigInteger value)
	{
		if (_sign == 0)
			return value;

		if (_sign == value._sign)
			return AddToMagnitude(value._magnitude);

		if (value._sign == 0)
			return this;

		if (value._sign < 0)
			return Subtract(value.Negate());

		return value.Subtract(Negate());
	}

	/// <summary>
	///   Adds to magnitude.
	/// </summary>
	/// <param name="magToAdd">The mag to add.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private BigInteger AddToMagnitude(uint[] magToAdd)
	{
		uint[] big, small;
		if (_magnitude.Length < magToAdd.Length)
		{
			big   = magToAdd;
			small = _magnitude;
		}
		else
		{
			big   = _magnitude;
			small = magToAdd;
		}

		// Conservatively avoid over-allocation when no overflow possible
		var limit                             = uint.MaxValue;
		if (big.Length == small.Length) limit -= small[0];

		var possibleOverflow = big[0] >= limit;

		uint[] bigCopy;
		if (possibleOverflow)
		{
			bigCopy = new uint[big.Length + 1];
			big.CopyTo(bigCopy, 1);
		}
		else
		{
			bigCopy = (uint[]) big.Clone();
		}

		bigCopy = AddMagnitudes(bigCopy, small);

		return new BigInteger(_sign, bigCopy, possibleOverflow);
	}

	/// <summary>
	///   Ands the specified value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger And(BigInteger value)
	{
		if (_sign == 0 || value._sign == 0)
			return Zero;

		var aMag = _sign       > 0 ? _magnitude : Add(One)._magnitude;
		var bMag = value._sign > 0 ? value._magnitude : value.Add(One)._magnitude;

		var resultNeg    = _sign < 0 && value._sign < 0;
		var resultLength = System.Math.Max(aMag.Length, bMag.Length);
		var resultMag    = new uint[resultLength];

		var aStart = resultMag.Length - aMag.Length;
		var bStart = resultMag.Length - bMag.Length;

		for (var i = 0; i < resultMag.Length; ++i)
		{
			var aWord = i >= aStart ? aMag[i - aStart] : 0U;
			var bWord = i >= bStart ? bMag[i - bStart] : 0U;

			if (_sign < 0) aWord = ~aWord;

			if (value._sign < 0) bWord = ~bWord;

			resultMag[i] = aWord & bWord;

			if (resultNeg) resultMag[i] = ~resultMag[i];
		}

		var result = new BigInteger(1, resultMag, true);

		// TODO Optimise this case
		if (resultNeg) result = result.Not();

		return result;
	}

	/// <summary>
	///   Ands the not.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger AndNot(BigInteger val) => And(val.Not());

	/// <summary>
	///   Initializes the little endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <param name="sign">The sign.</param>
	/// <returns>uint[].</returns>
	private static uint[] InitLittleEndian(byte[] bytes, int offset, int length, out int sign)
	{
		var end = offset + length;

		// TODO Move this processing into MakeMagnitudeLittleEndian (provide sign argument)
		if ((sbyte) bytes[end - 1] >= 0)
		{
			var magnitude = MakeMagnitudeLittleEndian(bytes, offset, length);
			sign = magnitude.Length > 0 ? 1 : 0;
			return magnitude;
		}

		sign = -1;

		// strip leading sign bytes
		var last = length;
		while (--last >= 0 && bytes[offset + last] == byte.MaxValue) { }

		if (last < 0)
			return One._magnitude;

		var numBytes = last + 1;

		var inverse = numBytes <= 512
			              ? stackalloc byte[numBytes]
			              : new byte[numBytes];


		for (var i = 0; i < numBytes; ++i) inverse[i] = (byte) ~bytes[offset + i];

		var index                                                = 0;
		while (inverse[index] == byte.MaxValue) inverse[index++] = byte.MinValue;

		inverse[index]++;

		return MakeMagnitudeLittleEndian(inverse);
	}

	/// <summary>
	///   Makes the magnitude big endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>uint[].</returns>
	private static uint[] MakeMagnitudeBigEndian(byte[] bytes) => MakeMagnitudeBigEndian(bytes, 0, bytes.Length);

	/// <summary>
	///   Makes the magnitude little endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>uint[].</returns>
	private static uint[] MakeMagnitudeLittleEndian(byte[] bytes) => MakeMagnitudeLittleEndian(bytes, 0, bytes.Length);

	/// <summary>
	///   Makes the magnitude big endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <returns>uint[].</returns>
	private static uint[] MakeMagnitudeBigEndian(byte[] bytes, int offset, int length) => MakeMagnitudeBigEndian(bytes.AsSpan(offset, length));

	/// <summary>
	///   Makes the magnitude little endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <returns>uint[].</returns>
	private static uint[] MakeMagnitudeLittleEndian(byte[] bytes, int offset, int length) => MakeMagnitudeLittleEndian(bytes.AsSpan(offset, length));

	/// <summary>
	///   Makes the magnitude big endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>uint[].</returns>
	private static uint[] MakeMagnitudeBigEndian(ReadOnlySpan<byte> bytes)
	{
		var end = bytes.Length;

		// strip leading zeros
		int start;
		for (start = 0; start < end && bytes[start] == 0; start++) { }

		var nBytes = end - start;
		if (nBytes <= 0)
			return ZeroMagnitude;

		var nInts = (nBytes + BytesPerInt - 1) / BytesPerInt;
		Debug.Assert(nInts > 0);

		var magnitude = new uint[nInts];

		var first = (nBytes - 1) % BytesPerInt + 1;
		magnitude[0] = Pack.BigEndian_To_UInt32_Low(bytes.Slice(start, first));
		Pack.BigEndian_To_UInt32(bytes[(start + first)..], magnitude.AsSpan(1));

		return magnitude;
	}

	/// <summary>
	///   Makes the magnitude little endian.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>uint[].</returns>
	private static uint[] MakeMagnitudeLittleEndian(ReadOnlySpan<byte> bytes)
	{
		// strip leading zeros
		var last = bytes.Length;
		while (--last >= 0 && bytes[last] == 0) { }

		if (last < 0)
			return ZeroMagnitude;

		var nInts = (last + BytesPerInt) / BytesPerInt;
		Debug.Assert(nInts > 0);

		var magnitude = new uint[nInts];

		var partial = last % BytesPerInt;
		var first   = partial + 1;
		var pos     = last    - partial;

		magnitude[0] = Pack.LittleEndian_To_UInt32_Low(bytes.Slice(pos, first));
		for (var i = 1; i < nInts; ++i)
		{
			pos          -= BytesPerInt;
			magnitude[i] =  Pack.LittleEndian_To_UInt32(bytes, pos);
		}

		Debug.Assert(pos == 0);

		return magnitude;
	}


	/// <summary>
	///   Called when [deserialized].
	/// </summary>
	/// <param name="context">The context.</param>
	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
		_nBits      = -1;
		_nBitLength = -1;
	}

	/// <summary>
	///   Gets the length of the bytes.
	/// </summary>
	/// <param name="nBits">The n bits.</param>
	/// <returns>int.</returns>
	private static int GetBytesLength(int nBits) => (nBits + BitsPerByte - 1) / BitsPerByte;

	/// <summary>
	///   Gets the length of the ints.
	/// </summary>
	/// <param name="nBits">The n bits.</param>
	/// <returns>int.</returns>
	private static int GetIntsLength(int nBits) => (nBits + BitsPerInt - 1) / BitsPerInt;

	/// <summary>
	///   Arbitraries the specified size in bits.
	/// </summary>
	/// <param name="sizeInBits">The size in bits.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public static BigInteger Arbitrary(int sizeInBits) => new(sizeInBits, SecureRandom.ArbitraryRandom);

	/// <summary>
	///   Divides the specified x.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <returns>uint[].</returns>
	private uint[] Divide(uint[] x, uint[] y)
	{
		var xStart = 0;
		while (xStart < x.Length && x[xStart] == 0U) ++xStart;

		var yStart = 0;
		while (yStart < y.Length && y[yStart] == 0U) ++yStart;

		Debug.Assert(yStart < y.Length);

		var    xyCmp = CompareNoLeadingZeroes(xStart, x, yStart, y);
		uint[] count;

		if (xyCmp > 0)
		{
			var yBitLength = CalcBitLength(1, yStart, y);
			var xBitLength = CalcBitLength(1, xStart, x);
			var shift      = xBitLength - yBitLength;

			uint[] iCount;
			var    iCountStart = 0;

			uint[] c;
			var    cStart     = 0;
			var    cBitLength = yBitLength;
			if (shift > 0)
			{
				iCount    = new uint[(shift >> 5) + 1];
				iCount[0] = 1U << (shift % 32);

				c          =  ShiftLeft(y, shift);
				cBitLength += shift;
			}
			else
			{
				iCount = new uint[1] {1U};

				var len = y.Length - yStart;
				c = new uint[len];
				Array.Copy(y, yStart, c, 0, len);
			}

			count = new uint[iCount.Length];

			for (;;)
			{
				if (cBitLength < xBitLength || CompareNoLeadingZeroes(xStart, x, cStart, c) >= 0)
				{
					Subtract(xStart, x, cStart, c);
					AddMagnitudes(count, iCount);

					while (x[xStart] == 0U)
					{
						if (++xStart == x.Length)
							return count;
					}

					xBitLength = 32 * (x.Length - xStart - 1) + BitLen(x[xStart]);

					if (xBitLength <= yBitLength)
					{
						if (xBitLength < yBitLength)
							return count;

						xyCmp = CompareNoLeadingZeroes(xStart, x, yStart, y);

						if (xyCmp <= 0)
							break;
					}
				}

				shift = cBitLength - xBitLength;

				// NB: The case where c[cStart] is 1-bit is harmless
				if (shift == 1)
				{
					var firstC = c[cStart] >> 1;
					var firstX = x[xStart];
					if (firstC > firstX) ++shift;
				}

				if (shift < 2)
				{
					ShiftRightOneInPlace(cStart, c);
					--cBitLength;
					ShiftRightOneInPlace(iCountStart, iCount);
				}
				else
				{
					ShiftRightInPlace(cStart, c, shift);
					cBitLength -= shift;
					ShiftRightInPlace(iCountStart, iCount, shift);
				}

				while (c[cStart] == 0) ++cStart;

				while (iCount[iCountStart] == 0) ++iCountStart;
			}
		}
		else
		{
			count = new uint[1];
		}

		if (xyCmp == 0)
		{
			AddMagnitudes(count, One._magnitude);
			Array.Clear(x, xStart, x.Length - xStart);
		}

		return count;
	}

	/// <summary>
	///   Divides the specified value.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Division by zero error</exception>
	public BigInteger Divide(BigInteger val)
	{
		if (val._sign == 0)
			throw new ArithmeticException("Division by zero error");

		if (_sign == 0)
			return Zero;

		if (val.QuickPow2Check()) // val is power of two
		{
			var result = Abs().ShiftRight(val.Abs().BitLength - 1);
			return val._sign == _sign ? result : result.Negate();
		}

		var mag = (uint[]) _magnitude.Clone();

		return new BigInteger(_sign * val._sign, Divide(mag, val._magnitude), true);
	}

	/// <summary>
	///   Divides the and remainder.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <returns>core.Math.BigInteger[].</returns>
	/// <exception cref="ArithmeticException">Division by zero error</exception>
	public BigInteger[] DivideAndRemainder(BigInteger val)
	{
		if (val._sign == 0)
			throw new ArithmeticException("Division by zero error");

		var biggies = new BigInteger[2];

		if (_sign == 0)
		{
			biggies[0] = Zero;
			biggies[1] = Zero;
		}
		else if (val.QuickPow2Check()) // val is power of two
		{
			var e         = val.Abs().BitLength - 1;
			var quotient  = Abs().ShiftRight(e);
			var remainder = LastNBits(e);

			biggies[0] = val._sign == _sign ? quotient : quotient.Negate();
			biggies[1] = new BigInteger(_sign, remainder, true);
		}
		else
		{
			var remainder = (uint[]) _magnitude.Clone();
			var quotient  = Divide(remainder, val._magnitude);

			biggies[0] = new BigInteger(_sign * val._sign, quotient,  true);
			biggies[1] = new BigInteger(_sign,             remainder, true);
		}

		return biggies;
	}

	/// <summary>
	///   Multiplies the monty.
	/// </summary>
	/// <param name="a">a.</param>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <param name="m">The m.</param>
	/// <param name="mDash">The m dash.</param>
	/// <param name="smallMontyModulus">The small monty modulus.</param>
	private static void MultiplyMonty(uint[] a, uint[] x, IReadOnlyList<uint> y, uint[] m, uint mDash, bool smallMontyModulus)
		// mDash = -m^(-1) mod b
	{
		var n = m.Length;

		if (n == 1)
		{
			x[0] = MultiplyMontyNIsOne(x[0], y[0], m[0], mDash);
			return;
		}

		var  y0 = y[n - 1];
		uint aMax;

		{
			ulong xi = x[n - 1];

			var   carry = xi           * y0;
			ulong t     = (uint) carry * mDash;

			var prod2 = t * m[n - 1];
			carry += (uint) prod2;
			Debug.Assert((uint) carry == 0);
			carry = (carry >> 32) + (prod2 >> 32);

			for (var j = n - 2; j >= 0; --j)
			{
				var prod1 = xi * y[j];
				prod2 = t * m[j];

				carry    += (prod1 & UnsignedIntegerMask) + (uint) prod2;
				a[j + 2] =  (uint) carry;
				carry    =  (carry >> 32) + (prod1 >> 32) + (prod2 >> 32);
			}

			a[1] = (uint) carry;
			aMax = (uint) (carry >> 32);
		}

		for (var i = n - 2; i >= 0; --i)
		{
			var   a0 = a[n];
			ulong xi = x[i];

			var   prod1 = xi * y0;
			var   carry = (prod1 & UnsignedIntegerMask) + a0;
			ulong t     = (uint) carry * mDash;

			var prod2 = t * m[n - 1];
			carry += (uint) prod2;
			Debug.Assert((uint) carry == 0);
			carry = (carry >> 32) + (prod1 >> 32) + (prod2 >> 32);

			for (var j = n - 2; j >= 0; --j)
			{
				prod1 = xi * y[j];
				prod2 = t  * m[j];

				carry    += (prod1 & UnsignedIntegerMask) + (uint) prod2 + a[j + 1];
				a[j + 2] =  (uint) carry;
				carry    =  (carry >> 32) + (prod1 >> 32) + (prod2 >> 32);
			}

			carry += aMax;
			a[1]  =  (uint) carry;
			aMax  =  (uint) (carry >> 32);
		}

		a[0] = aMax;

		if (!smallMontyModulus && CompareTo(0, a, 0, m) >= 0) Subtract(0, a, 0, m);

		Array.Copy(a, 1, x, 0, n);
	}

	/// <summary>
	///   Multiplies the monty n is one.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <param name="m">The m.</param>
	/// <param name="mDash">The m dash.</param>
	/// <returns>uint.</returns>
	private static uint MultiplyMontyNIsOne(uint x, uint y, uint m, uint mDash)
	{
		var   carry = (ulong) x    * y;
		var   t     = (uint) carry * mDash;
		ulong um    = m;
		var   prod2 = um * t;
		carry += (uint) prod2;
		Debug.Assert((uint) carry == 0);
		carry = (carry >> 32) + (prod2 >> 32);
		if (carry > um) carry -= um;
		Debug.Assert(carry < um);
		return (uint) carry;
	}

	/// <summary>
	///   Negates this instance.
	/// </summary>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Negate() => _sign == 0 ? this : new BigInteger(-_sign, _magnitude, false);

	/// <summary>
	///   Probables the prime.
	/// </summary>
	/// <param name="bitLength">Length of the bit.</param>
	/// <param name="random">The random.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public static BigInteger ProbablePrime(int bitLength, System.Random random) => new(bitLength, 100, random);

	/// <summary>
	///   Nexts the probable prime.
	/// </summary>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">
	///   Cannot be called on value < 0</exception>
	public BigInteger NextProbablePrime()
	{
		if (_sign < 0)
			throw new ArithmeticException("Cannot be called on value < 0");

		if (CompareTo(Two) < 0)
			return Two;

		var n = Inc().SetBit(0);

		while (!n.CheckProbablePrime(100, SecureRandom.ArbitraryRandom, false)) n = n.Add(Two);

		return n;
	}

	/// <summary>
	///   Nots this instance.
	/// </summary>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Not() => Inc().Negate();

	/// <summary>
	///   Pows the specified exp.
	/// </summary>
	/// <param name="exp">The exp.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Negative exponent</exception>
	/// <exception cref="ArithmeticException">Result too large</exception>
	public BigInteger Pow(int exp)
	{
		if (exp <= 0)
		{
			if (exp < 0)
				throw new ArithmeticException("Negative exponent");

			return One;
		}

		if (_sign == 0) return this;

		if (QuickPow2Check())
		{
			var powOf2 = (long) exp * (BitLength - 1);
			if (powOf2 > int.MaxValue) throw new ArithmeticException("Result too large");
			return One.ShiftLeft((int) powOf2);
		}

		var y = One;
		var z = this;

		for (;;)
		{
			if ((exp & 0x1) == 1) y = y.Multiply(z);
			exp >>= 1;
			if (exp == 0) break;
			z = z.Multiply(z);
		}

		return y;
	}

	/// <summary>
	///   Subtracts the specified x start.
	/// </summary>
	/// <param name="xStart">The x start.</param>
	/// <param name="x">The x.</param>
	/// <param name="yStart">The y start.</param>
	/// <param name="y">The y.</param>
	/// <returns>uint[].</returns>
	private static uint[] Subtract(int xStart, uint[] x, int yStart, uint[] y)
	{
		Debug.Assert(yStart            < y.Length);
		Debug.Assert(x.Length - xStart >= y.Length - yStart);

		var iT     = x.Length;
		var iV     = y.Length;
		var borrow = 0;

		do
		{
			var m = (x[--iT] & IntegerMask) - (y[--iV] & IntegerMask) + borrow;
			x[iT] = (uint) m;

			borrow = (int) (m >> 63);
		} while (iV > yStart);

		if (borrow != 0)
			while (--x[--iT] == uint.MaxValue) { }

		return x;
	}

	/// <summary>
	///   Subtracts the specified n.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Subtract(BigInteger n)
	{
		if (n._sign == 0)
			return this;

		if (_sign == 0)
			return n.Negate();

		if (_sign != n._sign)
			return Add(n.Negate());

		var compare = CompareNoLeadingZeroes(0, _magnitude, 0, n._magnitude);
		if (compare == 0)
			return Zero;

		BigInteger bigun, lilun;
		if (compare < 0)
		{
			bigun = n;
			lilun = this;
		}
		else
		{
			bigun = this;
			lilun = n;
		}

		return new BigInteger(_sign * compare, DoSubBigLil(bigun._magnitude, lilun._magnitude), true);
	}

	/// <summary>
	///   Does the sub big lil.
	/// </summary>
	/// <param name="bigMag">The big mag.</param>
	/// <param name="lilMag">The lil mag.</param>
	/// <returns>uint[].</returns>
	private static uint[] DoSubBigLil(uint[] bigMag, uint[] lilMag)
	{
		var res = (uint[]) bigMag.Clone();

		return Subtract(0, res, 0, lilMag);
	}

	/// <summary>
	///   Gets the lengthof byte array.
	/// </summary>
	/// <returns>int.</returns>
	public int GetLengthofByteArray() => GetBytesLength(BitLength + 1);

	/// <summary>
	///   Gets the lengthof byte array unsigned.
	/// </summary>
	/// <returns>int.</returns>
	public int GetLengthofByteArrayUnsigned() => GetBytesLength(_sign < 0 ? BitLength + 1 : BitLength);

	/// <summary>
	///   Gets the lengthof u int32 array.
	/// </summary>
	/// <returns>int.</returns>
	public int GetLengthofUInt32Array() => GetIntsLength(BitLength + 1);

	/// <summary>
	///   Gets the lengthof u int32 array unsigned.
	/// </summary>
	/// <returns>int.</returns>
	public int GetLengthofUInt32ArrayUnsigned() => GetIntsLength(_sign < 0 ? BitLength + 1 : BitLength);

	/// <summary>
	///   Converts to bytearray.
	/// </summary>
	/// <returns>byte[].</returns>
	public byte[] ToByteArray() => ToByteArray(false);

	/// <summary>
	///   Converts to bytearray.
	/// </summary>
	/// <param name="output">The output.</param>
	public void ToByteArray(Span<byte> output) => ToByteArray(false, output);

	/// <summary>
	///   Converts to bytearrayunsigned.
	/// </summary>
	/// <returns>byte[].</returns>
	public byte[] ToByteArrayUnsigned() => ToByteArray(true);

	/// <summary>
	///   Converts to bytearrayunsigned.
	/// </summary>
	/// <param name="output">The output.</param>
	public void ToByteArrayUnsigned(Span<byte> output) => ToByteArray(true, output);


	/// <summary>
	///   Converts to uint32arraybigendianunsigned.
	/// </summary>
	/// <param name="output">The output.</param>
	public void ToUInt32ArrayBigEndianUnsigned(Span<uint> output) => ToUInt32ArrayBigEndian(true, output);


	/// <summary>
	///   Converts to uint32arraylittleendianunsigned.
	/// </summary>
	/// <param name="output">The output.</param>
	public void ToUInt32ArrayLittleEndianUnsigned(Span<uint> output) => ToUInt32ArrayLittleEndian(true, output);


	/// <summary>
	///   Converts to uint32arraybigendian.
	/// </summary>
	/// <param name="output">The output.</param>
	public void ToUInt32ArrayBigEndian(Span<uint> output) => ToUInt32ArrayBigEndian(false, output);


	/// <summary>
	///   Converts to uint32arraylittleendian.
	/// </summary>
	/// <param name="output">The output.</param>
	public void ToUInt32ArrayLittleEndian(Span<uint> output) => ToUInt32ArrayLittleEndian(false, output);

	/// <summary>
	///   Converts to bytearray.
	/// </summary>
	/// <param name="unsigned">The unsigned.</param>
	/// <returns>byte[].</returns>
	private byte[] ToByteArray(bool unsigned)
	{
		if (_sign == 0)
			return unsigned ? ZeroEncoding : new byte[1];

		var bitLength = unsigned && _sign > 0
			                ? BitLength
			                : BitLength + 1;

		var nBytes = GetBytesLength(bitLength);
		var bytes  = new byte[nBytes];

		var magIndex   = _magnitude.Length;
		var bytesIndex = bytes.Length;

		if (_sign > 0)
		{
			while (magIndex > 1)
			{
				var mag = _magnitude[--magIndex];
				bytesIndex -= 4;
				Pack.UInt32_To_BigEndian(mag, bytes, bytesIndex);
			}

			var lastMag = _magnitude[0];
			while (lastMag > byte.MaxValue)
			{
				bytes[--bytesIndex] =   (byte) lastMag;
				lastMag             >>= 8;
			}

			bytes[--bytesIndex] = (byte) lastMag;
			Debug.Assert((bytesIndex & 1) == bytesIndex);
			//if (bytesIndex != 0)
			//{
			//    bytes[0] = 0;
			//}
		}
		else // sign < 0
		{
			var carry = true;

			while (magIndex > 1)
			{
				var mag = ~_magnitude[--magIndex];

				if (carry) carry = ++mag == uint.MinValue;

				bytesIndex -= 4;
				Pack.UInt32_To_BigEndian(mag, bytes, bytesIndex);
			}

			var lastMag = _magnitude[0];

			if (carry)
				// Never wraps because magnitude[0] != 0
				--lastMag;

			while (lastMag > byte.MaxValue)
			{
				bytes[--bytesIndex] =   (byte) ~lastMag;
				lastMag             >>= 8;
			}

			bytes[--bytesIndex] = (byte) ~lastMag;
			Debug.Assert((bytesIndex & 1) == bytesIndex);
			if (bytesIndex != 0) bytes[--bytesIndex] = byte.MaxValue;
		}

		return bytes;
	}

	/// <summary>
	///   Converts to bytearray.
	/// </summary>
	/// <param name="unsigned">The unsigned.</param>
	/// <param name="output">The output.</param>
	/// <exception cref="ArgumentException">insufficient space, nameof(output)</exception>
	private void ToByteArray(bool unsigned, Span<byte> output)
	{
		if (_sign == 0)
		{
			if (!unsigned) output[0] = 0;
			return;
		}

		var bitLength = unsigned && _sign > 0 ? BitLength : BitLength + 1;

		var nBytes = GetBytesLength(bitLength);
		if (nBytes > output.Length)
			throw new ArgumentException("insufficient space", nameof(output));

		var magIndex   = _magnitude.Length;
		var bytesIndex = nBytes;

		if (_sign > 0)
		{
			while (magIndex > 1)
			{
				var mag = _magnitude[--magIndex];
				bytesIndex -= 4;
				Pack.UInt32_To_BigEndian(mag, output[bytesIndex..]);
			}

			var lastMag = _magnitude[0];
			while (lastMag > byte.MaxValue)
			{
				output[--bytesIndex] =   (byte) lastMag;
				lastMag              >>= 8;
			}

			output[--bytesIndex] = (byte) lastMag;
			Debug.Assert((bytesIndex & 1) == bytesIndex);
			if (bytesIndex != 0) output[0] = 0;
		}
		else // sign < 0
		{
			var carry = true;

			while (magIndex > 1)
			{
				var mag = ~_magnitude[--magIndex];

				if (carry) carry = ++mag == uint.MinValue;

				bytesIndex -= 4;
				Pack.UInt32_To_BigEndian(mag, output[bytesIndex..]);
			}

			var lastMag = _magnitude[0];

			if (carry)
				// Never wraps because magnitude[0] != 0
				--lastMag;

			while (lastMag > byte.MaxValue)
			{
				output[--bytesIndex] =   (byte) ~lastMag;
				lastMag              >>= 8;
			}

			output[--bytesIndex] = (byte) ~lastMag;
			Debug.Assert((bytesIndex & 1) == bytesIndex);
			if (bytesIndex != 0) output[--bytesIndex] = byte.MaxValue;
		}
	}

	/// <summary>
	///   Converts to uint32arraybigendian.
	/// </summary>
	/// <param name="unsigned">The unsigned.</param>
	/// <param name="output">The output.</param>
	/// <exception cref="ArgumentException">insufficient space, nameof(output)</exception>
	private void ToUInt32ArrayBigEndian(bool unsigned, Span<uint> output)
	{
		if (_sign == 0)
		{
			if (!unsigned) output[0] = uint.MinValue;
			return;
		}

		var bitLength = unsigned && _sign > 0 ? BitLength : BitLength + 1;

		var nInts = GetIntsLength(bitLength);
		if (nInts > output.Length)
			throw new ArgumentException("insufficient space", nameof(output));

		var magIndex  = _magnitude.Length;
		var intsIndex = nInts;

		if (_sign > 0)
		{
			while (magIndex > 0) output[--intsIndex] = _magnitude[--magIndex];

			Debug.Assert((intsIndex & 1) == intsIndex);
			if (intsIndex != 0) output[0] = uint.MinValue;
		}
		else // sign < 0
		{
			var cc = 1UL;
			while (magIndex > 0)
			{
				cc                  +=  ~_magnitude[--magIndex];
				output[--intsIndex] =   (uint) cc;
				cc                  >>= 32;
			}

			Debug.Assert(cc == 0UL);

			Debug.Assert((intsIndex & 1) == intsIndex);
			if (intsIndex != 0) output[--intsIndex] = uint.MaxValue;
		}
	}

	/// <summary>
	///   Converts to uint32arraylittleendian.
	/// </summary>
	/// <param name="unsigned">The unsigned.</param>
	/// <param name="output">The output.</param>
	/// <exception cref="ArgumentException">insufficient space, nameof(output)</exception>
	private void ToUInt32ArrayLittleEndian(bool unsigned, Span<uint> output)
	{
		if (_sign == 0)
		{
			if (!unsigned) output[0] = uint.MinValue;
			return;
		}

		var nBits = unsigned && _sign > 0 ? BitLength : BitLength + 1;

		var nInts = GetIntsLength(nBits);
		if (nInts > output.Length)
			throw new ArgumentException("insufficient space", nameof(output));

		var magIndex = _magnitude.Length;

		if (_sign > 0)
		{
			for (var intsIndex = 0; intsIndex < _magnitude.Length; ++intsIndex) output[intsIndex] = _magnitude[--magIndex];

			if (nInts > _magnitude.Length)
			{
				Debug.Assert(nInts == _magnitude.Length + 1);
				output[_magnitude.Length] = uint.MinValue;
			}
		}
		else // sign < 0
		{
			var cc = 1UL;
			for (var intsIndex = 0; intsIndex < _magnitude.Length; ++intsIndex)
			{
				cc                +=  ~_magnitude[--magIndex];
				output[intsIndex] =   (uint) cc;
				cc                >>= 32;
			}

			Debug.Assert(cc == 0UL);

			if (nInts > _magnitude.Length)
			{
				Debug.Assert(nInts == _magnitude.Length + 1);
				output[_magnitude.Length] = uint.MaxValue;
			}
		}
	}

	/// <summary>
	///   Remainders the specified m.
	/// </summary>
	/// <param name="m">The m.</param>
	/// <returns>int.</returns>
	private int Remainder(int m) => (int) _magnitude.Aggregate<uint, long>(0, (current, posVal) => ((current << 32) | posVal) % m);

	/// <summary>
	///   Remainders the specified x.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <returns>uint[].</returns>
	private static uint[] Remainder(uint[] x, uint[] y)
	{
		var xStart = 0;
		while (xStart < x.Length && x[xStart] == 0U) ++xStart;

		var yStart = 0;
		while (yStart < y.Length && y[yStart] == 0U) ++yStart;

		Debug.Assert(yStart < y.Length);

		var xyCmp = CompareNoLeadingZeroes(xStart, x, yStart, y);

		if (xyCmp > 0)
		{
			var yBitLength = CalcBitLength(1, yStart, y);
			var xBitLength = CalcBitLength(1, xStart, x);
			var shift      = xBitLength - yBitLength;

			uint[] c;
			var    cStart     = 0;
			var    cBitLength = yBitLength;
			if (shift > 0)
			{
				c          =  ShiftLeft(y, shift);
				cBitLength += shift;
				Debug.Assert(c[0] != 0U);
			}
			else
			{
				var len = y.Length - yStart;
				c = new uint[len];
				Array.Copy(y, yStart, c, 0, len);
			}

			for (;;)
			{
				if (cBitLength < xBitLength || CompareNoLeadingZeroes(xStart, x, cStart, c) >= 0)
				{
					Subtract(xStart, x, cStart, c);

					while (x[xStart] == 0U)
					{
						if (++xStart == x.Length)
							return x;
					}

					xBitLength = 32 * (x.Length - xStart - 1) + BitLen(x[xStart]);

					if (xBitLength <= yBitLength)
					{
						if (xBitLength < yBitLength)
							return x;

						xyCmp = CompareNoLeadingZeroes(xStart, x, yStart, y);

						if (xyCmp <= 0)
							break;
					}
				}

				shift = cBitLength - xBitLength;

				// NB: The case where c[cStart] is 1-bit is harmless
				if (shift == 1)
				{
					var firstC = c[cStart] >> 1;
					var firstX = x[xStart];
					if (firstC > firstX) ++shift;
				}

				if (shift < 2)
				{
					ShiftRightOneInPlace(cStart, c);
					--cBitLength;
				}
				else
				{
					ShiftRightInPlace(cStart, c, shift);
					cBitLength -= shift;
				}

				while (c[cStart] == 0U) ++cStart;
			}
		}

		if (xyCmp == 0) Array.Clear(x, xStart, x.Length - xStart);

		return x;
	}

	/// <summary>
	///   Remainders the specified n.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Division by zero error</exception>
	public BigInteger Remainder(BigInteger n)
	{
		if (n._sign == 0) throw new ArithmeticException("Division by zero error");

		if (_sign == 0) return Zero;

		// For small values, use fast remainder method
		if (n._magnitude.Length == 1)
		{
			var val = (int) n._magnitude[0];

			if (val > 0)
			{
				if (val == 1)
					return Zero;

				// TODO Make this func work on uint, and handle val == 1?
				var rem = Remainder(val);

				return rem == 0
					       ? Zero
					       : new BigInteger(_sign, new uint[1] {(uint) rem}, false);
			}
		}

		if (CompareNoLeadingZeroes(0, _magnitude, 0, n._magnitude) < 0)
			return this;

		uint[] result;
		if (n.QuickPow2Check()) // n is power of two
		{
			// TODO Move before small values branch above?
			result = LastNBits(n.Abs().BitLength - 1);
		}
		else
		{
			result = (uint[]) _magnitude.Clone();
			result = Remainder(result, n._magnitude);
		}

		return new BigInteger(_sign, result, true);
	}

	/// <summary>
	///   Lasts the n bits.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>uint[].</returns>
	private uint[] LastNBits(int n)
	{
		if (n < 1) return ZeroMagnitude;

		var numWords = (n + BitsPerInt - 1) / BitsPerInt;
		numWords = System.Math.Min(numWords, _magnitude.Length);
		var result = new uint[numWords];

		Array.Copy(_magnitude, _magnitude.Length - numWords, result, 0, numWords);

		var excessBits                = (numWords << 5) - n;
		if (excessBits > 0) result[0] &= uint.MaxValue >> excessBits;

		return result;
	}

	/// <summary>
	///   Divides the words.
	/// </summary>
	/// <param name="w">The w.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private BigInteger DivideWords(int w)
	{
		Debug.Assert(w >= 0);
		var n = _magnitude.Length;
		if (w >= n) return Zero;
		var mag = new uint[n - w];
		Array.Copy(_magnitude, 0, mag, 0, n - w);
		return new BigInteger(_sign, mag, false);
	}

	/// <summary>
	///   Remainders the words.
	/// </summary>
	/// <param name="w">The w.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private BigInteger RemainderWords(int w)
	{
		Debug.Assert(w >= 0);
		var n = _magnitude.Length;
		if (w >= n) return this;
		var mag = new uint[w];
		Array.Copy(_magnitude, n - w, mag, 0, w);
		return new BigInteger(_sign, mag, false);
	}

	/// <summary>
	///   Squares this instance.
	/// </summary>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Square()
	{
		if (_sign == 0) return Zero;
		if (QuickPow2Check()) return ShiftLeft(Abs().BitLength - 1);
		var resLength = _magnitude.Length << 1;
		if (_magnitude[0] >> 16 == 0U) --resLength;
		var res = new uint[resLength];
		Square(res, _magnitude);
		return new BigInteger(1, res, false);
	}

	/// <summary>
	///   Multiplies the specified value.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Multiply(BigInteger val)
	{
		if (val.Equals(this)) return Square();

		if ((_sign & val._sign) == 0) return Zero;

		if (val.QuickPow2Check()) // val is power of two
		{
			var result = ShiftLeft(val.Abs().BitLength - 1);
			return val._sign > 0 ? result : result.Negate();
		}

		if (QuickPow2Check()) // this is power of two
		{
			var result = val.ShiftLeft(Abs().BitLength - 1);
			return _sign > 0 ? result : result.Negate();
		}

		var resLength = _magnitude.Length + val._magnitude.Length;
		var res       = new uint[resLength];

		Multiply(res, _magnitude, val._magnitude);

		var resSign = _sign ^ val._sign ^ 1;
		return new BigInteger(resSign, res, true);
	}

	/// <summary>
	///   Shifts the left.
	/// </summary>
	/// <param name="mag">The mag.</param>
	/// <param name="n">The n.</param>
	/// <returns>uint[].</returns>
	private static uint[] ShiftLeft(uint[] mag, int n)
	{
		var    nInts  = n >>> 5;
		var    nBits  = n & 0x1f;
		var    magLen = mag.Length;
		uint[] newMag;

		if (nBits == 0)
		{
			newMag = new uint[magLen + nInts];
			mag.CopyTo(newMag, 0);
		}
		else
		{
			var i        = 0;
			var nBits2   = 32 - nBits;
			var highBits = mag[0] >> nBits2;

			if (highBits != 0U)
			{
				newMag      = new uint[magLen + nInts + 1];
				newMag[i++] = highBits;
			}
			else
			{
				newMag = new uint[magLen + nInts];
			}

			var m = mag[0];
			for (var j = 0; j < magLen - 1; j++)
			{
				var next = mag[j + 1];

				newMag[i++] = (m << nBits) | (next >> nBits2);
				m           = next;
			}

			newMag[i] = mag[magLen - 1] << nBits;
		}

		return newMag;
	}

	/// <summary>
	///   Shifts the left one in place.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="carry">The carry.</param>
	/// <returns>int.</returns>
	private static int ShiftLeftOneInPlace(int[] x, int carry)
	{
		Debug.Assert(carry == 0 || carry == 1);
		var pos = x.Length;
		while (--pos >= 0)
		{
			var val = (uint) x[pos];
			x[pos] = (int) (val << 1) | carry;
			carry  = (int) (val >> 31);
		}

		return carry;
	}

	/// <summary>
	///   Shifts the left.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger ShiftLeft(int n)
	{
		if (_sign == 0 || _magnitude.Length == 0) return Zero;
		switch (n)
		{
			case 0:
				return this;
			case < 0:
				return ShiftRight(-n);
		}

		var result = new BigInteger(_sign, ShiftLeft(_magnitude, n), true);

		if (_nBits != -1)
			result._nBits = _sign > 0
				                ? _nBits
				                : _nBits + n;

		if (_nBitLength != -1) result._nBitLength = _nBitLength + n;

		return result;
	}

	/// <summary>
	///   Shifts the right one in place.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <param name="mag">The mag.</param>
	private static void ShiftRightOneInPlace(int start, uint[] mag)
	{
		var i = mag.Length;
		var m = mag[i - 1];

		while (--i > start)
		{
			var next = mag[i - 1];
			mag[i] = (m >> 1) | (next << 31);
			m      = next;
		}

		mag[start] = mag[start] >> 1;
	}

	/// <summary>
	///   Shifts the right.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger ShiftRight(int n)
	{
		if (n == 0)
			return this;

		if (n < 0)
			return ShiftLeft(-n);

		if (n >= BitLength)
			return _sign < 0 ? One.Negate() : Zero;

		var resultLength = (BitLength - n + 31) >> 5;
		var res          = new uint[resultLength];

		var numInts = n >> 5;
		var numBits = n & 31;

		if (numBits == 0)
		{
			Array.Copy(_magnitude, 0, res, 0, res.Length);
		}
		else
		{
			var numBits2 = 32 - numBits;

			var magPos = _magnitude.Length - 1 - numInts;
			for (var i = resultLength - 1; i >= 0; --i)
			{
				res[i] = _magnitude[magPos--] >> numBits;

				if (magPos >= 0) res[i] |= _magnitude[magPos] << numBits2;
			}
		}

		Debug.Assert(res[0] != 0U);

		return new BigInteger(_sign, res, false);
	}


	/// <summary>
	///   Shifts the right in place.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <param name="mag">The mag.</param>
	/// <param name="n">The n.</param>
	private static void ShiftRightInPlace(int start, uint[] mag, int n)
	{
		var nInts  = (n >>> 5) + start;
		var nBits  = n & 0x1f;
		var magEnd = mag.Length - 1;

		if (nInts != start)
		{
			var delta = nInts - start;

			for (var i = magEnd; i    >= nInts; i--) mag[i] = mag[i - delta];
			for (var i = nInts - 1; i >= start; i--) mag[i] = 0U;
		}

		if (nBits != 0)
		{
			var nBits2 = 32 - nBits;
			var m      = mag[magEnd];

			for (var i = magEnd; i > nInts; --i)
			{
				var next = mag[i - 1];

				mag[i] = (m >> nBits) | (next << nBits2);
				m      = next;
			}

			mag[nInts] = mag[nInts] >> nBits;
		}
	}


	/// <summary>
	///   Squares the monty.
	/// </summary>
	/// <param name="a">a.</param>
	/// <param name="x">The x.</param>
	/// <param name="m">The m.</param>
	/// <param name="mDash">The m dash.</param>
	/// <param name="smallMontyModulus">The small monty modulus.</param>
	private static void SquareMonty(uint[] a, uint[] x, uint[] m, uint mDash, bool smallMontyModulus)
		// mDash = -m^(-1) mod b
	{
		var n = m.Length;

		if (n == 1)
		{
			var xVal = x[0];
			x[0] = MultiplyMontyNIsOne(xVal, xVal, m[0], mDash);
			return;
		}

		ulong x0 = x[n - 1];
		uint  aMax;

		{
			var   carry = x0           * x0;
			ulong t     = (uint) carry * mDash;

			var prod2 = t * m[n - 1];
			carry += (uint) prod2;
			Debug.Assert((uint) carry == 0);
			carry = (carry >> 32) + (prod2 >> 32);

			for (var j = n - 2; j >= 0; --j)
			{
				var prod1 = x0 * x[j];
				prod2 = t * m[j];

				carry    += (prod2 & UnsignedIntegerMask) + ((uint) prod1 << 1);
				a[j + 2] =  (uint) carry;
				carry    =  (carry >> 32) + (prod1 >> 31) + (prod2 >> 32);
			}

			a[1] = (uint) carry;
			aMax = (uint) (carry >> 32);
		}

		for (var i = n - 2; i >= 0; --i)
		{
			var   a0 = a[n];
			ulong t  = a0 * mDash;

			var carry = t * m[n - 1] + a0;
			Debug.Assert((uint) carry == 0);
			carry >>= 32;

			for (var j = n - 2; j > i; --j)
			{
				carry    +=  t * m[j] + a[j + 1];
				a[j + 2] =   (uint) carry;
				carry    >>= 32;
			}

			ulong xi = x[i];

			{
				var prod1 = xi * xi;
				var prod2 = t  * m[i];

				carry    += (prod1 & UnsignedIntegerMask) + (uint) prod2 + a[i + 1];
				a[i + 2] =  (uint) carry;
				carry    =  (carry >> 32) + (prod1 >> 32) + (prod2 >> 32);
			}

			for (var j = i - 1; j >= 0; --j)
			{
				var prod1 = xi * x[j];
				var prod2 = t  * m[j];

				carry    += (prod2 & UnsignedIntegerMask) + ((uint) prod1 << 1) + a[j + 1];
				a[j + 2] =  (uint) carry;
				carry    =  (carry >> 32) + (prod1 >> 31) + (prod2 >> 32);
			}

			carry += aMax;
			a[1]  =  (uint) carry;
			aMax  =  (uint) (carry >> 32);
		}

		a[0] = aMax;

		if (!smallMontyModulus && CompareTo(0, a, 0, m) >= 0) Subtract(0, a, 0, m);

		Array.Copy(a, 1, x, 0, n);
	}

	/// <summary>
	///   Montgomeries the reduce.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="m">The m.</param>
	/// <param name="mDash">The m dash.</param>
	private static void MontgomeryReduce(uint[] x, uint[] m, uint mDash) // mDash = -m^(-1) mod b
	{
		// NOTE: Not a general purpose reduction (which would allow x up to twice the bitlength of m)
		Debug.Assert(x.Length == m.Length);

		var n = m.Length;

		for (var i = n - 1; i >= 0; --i)
		{
			var   x0 = x[n - 1];
			ulong t  = x0 * mDash;

			var carry = t * m[n - 1] + x0;
			Debug.Assert((uint) carry == 0);
			carry >>= 32;

			for (var j = n - 2; j >= 0; --j)
			{
				carry    +=  t * m[j] + x[j];
				x[j + 1] =   (uint) carry;
				carry    >>= 32;
			}

			x[0] = (uint) carry;
			Debug.Assert(carry >> 32 == 0);
		}

		if (CompareTo(0, x, 0, m) >= 0) Subtract(0, x, 0, m);
	}


	/// <summary>
	///   Compares to.
	/// </summary>
	/// <param name="xIndx">The x indx.</param>
	/// <param name="x">The x.</param>
	/// <param name="yIndx">The y indx.</param>
	/// <param name="y">The y.</param>
	/// <returns>int.</returns>
	private static int CompareTo(int xIndx, uint[] x, int yIndx, uint[] y)
	{
		while (xIndx != x.Length && x[xIndx] == 0U) xIndx++;

		while (yIndx != y.Length && y[yIndx] == 0U) yIndx++;

		return CompareNoLeadingZeroes(xIndx, x, yIndx, y);
	}

	/// <summary>
	///   Compares the no leading zeroes.
	/// </summary>
	/// <param name="xIndx">The x indx.</param>
	/// <param name="x">The x.</param>
	/// <param name="yIndx">The y indx.</param>
	/// <param name="y">The y.</param>
	/// <returns>int.</returns>
	private static int CompareNoLeadingZeroes(int xIndx, IReadOnlyList<uint> x, int yIndx, IReadOnlyList<uint> y)
	{
		var diff = x.Count - y.Count - (xIndx - yIndx);

		if (diff != 0)
			return diff < 0 ? -1 : 1;

		// lengths of magnitudes the same, test the magnitude values

		while (xIndx < x.Count)
		{
			var v1 = x[xIndx++];
			var v2 = y[yIndx++];

			if (v1 != v2)
				return v1 < v2 ? -1 : 1;
		}

		return 0;
	}

	/// <summary>
	///   GCDs the specified value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Gcd(BigInteger value)
	{
		if (value._sign == 0) return Abs();
		if (_sign       == 0) return value.Abs();

		var u = this;
		var v = value;

		while (v._sign != 0)
		{
			var r = u.Mod(v);
			u = v;
			v = r;
		}

		return u;
	}

	/// <summary>
	///   Determines whether [is equal magnitude] [the specified x].
	/// </summary>
	/// <param name="x">The x.</param>
	/// <returns>bool.</returns>
	private bool IsEqualMagnitude(BigInteger x) => _magnitude.Length == x._magnitude.Length && !_magnitude.Where((t, i) => t != x._magnitude[i]).Any();

	/// <summary>
	///   Gets the hash code.
	/// </summary>
	/// <returns>int.</returns>
	public override int GetHashCode()
	{
		var hc = _magnitude.Length;
		switch (_magnitude.Length)
		{
			case > 0:
			{
				hc ^= (int) _magnitude[0];
				if (_magnitude.Length > 1) hc ^= (int) _magnitude[^1];
				break;
			}
		}

		return _sign < 0 ? ~hc : hc;
	}

	/// <summary>
	///   Incs this instance.
	/// </summary>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Inc() =>
		_sign switch
		{
			0   => One,
			< 0 => new BigInteger(-1, DoSubBigLil(_magnitude, One._magnitude), true),
			_   => AddToMagnitude(One._magnitude)
		};

	/// <summary>
	///   Equalses the specified object.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>bool.</returns>
	public override bool Equals(object? obj)
	{
		if (Equals(obj, this))
			return true;
		if (obj is not BigInteger biggie)
			return false;

		return _sign == biggie._sign && IsEqualMagnitude(biggie);
	}

	/// <summary>
	///   Converts to string.
	/// </summary>
	/// <returns>string.</returns>
	public override string ToString() => ToString(10);

	/// <summary>
	///   Converts to string.
	/// </summary>
	/// <param name="radix">The radix.</param>
	/// <returns>string.</returns>
	/// <exception cref="FormatException">Only bases 2, 8, 10, 16 are allowed</exception>
	public string ToString(int radix)
	{
		// TODO Make this method work for other radices (ideally 2 <= radix <= 36)

		switch (radix)
		{
			case 2:
			case 8:
			case 10:
			case 16:
				break;
			default:
				throw new FormatException("Only bases 2, 8, 10, 16 are allowed");
		}

		if (_sign == 0)
			return "0";

		// NOTE: This *should* be unnecessary, since the magnitude *should* never have leading zero digits
		var firstNonZero = 0;
		while (firstNonZero < _magnitude.Length)
		{
			if (_magnitude[firstNonZero] != 0U)
				break;

			++firstNonZero;
		}

		if (firstNonZero == _magnitude.Length)
			return "0";

		var sb = new StringBuilder();
		if (_sign == -1) sb.Append('-');

		switch (radix)
		{
			case 2:
			{
				var pos = firstNonZero;
				sb.Append(Convert.ToString(_magnitude[pos], 2));
				while (++pos < _magnitude.Length) AppendZeroExtendedString(sb, Convert.ToString(_magnitude[pos], 2), 32);
				break;
			}
			case 8:
				var       s    = new List<string>();
				const int mask = (1 << 30) - 1;
			{
				var u    = Abs();
				var bits = u.BitLength;
				while (bits > 30)
				{
					s.Add(Convert.ToString(u.IntValue & mask, 8));
					u    =  u.ShiftRight(30);
					bits -= 30;
				}

				sb.Append(Convert.ToString(u.IntValue, 8));
				for (var i = s.Count - 1; i >= 0; --i) AppendZeroExtendedString(sb, s[i], 10);
				break;
			}
			case 16:
			{
				var pos = firstNonZero;
				sb.Append(Convert.ToString(_magnitude[pos], 16));
				while (++pos < _magnitude.Length) AppendZeroExtendedString(sb, Convert.ToString(_magnitude[pos], 16), 8);
				break;
			}
			// TODO This could work for other radices if there is an alternative to Convert.ToString method
			//default:
			case 10:
			{
				var q = Abs();
				if (q.BitLength < 64)
				{
					sb.Append(Convert.ToString(q.LongValue, radix));
					break;
				}

				// TODO Could cache the moduli for each radix (soft reference?)
				var moduli = new List<BigInteger>();
				var R      = ValueOf(radix);
				while (R.CompareTo(q) <= 0)
				{
					moduli.Add(R);
					R = R.Square();
				}

				var scale = moduli.Count;
				sb.EnsureCapacity(sb.Length + (1 << scale));

				ToString(sb, radix, moduli, scale, q);

				break;
			}
		}

		return sb.ToString();
	}

	/// <summary>
	///   Converts to string.
	/// </summary>
	/// <param name="sb">The sb.</param>
	/// <param name="radix">The radix.</param>
	/// <param name="moduli">The moduli.</param>
	/// <param name="scale">The scale.</param>
	/// <param name="pos">The position.</param>
	private static void ToString(StringBuilder sb, int radix, IList<BigInteger> moduli, int scale, BigInteger pos)
	{
		if (pos.BitLength < 64)
		{
			var s = Convert.ToString(pos.LongValue, radix);
			if (sb.Length > 1 || (sb.Length == 1 && sb[0] != '-'))
				AppendZeroExtendedString(sb, s, 1 << scale);
			else if (pos.SignValue != 0) sb.Append(s);
			return;
		}

		var qr = pos.DivideAndRemainder(moduli[--scale]);

		ToString(sb, radix, moduli, scale, qr[0]);
		ToString(sb, radix, moduli, scale, qr[1]);
	}

	/// <summary>
	///   Appends the zero extended string.
	/// </summary>
	/// <param name="sb">The sb.</param>
	/// <param name="s">The s.</param>
	/// <param name="minLength">The minimum length.</param>
	private static void AppendZeroExtendedString(StringBuilder sb, string s, int minLength)
	{
		for (var len = s.Length; len < minLength; ++len) sb.Append('0');
		sb.Append(s);
	}

	/// <summary>
	///   Creates the u value of.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private static BigInteger CreateUValueOf(ulong value)
	{
		var msw = (uint) (value >> 32);
		var lsw = (uint) value;

		if (msw != 0)
			return new BigInteger(1, new[] {msw, lsw}, false);

		if (lsw != 0)
		{
			var n = new BigInteger(1, new[] {lsw}, false);
			// Check for a power of two
			if ((lsw & -lsw) == lsw) n._nBits = 1;
			return n;
		}

		return Zero;
	}

	/// <summary>
	///   Creates the value of.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private static BigInteger CreateValueOf(long value) =>
		value switch
		{
			< 0 and long.MinValue => CreateValueOf(~value).Not(),
			< 0                   => CreateValueOf(-value).Negate(),
			_                     => CreateUValueOf((ulong) value)
		};

	/// <summary>
	///   Values the of.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public static BigInteger ValueOf(long value) => value >= 0 && value < SmallConstants.Length ? SmallConstants[value] : CreateValueOf(value);

	/// <summary>
	///   Gets the lowest set bit.
	/// </summary>
	/// <returns>int.</returns>
	public int GetLowestSetBit()
	{
		if (_sign == 0) return -1;

		return GetLowestSetBitMaskFirst(uint.MaxValue);
	}

	/// <summary>
	///   Gets the lowest set bit mask first.
	/// </summary>
	/// <param name="firstWordMaskX">The first word mask x.</param>
	/// <returns>int.</returns>
	private int GetLowestSetBitMaskFirst(uint firstWordMaskX)
	{
		int w = _magnitude.Length, offset = 0;

		var word = _magnitude[--w] & firstWordMaskX;
		Debug.Assert(_magnitude[0] != 0U);

		while (word == 0)
		{
			word   =  _magnitude[--w];
			offset += 32;
		}

		offset += BitOperations.TrailingZeroCount(word);

		return offset;
	}

	/// <summary>
	///   Tests the bit.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>bool.</returns>
	/// <exception cref="ArithmeticException">Bit position must not be negative</exception>
	public bool TestBit(int n)
	{
		if (n < 0)
			throw new ArithmeticException("Bit position must not be negative");

		if (_sign < 0)
			return !Not().TestBit(n);

		var wordNum = n / 32;
		if (wordNum >= _magnitude.Length)
			return false;

		var word = _magnitude[_magnitude.Length - 1 - wordNum];
		return ((word >> (n % 32)) & 1U) != 0;
	}

	/// <summary>
	///   Ors the specified value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Or(BigInteger value)
	{
		if (_sign == 0)
			return value;

		if (value._sign == 0)
			return this;

		var aMag = _sign       > 0 ? _magnitude : Add(One)._magnitude;
		var bMag = value._sign > 0 ? value._magnitude : value.Add(One)._magnitude;

		var resultNeg    = _sign < 0 || value._sign < 0;
		var resultLength = System.Math.Max(aMag.Length, bMag.Length);
		var resultMag    = new uint[resultLength];

		var aStart = resultMag.Length - aMag.Length;
		var bStart = resultMag.Length - bMag.Length;

		for (var i = 0; i < resultMag.Length; ++i)
		{
			var aWord = i >= aStart ? aMag[i - aStart] : 0U;
			var bWord = i >= bStart ? bMag[i - bStart] : 0U;

			if (_sign < 0) aWord = ~aWord;

			if (value._sign < 0) bWord = ~bWord;

			resultMag[i] = aWord | bWord;

			if (resultNeg) resultMag[i] = ~resultMag[i];
		}

		var result = new BigInteger(1, resultMag, true);

		// TODO Optimise this case
		if (resultNeg) result = result.Not();

		return result;
	}

	/// <summary>
	///   Xors the specified value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>core.Math.BigInteger.</returns>
	public BigInteger Xor(BigInteger value)
	{
		if (_sign == 0)
			return value;

		if (value._sign == 0)
			return this;

		var aMag = _sign       > 0 ? _magnitude : Add(One)._magnitude;
		var bMag = value._sign > 0 ? value._magnitude : value.Add(One)._magnitude;

		// TODO Can just replace with sign != value.sign?
		var resultNeg    = (_sign < 0 && value._sign >= 0) || (_sign >= 0 && value._sign < 0);
		var resultLength = System.Math.Max(aMag.Length, bMag.Length);
		var resultMag    = new uint[resultLength];

		var aStart = resultMag.Length - aMag.Length;
		var bStart = resultMag.Length - bMag.Length;

		for (var i = 0; i < resultMag.Length; ++i)
		{
			var aWord = i >= aStart ? aMag[i - aStart] : 0U;
			var bWord = i >= bStart ? bMag[i - bStart] : 0U;

			if (_sign < 0) aWord = ~aWord;

			if (value._sign < 0) bWord = ~bWord;

			resultMag[i] = aWord ^ bWord;

			if (resultNeg) resultMag[i] = ~resultMag[i];
		}

		var result = new BigInteger(1, resultMag, true);

		// TODO Optimise this case
		if (resultNeg) result = result.Not();

		return result;
	}

	/// <summary>
	///   Sets the bit.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Bit address less than zero</exception>
	public BigInteger SetBit(int n)
	{
		if (n < 0)
			throw new ArithmeticException("Bit address less than zero");

		if (TestBit(n))
			return this;

		// TODO Handle negative values and zero
		if (_sign > 0 && n < BitLength - 1)
			return FlipExistingBit(n);

		return Or(One.ShiftLeft(n));
	}

	/// <summary>
	///   Clears the bit.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Bit address less than zero</exception>
	public BigInteger ClearBit(int n)
	{
		if (n < 0)
			throw new ArithmeticException("Bit address less than zero");

		if (!TestBit(n))
			return this;

		// TODO Handle negative values
		if (_sign > 0 && n < BitLength - 1)
			return FlipExistingBit(n);

		return AndNot(One.ShiftLeft(n));
	}

	/// <summary>
	///   Flips the bit.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	/// <exception cref="ArithmeticException">Bit address less than zero</exception>
	public BigInteger FlipBit(int n)
	{
		if (n < 0)
			throw new ArithmeticException("Bit address less than zero");

		// TODO Handle negative values and zero
		if (_sign > 0 && n < BitLength - 1)
			return FlipExistingBit(n);

		return Xor(One.ShiftLeft(n));
	}

	/// <summary>
	///   Flips the existing bit.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>core.Math.BigInteger.</returns>
	private BigInteger FlipExistingBit(int n)
	{
		Debug.Assert(_sign > 0);
		Debug.Assert(n     >= 0);
		Debug.Assert(n     < BitLength - 1);

		var mag = (uint[]) _magnitude.Clone();
		mag[mag.Length - 1 - (n >> 5)] ^= 1U << (n & 31); // Flip bit
		return new BigInteger(_sign, mag, false);
	}
}
