// perticula - core - Mod.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using core.Cryptography;
using core.Random;

namespace core.Math.Raw;

public static class Mod
{
	private const int   M30             = 0x3FFFFFFF;
	private const ulong M32UnsignedLong = 0xFFFFFFFFUL;

	public static void CheckedModOddInverse(ReadOnlySpan<uint> m, ReadOnlySpan<uint> x, Span<uint> z)
	{
		if (0 == ModOddInverse(m, x, z)) throw new ArithmeticException("Inverse does not exist.");
	}

	public static void CheckedModOddInverseVar(ReadOnlySpan<uint> m, ReadOnlySpan<uint> x, Span<uint> z)
	{
		if (!ModOddInverseVar(m, x, z))
			throw new ArithmeticException("Inverse does not exist.");
	}

	public static uint Inverse32(uint d)
	{
		Debug.Assert((d & 1U) == 1U);

		//int x = d + (((d + 1) & 4) << 1);   // d.x == 1 mod 2**4
		var x = d;      // d.x == 1 mod 2**3
		x *= 2 - d * x; // d.x == 1 mod 2**6
		x *= 2 - d * x; // d.x == 1 mod 2**12
		x *= 2 - d * x; // d.x == 1 mod 2**24
		x *= 2 - d * x; // d.x == 1 mod 2**48
		Debug.Assert(d * x == 1U);
		return x;
	}

	public static ulong Inverse64(ulong d)
	{
		Debug.Assert((d & 1UL) == 1UL);

		//ulong x = d + (((d + 1) & 4) << 1);   // d.x == 1 mod 2**4
		var x = d;      // d.x == 1 mod 2**3
		x *= 2 - d * x; // d.x == 1 mod 2**6
		x *= 2 - d * x; // d.x == 1 mod 2**12
		x *= 2 - d * x; // d.x == 1 mod 2**24
		x *= 2 - d * x; // d.x == 1 mod 2**48
		x *= 2 - d * x; // d.x == 1 mod 2**96
		Debug.Assert(d * x == 1UL);
		return x;
	}

	public static uint ModOddInverse(uint[] m, uint[] x, uint[] z) => ModOddInverse(m.AsSpan(), x.AsSpan(), z.AsSpan());

	public static uint ModOddInverse(ReadOnlySpan<uint> valueM, ReadOnlySpan<uint> valueX, Span<uint> valueZ)
	{
		var len32 = valueM.Length;
		Debug.Assert(len32             > 0);
		Debug.Assert((valueM[0] & 1)   != 0);
		Debug.Assert(valueM[len32 - 1] != 0);

		var bits  = (len32 << 5) - Integers.NumberOfLeadingZeros((int) valueM[len32 - 1]);
		var len30 = (bits + 29) / 30;

		var alloc = len30 <= 50
			            ? stackalloc int[len30 * 5]
			            : new int[len30        * 5];

		Span<int> t = stackalloc int[4];

		var d = alloc[..len30];
		alloc = alloc[len30..];
		var e = alloc[..len30];
		alloc = alloc[len30..];
		var f = alloc[..len30];
		alloc = alloc[len30..];
		var g = alloc[..len30];
		alloc = alloc[len30..];
		var m = alloc[..len30];

		e[0] = 1;
		Encode30(bits, valueX, g);
		Encode30(bits, valueM, m);
		m.CopyTo(f);

		var delta       = 0;
		var m0Inv32     = (int) Inverse32((uint) m[0]);
		var maxDivsteps = GetMaximumDivsteps(bits);

		for (var divSteps = 0; divSteps < maxDivsteps; divSteps += 30)
		{
			delta = Divsteps30(delta, f[0], g[0], t);
			UpdateDe30(len30, d, e, t, m0Inv32, m);
			UpdateFg30(len30, f, g, t);
		}

		var signF = f[len30 - 1] >> 31;
		CNegate30(len30, signF, f);

		CNormalize30(len30, signF, d, m);

		Decode30(bits, d, valueZ);
		Debug.Assert(0 != Nat.LessThan(valueM.Length, valueZ, valueM));

		return (uint) (EqualTo(len30, f, 1) & EqualToZero(len30, g));
	}

