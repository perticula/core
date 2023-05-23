// perticula - core - VmpcRandomGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Cryptography;

namespace core.Random;

/// <summary>
///   Class VmpcRandomGenerator.
///   Implements the <see cref="core.Random.IRandomGenerator" />
/// </summary>
/// <seealso cref="core.Random.IRandomGenerator" />
public class VmpcRandomGenerator : IRandomGenerator
{
	/// <summary>
	///   The permutation
	/// </summary>
	private readonly byte[] _permutation =
	{
		0XBB, 0X2C, 0X62, 0X7F, 0XB5, 0XAA, 0XD4, 0X0D, 0X81, 0XFE, 0XB2, 0X82, 0XCB, 0XA0, 0XA1, 0X08,
		0X18, 0X71, 0X56, 0XE8, 0X49, 0X02, 0X10, 0XC4, 0XDE, 0X35, 0XA5, 0XEC, 0X80, 0X12, 0XB8, 0X69,
		0XDA, 0X2F, 0X75, 0XCC, 0XA2, 0X09, 0X36, 0X03, 0X61, 0X2D, 0XFD, 0XE0, 0XDD, 0X05, 0X43, 0X90,
		0XAD, 0XC8, 0XE1, 0XAF, 0X57, 0X9B, 0X4C, 0XD8, 0X51, 0XAE, 0X50, 0X85, 0X3C, 0X0A, 0XE4, 0XF3,
		0X9C, 0X26, 0X23, 0X53, 0XC9, 0X83, 0X97, 0X46, 0XB1, 0X99, 0X64, 0X31, 0X77, 0XD5, 0X1D, 0XD6,
		0X78, 0XBD, 0X5E, 0XB0, 0X8A, 0X22, 0X38, 0XF8, 0X68, 0X2B, 0X2A, 0XC5, 0XD3, 0XF7, 0XBC, 0X6F,
		0XDF, 0X04, 0XE5, 0X95, 0X3E, 0X25, 0X86, 0XA6, 0X0B, 0X8F, 0XF1, 0X24, 0X0E, 0XD7, 0X40, 0XB3,
		0XCF, 0X7E, 0X06, 0X15, 0X9A, 0X4D, 0X1C, 0XA3, 0XDB, 0X32, 0X92, 0X58, 0X11, 0X27, 0XF4, 0X59,
		0XD0, 0X4E, 0X6A, 0X17, 0X5B, 0XAC, 0XFF, 0X07, 0XC0, 0X65, 0X79, 0XFC, 0XC7, 0XCD, 0X76, 0X42,
		0X5D, 0XE7, 0X3A, 0X34, 0X7A, 0X30, 0X28, 0X0F, 0X73, 0X01, 0XF9, 0XD1, 0XD2, 0X19, 0XE9, 0X91,
		0XB9, 0X5A, 0XED, 0X41, 0X6D, 0XB4, 0XC3, 0X9E, 0XBF, 0X63, 0XFA, 0X1F, 0X33, 0X60, 0X47, 0X89,
		0XF0, 0X96, 0X1A, 0X5F, 0X93, 0X3D, 0X37, 0X4B, 0XD9, 0XA8, 0XC1, 0X1B, 0XF6, 0X39, 0X8B, 0XB7,
		0X0C, 0X20, 0XCE, 0X88, 0X6E, 0XB6, 0X74, 0X8E, 0X8D, 0X16, 0X29, 0XF2, 0X87, 0XF5, 0XEB, 0X70,
		0XE3, 0XFB, 0X55, 0X9F, 0XC6, 0X44, 0X4A, 0X45, 0X7D, 0XE2, 0X6B, 0X5C, 0X6C, 0X66, 0XA9, 0X8C,
		0XEE, 0X84, 0X13, 0XA7, 0X1E, 0X9D, 0XDC, 0X67, 0X48, 0XBA, 0X2E, 0XE6, 0XA4, 0XAB, 0X7C, 0X94,
		0X00, 0X21, 0XEF, 0XEA, 0XBE, 0XCA, 0X72, 0X4F, 0X52, 0X98, 0X3F, 0XC2, 0X14, 0X7B, 0X3B, 0X54
	};

	/// <summary>
	///   The n
	/// </summary>
	private byte _n;

	/// <summary>
	///   The s
	/// </summary>
	private byte _s = 0xBE;

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public void AddSeedMaterial(byte[]? seed)
	{
		if (seed == null) return;

		AddSeedMaterial(seed.AsSpan());
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public void AddSeedMaterial(ReadOnlySpan<byte> seed)
	{
		foreach (var @byte in seed)
		{
			var pn = _permutation[_n];
			_s               = _permutation[(_s + pn + @byte) & 0xff];
			_permutation[_n] = _permutation[_s];
			_permutation[_s] = pn;
			_n               = (byte) (_n + 1);
		}
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public void AddSeedMaterial(long seed)
	{
		Span<byte> bytes = stackalloc byte[8];
		Pack.UInt64_To_BigEndian((ulong) seed, bytes);
		AddSeedMaterial(bytes);
	}

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public void NextBytes(byte[] bytes) => NextBytes(bytes, 0, bytes.Length);

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="start">The start.</param>
	/// <param name="len">The length.</param>
	public void NextBytes(byte[] bytes, int start, int len) => NextBytes(bytes.AsSpan(start, len));

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public void NextBytes(Span<byte> bytes)
	{
		lock (_permutation)
		{
			for (var i = 0; i < bytes.Length; ++i)
			{
				var pn = _permutation[_n];
				_s = _permutation[(_s + pn) & 0xFF];
				var ps = _permutation[_s];
				bytes[i]         = _permutation[(_permutation[ps] + 1) & 0xFF];
				_permutation[_s] = pn;
				_permutation[_n] = ps;
				_n               = (byte) (_n + 1);
			}
		}
	}
}
