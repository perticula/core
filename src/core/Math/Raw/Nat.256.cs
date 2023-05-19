// perticula - core - Nat.256.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using core.Cryptography;

namespace core.Math.Raw;

public static class Nat256
{
	private const ulong M = 0xFFFFFFFFUL;

	public static uint Add(uint[] x, uint[] y, uint[] z)
	{
		ulong c = 0;
		c    +=  (ulong) x[0] + y[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[1] + y[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[2] + y[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[3] + y[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[4] + y[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[5] + y[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[6] + y[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[7] + y[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (uint) c;
	}

	public static uint Add(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		ulong c = 0;
		c           +=  (ulong) x[xOff + 0] + y[yOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 1] + y[yOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 2] + y[yOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 3] + y[yOff + 3];
		z[zOff + 3] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 4] + y[yOff + 4];
		z[zOff + 4] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 5] + y[yOff + 5];
		z[zOff + 5] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 6] + y[yOff + 6];
		z[zOff + 6] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 7] + y[yOff + 7];
		z[zOff + 7] =   (uint) c;
		c           >>= 32;
		return (uint) c;
	}

	public static uint AddBothTo(uint[] x, uint[] y, uint[] z)
	{
		ulong c = 0;
		c    +=  (ulong) x[0] + y[0] + z[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[1] + y[1] + z[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[2] + y[2] + z[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[3] + y[3] + z[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[4] + y[4] + z[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[5] + y[5] + z[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[6] + y[6] + z[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[7] + y[7] + z[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (uint) c;
	}

	public static uint AddBothTo(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		ulong c = 0;
		c           +=  (ulong) x[xOff + 0] + y[yOff + 0] + z[zOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 1] + y[yOff + 1] + z[zOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 2] + y[yOff + 2] + z[zOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 3] + y[yOff + 3] + z[zOff + 3];
		z[zOff + 3] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 4] + y[yOff + 4] + z[zOff + 4];
		z[zOff + 4] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 5] + y[yOff + 5] + z[zOff + 5];
		z[zOff + 5] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 6] + y[yOff + 6] + z[zOff + 6];
		z[zOff + 6] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 7] + y[yOff + 7] + z[zOff + 7];
		z[zOff + 7] =   (uint) c;
		c           >>= 32;
		return (uint) c;
	}

	public static uint AddTo(uint[] x, uint[] z, uint cIn)
	{
		ulong c = cIn;
		c    +=  (ulong) x[0] + z[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[1] + z[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[2] + z[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[3] + z[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[4] + z[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[5] + z[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[6] + z[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[7] + z[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (uint) c;
	}

	public static uint AddTo(uint[] x, int xOff, uint[] z, int zOff, uint cIn)
	{
		ulong c = cIn;
		c           +=  (ulong) x[xOff + 0] + z[zOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 1] + z[zOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 2] + z[zOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 3] + z[zOff + 3];
		z[zOff + 3] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 4] + z[zOff + 4];
		z[zOff + 4] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 5] + z[zOff + 5];
		z[zOff + 5] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 6] + z[zOff + 6];
		z[zOff + 6] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) x[xOff + 7] + z[zOff + 7];
		z[zOff + 7] =   (uint) c;
		c           >>= 32;
		return (uint) c;
	}

	public static uint AddTo(ReadOnlySpan<uint> x, Span<uint> z, uint cIn)
	{
		ulong c = cIn;
		c    +=  (ulong) x[0] + z[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[1] + z[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[2] + z[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[3] + z[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[4] + z[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[5] + z[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[6] + z[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (ulong) x[7] + z[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (uint) c;
	}

	public static uint AddToEachOther(uint[] u, int uOff, uint[] v, int vOff)
	{
		ulong c = 0;
		c           +=  (ulong) u[uOff + 0] + v[vOff + 0];
		u[uOff + 0] =   (uint) c;
		v[vOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) u[uOff + 1] + v[vOff + 1];
		u[uOff + 1] =   (uint) c;
		v[vOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) u[uOff + 2] + v[vOff + 2];
		u[uOff + 2] =   (uint) c;
		v[vOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) u[uOff + 3] + v[vOff + 3];
		u[uOff + 3] =   (uint) c;
		v[vOff + 3] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) u[uOff + 4] + v[vOff + 4];
		u[uOff + 4] =   (uint) c;
		v[vOff + 4] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) u[uOff + 5] + v[vOff + 5];
		u[uOff + 5] =   (uint) c;
		v[vOff + 5] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) u[uOff + 6] + v[vOff + 6];
		u[uOff + 6] =   (uint) c;
		v[vOff + 6] =   (uint) c;
		c           >>= 32;
		c           +=  (ulong) u[uOff + 7] + v[vOff + 7];
		u[uOff + 7] =   (uint) c;
		v[vOff + 7] =   (uint) c;
		c           >>= 32;
		return (uint) c;
	}

	public static void Copy(uint[] x, uint[] z)
	{
		z[0] = x[0];
		z[1] = x[1];
		z[2] = x[2];
		z[3] = x[3];
		z[4] = x[4];
		z[5] = x[5];
		z[6] = x[6];
		z[7] = x[7];
	}

	public static void Copy(uint[] x, int xOff, uint[] z, int zOff)
	{
		z[zOff + 0] = x[xOff + 0];
		z[zOff + 1] = x[xOff + 1];
		z[zOff + 2] = x[xOff + 2];
		z[zOff + 3] = x[xOff + 3];
		z[zOff + 4] = x[xOff + 4];
		z[zOff + 5] = x[xOff + 5];
		z[zOff + 6] = x[xOff + 6];
		z[zOff + 7] = x[xOff + 7];
	}

	public static void Copy64(ulong[] x, ulong[] z)
	{
		z[0] = x[0];
		z[1] = x[1];
		z[2] = x[2];
		z[3] = x[3];
	}

	public static void Copy64(ulong[] x, int xOff, ulong[] z, int zOff)
	{
		z[zOff + 0] = x[xOff + 0];
		z[zOff + 1] = x[xOff + 1];
		z[zOff + 2] = x[xOff + 2];
		z[zOff + 3] = x[xOff + 3];
	}

	public static void Copy64(ReadOnlySpan<ulong> x, Span<ulong> z)
	{
		z[0] = x[0];
		z[1] = x[1];
		z[2] = x[2];
		z[3] = x[3];
	}

	public static uint[] Create() => new uint[8];

	public static ulong[] Create64() => new ulong[4];

	public static uint[] CreateExt() => new uint[16];

	public static ulong[] CreateExt64() => new ulong[8];

	public static bool Diff(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		var pos = Gte(x, xOff, y, yOff);
		if (pos)
			Sub(x, xOff, y, yOff, z, zOff);
		else
			Sub(y, yOff, x, xOff, z, zOff);
		return pos;
	}

	public static bool Eq(uint[] x, uint[] y)
	{
		for (var i = 7; i >= 0; --i)
		{
			if (x[i] != y[i])
				return false;
		}

		return true;
	}

	public static bool Eq64(ulong[] x, ulong[] y)
	{
		for (var i = 3; i >= 0; --i)
		{
			if (x[i] != y[i]) return false;
		}

		return true;
	}

	public static uint GetBit(uint[] x, int bit)
	{
		if (bit         == 0) return x[0] & 1;
		if ((bit & 255) != bit) return 0;
		var w = bit >> 5;
		var b = bit & 31;
		return (x[w] >> b) & 1;
	}

	public static bool Gte(uint[] x, uint[] y)
	{
		for (var i = 7; i >= 0; --i)
		{
			uint x_i = x[i], y_i = y[i];
			if (x_i < y_i)
				return false;
			if (x_i > y_i)
				return true;
		}

		return true;
	}

	public static bool Gte(uint[] x, int xOff, uint[] y, int yOff)
	{
		for (var i = 7; i >= 0; --i)
		{
			uint x_i = x[xOff + i], y_i = y[yOff + i];
			if (x_i < y_i)
				return false;
			if (x_i > y_i)
				return true;
		}

		return true;
	}

	public static bool Gte(ReadOnlySpan<uint> x, ReadOnlySpan<uint> y)
	{
		for (var i = 7; i >= 0; --i)
		{
			uint x_i = x[i], y_i = y[i];
			if (x_i < y_i)
				return false;
			if (x_i > y_i)
				return true;
		}

		return true;
	}

	public static bool IsOne(uint[] x)
	{
		if (x[0] != 1) return false;
		for (var i = 1; i < 8; ++i)
		{
			if (x[i] != 0) return false;
		}

		return true;
	}

	public static bool IsOne64(ulong[] x)
	{
		if (x[0] != 1UL) return false;
		for (var i = 1; i < 4; ++i)
		{
			if (x[i] != 0UL) return false;
		}

		return true;
	}

	public static bool IsZero(ReadOnlySpan<uint> x)
	{
		for (var i = 0; i < 8; ++i)
		{
			if (x[i] != 0)
				return false;
		}

		return true;
	}

	public static bool IsZero64(ReadOnlySpan<ulong> x)
	{
		for (var i = 0; i < 4; ++i)
		{
			if (x[i] != 0UL)
				return false;
		}

		return true;
	}

	public static void Mul(uint[] x, uint[] y, uint[] zz)
	{
		ulong y_0 = y[0];
		ulong y_1 = y[1];
		ulong y_2 = y[2];
		ulong y_3 = y[3];
		ulong y_4 = y[4];
		ulong y_5 = y[5];
		ulong y_6 = y[6];
		ulong y_7 = y[7];

		{
			ulong c = 0, x_0 = x[0];
			c     +=  x_0 * y_0;
			zz[0] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_1;
			zz[1] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_2;
			zz[2] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_3;
			zz[3] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_4;
			zz[4] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_5;
			zz[5] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_6;
			zz[6] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_7;
			zz[7] =   (uint) c;
			c     >>= 32;
			zz[8] =   (uint) c;
		}

		for (var i = 1; i < 8; ++i)
		{
			ulong c = 0, x_i = x[i];
			c         +=  x_i * y_0 + zz[i + 0];
			zz[i + 0] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_1 + zz[i + 1];
			zz[i + 1] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_2 + zz[i + 2];
			zz[i + 2] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_3 + zz[i + 3];
			zz[i + 3] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_4 + zz[i + 4];
			zz[i + 4] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_5 + zz[i + 5];
			zz[i + 5] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_6 + zz[i + 6];
			zz[i + 6] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_7 + zz[i + 7];
			zz[i + 7] =   (uint) c;
			c         >>= 32;
			zz[i + 8] =   (uint) c;
		}
	}

	public static void Mul(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
	{
		ulong y_0 = y[yOff + 0];
		ulong y_1 = y[yOff + 1];
		ulong y_2 = y[yOff + 2];
		ulong y_3 = y[yOff + 3];
		ulong y_4 = y[yOff + 4];
		ulong y_5 = y[yOff + 5];
		ulong y_6 = y[yOff + 6];
		ulong y_7 = y[yOff + 7];

		{
			ulong c = 0, x_0 = x[xOff + 0];
			c             +=  x_0 * y_0;
			zz[zzOff + 0] =   (uint) c;
			c             >>= 32;
			c             +=  x_0 * y_1;
			zz[zzOff + 1] =   (uint) c;
			c             >>= 32;
			c             +=  x_0 * y_2;
			zz[zzOff + 2] =   (uint) c;
			c             >>= 32;
			c             +=  x_0 * y_3;
			zz[zzOff + 3] =   (uint) c;
			c             >>= 32;
			c             +=  x_0 * y_4;
			zz[zzOff + 4] =   (uint) c;
			c             >>= 32;
			c             +=  x_0 * y_5;
			zz[zzOff + 5] =   (uint) c;
			c             >>= 32;
			c             +=  x_0 * y_6;
			zz[zzOff + 6] =   (uint) c;
			c             >>= 32;
			c             +=  x_0 * y_7;
			zz[zzOff + 7] =   (uint) c;
			c             >>= 32;
			zz[zzOff + 8] =   (uint) c;
		}

		for (var i = 1; i < 8; ++i)
		{
			++zzOff;
			ulong c = 0, x_i = x[xOff + i];
			c             +=  x_i * y_0 + zz[zzOff + 0];
			zz[zzOff + 0] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_1 + zz[zzOff + 1];
			zz[zzOff + 1] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_2 + zz[zzOff + 2];
			zz[zzOff + 2] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_3 + zz[zzOff + 3];
			zz[zzOff + 3] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_4 + zz[zzOff + 4];
			zz[zzOff + 4] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_5 + zz[zzOff + 5];
			zz[zzOff + 5] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_6 + zz[zzOff + 6];
			zz[zzOff + 6] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_7 + zz[zzOff + 7];
			zz[zzOff + 7] =   (uint) c;
			c             >>= 32;
			zz[zzOff + 8] =   (uint) c;
		}
	}

	public static void Mul(ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> zz)
	{
		ulong y_0 = y[0];
		ulong y_1 = y[1];
		ulong y_2 = y[2];
		ulong y_3 = y[3];
		ulong y_4 = y[4];
		ulong y_5 = y[5];
		ulong y_6 = y[6];
		ulong y_7 = y[7];

		{
			ulong c = 0, x_0 = x[0];
			c     +=  x_0 * y_0;
			zz[0] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_1;
			zz[1] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_2;
			zz[2] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_3;
			zz[3] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_4;
			zz[4] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_5;
			zz[5] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_6;
			zz[6] =   (uint) c;
			c     >>= 32;
			c     +=  x_0 * y_7;
			zz[7] =   (uint) c;
			c     >>= 32;
			zz[8] =   (uint) c;
		}

		for (var i = 1; i < 8; ++i)
		{
			ulong c = 0, x_i = x[i];
			c         +=  x_i * y_0 + zz[i + 0];
			zz[i + 0] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_1 + zz[i + 1];
			zz[i + 1] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_2 + zz[i + 2];
			zz[i + 2] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_3 + zz[i + 3];
			zz[i + 3] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_4 + zz[i + 4];
			zz[i + 4] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_5 + zz[i + 5];
			zz[i + 5] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_6 + zz[i + 6];
			zz[i + 6] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_7 + zz[i + 7];
			zz[i + 7] =   (uint) c;
			c         >>= 32;
			zz[i + 8] =   (uint) c;
		}
	}

	public static void Mul128(uint[] x, uint[] y128, uint[] zz)
	{
		ulong x_0 = x[0];
		ulong x_1 = x[1];
		ulong x_2 = x[2];
		ulong x_3 = x[3];
		ulong x_4 = x[4];
		ulong x_5 = x[5];
		ulong x_6 = x[6];
		ulong x_7 = x[7];

		{
			ulong c = 0, y_0 = y128[0];
			c     +=  y_0 * x_0;
			zz[0] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_1;
			zz[1] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_2;
			zz[2] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_3;
			zz[3] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_4;
			zz[4] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_5;
			zz[5] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_6;
			zz[6] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_7;
			zz[7] =   (uint) c;
			c     >>= 32;
			zz[8] =   (uint) c;
		}

		for (var i = 1; i < 4; ++i)
		{
			ulong c = 0, y_i = y128[i];
			c         +=  y_i * x_0 + zz[i + 0];
			zz[i + 0] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_1 + zz[i + 1];
			zz[i + 1] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_2 + zz[i + 2];
			zz[i + 2] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_3 + zz[i + 3];
			zz[i + 3] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_4 + zz[i + 4];
			zz[i + 4] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_5 + zz[i + 5];
			zz[i + 5] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_6 + zz[i + 6];
			zz[i + 6] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_7 + zz[i + 7];
			zz[i + 7] =   (uint) c;
			c         >>= 32;
			zz[i + 8] =   (uint) c;
		}
	}

	public static void Mul128(ReadOnlySpan<uint> x, ReadOnlySpan<uint> y128, Span<uint> zz)
	{
		ulong x_0 = x[0];
		ulong x_1 = x[1];
		ulong x_2 = x[2];
		ulong x_3 = x[3];
		ulong x_4 = x[4];
		ulong x_5 = x[5];
		ulong x_6 = x[6];
		ulong x_7 = x[7];

		{
			ulong c = 0, y_0 = y128[0];
			c     +=  y_0 * x_0;
			zz[0] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_1;
			zz[1] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_2;
			zz[2] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_3;
			zz[3] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_4;
			zz[4] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_5;
			zz[5] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_6;
			zz[6] =   (uint) c;
			c     >>= 32;
			c     +=  y_0 * x_7;
			zz[7] =   (uint) c;
			c     >>= 32;
			zz[8] =   (uint) c;
		}

		for (var i = 1; i < 4; ++i)
		{
			ulong c = 0, y_i = y128[i];
			c         +=  y_i * x_0 + zz[i + 0];
			zz[i + 0] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_1 + zz[i + 1];
			zz[i + 1] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_2 + zz[i + 2];
			zz[i + 2] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_3 + zz[i + 3];
			zz[i + 3] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_4 + zz[i + 4];
			zz[i + 4] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_5 + zz[i + 5];
			zz[i + 5] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_6 + zz[i + 6];
			zz[i + 6] =   (uint) c;
			c         >>= 32;
			c         +=  y_i * x_7 + zz[i + 7];
			zz[i + 7] =   (uint) c;
			c         >>= 32;
			zz[i + 8] =   (uint) c;
		}
	}

	public static uint MulAddTo(uint[] x, uint[] y, uint[] zz)
	{
		ulong y_0 = y[0];
		ulong y_1 = y[1];
		ulong y_2 = y[2];
		ulong y_3 = y[3];
		ulong y_4 = y[4];
		ulong y_5 = y[5];
		ulong y_6 = y[6];
		ulong y_7 = y[7];

		ulong zc = 0;
		for (var i = 0; i < 8; ++i)
		{
			ulong c = 0, x_i = x[i];
			c         +=  x_i * y_0 + zz[i + 0];
			zz[i + 0] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_1 + zz[i + 1];
			zz[i + 1] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_2 + zz[i + 2];
			zz[i + 2] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_3 + zz[i + 3];
			zz[i + 3] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_4 + zz[i + 4];
			zz[i + 4] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_5 + zz[i + 5];
			zz[i + 5] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_6 + zz[i + 6];
			zz[i + 6] =   (uint) c;
			c         >>= 32;
			c         +=  x_i * y_7 + zz[i + 7];
			zz[i + 7] =   (uint) c;
			c         >>= 32;

			zc        +=  c + zz[i + 8];
			zz[i + 8] =   (uint) zc;
			zc        >>= 32;
		}

		return (uint) zc;
	}

	public static uint MulAddTo(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
	{
		ulong y_0 = y[yOff + 0];
		ulong y_1 = y[yOff + 1];
		ulong y_2 = y[yOff + 2];
		ulong y_3 = y[yOff + 3];
		ulong y_4 = y[yOff + 4];
		ulong y_5 = y[yOff + 5];
		ulong y_6 = y[yOff + 6];
		ulong y_7 = y[yOff + 7];

		ulong zc = 0;
		for (var i = 0; i < 8; ++i)
		{
			ulong c = 0, x_i = x[xOff + i];
			c             +=  x_i * y_0 + zz[zzOff + 0];
			zz[zzOff + 0] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_1 + zz[zzOff + 1];
			zz[zzOff + 1] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_2 + zz[zzOff + 2];
			zz[zzOff + 2] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_3 + zz[zzOff + 3];
			zz[zzOff + 3] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_4 + zz[zzOff + 4];
			zz[zzOff + 4] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_5 + zz[zzOff + 5];
			zz[zzOff + 5] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_6 + zz[zzOff + 6];
			zz[zzOff + 6] =   (uint) c;
			c             >>= 32;
			c             +=  x_i * y_7 + zz[zzOff + 7];
			zz[zzOff + 7] =   (uint) c;
			c             >>= 32;

			zc            +=  c + zz[zzOff + 8];
			zz[zzOff + 8] =   (uint) zc;
			zc            >>= 32;
			++zzOff;
		}

		return (uint) zc;
	}

	public static ulong Mul33Add(uint w, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		Debug.Assert(w >> 31 == 0);

		ulong c  = 0, wVal = w;
		ulong x0 = x[xOff + 0];
		c           +=  wVal * x0 + y[yOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		ulong x1 = x[xOff + 1];
		c           +=  wVal * x1 + x0 + y[yOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		ulong x2 = x[xOff + 2];
		c           +=  wVal * x2 + x1 + y[yOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		ulong x3 = x[xOff + 3];
		c           +=  wVal * x3 + x2 + y[yOff + 3];
		z[zOff + 3] =   (uint) c;
		c           >>= 32;
		ulong x4 = x[xOff + 4];
		c           +=  wVal * x4 + x3 + y[yOff + 4];
		z[zOff + 4] =   (uint) c;
		c           >>= 32;
		ulong x5 = x[xOff + 5];
		c           +=  wVal * x5 + x4 + y[yOff + 5];
		z[zOff + 5] =   (uint) c;
		c           >>= 32;
		ulong x6 = x[xOff + 6];
		c           +=  wVal * x6 + x5 + y[yOff + 6];
		z[zOff + 6] =   (uint) c;
		c           >>= 32;
		ulong x7 = x[xOff + 7];
		c           +=  wVal * x7 + x6 + y[yOff + 7];
		z[zOff + 7] =   (uint) c;
		c           >>= 32;
		c           +=  x7;
		return c;
	}

	public static uint MulByWord(uint x, uint[] z)
	{
		ulong c = 0, xVal = x;
		c    +=  xVal * z[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (uint) c;
	}

	public static uint MulByWordAddTo(uint x, uint[] y, uint[] z)
	{
		ulong c = 0, xVal = x;
		c    +=  xVal * z[0] + y[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[1] + y[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[2] + y[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[3] + y[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[4] + y[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[5] + y[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[6] + y[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  xVal * z[7] + y[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (uint) c;
	}

	public static uint MulWordAddTo(uint x, uint[] y, int yOff, uint[] z, int zOff)
	{
		ulong c = 0, xVal = x;
		c           +=  xVal * y[yOff + 0] + z[zOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * y[yOff + 1] + z[zOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * y[yOff + 2] + z[zOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * y[yOff + 3] + z[zOff + 3];
		z[zOff + 3] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * y[yOff + 4] + z[zOff + 4];
		z[zOff + 4] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * y[yOff + 5] + z[zOff + 5];
		z[zOff + 5] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * y[yOff + 6] + z[zOff + 6];
		z[zOff + 6] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * y[yOff + 7] + z[zOff + 7];
		z[zOff + 7] =   (uint) c;
		c           >>= 32;
		return (uint) c;
	}

	public static uint Mul33DWordAdd(uint x, ulong y, uint[] z, int zOff)
	{
		Debug.Assert(x >> 31 == 0);
		Debug.Assert(zOff    <= 4);
		ulong c   = 0, xVal = x;
		var   y00 = y & M;
		c           +=  xVal * y00 + z[zOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		var y01 = y >> 32;
		c           +=  xVal * y01 + y00 + z[zOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  y01 + z[zOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  z[zOff + 3];
		z[zOff                 + 3] =   (uint) c;
		c                           >>= 32;
		return c == 0 ? 0 : Nat.IncAt(8, z, zOff, 4);
	}

	public static uint Mul33WordAdd(uint x, uint y, uint[] z, int zOff)
	{
		Debug.Assert(x >> 31 == 0);
		Debug.Assert(zOff    <= 5);
		ulong c = 0, yVal = y;
		c           +=  yVal * x + z[zOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  yVal + z[zOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  z[zOff + 2];
		z[zOff                 + 2] =   (uint) c;
		c                           >>= 32;
		return c == 0 ? 0 : Nat.IncAt(8, z, zOff, 3);
	}

	public static uint MulWordDwordAdd(uint x, ulong y, uint[] z, int zOff)
	{
		Debug.Assert(zOff <= 5);
		ulong c = 0, xVal = x;
		c           +=  xVal * y + z[zOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  xVal * (y >> 32) + z[zOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  z[zOff + 2];
		z[zOff                 + 2] =   (uint) c;
		c                           >>= 32;
		return c == 0 ? 0 : Nat.IncAt(8, z, zOff, 3);
	}

	public static uint MulWord(uint x, uint[] y, uint[] z, int zOff)
	{
		ulong c = 0, xVal = x;
		var   i = 0;
		do
		{
			c           +=  xVal * y[i];
			z[zOff + i] =   (uint) c;
			c           >>= 32;
		} while (++i < 8);

		return (uint) c;
	}

	public static void Square(uint[] x, uint[] zz)
	{
		ulong x_0 = x[0];
		ulong zz_1;

		uint c = 0, w;
		{
			int i = 7, j = 16;
			do
			{
				ulong xVal = x[i--];
				var   p    = xVal * xVal;
				zz[--j] = (c        << 31) | (uint) (p >> 33);
				zz[--j] = (uint) (p >> 1);
				c       = (uint) p;
			} while (i > 0);

			{
				var p = x_0 * x_0;
				zz_1  = c << 31 | (p >> 33);
				zz[0] = (uint) p;
				c     = (uint) (p >> 32) & 1;
			}
		}

		ulong x_1  = x[1];
		ulong zz_2 = zz[2];

		{
			zz_1  += x_1 * x_0;
			w     =  (uint) zz_1;
			zz[1] =  (w << 1) | c;
			c     =  w    >> 31;
			zz_2  += zz_1 >> 32;
		}

		ulong x_2  = x[2];
		ulong zz_3 = zz[3];
		ulong zz_4 = zz[4];
		{
			zz_2  += x_2 * x_0;
			w     =  (uint) zz_2;
			zz[2] =  (w << 1) | c;
			c     =  w >> 31;
			zz_3  += (zz_2 >> 32) + x_2 * x_1;
			zz_4  += zz_3 >> 32;
			zz_3  &= M;
		}

		ulong x_3  = x[3];
		var   zz_5 = zz[5] + (zz_4 >> 32);
		zz_4 &= M;
		var zz_6 = zz[6] + (zz_5 >> 32);
		zz_5 &= M;
		{
			zz_3  += x_3 * x_0;
			w     =  (uint) zz_3;
			zz[3] =  (w << 1) | c;
			c     =  w >> 31;
			zz_4  += (zz_3 >> 32) + x_3 * x_1;
			zz_5  += (zz_4 >> 32) + x_3 * x_2;
			zz_4  &= M;
			zz_6  += zz_5 >> 32;
			zz_5  &= M;
		}

		ulong x_4  = x[4];
		var   zz_7 = zz[7] + (zz_6 >> 32);
		zz_6 &= M;
		var zz_8 = zz[8] + (zz_7 >> 32);
		zz_7 &= M;
		{
			zz_4  += x_4 * x_0;
			w     =  (uint) zz_4;
			zz[4] =  (w << 1) | c;
			c     =  w >> 31;
			zz_5  += (zz_4 >> 32) + x_4 * x_1;
			zz_6  += (zz_5 >> 32) + x_4 * x_2;
			zz_5  &= M;
			zz_7  += (zz_6 >> 32) + x_4 * x_3;
			zz_6  &= M;
			zz_8  += zz_7 >> 32;
			zz_7  &= M;
		}

		ulong x_5  = x[5];
		var   zz_9 = zz[9] + (zz_8 >> 32);
		zz_8 &= M;
		var zz_10 = zz[10] + (zz_9 >> 32);
		zz_9 &= M;
		{
			zz_5  += x_5 * x_0;
			w     =  (uint) zz_5;
			zz[5] =  (w << 1) | c;
			c     =  w >> 31;
			zz_6  += (zz_5 >> 32) + x_5 * x_1;
			zz_7  += (zz_6 >> 32) + x_5 * x_2;
			zz_6  &= M;
			zz_8  += (zz_7 >> 32) + x_5 * x_3;
			zz_7  &= M;
			zz_9  += (zz_8 >> 32) + x_5 * x_4;
			zz_8  &= M;
			zz_10 += zz_9 >> 32;
			zz_9  &= M;
		}

		ulong x_6   = x[6];
		var   zz_11 = zz[11] + (zz_10 >> 32);
		zz_10 &= M;
		var zz_12 = zz[12] + (zz_11 >> 32);
		zz_11 &= M;
		{
			zz_6  += x_6 * x_0;
			w     =  (uint) zz_6;
			zz[6] =  (w << 1) | c;
			c     =  w >> 31;
			zz_7  += (zz_6 >> 32) + x_6 * x_1;
			zz_8  += (zz_7 >> 32) + x_6 * x_2;
			zz_7  &= M;
			zz_9  += (zz_8 >> 32) + x_6 * x_3;
			zz_8  &= M;
			zz_10 += (zz_9 >> 32) + x_6 * x_4;
			zz_9  &= M;
			zz_11 += (zz_10 >> 32) + x_6 * x_5;
			zz_10 &= M;
			zz_12 += zz_11 >> 32;
			zz_11 &= M;
		}

		ulong x_7   = x[7];
		var   zz_13 = zz[13] + (zz_12 >> 32);
		zz_12 &= M;
		var zz_14 = zz[14] + (zz_13 >> 32);
		zz_13 &= M;
		{
			zz_7  += x_7 * x_0;
			w     =  (uint) zz_7;
			zz[7] =  (w << 1) | c;
			c     =  w >> 31;
			zz_8  += (zz_7  >> 32) + x_7 * x_1;
			zz_9  += (zz_8  >> 32) + x_7 * x_2;
			zz_10 += (zz_9  >> 32) + x_7 * x_3;
			zz_11 += (zz_10 >> 32) + x_7 * x_4;
			zz_12 += (zz_11 >> 32) + x_7 * x_5;
			zz_13 += (zz_12 >> 32) + x_7 * x_6;
			zz_14 += zz_13 >> 32;
		}

		w      = (uint) zz_8;
		zz[8]  = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_9;
		zz[9]  = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_10;
		zz[10] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_11;
		zz[11] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_12;
		zz[12] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_13;
		zz[13] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_14;
		zz[14] = (w << 1) | c;
		c      = w >> 31;
		w      = zz[15] + (uint) (zz_14 >> 32);
		zz[15] = (w << 1) | c;
	}

	public static void Square(uint[] x, int xOff, uint[] zz, int zzOff)
	{
		ulong x_0 = x[xOff + 0];
		ulong zz_1;

		uint c = 0, w;
		{
			int i = 7, j = 16;
			do
			{
				ulong xVal = x[xOff + i--];
				var   p    = xVal * xVal;
				zz[zzOff + --j] = (c        << 31) | (uint) (p >> 33);
				zz[zzOff + --j] = (uint) (p >> 1);
				c               = (uint) p;
			} while (i > 0);

			{
				var p = x_0 * x_0;
				zz_1          = c << 31 | (p >> 33);
				zz[zzOff + 0] = (uint) p;
				c             = (uint) (p >> 32) & 1;
			}
		}

		ulong x_1  = x[xOff   + 1];
		ulong zz_2 = zz[zzOff + 2];

		{
			zz_1          += x_1 * x_0;
			w             =  (uint) zz_1;
			zz[zzOff + 1] =  (w << 1) | c;
			c             =  w    >> 31;
			zz_2          += zz_1 >> 32;
		}

		ulong x_2  = x[xOff   + 2];
		ulong zz_3 = zz[zzOff + 3];
		ulong zz_4 = zz[zzOff + 4];
		{
			zz_2          += x_2 * x_0;
			w             =  (uint) zz_2;
			zz[zzOff + 2] =  (w << 1) | c;
			c             =  w >> 31;
			zz_3          += (zz_2 >> 32) + x_2 * x_1;
			zz_4          += zz_3 >> 32;
			zz_3          &= M;
		}

		ulong x_3  = x[xOff + 3];
		var   zz_5 = zz[zzOff + 5] + (zz_4 >> 32);
		zz_4 &= M;
		var zz_6 = zz[zzOff + 6] + (zz_5 >> 32);
		zz_5 &= M;
		{
			zz_3          += x_3 * x_0;
			w             =  (uint) zz_3;
			zz[zzOff + 3] =  (w << 1) | c;
			c             =  w >> 31;
			zz_4          += (zz_3 >> 32) + x_3 * x_1;
			zz_5          += (zz_4 >> 32) + x_3 * x_2;
			zz_4          &= M;
			zz_6          += zz_5 >> 32;
			zz_5          &= M;
		}

		ulong x_4  = x[xOff + 4];
		var   zz_7 = zz[zzOff + 7] + (zz_6 >> 32);
		zz_6 &= M;
		var zz_8 = zz[zzOff + 8] + (zz_7 >> 32);
		zz_7 &= M;
		{
			zz_4          += x_4 * x_0;
			w             =  (uint) zz_4;
			zz[zzOff + 4] =  (w << 1) | c;
			c             =  w >> 31;
			zz_5          += (zz_4 >> 32) + x_4 * x_1;
			zz_6          += (zz_5 >> 32) + x_4 * x_2;
			zz_5          &= M;
			zz_7          += (zz_6 >> 32) + x_4 * x_3;
			zz_6          &= M;
			zz_8          += zz_7 >> 32;
			zz_7          &= M;
		}

		ulong x_5  = x[xOff + 5];
		var   zz_9 = zz[zzOff + 9] + (zz_8 >> 32);
		zz_8 &= M;
		var zz_10 = zz[zzOff + 10] + (zz_9 >> 32);
		zz_9 &= M;
		{
			zz_5          += x_5 * x_0;
			w             =  (uint) zz_5;
			zz[zzOff + 5] =  (w << 1) | c;
			c             =  w >> 31;
			zz_6          += (zz_5 >> 32) + x_5 * x_1;
			zz_7          += (zz_6 >> 32) + x_5 * x_2;
			zz_6          &= M;
			zz_8          += (zz_7 >> 32) + x_5 * x_3;
			zz_7          &= M;
			zz_9          += (zz_8 >> 32) + x_5 * x_4;
			zz_8          &= M;
			zz_10         += zz_9 >> 32;
			zz_9          &= M;
		}

		ulong x_6   = x[xOff + 6];
		var   zz_11 = zz[zzOff + 11] + (zz_10 >> 32);
		zz_10 &= M;
		var zz_12 = zz[zzOff + 12] + (zz_11 >> 32);
		zz_11 &= M;
		{
			zz_6          += x_6 * x_0;
			w             =  (uint) zz_6;
			zz[zzOff + 6] =  (w << 1) | c;
			c             =  w >> 31;
			zz_7          += (zz_6 >> 32) + x_6 * x_1;
			zz_8          += (zz_7 >> 32) + x_6 * x_2;
			zz_7          &= M;
			zz_9          += (zz_8 >> 32) + x_6 * x_3;
			zz_8          &= M;
			zz_10         += (zz_9 >> 32) + x_6 * x_4;
			zz_9          &= M;
			zz_11         += (zz_10 >> 32) + x_6 * x_5;
			zz_10         &= M;
			zz_12         += zz_11 >> 32;
			zz_11         &= M;
		}

		ulong x_7   = x[xOff + 7];
		var   zz_13 = zz[zzOff + 13] + (zz_12 >> 32);
		zz_12 &= M;
		var zz_14 = zz[zzOff + 14] + (zz_13 >> 32);
		zz_13 &= M;
		{
			zz_7          += x_7 * x_0;
			w             =  (uint) zz_7;
			zz[zzOff + 7] =  (w << 1) | c;
			c             =  w >> 31;
			zz_8          += (zz_7  >> 32) + x_7 * x_1;
			zz_9          += (zz_8  >> 32) + x_7 * x_2;
			zz_10         += (zz_9  >> 32) + x_7 * x_3;
			zz_11         += (zz_10 >> 32) + x_7 * x_4;
			zz_12         += (zz_11 >> 32) + x_7 * x_5;
			zz_13         += (zz_12 >> 32) + x_7 * x_6;
			zz_14         += zz_13 >> 32;
		}

		w              = (uint) zz_8;
		zz[zzOff + 8]  = (w << 1) | c;
		c              = w >> 31;
		w              = (uint) zz_9;
		zz[zzOff + 9]  = (w << 1) | c;
		c              = w >> 31;
		w              = (uint) zz_10;
		zz[zzOff + 10] = (w << 1) | c;
		c              = w >> 31;
		w              = (uint) zz_11;
		zz[zzOff + 11] = (w << 1) | c;
		c              = w >> 31;
		w              = (uint) zz_12;
		zz[zzOff + 12] = (w << 1) | c;
		c              = w >> 31;
		w              = (uint) zz_13;
		zz[zzOff + 13] = (w << 1) | c;
		c              = w >> 31;
		w              = (uint) zz_14;
		zz[zzOff + 14] = (w << 1) | c;
		c              = w >> 31;
		w              = zz[zzOff + 15] + (uint) (zz_14 >> 32);
		zz[zzOff + 15] = (w << 1) | c;
	}

	public static void Square(ReadOnlySpan<uint> x, Span<uint> zz)
	{
		ulong x_0 = x[0];
		ulong zz_1;

		uint c = 0, w;
		{
			int i = 7, j = 16;
			do
			{
				ulong xVal = x[i--];
				var   p    = xVal * xVal;
				zz[--j] = (c        << 31) | (uint) (p >> 33);
				zz[--j] = (uint) (p >> 1);
				c       = (uint) p;
			} while (i > 0);

			{
				var p = x_0 * x_0;
				zz_1  = c << 31 | (p >> 33);
				zz[0] = (uint) p;
				c     = (uint) (p >> 32) & 1;
			}
		}

		ulong x_1  = x[1];
		ulong zz_2 = zz[2];

		{
			zz_1  += x_1 * x_0;
			w     =  (uint) zz_1;
			zz[1] =  (w << 1) | c;
			c     =  w    >> 31;
			zz_2  += zz_1 >> 32;
		}

		ulong x_2  = x[2];
		ulong zz_3 = zz[3];
		ulong zz_4 = zz[4];
		{
			zz_2  += x_2 * x_0;
			w     =  (uint) zz_2;
			zz[2] =  (w << 1) | c;
			c     =  w >> 31;
			zz_3  += (zz_2 >> 32) + x_2 * x_1;
			zz_4  += zz_3 >> 32;
			zz_3  &= M;
		}

		ulong x_3  = x[3];
		var   zz_5 = zz[5] + (zz_4 >> 32);
		zz_4 &= M;
		var zz_6 = zz[6] + (zz_5 >> 32);
		zz_5 &= M;
		{
			zz_3  += x_3 * x_0;
			w     =  (uint) zz_3;
			zz[3] =  (w << 1) | c;
			c     =  w >> 31;
			zz_4  += (zz_3 >> 32) + x_3 * x_1;
			zz_5  += (zz_4 >> 32) + x_3 * x_2;
			zz_4  &= M;
			zz_6  += zz_5 >> 32;
			zz_5  &= M;
		}

		ulong x_4  = x[4];
		var   zz_7 = zz[7] + (zz_6 >> 32);
		zz_6 &= M;
		var zz_8 = zz[8] + (zz_7 >> 32);
		zz_7 &= M;
		{
			zz_4  += x_4 * x_0;
			w     =  (uint) zz_4;
			zz[4] =  (w << 1) | c;
			c     =  w >> 31;
			zz_5  += (zz_4 >> 32) + x_4 * x_1;
			zz_6  += (zz_5 >> 32) + x_4 * x_2;
			zz_5  &= M;
			zz_7  += (zz_6 >> 32) + x_4 * x_3;
			zz_6  &= M;
			zz_8  += zz_7 >> 32;
			zz_7  &= M;
		}

		ulong x_5  = x[5];
		var   zz_9 = zz[9] + (zz_8 >> 32);
		zz_8 &= M;
		var zz_10 = zz[10] + (zz_9 >> 32);
		zz_9 &= M;
		{
			zz_5  += x_5 * x_0;
			w     =  (uint) zz_5;
			zz[5] =  (w << 1) | c;
			c     =  w >> 31;
			zz_6  += (zz_5 >> 32) + x_5 * x_1;
			zz_7  += (zz_6 >> 32) + x_5 * x_2;
			zz_6  &= M;
			zz_8  += (zz_7 >> 32) + x_5 * x_3;
			zz_7  &= M;
			zz_9  += (zz_8 >> 32) + x_5 * x_4;
			zz_8  &= M;
			zz_10 += zz_9 >> 32;
			zz_9  &= M;
		}

		ulong x_6   = x[6];
		var   zz_11 = zz[11] + (zz_10 >> 32);
		zz_10 &= M;
		var zz_12 = zz[12] + (zz_11 >> 32);
		zz_11 &= M;
		{
			zz_6  += x_6 * x_0;
			w     =  (uint) zz_6;
			zz[6] =  (w << 1) | c;
			c     =  w >> 31;
			zz_7  += (zz_6 >> 32) + x_6 * x_1;
			zz_8  += (zz_7 >> 32) + x_6 * x_2;
			zz_7  &= M;
			zz_9  += (zz_8 >> 32) + x_6 * x_3;
			zz_8  &= M;
			zz_10 += (zz_9 >> 32) + x_6 * x_4;
			zz_9  &= M;
			zz_11 += (zz_10 >> 32) + x_6 * x_5;
			zz_10 &= M;
			zz_12 += zz_11 >> 32;
			zz_11 &= M;
		}

		ulong x_7   = x[7];
		var   zz_13 = zz[13] + (zz_12 >> 32);
		zz_12 &= M;
		var zz_14 = zz[14] + (zz_13 >> 32);
		zz_13 &= M;
		{
			zz_7  += x_7 * x_0;
			w     =  (uint) zz_7;
			zz[7] =  (w << 1) | c;
			c     =  w >> 31;
			zz_8  += (zz_7  >> 32) + x_7 * x_1;
			zz_9  += (zz_8  >> 32) + x_7 * x_2;
			zz_10 += (zz_9  >> 32) + x_7 * x_3;
			zz_11 += (zz_10 >> 32) + x_7 * x_4;
			zz_12 += (zz_11 >> 32) + x_7 * x_5;
			zz_13 += (zz_12 >> 32) + x_7 * x_6;
			zz_14 += zz_13 >> 32;
		}

		w      = (uint) zz_8;
		zz[8]  = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_9;
		zz[9]  = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_10;
		zz[10] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_11;
		zz[11] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_12;
		zz[12] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_13;
		zz[13] = (w << 1) | c;
		c      = w >> 31;
		w      = (uint) zz_14;
		zz[14] = (w << 1) | c;
		c      = w >> 31;
		w      = zz[15] + (uint) (zz_14 >> 32);
		zz[15] = (w << 1) | c;
	}

	public static int Sub(uint[] x, uint[] y, uint[] z)
	{
		long c = 0;
		c    +=  (long) x[0] - y[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (long) x[1] - y[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (long) x[2] - y[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (long) x[3] - y[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (long) x[4] - y[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (long) x[5] - y[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (long) x[6] - y[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (long) x[7] - y[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (int) c;
	}

	public static int Sub(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
	{
		long c = 0;
		c           +=  (long) x[xOff + 0] - y[yOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (long) x[xOff + 1] - y[yOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  (long) x[xOff + 2] - y[yOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  (long) x[xOff + 3] - y[yOff + 3];
		z[zOff + 3] =   (uint) c;
		c           >>= 32;
		c           +=  (long) x[xOff + 4] - y[yOff + 4];
		z[zOff + 4] =   (uint) c;
		c           >>= 32;
		c           +=  (long) x[xOff + 5] - y[yOff + 5];
		z[zOff + 5] =   (uint) c;
		c           >>= 32;
		c           +=  (long) x[xOff + 6] - y[yOff + 6];
		z[zOff + 6] =   (uint) c;
		c           >>= 32;
		c           +=  (long) x[xOff + 7] - y[yOff + 7];
		z[zOff + 7] =   (uint) c;
		c           >>= 32;
		return (int) c;
	}

	public static int SubBothFrom(uint[] x, uint[] y, uint[] z)
	{
		long c = 0;
		c    +=  (long) z[0] - x[0] - y[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[1] - x[1] - y[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[2] - x[2] - y[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[3] - x[3] - y[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[4] - x[4] - y[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[5] - x[5] - y[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[6] - x[6] - y[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[7] - x[7] - y[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (int) c;
	}

	public static int SubFrom(uint[] x, uint[] z, int cIn)
	{
		long c = cIn;
		c    +=  (long) z[0] - x[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[1] - x[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[2] - x[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[3] - x[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[4] - x[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[5] - x[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[6] - x[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[7] - x[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (int) c;
	}

	public static int SubFrom(uint[] x, int xOff, uint[] z, int zOff, int cIn)
	{
		long c = cIn;
		c           +=  (long) z[zOff + 0] - x[xOff + 0];
		z[zOff + 0] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 1] - x[xOff + 1];
		z[zOff + 1] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 2] - x[xOff + 2];
		z[zOff + 2] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 3] - x[xOff + 3];
		z[zOff + 3] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 4] - x[xOff + 4];
		z[zOff + 4] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 5] - x[xOff + 5];
		z[zOff + 5] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 6] - x[xOff + 6];
		z[zOff + 6] =   (uint) c;
		c           >>= 32;
		c           +=  (long) z[zOff + 7] - x[xOff + 7];
		z[zOff + 7] =   (uint) c;
		c           >>= 32;
		return (int) c;
	}

	public static int SubFrom(ReadOnlySpan<uint> x, Span<uint> z, int cIn)
	{
		long c = cIn;
		c    +=  (long) z[0] - x[0];
		z[0] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[1] - x[1];
		z[1] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[2] - x[2];
		z[2] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[3] - x[3];
		z[3] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[4] - x[4];
		z[4] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[5] - x[5];
		z[5] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[6] - x[6];
		z[6] =   (uint) c;
		c    >>= 32;
		c    +=  (long) z[7] - x[7];
		z[7] =   (uint) c;
		c    >>= 32;
		return (int) c;
	}

	public static BigInteger ToBigInteger(uint[] x)
	{
		var bs = new byte[32];
		for (var i = 0; i < 8; ++i)
		{
			var x_i = x[i];
			if (x_i != 0) Pack.UInt32_To_BigEndian(x_i, bs, (7 - i) << 2);
		}

		return new BigInteger(1, bs);
	}

	public static BigInteger ToBigInteger64(ulong[] x)
	{
		var bs = new byte[32];
		for (var i = 0; i < 4; ++i)
		{
			var x_i = x[i];
			if (x_i != 0L) Pack.UInt64_To_BigEndian(x_i, bs, (3 - i) << 3);
		}

		return new BigInteger(1, bs);
	}

	public static void Xor(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff) => Xor(x.AsSpan(xOff), y.AsSpan(yOff), z.AsSpan(zOff));

	public static void Xor(ReadOnlySpan<uint> x, ReadOnlySpan<uint> y, Span<uint> z)
	{
		if (Avx2.IsSupported && Unsafe.SizeOf<Vector256<byte>>() == 32)
		{
			var X = MemoryMarshal.AsBytes(x[..8]);
			var Y = MemoryMarshal.AsBytes(y[..8]);
			var Z = MemoryMarshal.AsBytes(z[..8]);

			var X0 = MemoryMarshal.Read<Vector256<byte>>(X[..0x20]);
			var Y0 = MemoryMarshal.Read<Vector256<byte>>(Y[..0x20]);

			var Z0 = Avx2.Xor(X0, Y0);

			MemoryMarshal.Write(Z[..0x20], ref Z0);
			return;
		}

		if (Sse2.IsSupported && Unsafe.SizeOf<Vector128<byte>>() == 16)
		{
			var X = MemoryMarshal.AsBytes(x[..8]);
			var Y = MemoryMarshal.AsBytes(y[..8]);
			var Z = MemoryMarshal.AsBytes(z[..8]);

			var X0 = MemoryMarshal.Read<Vector128<byte>>(X[..0x10]);
			var X1 = MemoryMarshal.Read<Vector128<byte>>(X[0x10..0x20]);

			var Y0 = MemoryMarshal.Read<Vector128<byte>>(Y[..0x10]);
			var Y1 = MemoryMarshal.Read<Vector128<byte>>(Y[0x10..0x20]);

			var Z0 = Sse2.Xor(X0, Y0);
			var Z1 = Sse2.Xor(X1, Y1);

			MemoryMarshal.Write(Z[..0x10],     ref Z0);
			MemoryMarshal.Write(Z[0x10..0x20], ref Z1);
			return;
		}

		for (var i = 0; i < 8; i += 4)
		{
			z[i + 0] = x[i + 0] ^ y[i + 0];
			z[i + 1] = x[i + 1] ^ y[i + 1];
			z[i + 2] = x[i + 2] ^ y[i + 2];
			z[i + 3] = x[i + 3] ^ y[i + 3];
		}
	}

	public static void Zero(uint[] z)
	{
		z[0] = 0;
		z[1] = 0;
		z[2] = 0;
		z[3] = 0;
		z[4] = 0;
		z[5] = 0;
		z[6] = 0;
		z[7] = 0;
	}
}