	public static bool ModOddInverseVar(uint[] m, uint[] x, uint[] z) => ModOddInverseVar(m.AsSpan(), x.AsSpan(), z.AsSpan());

	public static bool ModOddInverseVar(ReadOnlySpan<uint> valueM, ReadOnlySpan<uint> valueX, Span<uint> valueZ)
	{
		var len32 = valueM.Length;
		Debug.Assert(len32             > 0);
		Debug.Assert((valueM[0] & 1)   != 0);
		Debug.Assert(valueM[len32 - 1] != 0);

		var bits  = (len32 << 5) - Integers.NumberOfLeadingZeros((int) valueM[len32 - 1]);
		var len30 = (bits + 29) / 30;

		var alloc = len30 <= 50
			            ? stackalloc int[len30 * 5]
			            : new int[len30        * 5];

		Span<int> t = stackalloc int[4];
		var       d = alloc[..len30];
		alloc = alloc[len30..];
		var e = alloc[..len30];
		alloc = alloc[len30..];
		var f = alloc[..len30];
		alloc = alloc[len30..];
		var g = alloc[..len30];
		alloc = alloc[len30..];
		var m = alloc[..len30];

		e[0] = 1;
		Encode30(bits, valueX, g);
		Encode30(bits, valueM, m);
		m.CopyTo(f);

		var clzG        = Integers.NumberOfLeadingZeros(g[len30 - 1] | 1) - (len30 * 30 + 2 - bits);
		var eta         = -1                                              - clzG;
		var lenFg       = len30;
		var m0Inv32     = (int) Inverse32((uint) m[0]);
		var maxDivsteps = GetMaximumDivsteps(bits);

		var divsteps = 0;
		while (!EqualToZeroVar_Unlikely(lenFg, g))
		{
			if (divsteps >= maxDivsteps)
				return false;

			divsteps += 30;

			eta = Divsteps30Var(eta, f[0], g[0], t);
			UpdateDe30(len30, d, e, t, m0Inv32, m);
			UpdateFg30(lenFg, f, g, t);

			var fn = f[lenFg - 1];
			var gn = g[lenFg - 1];

			var cond = (lenFg - 2) >> 31;
			cond |= fn ^ (fn       >> 31);
			cond |= gn ^ (gn       >> 31);

			if (cond == 0)
			{
				f[lenFg - 2] |= fn << 30;
				g[lenFg - 2] |= gn << 30;
				--lenFg;
			}
		}

		var signF = f[lenFg - 1] >> 31;

		/*
		 * D is in the range (-2.M, M). First, conditionally add M if D is negative, to bring it
		 * into the range (-M, M). Then normalize by conditionally negating (according to signF)
		 * and/or then adding M, to bring it into the range [0, M).
		 */
		var signD            = d[len30 - 1] >> 31;
		if (signD < 0) signD = Add30(len30, d, m);
		if (signF < 0)
		{
			signD = Negate30(len30, d);
			signF = Negate30(lenFg, f);
		}

		Debug.Assert(0 == signF);

		if (!EqualToOneVar_Expected(lenFg, f))
			return false;

		if (signD < 0) signD = Add30(len30, d, m);
		Debug.Assert(0 == signD);

		Decode30(bits, d, valueZ);
		Debug.Assert(!Nat.Gte(valueM.Length, valueZ, valueM));

		return true;
	}

	public static uint[] Random(SecureRandom random, uint[] p)
	{
		var len = p.Length;
		var s   = Nat.Create(len);

		var m = p[len - 1];
		m |= m >> 1;
		m |= m >> 2;
		m |= m >> 4;
		m |= m >> 8;
		m |= m >> 16;

		var bytes = new byte[len << 2];
		do
		{
			random.NextBytes(bytes);
			Pack.BigEndian_To_UInt32(bytes, 0, s);
			s[len - 1] &= m;
		} while (Nat.Gte(len, s, p));

		return s;
	}

