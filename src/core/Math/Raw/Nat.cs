// perticula - core - Nat.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Numerics;
using core.Cryptography;

namespace core.Math.Raw;

public static class Nat
{
	private const ulong M = 0xFFFFFFFFUL;

	public static uint Add(int len, uint[] x, uint[] y, uint[] z)
	{
		var c = 0UL;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint Add(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		var c = 0UL;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint Add33At(int len, uint x, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = (ulong) z[zPos + 0] + x;
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) z[zPos + 1] + 1;
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 2);
	}

	public static uint Add33At(int len, uint x, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = (ulong) z[zOff + zPos] + x;
		z[zOff + zPos]     =   (uint) c;
		c                  >>= 32;
		c                  +=  (ulong) z[zOff + zPos + 1] + 1;
		z[zOff + zPos + 1] =   (uint) c;
		c                  >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zOff, zPos + 2);
	}

	public static uint Add33At(int len, uint x, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = (ulong) z[zPos + 0] + x;
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) z[zPos + 1] + 1;
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 2);
	}

	public static uint Add33To(int len, uint x, uint[] z)
	{
		var c = (ulong) z[0] + x;
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) z[1] + 1;
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : IncAt(len, z, 2);
	}

	public static uint Add33To(int len, uint x, uint[] z, int zOff)
	{
		var c = (ulong) z[zOff + 0] + x;
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) z[zOff + 1] + 1;
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zOff, 2);
	}

	public static uint Add33To(int len, uint x, Span<uint> z)
	{
		var c = (ulong) z[0] + x;
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) z[1] + 1;
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : IncAt(len, z, 2);
	}

	public static uint AddBothTo(int len, uint[] x, uint[] y, uint[] z)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + y[i] + z[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint AddBothTo(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c           +=  (ulong) x[xOff + i] + y[yOff + i] + z[zOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (uint) c;
	}

	public static uint AddBothTo(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + y[i] + z[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint AddDWordAt(int len, ulong x, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = z[zPos + 0] + (x & M);
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  z[zPos + 1] + (x >> 32);
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 2);
	}

	public static uint AddDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = z[zOff + zPos] + (x & M);
		z[zOff + zPos]     =   (uint) c;
		c                  >>= 32;
		c                  +=  z[zOff + zPos + 1] + (x >> 32);
		z[zOff + zPos + 1] =   (uint) c;
		c                  >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zOff, zPos + 2);
	}

	public static uint AddDWordAt(int len, ulong x, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = z[zPos + 0] + (x & M);
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  z[zPos + 1] + (x >> 32);
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 2);
	}

	public static uint AddDWordTo(int len, ulong x, uint[] z)
	{
		var c = z[0] + (x & M);
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  z[1] + (x >> 32);
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : IncAt(len, z, 2);
	}

	public static uint AddDWordTo(int len, ulong x, uint[] z, int zOff)
	{
		var c = z[zOff + 0] + (x & M);
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  z[zOff + 1] + (x >> 32);
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zOff, 2);
	}

	public static uint AddDWordTo(int len, ulong x, Span<uint> z)
	{
		var c = z[0] + (x & M);
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  z[1] + (x >> 32);
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : IncAt(len, z, 2);
	}

	public static uint AddTo(int len, uint[] x, uint[] z)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + z[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint AddTo(int len, uint[] x, int xOff, uint[] z, int zOff)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c           +=  (ulong) x[xOff + i] + z[zOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (uint) c;
	}

	public static uint AddTo(int len, ReadOnlySpan<uint> x, Span<uint> z)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + z[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint AddTo(int len, uint[] x, int xOff, uint[] z, int zOff, uint cIn)
	{
		ulong c = cIn;
		for (var i = 0; i < len; ++i)
		{
			c           +=  (ulong) x[xOff + i] + z[zOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (uint) c;
	}

	public static uint AddTo(int len, ReadOnlySpan<uint> x, Span<uint> z, uint cIn)
	{
		ulong c = cIn;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + z[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint AddToEachOther(int len, uint[] u, int uOff, uint[] v, int vOff)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c           +=  (ulong) u[uOff + i] + v[vOff + i];
			u[uOff + i] =   (uint) c;
			v[vOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (uint) c;
	}

	public static uint AddToEachOther(int len, Span<uint> u, Span<uint> v)
	{
		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) u[i] + v[i];
			u[i] =   (uint) c;
			v[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint AddWordAt(int len, uint x, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len - 1);
		var c = (ulong) x + z[zPos];
		z[zPos] =   (uint) c;
		c       >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 1);
	}

	public static uint AddWordAt(int len, uint x, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len - 1);
		var c = (ulong) x + z[zOff + zPos];
		z[zOff + zPos] =   (uint) c;
		c              >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zOff, zPos + 1);
	}

	public static uint AddWordAt(int len, uint x, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len - 1);
		var c = (ulong) x + z[zPos];
		z[zPos] =   (uint) c;
		c       >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 1);
	}

	public static uint AddWordTo(int len, uint x, uint[] z)
	{
		var c = (ulong) x + z[0];
		z[0] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : IncAt(len, z, 1);
	}

	public static uint AddWordTo(int len, uint x, uint[] z, int zOff)
	{
		var c = (ulong) x + z[zOff];
		z[zOff] =   (uint) c;
		c       >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zOff, 1);
	}

	public static uint AddWordTo(int len, uint x, Span<uint> z)
	{
		var c = (ulong) x + z[0];
		z[0] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : IncAt(len, z, 1);
	}

	public static uint CAdd(int len, int mask, uint[] x, uint[] y, uint[] z)
	{
		var lMask = (uint) -(mask & 1);

		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + (y[i] & lMask);
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static uint CAdd(int len, int mask, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		var lMask = (uint) -(mask & 1);

		ulong c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (ulong) x[i] + (y[i] & lMask);
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (uint) c;
	}

	public static void CMov(int len, int mask, uint[] x, int xOff, uint[] z, int zOff)
	{
		var lMask = (uint) -(mask & 1);

		for (var i = 0; i < len; ++i)
		{
			uint zI = z[zOff + i], diff = zI ^ x[xOff + i];
			zI         ^= diff & lMask;
			z[zOff + i] =  zI;
		}

		//uint half = 0x55555555U, rest = half << (-(int)MASK);

		//for (int i = 0; i < len; ++i)
		//{
		//    uint z_i = z[zOff + i], diff = z_i ^ x[xOff + i];
		//    z_i ^= (diff & half);
		//    z_i ^= (diff & rest);
		//    z[zOff + i] = z_i;
		//}
	}

	public static void CMov(int len, int mask, ReadOnlySpan<uint> x, Span<uint> z)
	{
		var lMask = (uint) -(mask & 1);

		for (var i = 0; i < len; ++i)
		{
			uint zI = z[i], diff = zI ^ x[i];
			zI  ^= diff & lMask;
			z[i] =  zI;
		}

		//uint half = 0x55555555U, rest = half << (-(int)MASK);

		//for (int i = 0; i < len; ++i)
		//{
		//    uint z_i = z[i], diff = z_i ^ x[i];
		//    z_i ^= (diff & half);
		//    z_i ^= (diff & rest);
		//    z[i] = z_i;
		//}
	}

	public static int Compare(int len, uint[] x, uint[] y)
	{
		for (var i = len - 1; i >= 0; --i)
		{
			var xI = x[i];
			var yI = y[i];
			if (xI < yI)
				return -1;
			if (xI > yI)
				return 1;
		}

		return 0;
	}

	public static int Compare(int len, uint[] x, int xOff, uint[] y, int yOff)
	{
		for (var i = len - 1; i >= 0; --i)
		{
			var xI = x[xOff + i];
			var yI = y[yOff + i];
			if (xI < yI)
				return -1;
			if (xI > yI)
				return 1;
		}

		return 0;
	}

	public static int Compare(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y)
	{
		for (var i = len - 1; i >= 0; --i)
		{
			var xI = x[i];
			var yI = y[i];
			if (xI < yI)
				return -1;
			if (xI > yI)
				return 1;
		}

		return 0;
	}

	public static uint[] Copy(int len, uint[] x)
	{
		var z = new uint[len];
		Array.Copy(x, 0, z, 0, len);
		return z;
	}

	public static void Copy(int len, uint[] x, uint[] z)
	{
		Array.Copy(x, 0, z, 0, len);
	}

	public static void Copy(int len, uint[] x, int xOff, uint[] z, int zOff)
	{
		Array.Copy(x, xOff, z, zOff, len);
	}

	public static void Copy(int len, ReadOnlySpan<uint> x, Span<uint> z)
	{
		x[..len].CopyTo(z);
	}

	public static ulong[] Copy64(int len, ulong[] x)
	{
		var z = new ulong[len];
		Array.Copy(x, 0, z, 0, len);
		return z;
	}

	public static void Copy64(int len, ulong[] x, ulong[] z)
	{
		Array.Copy(x, 0, z, 0, len);
	}

	public static void Copy64(int len, ulong[] x, int xOff, ulong[] z, int zOff)
	{
		Array.Copy(x, xOff, z, zOff, len);
	}

	public static void Copy64(int len, ReadOnlySpan<ulong> x, Span<ulong> z)
	{
		x[..len].CopyTo(z);
	}

	public static uint[] Create(int len) => new uint[len];

	public static ulong[] Create64(int len) => new ulong[len];

	public static int CSub(int len, int mask, uint[] x, uint[] y, uint[] z)
	{
		long lMask = (uint) -(mask & 1);
		long c    = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  x[i] - (y[i] & lMask);
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int CSub(int len, int mask, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		long lMask = (uint) -(mask & 1);
		long c    = 0;
		for (var i = 0; i < len; ++i)
		{
			c           +=  x[xOff + i] - (y[yOff + i] & lMask);
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (int) c;
	}

	public static int CSub(int len, int mask, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		long lMask = (uint) -(mask & 1);
		long c    = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  x[i] - (y[i] & lMask);
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int Dec(int len, uint[] z)
	{
		for (var i = 0; i < len; ++i)
		{
			if (--z[i] != uint.MaxValue)
				return 0;
		}

		return -1;
	}

	public static int Dec(int len, Span<uint> z)
	{
		for (var i = 0; i < len; ++i)
		{
			if (--z[i] != uint.MaxValue)
				return 0;
		}

		return -1;
	}

	public static int Dec(int len, uint[] x, uint[] z)
	{
		var i = 0;
		while (i < len)
		{
			var c = x[i] - 1;
			z[i] = c;
			++i;
			if (c != uint.MaxValue)
			{
				while (i < len)
				{
					z[i] = x[i];
					++i;
				}

				return 0;
			}
		}

		return -1;
	}

	public static int Dec(int len, ReadOnlySpan<uint> x, Span<uint> z)
	{
		var i = 0;
		while (i < len)
		{
			var c = x[i] - 1;
			z[i] = c;
			++i;
			if (c != uint.MaxValue)
			{
				while (i < len)
				{
					z[i] = x[i];
					++i;
				}

				return 0;
			}
		}

		return -1;
	}

	public static int DecAt(int len, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len);
		for (var i = zPos; i < len; ++i)
		{
			if (--z[i] != uint.MaxValue)
				return 0;
		}

		return -1;
	}

	public static int DecAt(int len, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len);
		for (var i = zPos; i < len; ++i)
		{
			if (--z[zOff + i] != uint.MaxValue)
				return 0;
		}

		return -1;
	}

	public static int DecAt(int len, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len);
		for (var i = zPos; i < len; ++i)
		{
			if (--z[i] != uint.MaxValue)
				return 0;
		}

		return -1;
	}

	public static bool Eq(int len, uint[] x, uint[] y)
	{
		for (var i = len - 1; i >= 0; --i)
		{
			if (x[i] != y[i])
				return false;
		}

		return true;
	}

	public static bool Eq(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y)
	{
		for (var i = len - 1; i >= 0; --i)
		{
			if (x[i] != y[i])
				return false;
		}

		return true;
	}

	public static uint EqualTo(int len, uint[] x, uint y)
	{
		var d                           = x[0] ^ y;
		for (var i = 1; i < len; ++i) d |= x[i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualTo(int len, uint[] x, int xOff, uint y)
	{
		var d                           = x[xOff] ^ y;
		for (var i = 1; i < len; ++i) d |= x[xOff + i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualTo(int len, ReadOnlySpan<uint> x, uint y)
	{
		var d                           = x[0] ^ y;
		for (var i = 1; i < len; ++i) d |= x[i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualTo(int len, uint[] x, uint[] y)
	{
		uint d                          = 0;
		for (var i = 0; i < len; ++i) d |= x[i] ^ y[i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualTo(int len, uint[] x, int xOff, uint[] y, int yOff)
	{
		uint d                          = 0;
		for (var i = 0; i < len; ++i) d |= x[xOff + i] ^ y[yOff + i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualTo(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y)
	{
		uint d                          = 0;
		for (var i = 0; i < len; ++i) d |= x[i] ^ y[i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualToZero(int len, uint[] x)
	{
		uint d                          = 0;
		for (var i = 0; i < len; ++i) d |= x[i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualToZero(int len, uint[] x, int xOff)
	{
		uint d                          = 0;
		for (var i = 0; i < len; ++i) d |= x[xOff + i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint EqualToZero(int len, ReadOnlySpan<uint> x)
	{
		uint d                          = 0;
		for (var i = 0; i < len; ++i) d |= x[i];
		d = (d >> 1) | (d & 1);
		return (uint) (((int) d - 1) >> 31);
	}

	public static uint[] FromBigInteger(int bits, BigInteger x)
	{
		if (x.SignValue < 0 || x.BitLength > bits)
			throw new ArgumentException("invalid bit range", nameof(bits));

		var len = GetLengthForBits(bits);
		var z   = Create(len);

		var xLen = x.GetLengthofUInt32ArrayUnsigned();
		x.ToUInt32ArrayLittleEndianUnsigned(z.AsSpan(0, xLen));

		return z;
	}

	public static void FromBigInteger(int bits, BigInteger x, Span<uint> z)
	{
		if (x.SignValue < 0 || x.BitLength > bits)
			throw new ArgumentException("invalid bit range", nameof(bits));

		var len = GetLengthForBits(bits);
		if (z.Length < len)
			throw new ArgumentException("invalid bit range", nameof(bits));

		var xLen = x.GetLengthofUInt32ArrayUnsigned();
		x.ToUInt32ArrayLittleEndianUnsigned(z[..xLen]);
		z[xLen..].Clear();
	}

	public static ulong[] FromBigInteger64(int bits, BigInteger x)
	{
		if (x.SignValue < 0 || x.BitLength > bits)
			throw new ArgumentException("invalid bit range", nameof(bits));

		var len = GetLengthForBits64(bits);
		var z   = Create64(len);

		// NOTE: Use a fixed number of loop iterations
		z[0] = (ulong) x.LongValue;
		for (var i = 1; i < len; ++i)
		{
			x    = x.ShiftRight(64);
			z[i] = (ulong) x.LongValue;
		}

		return z;
	}

	public static void FromBigInteger64(int bits, BigInteger x, Span<ulong> z)
	{
		if (x.SignValue < 0 || x.BitLength > bits)
			throw new ArgumentException("invalid bit range", nameof(bits));

		var len = GetLengthForBits64(bits);
		if (z.Length < len)
			throw new ArgumentException("invalid bit range", nameof(bits));

		// NOTE: Use a fixed number of loop iterations
		z[0] = (ulong) x.LongValue;
		for (var i = 1; i < len; ++i)
		{
			x    = x.ShiftRight(64);
			z[i] = (ulong) x.LongValue;
		}
	}

	public static uint GetBit(uint[] x, int bit)
	{
		if (bit == 0)
			return x[0] & 1;

		var w = bit >> 5;
		if (w < 0 || w >= x.Length)
			return 0;

		var b = bit & 31;
		return (x[w] >> b) & 1;
	}

	public static uint GetBit(ReadOnlySpan<uint> x, int bit)
	{
		if (bit == 0)
			return x[0] & 1;

		var w = bit >> 5;
		if (w < 0 || w >= x.Length)
			return 0;

		var b = bit & 31;
		return (x[w] >> b) & 1;
	}

	public static int GetLengthForBits(int bits)
	{
		if (bits < 1)
			throw new ArgumentException("bits < 1", nameof(bits));

		return (int) (((uint) bits + 31) >> 5);
	}

	public static int GetLengthForBits64(int bits)
	{
		if (bits < 1)
			throw new ArgumentException("bits < 1", nameof(bits));

		return (int) (((uint) bits + 63) >> 6);
	}

	public static bool Gte(int len, uint[] x, uint[] y)
	{
		for (var i = len - 1; i >= 0; --i)
		{
			uint xI = x[i], yI = y[i];
			if (xI < yI)
				return false;
			if (xI > yI)
				return true;
		}

		return true;
	}

	public static bool Gte(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y)
	{
		for (var i = len - 1; i >= 0; --i)
		{
			uint xI = x[i], yI = y[i];
			if (xI < yI)
				return false;
			if (xI > yI)
				return true;
		}

		return true;
	}

	public static uint Inc(int len, uint[] z)
	{
		for (var i = 0; i < len; ++i)
		{
			if (++z[i] != uint.MinValue)
				return 0;
		}

		return 1;
	}

	public static uint Inc(int len, Span<uint> z)
	{
		for (var i = 0; i < len; ++i)
		{
			if (++z[i] != uint.MinValue)
				return 0;
		}

		return 1;
	}

	public static uint Inc(int len, uint[] x, uint[] z)
	{
		var i = 0;
		while (i < len)
		{
			var c = x[i] + 1;
			z[i] = c;
			++i;
			if (c != 0)
			{
				while (i < len)
				{
					z[i] = x[i];
					++i;
				}

				return 0;
			}
		}

		return 1;
	}

	public static uint Inc(int len, ReadOnlySpan<uint> x, Span<uint> z)
	{
		var i = 0;
		while (i < len)
		{
			var c = x[i] + 1;
			z[i] = c;
			++i;
			if (c != 0)
			{
				while (i < len)
				{
					z[i] = x[i];
					++i;
				}

				return 0;
			}
		}

		return 1;
	}

	public static uint IncAt(int len, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len);
		for (var i = zPos; i < len; ++i)
		{
			if (++z[i] != uint.MinValue)
				return 0;
		}

		return 1;
	}

	public static uint IncAt(int len, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len);
		for (var i = zPos; i < len; ++i)
		{
			if (++z[zOff + i] != uint.MinValue)
				return 0;
		}

		return 1;
	}

	public static uint IncAt(int len, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len);
		for (var i = zPos; i < len; ++i)
		{
			if (++z[i] != uint.MinValue)
				return 0;
		}

		return 1;
	}

	public static bool IsOne(int len, uint[] x)
	{
		if (x[0] != 1)
			return false;

		for (var i = 1; i < len; ++i)
		{
			if (x[i] != 0)
				return false;
		}

		return true;
	}

	public static bool IsOne(int len, ReadOnlySpan<uint> x)
	{
		if (x[0] != 1)
			return false;

		for (var i = 1; i < len; ++i)
		{
			if (x[i] != 0)
				return false;
		}

		return true;
	}

	public static bool IsZero(int len, uint[] x)
	{
		if (x[0] != 0)
			return false;

		for (var i = 1; i < len; ++i)
		{
			if (x[i] != 0)
				return false;
		}

		return true;
	}

	public static bool IsZero(int len, ReadOnlySpan<uint> x)
	{
		if (x[0] != 0)
			return false;

		for (var i = 1; i < len; ++i)
		{
			if (x[i] != 0)
				return false;
		}

		return true;
	}

	public static int LessThan(int len, uint[] x, uint[] y)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c +=  (long) x[i] - y[i];
			c >>= 32;
		}

		Debug.Assert(c == 0L || c == -1L);
		return (int) c;
	}

	public static int LessThan(int len, uint[] x, int xOff, uint[] y, int yOff)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c +=  (long) x[xOff + i] - y[yOff + i];
			c >>= 32;
		}

		Debug.Assert(c == 0L || c == -1L);
		return (int) c;
	}

	public static int LessThan(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c +=  (long) x[i] - y[i];
			c >>= 32;
		}

		Debug.Assert(c == 0L || c == -1L);
		return (int) c;
	}

	public static void Mul(int len, uint[] x, uint[] y, uint[] zz)
	{
		zz[len] = MulWord(len, x[0], y, zz);

		for (var i = 1; i < len; ++i) zz[i + len] = MulWordAddTo(len, x[i], y, 0, zz, i);
	}

	public static void Mul(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
	{
		zz[zzOff + len] = MulWord(len, x[xOff], y, yOff, zz, zzOff);

		for (var i = 1; i < len; ++i) zz[zzOff + i + len] = MulWordAddTo(len, x[xOff + i], y, yOff, zz, zzOff + i);
	}

	public static void Mul(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> zz)
	{
		zz[len] = MulWord(len, x[0], y, zz);

		for (var i = 1; i < len; ++i) zz[i + len] = MulWordAddTo(len, x[i], y, zz[i..]);
	}

	public static void Mul(uint[] x, int xOff, int xLen, uint[] y, int yOff, int yLen, uint[] zz, int zzOff)
	{
		zz[zzOff + yLen] = MulWord(yLen, x[xOff], y, yOff, zz, zzOff);

		for (var i = 1; i < xLen; ++i) zz[zzOff + i + yLen] = MulWordAddTo(yLen, x[xOff + i], y, yOff, zz, zzOff + i);
	}

	public static void Mul(ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> zz)
	{
		int xLen = x.Length, yLen = y.Length;
		zz[yLen] = MulWord(yLen, x[0], y, zz);

		for (var i = 1; i < xLen; ++i) zz[i + yLen] = MulWordAddTo(yLen, x[i], y, zz[i..]);
	}

	public static uint MulAddTo(int len, uint[] x, uint[] y, uint[] zz)
	{
		ulong zc = 0;
		for (var i = 0; i < len; ++i)
		{
			zc          +=  MulWordAddTo(len, x[i], y, 0, zz, i) & M;
			zc          +=  zz[i + len]                          & M;
			zz[i + len] =   (uint) zc;
			zc          >>= 32;
		}

		return (uint) zc;
	}

	public static uint MulAddTo(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
	{
		ulong zc = 0;
		for (var i = 0; i < len; ++i)
		{
			zc              +=  MulWordAddTo(len, x[xOff + i], y, yOff, zz, zzOff) & M;
			zc              +=  zz[zzOff + len]                                    & M;
			zz[zzOff + len] =   (uint) zc;
			zc              >>= 32;
			++zzOff;
		}

		return (uint) zc;
	}

	public static uint MulAddTo(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> zz)
	{
		ulong zc = 0;
		for (var i = 0; i < len; ++i)
		{
			zc          +=  MulWordAddTo(len, x[i], y, zz[i..]) & M;
			zc          +=  zz[i + len]                         & M;
			zz[i + len] =   (uint) zc;
			zc          >>= 32;
		}

		return (uint) zc;
	}

	public static uint Mul31BothAdd(int len, uint a, uint[] x, uint b, uint[] y, uint[] z, int zOff)
	{
		ulong c = 0, aVal = a, bVal = b;
		var   i = 0;
		do
		{
			c           +=  aVal * x[i] + bVal * y[i] + z[zOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		} while (++i < len);

		return (uint) c;
	}

	public static uint Mul31BothAdd(int len, uint a, ReadOnlySpan<uint> x, uint b, ReadOnlySpan<uint> y, Span<uint> z)
	{
		ulong c = 0, aVal = a, bVal = b;
		var   i = 0;
		do
		{
			c    +=  aVal * x[i] + bVal * y[i] + z[i];
			z[i] =   (uint) c;
			c    >>= 32;
		} while (++i < len);

		return (uint) c;
	}

	public static uint MulWord(int len, uint x, uint[] y, uint[] z)
	{
		ulong c = 0, xVal = x;
		var   i = 0;
		do
		{
			c    +=  xVal * y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		} while (++i < len);

		return (uint) c;
	}

	public static uint MulWord(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
	{
		ulong c = 0, xVal = x;
		var   i = 0;
		do
		{
			c           +=  xVal * y[yOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		} while (++i < len);

		return (uint) c;
	}

	public static uint MulWord(int len, uint x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		ulong c = 0, xVal = x;
		var   i = 0;
		do
		{
			c    +=  xVal * y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		} while (++i < len);

		return (uint) c;
	}

	public static uint MulWordAddTo(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
	{
		ulong c = 0, xVal = x;
		var   i = 0;
		do
		{
			c           +=  xVal * y[yOff + i] + z[zOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		} while (++i < len);

		return (uint) c;
	}

	public static uint MulWordAddTo(int len, uint x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		ulong c = 0, xVal = x;
		var   i = 0;
		do
		{
			c    +=  xVal * y[i] + z[i];
			z[i] =   (uint) c;
			c    >>= 32;
		} while (++i < len);

		return (uint) c;
	}

	public static uint MulWordDwordAddAt(int len, uint x, ulong y, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len - 3);
		ulong c = 0, xVal = x;
		c           +=  xVal * (uint) y + z[zPos + 0];
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * (y >> 32) + z[zPos + 1];
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		c           +=  z[zPos + 2];
		z[zPos                 + 2] =   (uint) c;
		c                           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 3);
	}

	public static uint MulWordDwordAddAt(int len, uint x, ulong y, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len - 3);
		ulong c = 0, xVal = x;
		c           +=  xVal * (uint) y + z[zPos + 0];
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * (y >> 32) + z[zPos + 1];
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		c           +=  z[zPos + 2];
		z[zPos                 + 2] =   (uint) c;
		c                           >>= 32;
		return c == 0 ? 0 : IncAt(len, z, zPos + 3);
	}

	public static int Negate(int len, uint[] x, uint[] z)
	{
		var c = 0L;
		for (var i = 0; i < len; ++i)
		{
			c    -=  x[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int Negate(int len, ReadOnlySpan<uint> x, Span<uint> z)
	{
		var c = 0L;
		for (var i = 0; i < len; ++i)
		{
			c    -=  x[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static uint ShiftDownBit(int len, uint[] z, uint c)
	{
		var i = len;
		while (--i >= 0)
		{
			var next = z[i];
			z[i] = (next >> 1) | (c << 31);
			c    = next;
		}

		return c << 31;
	}

	public static uint ShiftDownBit(int len, uint[] z, int zOff, uint c)
	{
		var i = len;
		while (--i >= 0)
		{
			var next = z[zOff + i];
			z[zOff + i] = (next >> 1) | (c << 31);
			c           = next;
		}

		return c << 31;
	}

	public static uint ShiftDownBit(int len, Span<uint> z, uint c)
	{
		var i = len;
		while (--i >= 0)
		{
			var next = z[i];
			z[i] = (next >> 1) | (c << 31);
			c    = next;
		}

		return c << 31;
	}

	public static uint ShiftDownBit(int len, uint[] x, uint c, uint[] z)
	{
		var i = len;
		while (--i >= 0)
		{
			var next = x[i];
			z[i] = (next >> 1) | (c << 31);
			c    = next;
		}

		return c << 31;
	}

	public static uint ShiftDownBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff)
	{
		var i = len;
		while (--i >= 0)
		{
			var next = x[xOff + i];
			z[zOff + i] = (next >> 1) | (c << 31);
			c           = next;
		}

		return c << 31;
	}

	public static uint ShiftDownBit(int len, ReadOnlySpan<uint> x, uint c, Span<uint> z)
	{
		var i = len;
		while (--i >= 0)
		{
			var next = x[i];
			z[i] = (next >> 1) | (c << 31);
			c    = next;
		}

		return c << 31;
	}

	public static uint ShiftDownBits(int len, uint[] z, int bits, uint c)
	{
		Debug.Assert(bits is > 0 and < 32);
		var i = len;
		while (--i >= 0)
		{
			var next = z[i];
			z[i] = (next >> bits) | (c << -bits);
			c    = next;
		}

		return c << -bits;
	}

	public static uint ShiftDownBits(int len, uint[] z, int zOff, int bits, uint c)
	{
		Debug.Assert(bits is > 0 and < 32);
		var i = len;
		while (--i >= 0)
		{
			var next = z[zOff + i];
			z[zOff + i] = (next >> bits) | (c << -bits);
			c           = next;
		}

		return c << -bits;
	}

	public static uint ShiftDownBits(int len, Span<uint> z, int bits, uint c)
	{
		Debug.Assert(bits is > 0 and < 32);
		var i = len;
		while (--i >= 0)
		{
			var next = z[i];
			z[i] = (next >> bits) | (c << -bits);
			c    = next;
		}

		return c << -bits;
	}

	public static uint ShiftDownBits(int len, uint[] x, int bits, uint c, uint[] z)
	{
		Debug.Assert(bits is < 32 and > 0);
		var i = len;
		while (--i >= 0)
		{
			var next = x[i];
			z[i] = (next >> bits) | (c << -bits);
			c    = next;
		}

		return c << -bits;
	}

	public static uint ShiftDownBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
	{
		Debug.Assert(bits is > 0 and < 32);
		var i = len;
		while (--i >= 0)
		{
			var next = x[xOff + i];
			z[zOff + i] = (next >> bits) | (c << -bits);
			c           = next;
		}

		return c << -bits;
	}

	public static uint ShiftDownBits(int len, ReadOnlySpan<uint> x, int bits, uint c, Span<uint> z)
	{
		Debug.Assert(bits is > 0 and < 32);
		var i = len;
		while (--i >= 0)
		{
			var next = x[i];
			z[i] = (next >> bits) | (c << -bits);
			c    = next;
		}

		return c << -bits;
	}

	public static ulong ShiftDownBits64(int len, ulong[] z, int zOff, int bits, ulong c)
	{
		Debug.Assert(bits is > 0 and < 64);
		var i = len;
		while (--i >= 0)
		{
			var next = z[zOff + i];
			z[zOff + i] = (next >> bits) | (c << -bits);
			c           = next;
		}

		return c << -bits;
	}

	public static uint ShiftDownWord(int len, uint[] z, uint c)
	{
		var i                      = len;
		while (--i >= 0) (z[i], c) = (c, z[i]);
		return c;
	}

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
	public static uint ShiftDownWord(int len, Span<uint> z, uint c)
	{
		var i                      = len;
		while (--i >= 0) (z[i], c) = (c, z[i]);
		return c;
	}
#endif

	public static uint ShiftUpBit(int len, uint[] z, uint c) => ShiftUpBit(len, z.AsSpan(0, len), c);

	public static uint ShiftUpBit(int len, uint[] z, int zOff, uint c) => ShiftUpBit(len, z.AsSpan(zOff, len), c);

	public static uint ShiftUpBit(int len, Span<uint> z, uint c)
	{
		int i = 0, limit4 = len - 4;
		while (i <= limit4)
		{
			var next0 = z[i + 0];
			var next1 = z[i + 1];
			var next2 = z[i + 2];
			var next3 = z[i + 3];
			z[i + 0] =  (next0 << 1) | (c     >> 31);
			z[i + 1] =  (next1 << 1) | (next0 >> 31);
			z[i + 2] =  (next2 << 1) | (next1 >> 31);
			z[i + 3] =  (next3 << 1) | (next2 >> 31);
			c        =  next3;
			i        += 4;
		}

		while (i < len)
		{
			var next = z[i];
			z[i] = (next << 1) | (c >> 31);
			c    = next;
			++i;
		}

		return c >> 31;
	}

	public static uint ShiftUpBit(int len, uint[] x, uint c, uint[] z) => ShiftUpBit(len, x.AsSpan(0, len), c, z.AsSpan(0, len));

	public static uint ShiftUpBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff) => ShiftUpBit(len, x.AsSpan(xOff, len), c, z.AsSpan(zOff, len));

	public static uint ShiftUpBit(int len, ReadOnlySpan<uint> x, uint c, Span<uint> z)
	{
		int i = 0, limit4 = len - 4;
		while (i <= limit4)
		{
			var next0 = x[i + 0];
			var next1 = x[i + 1];
			var next2 = x[i + 2];
			var next3 = x[i + 3];
			z[i + 0] =  (next0 << 1) | (c     >> 31);
			z[i + 1] =  (next1 << 1) | (next0 >> 31);
			z[i + 2] =  (next2 << 1) | (next1 >> 31);
			z[i + 3] =  (next3 << 1) | (next2 >> 31);
			c        =  next3;
			i        += 4;
		}

		while (i < len)
		{
			var next = x[i];
			z[i] = (next << 1) | (c >> 31);
			c    = next;
			++i;
		}

		return c >> 31;
	}

	public static ulong ShiftUpBit64(int len, ulong[] x, ulong c, ulong[] z) => ShiftUpBit64(len, x.AsSpan(0, len), c, z.AsSpan(0, len));

	public static ulong ShiftUpBit64(int len, ulong[] x, int xOff, ulong c, ulong[] z, int zOff) => ShiftUpBit64(len, x.AsSpan(xOff, len), c, z.AsSpan(zOff, len));

	public static ulong ShiftUpBit64(int len, ReadOnlySpan<ulong> x, ulong c, Span<ulong> z)
	{
		int i = 0, limit4 = len - 4;
		while (i <= limit4)
		{
			var next0 = x[i + 0];
			var next1 = x[i + 1];
			var next2 = x[i + 2];
			var next3 = x[i + 3];
			z[i + 0] =  (next0 << 1) | (c     >> 63);
			z[i + 1] =  (next1 << 1) | (next0 >> 63);
			z[i + 2] =  (next2 << 1) | (next1 >> 63);
			z[i + 3] =  (next3 << 1) | (next2 >> 63);
			c        =  next3;
			i        += 4;
		}

		while (i < len)
		{
			var next = x[i];
			z[i] = (next << 1) | (c >> 63);
			c    = next;
			++i;
		}

		return c >> 63;
	}

	public static uint ShiftUpBits(int len, uint[] z, int bits, uint c) => ShiftUpBits(len, z.AsSpan(0, len), bits, c);

	public static uint ShiftUpBits(int len, uint[] z, int zOff, int bits, uint c) => ShiftUpBits(len, z.AsSpan(zOff, len), bits, c);

	public static uint ShiftUpBits(int len, Span<uint> z, int bits, uint c)
	{
		Debug.Assert(bits is > 0 and < 32);
		int i = 0, limit4 = len - 4;
		while (i <= limit4)
		{
			var next0 = z[i + 0];
			var next1 = z[i + 1];
			var next2 = z[i + 2];
			var next3 = z[i + 3];
			z[i + 0] =  (next0 << bits) | (c     >> -bits);
			z[i + 1] =  (next1 << bits) | (next0 >> -bits);
			z[i + 2] =  (next2 << bits) | (next1 >> -bits);
			z[i + 3] =  (next3 << bits) | (next2 >> -bits);
			c        =  next3;
			i        += 4;
		}

		while (i < len)
		{
			var next = z[i];
			z[i] = (next << bits) | (c >> -bits);
			c    = next;
			++i;
		}

		return c >> -bits;
	}

	public static uint ShiftUpBits(int len, uint[] x, int bits, uint c, uint[] z) => ShiftUpBits(len, x.AsSpan(0, len), bits, c, z.AsSpan(0, len));

	public static uint ShiftUpBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff) => ShiftUpBits(len, x.AsSpan(xOff, len), bits, c, z.AsSpan(zOff, len));

	public static uint ShiftUpBits(int len, ReadOnlySpan<uint> x, int bits, uint c, Span<uint> z)
	{
		Debug.Assert(bits is > 0 and < 32);
		int i = 0, limit4 = len - 4;
		while (i <= limit4)
		{
			var next0 = x[i + 0];
			var next1 = x[i + 1];
			var next2 = x[i + 2];
			var next3 = x[i + 3];
			z[i + 0] =  (next0 << bits) | (c     >> -bits);
			z[i + 1] =  (next1 << bits) | (next0 >> -bits);
			z[i + 2] =  (next2 << bits) | (next1 >> -bits);
			z[i + 3] =  (next3 << bits) | (next2 >> -bits);
			c        =  next3;
			i        += 4;
		}

		while (i < len)
		{
			var next = x[i];
			z[i] = (next << bits) | (c >> -bits);
			c    = next;
			++i;
		}

		return c >> -bits;
	}

	public static ulong ShiftUpBits64(int len, ulong[] z, int bits, ulong c) => ShiftUpBits64(len, z.AsSpan(0, len), bits, c);

	public static ulong ShiftUpBits64(int len, ulong[] z, int zOff, int bits, ulong c) => ShiftUpBits64(len, z.AsSpan(zOff, len), bits, c);

	public static ulong ShiftUpBits64(int len, Span<ulong> z, int bits, ulong c)
	{
		Debug.Assert(bits is > 0 and < 64);
		int i = 0, limit4 = len - 4;
		while (i <= limit4)
		{
			var next0 = z[i + 0];
			var next1 = z[i + 1];
			var next2 = z[i + 2];
			var next3 = z[i + 3];
			z[i + 0] =  (next0 << bits) | (c     >> -bits);
			z[i + 1] =  (next1 << bits) | (next0 >> -bits);
			z[i + 2] =  (next2 << bits) | (next1 >> -bits);
			z[i + 3] =  (next3 << bits) | (next2 >> -bits);
			c        =  next3;
			i        += 4;
		}

		while (i < len)
		{
			var next = z[i];
			z[i] = (next << bits) | (c >> -bits);
			c    = next;
			++i;
		}

		return c >> -bits;
	}

	public static ulong ShiftUpBits64(int len, ulong[] x, int bits, ulong c, ulong[] z) => ShiftUpBits64(len, x.AsSpan(0, len), bits, c, z.AsSpan(0, len));

	public static ulong ShiftUpBits64(int len, ulong[] x, int xOff, int bits, ulong c, ulong[] z, int zOff) => ShiftUpBits64(len, x.AsSpan(xOff, len), bits, c, z.AsSpan(zOff, len));

	public static ulong ShiftUpBits64(int len, ReadOnlySpan<ulong> x, int bits, ulong c, Span<ulong> z)
	{
		Debug.Assert(bits is > 0 and < 64);
		int i = 0, limit4 = len - 4;
		while (i <= limit4)
		{
			var next0 = x[i + 0];
			var next1 = x[i + 1];
			var next2 = x[i + 2];
			var next3 = x[i + 3];
			z[i + 0] =  (next0 << bits) | (c     >> -bits);
			z[i + 1] =  (next1 << bits) | (next0 >> -bits);
			z[i + 2] =  (next2 << bits) | (next1 >> -bits);
			z[i + 3] =  (next3 << bits) | (next2 >> -bits);
			c        =  next3;
			i        += 4;
		}

		while (i < len)
		{
			var next = x[i];
			z[i] = (next << bits) | (c >> -bits);
			c    = next;
			++i;
		}

		return c >> -bits;
	}

	public static void Square(int len, uint[] x, uint[] zz)
	{
		var  extLen = len << 1;
		uint c      = 0;
		int  j      = len, k = extLen;
		do
		{
			ulong xVal = x[--j];
			var   p    = xVal * xVal;
			zz[--k] = (c        << 31) | (uint) (p >> 33);
			zz[--k] = (uint) (p >> 1);
			c       = (uint) p;
		} while (j > 0);

		var d     = 0UL;
		var zzPos = 2;

		for (var i = 1; i < len; ++i)
		{
			d           +=  SquareWordAddTo(x, i, zz);
			d           +=  zz[zzPos];
			zz[zzPos++] =   (uint) d;
			d           >>= 32;
			d           +=  zz[zzPos];
			zz[zzPos++] =   (uint) d;
			d           >>= 32;
		}

		Debug.Assert(0UL == d);

		ShiftUpBit(extLen, zz, x[0] << 31);
	}

	public static void Square(int len, uint[] x, int xOff, uint[] zz, int zzOff)
	{
		var  extLen = len << 1;
		uint c      = 0;
		int  j      = len, k = extLen;
		do
		{
			ulong xVal = x[xOff + --j];
			var   p    = xVal * xVal;
			zz[zzOff + --k] = (c        << 31) | (uint) (p >> 33);
			zz[zzOff + --k] = (uint) (p >> 1);
			c               = (uint) p;
		} while (j > 0);

		var d     = 0UL;
		var zzPos = zzOff + 2;

		for (var i = 1; i < len; ++i)
		{
			d           +=  SquareWordAddTo(x, xOff, i, zz, zzOff);
			d           +=  zz[zzPos];
			zz[zzPos++] =   (uint) d;
			d           >>= 32;
			d           +=  zz[zzPos];
			zz[zzPos++] =   (uint) d;
			d           >>= 32;
		}

		Debug.Assert(0UL == d);

		ShiftUpBit(extLen, zz, zzOff, x[xOff] << 31);
	}

	public static void Square(int len, ReadOnlySpan<uint> x, Span<uint> zz)
	{
		var  extLen = len << 1;
		uint c      = 0;
		int  j      = len, k = extLen;
		do
		{
			ulong xVal = x[--j];
			var   p    = xVal * xVal;
			zz[--k] = (c        << 31) | (uint) (p >> 33);
			zz[--k] = (uint) (p >> 1);
			c       = (uint) p;
		} while (j > 0);

		var d     = 0UL;
		var zzPos = 2;

		for (var i = 1; i < len; ++i)
		{
			d           +=  SquareWordAddTo(x, i, zz);
			d           +=  zz[zzPos];
			zz[zzPos++] =   (uint) d;
			d           >>= 32;
			d           +=  zz[zzPos];
			zz[zzPos++] =   (uint) d;
			d           >>= 32;
		}

		Debug.Assert(0UL == d);

		ShiftUpBit(extLen, zz, x[0] << 31);
	}

	public static uint SquareWordAddTo(uint[] x, int xPos, uint[] z)
	{
		ulong c = 0, xVal = x[xPos];
		var   i = 0;
		do
		{
			c           +=  xVal * x[i] + z[xPos + i];
			z[xPos + i] =   (uint) c;
			c           >>= 32;
		} while (++i < xPos);

		return (uint) c;
	}

	public static uint SquareWordAddTo(uint[] x, int xOff, int xPos, uint[] z, int zOff)
	{
		ulong c = 0, xVal = x[xOff + xPos];
		var   i = 0;
		do
		{
			c              +=  xVal * (x[xOff + i] & M) + (z[xPos + zOff] & M);
			z[xPos + zOff] =   (uint) c;
			c              >>= 32;
			++zOff;
		} while (++i < xPos);

		return (uint) c;
	}

	public static uint SquareWordAddTo(ReadOnlySpan<uint> x, int xPos, Span<uint> z)
	{
		ulong c = 0, xVal = x[xPos];
		var   i = 0;
		do
		{
			c           +=  xVal * x[i] + z[xPos + i];
			z[xPos + i] =   (uint) c;
			c           >>= 32;
		} while (++i < xPos);

		return (uint) c;
	}

	public static int Sub(int len, uint[] x, uint[] y, uint[] z)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (long) x[i] - y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int Sub(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c           +=  (long) x[xOff + i] - y[yOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (int) c;
	}

	public static int Sub(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (long) x[i] - y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int Sub33At(int len, uint x, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = (long) z[zPos + 0] - x;
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zPos + 1] - 1;
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zPos + 2);
	}

	public static int Sub33At(int len, uint x, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = (long) z[zOff + zPos] - x;
		z[zOff + zPos]     =   (uint) c;
		c                  >>= 32;
		c                  +=  (long) z[zOff + zPos + 1] - 1;
		z[zOff + zPos + 1] =   (uint) c;
		c                  >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zOff, zPos + 2);
	}

	public static int Sub33At(int len, uint x, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = (long) z[zPos + 0] - x;
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zPos + 1] - 1;
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zPos + 2);
	}

	public static int Sub33From(int len, uint x, uint[] z)
	{
		var c = (long) z[0] - x;
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[1] - 1;
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : DecAt(len, z, 2);
	}

	public static int Sub33From(int len, uint x, uint[] z, int zOff)
	{
		var c = (long) z[zOff + 0] - x;
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 1] - 1;
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zOff, 2);
	}

	public static int Sub33From(int len, uint x, Span<uint> z)
	{
		var c = (long) z[0] - x;
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[1] - 1;
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : DecAt(len, z, 2);
	}

	public static int SubBothFrom(int len, uint[] x, uint[] y, uint[] z)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (long) z[i] - x[i] - y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int SubBothFrom(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c           +=  (long) z[zOff + i] - x[xOff + i] - y[yOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (int) c;
	}

	public static int SubBothFrom(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (long) z[i] - x[i] - y[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int SubDWordAt(int len, ulong x, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = z[zPos + 0] - (long) (x & M);
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  z[zPos + 1] - (long) (x >> 32);
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zPos + 2);
	}

	public static int SubDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = z[zOff + zPos] - (long) (x & M);
		z[zOff + zPos]     =   (uint) c;
		c                  >>= 32;
		c                  +=  z[zOff + zPos + 1] - (long) (x >> 32);
		z[zOff + zPos + 1] =   (uint) c;
		c                  >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zOff, zPos + 2);
	}

	public static int SubDWordAt(int len, ulong x, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len - 2);
		var c = z[zPos + 0] - (long) (x & M);
		z[zPos + 0] =   (uint) c;
		c           >>= 32;
		c           +=  z[zPos + 1] - (long) (x >> 32);
		z[zPos + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zPos + 2);
	}

	public static int SubDWordFrom(int len, ulong x, uint[] z)
	{
		var c = z[0] - (long) (x & M);
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  z[1] - (long) (x >> 32);
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : DecAt(len, z, 2);
	}

	public static int SubDWordFrom(int len, ulong x, uint[] z, int zOff)
	{
		var c = z[zOff + 0] - (long) (x & M);
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  z[zOff + 1] - (long) (x >> 32);
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zOff, 2);
	}

	public static int SubDWordFrom(int len, ulong x, Span<uint> z)
	{
		var c = z[0] - (long) (x & M);
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  z[1] - (long) (x >> 32);
		z[1] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : DecAt(len, z, 2);
	}

	public static int SubFrom(int len, uint[] x, uint[] z)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (long) z[i] - x[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int SubFrom(int len, uint[] x, int xOff, uint[] z, int zOff)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c           +=  (long) z[zOff + i] - x[xOff + i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		}

		return (int) c;
	}

	public static int SubFrom(int len, ReadOnlySpan<uint> x, Span<uint> z)
	{
		long c = 0;
		for (var i = 0; i < len; ++i)
		{
			c    +=  (long) z[i] - x[i];
			z[i] =   (uint) c;
			c    >>= 32;
		}

		return (int) c;
	}

	public static int SubInt32From(int len, int x, Span<uint> z)
	{
		var c = z[0] - x;
		z[0] =   (uint) c;
		c    >>= 32;

		var i = 1;
		while (c != 0L && i < len)
		{
			c      +=  z[i];
			z[i++] =   (uint) c;
			c      >>= 32;
		}

		return (int) c;
	}

	public static int SubWordAt(int len, uint x, uint[] z, int zPos)
	{
		Debug.Assert(zPos <= len - 1);
		var c = (long) z[zPos] - x;
		z[zPos] =   (uint) c;
		c       >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zPos + 1);
	}

	public static int SubWordAt(int len, uint x, uint[] z, int zOff, int zPos)
	{
		Debug.Assert(zPos <= len - 1);
		var c = (long) z[zOff + zPos] - x;
		z[zOff + zPos] =   (uint) c;
		c              >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zOff, zPos + 1);
	}

	public static int SubWordAt(int len, uint x, Span<uint> z, int zPos)
	{
		Debug.Assert(zPos <= len - 1);
		var c = (long) z[zPos] - x;
		z[zPos] =   (uint) c;
		c       >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zPos + 1);
	}

	public static int SubWordFrom(int len, uint x, uint[] z)
	{
		var c = (long) z[0] - x;
		z[0] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : DecAt(len, z, 1);
	}

	public static int SubWordFrom(int len, uint x, uint[] z, int zOff)
	{
		var c = (long) z[zOff + 0] - x;
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		return c == 0 ? 0 : DecAt(len, z, zOff, 1);
	}

	public static int SubWordFrom(int len, uint x, Span<uint> z)
	{
		var c = (long) z[0] - x;
		z[0] =   (uint) c;
		c    >>= 32;
		return c == 0 ? 0 : DecAt(len, z, 1);
	}

	public static BigInteger ToBigInteger(int len, uint[] x) => ToBigInteger(len, x.AsSpan());

	public static BigInteger ToBigInteger(int len, ReadOnlySpan<uint> x)
	{
		var bsLen = len << 2;
		var bs = bsLen <= 512
			         ? stackalloc byte[bsLen]
			         : new byte[bsLen];

		Pack.UInt32_To_LittleEndian(x, bs);

		return new BigInteger(1, bs, false);
	}

	public static void Xor(int len, uint[] x, uint[] y, uint[] z) => Xor(len, x.AsSpan(0, len), y.AsSpan(0, len), z.AsSpan(0, len));

	public static void Xor(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		Xor(len, x.AsSpan(xOff, len), y.AsSpan(yOff, len), z.AsSpan(zOff, len));
	}

	public static void Xor(int len, ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		int i = 0, limit16 = len - 16;
		while (i <= limit16)
		{
			Nat512.Xor(x[i..], y[i..], z[i..]);
			i += 16;
		}

		while (i < len)
		{
			z[i] = x[i] ^ y[i];
			++i;
		}
	}

	public static void Xor64(int len, ulong[] x, ulong y, ulong[] z) => Xor64(len, x.AsSpan(0, len), y, z.AsSpan(0, len));

	public static void Xor64(int len, ulong[] x, int xOff, ulong y, ulong[] z, int zOff) => Xor64(len, x.AsSpan(xOff, len), y, z.AsSpan(zOff, len));

	public static void Xor64(int len, ReadOnlySpan<ulong> x, ulong y, Span<ulong> z)
	{
		var i = 0;
		if (Vector.IsHardwareAccelerated)
		{
			var vy = new Vector<ulong>(y);

			var limit = len - Vector<ulong>.Count;
			while (i <= limit)
			{
				var vx = new Vector<ulong>(x[i..]);
				(vx ^ vy).CopyTo(z[i..]);
				i += Vector<ulong>.Count;
			}
		}
		else
		{
			var limit = len - 4;
			while (i <= limit)
			{
				z[i + 0] =  x[i + 0] ^ y;
				z[i + 1] =  x[i + 1] ^ y;
				z[i + 2] =  x[i + 2] ^ y;
				z[i + 3] =  x[i + 3] ^ y;
				i        += 4;
			}
		}

		while (i < len)
		{
			z[i] = x[i] ^ y;
			++i;
		}
	}

	public static void Xor64(int len, ulong[] x, ulong[] y, ulong[] z) => Xor64(len, x.AsSpan(0, len), y.AsSpan(0, len), z.AsSpan(0, len));

	public static void Xor64(int len, ulong[] x, int xOff, ulong[] y, int yOff, ulong[] z, int zOff)
	{
		Xor64(len, x.AsSpan(xOff, len), y.AsSpan(yOff, len), z.AsSpan(zOff, len));
	}

	public static void Xor64(int len, ReadOnlySpan<ulong> x, ReadOnlySpan<ulong> y, Span<ulong> z)
	{
		int i = 0, limit8 = len - 8;
		while (i <= limit8)
		{
			Nat512.Xor64(x[i..], y[i..], z[i..]);
			i += 8;
		}

		while (i < len)
		{
			z[i] = x[i] ^ y[i];
			++i;
		}
	}

	public static void XorTo(int len, uint[] x, uint[] z) => XorTo(len, x.AsSpan(0, len), z.AsSpan(0, len));

	public static void XorTo(int len, uint[] x, int xOff, uint[] z, int zOff) => XorTo(len, x.AsSpan(xOff, len), z.AsSpan(zOff, len));

	public static void XorTo(int len, ReadOnlySpan<uint> x, Span<uint> z)
	{
		int i = 0, limit16 = len - 16;
		while (i <= limit16)
		{
			Nat512.XorTo(x[i..], z[i..]);
			i += 16;
		}

		while (i < len)
		{
			z[i] ^= x[i];
			++i;
		}
	}

	public static void XorTo64(int len, ulong[] x, ulong[] z) => XorTo64(len, x.AsSpan(0, len), z.AsSpan(0, len));

	public static void XorTo64(int len, ulong[] x, int xOff, ulong[] z, int zOff) => XorTo64(len, x.AsSpan(xOff, len), z.AsSpan(zOff, len));

	public static void XorTo64(int len, ReadOnlySpan<ulong> x, Span<ulong> z)
	{
		int i = 0, limit8 = len - 8;
		while (i <= limit8)
		{
			Nat512.XorTo64(x[i..], z[i..]);
			i += 8;
		}

		while (i < len)
		{
			z[i] ^= x[i];
			++i;
		}
	}

	public static void Zero(int len, uint[] z) => z.AsSpan(0, len).Clear();

	public static void Zero(int len, Span<uint> z) => z[..len].Clear();

	public static void Zero64(int len, ulong[] z) => z.AsSpan(0, len).Clear();

	public static void Zero64(int len, Span<ulong> z) => z[..len].Clear();
}
