// perticula - core - BigIntegers.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Math.Raw;
using core.Random;

namespace core.Math;

public static class BigIntegers
{
	private const int MaxIterations = 1000;

	public static readonly BigInteger Zero = BigInteger.Zero;
	public static readonly BigInteger One  = BigInteger.One;

	public static void AsUint32ArrayLittleEndian(BigInteger n, Span<uint> buf)
	{
		var uintsLength = n.GetLengthofUInt32Array();

		if (uintsLength > buf.Length) throw new ArgumentException("standard length exceeded", nameof(n));

		n.ToUInt32ArrayLittleEndian(buf);

		var sign = (int) buf[uintsLength - 1] >> 31;
		buf[uintsLength..].Fill((uint) sign);
	}

	public static byte[] AsUnsignedByteArray(BigInteger n) => n.ToByteArrayUnsigned();

	public static byte[] AsUnsignedByteArray(int length, BigInteger n)
	{
		var bytesLength = n.GetLengthofByteArrayUnsigned();

		if (bytesLength > length)
			throw new ArgumentException("standard length exceeded", nameof(n));

		var bytes = new byte[length];
		n.ToByteArrayUnsigned(bytes.AsSpan(length - bytesLength));
		return bytes;
	}

	public static void AsUnsignedByteArray(BigInteger n, byte[] buf, int off, int len) => AsUnsignedByteArray(n, buf.AsSpan(off, len));

	public static void AsUnsignedByteArray(BigInteger n, Span<byte> buf)
	{
		var bytesLength = n.GetLengthofByteArrayUnsigned();

		if (bytesLength > buf.Length) throw new ArgumentException("standard length exceeded", nameof(n));

		buf[..^bytesLength].Clear();
		n.ToByteArrayUnsigned(buf[^bytesLength..]);
	}

	public static BigInteger CreateRandomBigInteger(int bitLength, SecureRandom secureRandom) => new(bitLength, secureRandom);

	public static BigInteger CreateRandomInRange(BigInteger min, BigInteger max, SecureRandom random)
	{
		var cmp = min.CompareTo(max);
		if (cmp >= 0)
		{
			if (cmp > 0) throw new ArgumentException("'min' may not be greater than 'max'");

			return min;
		}

		if (min.BitLength > max.BitLength / 2)
			return CreateRandomInRange(BigInteger.Zero, max.Subtract(min), random).Add(min);

		for (var i = 0; i < MaxIterations; ++i)
		{
			var x = new BigInteger(max.BitLength, random);
			if (x.CompareTo(min) >= 0 && x.CompareTo(max) <= 0)
				return x;
		}

		// fall back to a faster (restricted) method
		return new BigInteger(max.Subtract(min).BitLength - 1, random).Add(min);
	}

	public static BigInteger FromUnsignedByteArray(byte[] buf) => new(1, buf);

	public static BigInteger FromUnsignedByteArray(byte[] buf, int off, int length) => new(1, buf, off, length);

	public static int GetByteLength(BigInteger n) => n.GetLengthofByteArray();

	public static int GetUnsignedByteLength(BigInteger n) => n.GetLengthofByteArrayUnsigned();

	public static BigInteger ModOddInverse(BigInteger valueM, BigInteger valueX)
	{
		if (!valueM.TestBit(0)) throw new ArgumentException("must be odd", nameof(valueM));
		if (valueM.SignValue != 1) throw new ArithmeticException("BigInteger: modulus not positive");
		if (valueX.SignValue < 0 || valueX.CompareTo(valueM) >= 0) valueX = valueX.Mod(valueM);

		var bits = valueM.BitLength;

		if (bits <= 2048)
		{
			var        len = Nat.GetLengthForBits(bits);
			Span<uint> m   = stackalloc uint[len];
			Span<uint> x   = stackalloc uint[len];
			Span<uint> z   = stackalloc uint[len];
			Nat.FromBigInteger(bits, valueM, m);
			Nat.FromBigInteger(bits, valueX, x);
			if (0 == Mod.ModOddInverse(m, x, z))
				throw new ArithmeticException("BigInteger not invertible");
			return Nat.ToBigInteger(len, z);
		}
		else
		{
			var m   = Nat.FromBigInteger(bits, valueM);
			var x   = Nat.FromBigInteger(bits, valueX);
			var len = m.Length;
			var z   = Nat.Create(len);
			if (0 == Mod.ModOddInverse(m, x, z))
				throw new ArithmeticException("BigInteger not invertible");
			return Nat.ToBigInteger(len, z);
		}
	}

	public static BigInteger ModOddInverseVar(BigInteger valueM, BigInteger valueX)
	{
		if (!valueM.TestBit(0)) throw new ArgumentException("must be odd", nameof(valueM));
		if (valueM.SignValue != 1) throw new ArithmeticException("BigInteger: modulus not positive");
		if (valueM.Equals(One)) return Zero;
		if (valueX.SignValue < 0 || valueX.CompareTo(valueM) >= 0) valueX = valueX.Mod(valueM);
		if (valueX.Equals(One)) return One;

		var bits = valueM.BitLength;

		if (bits <= 2048)
		{
			var        len = Nat.GetLengthForBits(bits);
			Span<uint> m   = stackalloc uint[len];
			Span<uint> x   = stackalloc uint[len];
			Span<uint> z   = stackalloc uint[len];
			Nat.FromBigInteger(bits, valueM, m);
			Nat.FromBigInteger(bits, valueX, x);
			if (!Mod.ModOddInverseVar(m, x, z))
				throw new ArithmeticException("BigInteger not invertible");
			return Nat.ToBigInteger(len, z);
		}
		else
		{
			var m   = Nat.FromBigInteger(bits, valueM);
			var x   = Nat.FromBigInteger(bits, valueX);
			var len = m.Length;
			var z   = Nat.Create(len);
			if (!Mod.ModOddInverseVar(m, x, z))
				throw new ArithmeticException("BigInteger not invertible");
			return Nat.ToBigInteger(len, z);
		}
	}
}