	public static void Random(SecureRandom random, ReadOnlySpan<uint> p, Span<uint> z)
	{
		var len = p.Length;
		if (z.Length < len)
			throw new ArgumentException("insufficient space", nameof(z));

		var s = z[..len];

		var m = p[len - 1];
		m |= m >> 1;
		m |= m >> 2;
		m |= m >> 4;
		m |= m >> 8;
		m |= m >> 16;

		var bytes = len <= 256
			            ? stackalloc byte[len << 2]
			            : new byte[len        << 2];

		do
		{
			random.NextBytes(bytes);
			Pack.BigEndian_To_UInt32(bytes, s);
			s[len - 1] &= m;
		} while (Nat.Gte(len, s, p));
	}

	private static int Add30(int len30, Span<int> d, ReadOnlySpan<int> m)
	{
		Debug.Assert(len30    > 0);
		Debug.Assert(d.Length >= len30);
		Debug.Assert(m.Length >= len30);

		int c = 0, last = len30 - 1;
		for (var i = 0; i < last; ++i)
		{
			c    +=  d[i] + m[i];
			d[i] =   c & M30;
			c    >>= 30;
		}

		c       +=  d[last] + m[last];
		d[last] =   c;
		c       >>= 30;
		return c;
	}

	private static void CNegate30(int len30, int cond, Span<int> d)
	{
		Debug.Assert(len30    > 0);
		Debug.Assert(d.Length >= len30);

		int c = 0, last = len30 - 1;
		for (var i = 0; i < last; ++i)
		{
			c    +=  (d[i] ^ cond) - cond;
			d[i] =   c & M30;
			c    >>= 30;
		}

		c       += (d[last] ^ cond) - cond;
		d[last] =  c;
	}

	private static void CNormalize30(int len30, int condNegate, Span<int> d, ReadOnlySpan<int> m)
	{
		Debug.Assert(len30    > 0);
		Debug.Assert(d.Length >= len30);
		Debug.Assert(m.Length >= len30);

		/*
		 * D is in the range (-2.M, M). First, conditionally add M if D is negative, to bring it
		 * into the range (-M, M). Then normalize by conditionally negating (according to signF)
		 * and/or then adding M, to bring it into the range [0, M).
		 */

		var last = len30 - 1;

		{
			int c = 0, condAdd = d[last] >> 31;
			for (var i = 0; i < last; ++i)
			{
				var di = d[i] + (m[i] & condAdd);
				di   =   (di ^ condNegate) - condNegate;
				c    +=  di;
				d[i] =   c & M30;
				c    >>= 30;
			}

			{
				var di = d[last] + (m[last] & condAdd);
				di      =  (di ^ condNegate) - condNegate;
				c       += di;
				d[last] =  c;
			}
		}

		{
			int c = 0, condAdd = d[last] >> 31;
			for (var i = 0; i < last; ++i)
			{
				var di = d[i] + (m[i] & condAdd);
				c    +=  di;
				d[i] =   c & M30;
				c    >>= 30;
			}

			{
				var di = d[last] + (m[last] & condAdd);
				c       += di;
				d[last] =  c;
			}
			Debug.Assert(c >> 30 == 0);
		}
	}

	private static void Decode30(int bits, ReadOnlySpan<int> x, Span<uint> z)
	{
		Debug.Assert(bits > 0);

		var   avail = 0;
		ulong data  = 0L;

		int xOff = 0, zOff = 0;
		while (bits > 0)
		{
			while (avail < System.Math.Min(32, bits))
			{
				data  |= (ulong) x[xOff++] << avail;
				avail += 30;
			}

			z[zOff++] =   (uint) data;
			data      >>= 32;
			avail     -=  32;
			bits      -=  32;
		}
	}

	private static int Divsteps30(int delta, int f0, int g0, Span<int> t)
	{
		int u = 1 << 30, v = 0, q = 0, r = 1 << 30;

		var f = f0;
		var g = g0;

		for (var i = 0; i < 30; ++i)
		{
			Debug.Assert((f & 1)                                     == 1);
			Debug.Assert((u >> (30 - i)) * f0 + (v >> (30 - i)) * g0 == f << i);
			Debug.Assert((q >> (30 - i)) * f0 + (r >> (30 - i)) * g0 == g << i);

			var c1 = delta >> 31;
			var c2 = -(g & 1);

			var x = f ^ c1;
			var y = u ^ c1;
			var z = v ^ c1;

			g -= x & c2;
			q -= y & c2;
			r -= z & c2;

			c2    &= ~c1;
			delta =  (delta ^ c2) - (c2 - 1);

			f += g & c2;
			u += q & c2;
			v += r & c2;

			g >>= 1;
			q >>= 1;
			r >>= 1;
		}

		t[0] = u;
		t[1] = v;
		t[2] = q;
		t[3] = r;

		return delta;
	}

	private static int Divsteps30Var(int eta, int f0, int g0, Span<int> t)
	{
		int u = 1,  v = 0, q = 0, r = 1;
		int f = f0, g = g0;
		var i = 30;

		for (;;)
		{
			// Use a sentinel bit to count zeros only up to i.
			var zeros = Integers.NumberOfTrailingZeros(g | (-1 << i));

			g   >>= zeros;
			u   <<= zeros;
			v   <<= zeros;
			eta -=  zeros;
			i   -=  zeros;

			if (i <= 0)
				break;

			Debug.Assert((f & 1)         == 1);
			Debug.Assert((g & 1)         == 1);
			Debug.Assert(u * f0 + v * g0 == f << (30 - i));
			Debug.Assert(q * f0 + r * g0 == g << (30 - i));

			int limit;
			int m;
			int w;
			if (eta < 0)
			{
				eta = -eta;
				var x = f;
				f = g;
				g = -x;
				var y = u;
				u = q;
				q = -y;
				var z = v;
				v = r;
				r = -z;

				// Handle up to 6 divsteps at once, subject to eta and i.
				limit = eta + 1 > i ? i : eta + 1;
				m     = (int) ((uint.MaxValue >> (32 - limit)) & 63U);

				w = (f * g * (f * f - 2)) & m;
			}
			else
			{
				// Handle up to 4 divsteps at once, subject to eta and i.
				limit = eta + 1 > i ? i : eta + 1;
				m     = (int) ((uint.MaxValue >> (32 - limit)) & 15U);

				w = f + (((f + 1) & 4) << 1);
				w = (-w * g) & m;
			}

			g += f * w;
			q += u * w;
			r += v * w;

			Debug.Assert((g & m) == 0);
		}

		t[0] = u;
		t[1] = v;
		t[2] = q;
		t[3] = r;

		return eta;
	}

	private static void Encode30(int bits, ReadOnlySpan<uint> x, Span<int> z)
	{
		Debug.Assert(bits > 0);

		var avail = 0;
		var data  = 0UL;

		int xOff = 0, zOff = 0;
		while (bits > 0)
		{
			if (avail < System.Math.Min(30, bits))
			{
				data  |= (x[xOff++] & M32UnsignedLong) << avail;
				avail += 32;
			}

			z[zOff++] =   (int) data & M30;
			data      >>= 30;
			avail     -=  30;
			bits      -=  30;
		}
	}


	private static int EqualTo(int len, ReadOnlySpan<int> x, int y)
	{
		var d                           = x[0] ^ y;
		for (var i = 1; i < len; ++i) d |= x[i];
		d = (d >>> 1) | (d & 1);
		return (d - 1) >> 31;
	}

	private static bool EqualToOneVar_Expected(int len, ReadOnlySpan<int> x)
	{
		var d                           = x[0] ^ 1;
		for (var i = 1; i < len; ++i) d |= x[i];
		return d          == 0;
	}

	private static int EqualToZero(int len, ReadOnlySpan<int> x)
	{
		var d                           = 0;
		for (var i = 0; i < len; ++i) d |= x[i];
		d = (d >>> 1) | (d & 1);
		return (d - 1) >> 31;
	}

	private static bool EqualToZeroVar_Unlikely(int len, ReadOnlySpan<int> x)
	{
		var d = x[0];
		if (d != 0)
			return false;

		for (var i = 1; i < len; ++i) d |= x[i];
		return d          == 0;
	}

	private static int GetMaximumDivsteps(int bits) => (49 * bits + (bits < 46 ? 80 : 47)) / 17;

	private static int Negate30(int len30, Span<int> d)
	{
		Debug.Assert(len30    > 0);
		Debug.Assert(d.Length >= len30);

		int c = 0, last = len30 - 1;
		for (var i = 0; i < last; ++i)
		{
			c    -=  d[i];
			d[i] =   c & M30;
			c    >>= 30;
		}

		c       -=  d[last];
		d[last] =   c;
		c       >>= 30;
		return c;
	}

	private static void UpdateDe30(int len30, Span<int> d, Span<int> e, ReadOnlySpan<int> t, int m0Inv32, ReadOnlySpan<int> m)
	{
		Debug.Assert(len30          > 0);
		Debug.Assert(d.Length       >= len30);
		Debug.Assert(e.Length       >= len30);
		Debug.Assert(m.Length       >= len30);
		Debug.Assert(m0Inv32 * m[0] == 1);

		int u = t[0], v = t[1], q = t[2], r = t[3];
		int i;

		/*
		 * We accept D (E) in the range (-2.M, M) and conceptually add the modulus to the input
		 * value if it is initially negative. Instead of adding it explicitly, we add u and/or v (q
		 * and/or r) to md (me).
		 */
		var sd = d[len30 - 1] >> 31;
		var se = e[len30 - 1] >> 31;

		var md = (u & sd) + (v & se);
		var me = (q & sd) + (r & se);

		var mi = m[0];
		var di = d[0];
		var ei = e[0];

		var cd = (long) u * di + (long) v * ei;
		var ce = (long) q * di + (long) r * ei;

		/*
		 * Subtract from md/me an extra term in the range [0, 2^30) such that the low 30 bits of the
		 * intermediate D/E values will be 0, allowing clean division by 2^30. The final D/E are
		 * thus in the range (-2.M, M), consistent with the input constraint.
		 */
		md -= (m0Inv32 * (int) cd + md) & M30;
		me -= (m0Inv32 * (int) ce + me) & M30;

		cd += (long) mi * md;
		ce += (long) mi * me;

		Debug.Assert(((int) cd & M30) == 0);
		Debug.Assert(((int) ce & M30) == 0);

		cd >>= 30;
		ce >>= 30;

		for (i = 1; i < len30; ++i)
		{
			mi = m[i];
			di = d[i];
			ei = e[i];

			cd += (long) u * di + (long) v * ei + (long) mi * md;
			ce += (long) q * di + (long) r * ei + (long) mi * me;

			d[i - 1] =   (int) cd & M30;
			cd       >>= 30;
			e[i - 1] =   (int) ce & M30;
			ce       >>= 30;
		}

		d[len30 - 1] = (int) cd;
		e[len30 - 1] = (int) ce;
	}

	private static void UpdateFg30(int len30, Span<int> f, Span<int> g, ReadOnlySpan<int> t)
	{
		Debug.Assert(len30    > 0);
		Debug.Assert(f.Length >= len30);
		Debug.Assert(g.Length >= len30);

		int u = t[0], v = t[1], q = t[2], r = t[3];
		int i;

		var fi = f[0];
		var gi = g[0];

		var cf = (long) u * fi + (long) v * gi;
		var cg = (long) q * fi + (long) r * gi;

		Debug.Assert(((int) cf & M30) == 0);
		Debug.Assert(((int) cg & M30) == 0);

		cf >>= 30;
		cg >>= 30;

		for (i = 1; i < len30; ++i)
		{
			fi = f[i];
			gi = g[i];

			cf += (long) u * fi + (long) v * gi;
			cg += (long) q * fi + (long) r * gi;

			f[i - 1] =   (int) cf & M30;
			cf       >>= 30;
			g[i - 1] =   (int) cg & M30;
			cg       >>= 30;
		}

		f[len30 - 1] = (int) cf;
		g[len30 - 1] = (int) cg;
	}
}
