// perticula - core - Nat.512.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace core.Math.Raw;

public static class Nat512
{
	public static void Mul(uint[] x, uint[] y, uint[] zz)
	{
		Nat256.Mul(x, y, zz);
		Nat256.Mul(x, 8, y, 8, zz, 16);

		var c24 = Nat256.AddToEachOther(zz, 8, zz, 16);
		var c16 = c24 + Nat256.AddTo(zz, 0, zz, 8, 0U);
		c24 += Nat256.AddTo(zz, 24, zz, 16, c16);

		uint[] dx  = Nat256.Create(), dy = Nat256.Create();
		var    neg = Nat256.Diff(x, 8, x, 0, dx, 0) != Nat256.Diff(y, 8, y, 0, dy, 0);

		var tt = Nat256.CreateExt();
		Nat256.Mul(dx, dy, tt);

		c24 += neg ? Nat.AddTo(16, tt, 0, zz, 8) : (uint) Nat.SubFrom(16, tt, 0, zz, 8);
		Nat.AddWordAt(32, c24, zz, 24);
	}

	public static void Square(uint[] x, uint[] zz)
	{
		Nat256.Square(x, zz);
		Nat256.Square(x, 8, zz, 16);

		var c24 = Nat256.AddToEachOther(zz, 8, zz, 16);
		var c16 = c24 + Nat256.AddTo(zz, 0, zz, 8, 0U);
		c24 += Nat256.AddTo(zz, 24, zz, 16, c16);

		var dx = Nat256.Create();
		Nat256.Diff(x, 8, x, 0, dx, 0);

		var m = Nat256.CreateExt();
		Nat256.Square(dx, m);

		c24 += (uint) Nat.SubFrom(16, m, 0, zz, 8);
		Nat.AddWordAt(32, c24, zz, 24);
	}

	public static void Xor(uint[] xIn, int xOff, uint[] yIn, int yOff, uint[] zIn, int zOff) => Xor(xIn.AsSpan(xOff), yIn.AsSpan(yOff), zIn.AsSpan(zOff));

	public static void Xor(ReadOnlySpan<uint> xIn, ReadOnlySpan<uint> yIn, Span<uint> zIn)
	{
		if (Avx2.IsSupported && Unsafe.SizeOf<Vector256<byte>>() == 32)
		{
			var x = MemoryMarshal.AsBytes(xIn[..16]);
			var y = MemoryMarshal.AsBytes(yIn[..16]);
			var z = MemoryMarshal.AsBytes(zIn[..16]);

			var x0 = MemoryMarshal.Read<Vector256<byte>>(x[..0x20]);
			var x1 = MemoryMarshal.Read<Vector256<byte>>(x[0x20..0x40]);

			var y0 = MemoryMarshal.Read<Vector256<byte>>(y[..0x20]);
			var y1 = MemoryMarshal.Read<Vector256<byte>>(y[0x20..0x40]);

			var z0 = Avx2.Xor(x0, y0);
			var z1 = Avx2.Xor(x1, y1);

			MemoryMarshal.Write(z[..0x20],     ref z0);
			MemoryMarshal.Write(z[0x20..0x40], ref z1);
			return;
		}

		if (Sse2.IsSupported && Unsafe.SizeOf<Vector128<byte>>() == 16)
		{
			var x = MemoryMarshal.AsBytes(xIn[..16]);
			var y = MemoryMarshal.AsBytes(yIn[..16]);
			var z = MemoryMarshal.AsBytes(zIn[..16]);

			var x0 = MemoryMarshal.Read<Vector128<byte>>(x[..0x10]);
			var x1 = MemoryMarshal.Read<Vector128<byte>>(x[0x10..0x20]);
			var x2 = MemoryMarshal.Read<Vector128<byte>>(x[0x20..0x30]);
			var x3 = MemoryMarshal.Read<Vector128<byte>>(x[0x30..0x40]);

			var y0 = MemoryMarshal.Read<Vector128<byte>>(y[..0x10]);
			var y1 = MemoryMarshal.Read<Vector128<byte>>(y[0x10..0x20]);
			var y2 = MemoryMarshal.Read<Vector128<byte>>(y[0x20..0x30]);
			var y3 = MemoryMarshal.Read<Vector128<byte>>(y[0x30..0x40]);

			var z0 = Sse2.Xor(x0, y0);
			var z1 = Sse2.Xor(x1, y1);
			var z2 = Sse2.Xor(x2, y2);
			var z3 = Sse2.Xor(x3, y3);

			MemoryMarshal.Write(z[..0x10],     ref z0);
			MemoryMarshal.Write(z[0x10..0x20], ref z1);
			MemoryMarshal.Write(z[0x20..0x30], ref z2);
			MemoryMarshal.Write(z[0x30..0x40], ref z3);
			return;
		}

		for (var i = 0; i < 16; i += 4)
		{
			zIn[i + 0] = xIn[i + 0] ^ yIn[i + 0];
			zIn[i + 1] = xIn[i + 1] ^ yIn[i + 1];
			zIn[i + 2] = xIn[i + 2] ^ yIn[i + 2];
			zIn[i + 3] = xIn[i + 3] ^ yIn[i + 3];
		}
	}

	public static void XorTo(uint[] x, int xOff, uint[] z, int zOff) => XorTo(x.AsSpan(xOff), z.AsSpan(zOff));

	public static void XorTo(ReadOnlySpan<uint> xIn, Span<uint> zIn)
	{
		if (Avx2.IsSupported && Unsafe.SizeOf<Vector256<byte>>() == 32)
		{
			var x = MemoryMarshal.AsBytes(xIn[..16]);
			var z = MemoryMarshal.AsBytes(zIn[..16]);

			var x0 = MemoryMarshal.Read<Vector256<byte>>(x[..0x20]);
			var x1 = MemoryMarshal.Read<Vector256<byte>>(x[0x20..0x40]);

			var y0 = MemoryMarshal.Read<Vector256<byte>>(z[..0x20]);
			var y1 = MemoryMarshal.Read<Vector256<byte>>(z[0x20..0x40]);

			var z0 = Avx2.Xor(x0, y0);
			var z1 = Avx2.Xor(x1, y1);

			MemoryMarshal.Write(z[..0x20],     ref z0);
			MemoryMarshal.Write(z[0x20..0x40], ref z1);
			return;
		}

		if (Sse2.IsSupported && Unsafe.SizeOf<Vector128<byte>>() == 16)
		{
			var x = MemoryMarshal.AsBytes(xIn[..16]);
			var z = MemoryMarshal.AsBytes(zIn[..16]);

			var x0 = MemoryMarshal.Read<Vector128<byte>>(x[..0x10]);
			var x1 = MemoryMarshal.Read<Vector128<byte>>(x[0x10..0x20]);
			var x2 = MemoryMarshal.Read<Vector128<byte>>(x[0x20..0x30]);
			var x3 = MemoryMarshal.Read<Vector128<byte>>(x[0x30..0x40]);

			var y0 = MemoryMarshal.Read<Vector128<byte>>(z[..0x10]);
			var y1 = MemoryMarshal.Read<Vector128<byte>>(z[0x10..0x20]);
			var y2 = MemoryMarshal.Read<Vector128<byte>>(z[0x20..0x30]);
			var y3 = MemoryMarshal.Read<Vector128<byte>>(z[0x30..0x40]);

			var z0 = Sse2.Xor(x0, y0);
			var z1 = Sse2.Xor(x1, y1);
			var z2 = Sse2.Xor(x2, y2);
			var z3 = Sse2.Xor(x3, y3);

			MemoryMarshal.Write(z[..0x10],     ref z0);
			MemoryMarshal.Write(z[0x10..0x20], ref z1);
			MemoryMarshal.Write(z[0x20..0x30], ref z2);
			MemoryMarshal.Write(z[0x30..0x40], ref z3);
			return;
		}

		for (var i = 0; i < 16; i += 4)
		{
			zIn[i + 0] ^= xIn[i + 0];
			zIn[i + 1] ^= xIn[i + 1];
			zIn[i + 2] ^= xIn[i + 2];
			zIn[i + 3] ^= xIn[i + 3];
		}
	}

	public static void Xor64(ulong[] x, int xOff, ulong[] y, int yOff, ulong[] z, int zOff) => Xor64(x.AsSpan(xOff), y.AsSpan(yOff), z.AsSpan(zOff));

	public static void Xor64(ReadOnlySpan<ulong> xIn, ReadOnlySpan<ulong> yIn, Span<ulong> zIn)
	{
		if (Avx2.IsSupported && Unsafe.SizeOf<Vector256<byte>>() == 32)
		{
			var x = MemoryMarshal.AsBytes(xIn[..8]);
			var y = MemoryMarshal.AsBytes(yIn[..8]);
			var z = MemoryMarshal.AsBytes(zIn[..8]);

			var x0 = MemoryMarshal.Read<Vector256<byte>>(x[..0x20]);
			var x1 = MemoryMarshal.Read<Vector256<byte>>(x[0x20..0x40]);

			var y0 = MemoryMarshal.Read<Vector256<byte>>(y[..0x20]);
			var y1 = MemoryMarshal.Read<Vector256<byte>>(y[0x20..0x40]);

			var z0 = Avx2.Xor(x0, y0);
			var z1 = Avx2.Xor(x1, y1);

			MemoryMarshal.Write(z[..0x20],     ref z0);
			MemoryMarshal.Write(z[0x20..0x40], ref z1);
			return;
		}

		if (Sse2.IsSupported && Unsafe.SizeOf<Vector128<byte>>() == 16)
		{
			var x = MemoryMarshal.AsBytes(xIn[..8]);
			var y = MemoryMarshal.AsBytes(yIn[..8]);
			var z = MemoryMarshal.AsBytes(zIn[..8]);

			var x0 = MemoryMarshal.Read<Vector128<byte>>(x[..0x10]);
			var x1 = MemoryMarshal.Read<Vector128<byte>>(x[0x10..0x20]);
			var x2 = MemoryMarshal.Read<Vector128<byte>>(x[0x20..0x30]);
			var x3 = MemoryMarshal.Read<Vector128<byte>>(x[0x30..0x40]);

			var y0 = MemoryMarshal.Read<Vector128<byte>>(y[..0x10]);
			var y1 = MemoryMarshal.Read<Vector128<byte>>(y[0x10..0x20]);
			var y2 = MemoryMarshal.Read<Vector128<byte>>(y[0x20..0x30]);
			var y3 = MemoryMarshal.Read<Vector128<byte>>(y[0x30..0x40]);

			var z0 = Sse2.Xor(x0, y0);
			var z1 = Sse2.Xor(x1, y1);
			var z2 = Sse2.Xor(x2, y2);
			var z3 = Sse2.Xor(x3, y3);

			MemoryMarshal.Write(z[..0x10],     ref z0);
			MemoryMarshal.Write(z[0x10..0x20], ref z1);
			MemoryMarshal.Write(z[0x20..0x30], ref z2);
			MemoryMarshal.Write(z[0x30..0x40], ref z3);
			return;
		}

		for (var i = 0; i < 8; i += 4)
		{
			zIn[i + 0] = xIn[i + 0] ^ yIn[i + 0];
			zIn[i + 1] = xIn[i + 1] ^ yIn[i + 1];
			zIn[i + 2] = xIn[i + 2] ^ yIn[i + 2];
			zIn[i + 3] = xIn[i + 3] ^ yIn[i + 3];
		}
	}

	public static void XorTo64(ulong[] x, int xOff, ulong[] z, int zOff) => XorTo64(x.AsSpan(xOff), z.AsSpan(zOff));

	public static void XorTo64(ReadOnlySpan<ulong> xIn, Span<ulong> zIn)
	{
		if (Avx2.IsSupported && Unsafe.SizeOf<Vector256<byte>>() == 32)
		{
			var x = MemoryMarshal.AsBytes(xIn[..8]);
			var z = MemoryMarshal.AsBytes(zIn[..8]);

			var x0 = MemoryMarshal.Read<Vector256<byte>>(x[..0x20]);
			var x1 = MemoryMarshal.Read<Vector256<byte>>(x[0x20..0x40]);

			var y0 = MemoryMarshal.Read<Vector256<byte>>(z[..0x20]);
			var y1 = MemoryMarshal.Read<Vector256<byte>>(z[0x20..0x40]);

			var z0 = Avx2.Xor(x0, y0);
			var z1 = Avx2.Xor(x1, y1);

			MemoryMarshal.Write(z[..0x20],     ref z0);
			MemoryMarshal.Write(z[0x20..0x40], ref z1);
			return;
		}

		if (Sse2.IsSupported && Unsafe.SizeOf<Vector128<byte>>() == 16)
		{
			var x = MemoryMarshal.AsBytes(xIn[..8]);
			var z = MemoryMarshal.AsBytes(zIn[..8]);

			var x0 = MemoryMarshal.Read<Vector128<byte>>(x[..0x10]);
			var x1 = MemoryMarshal.Read<Vector128<byte>>(x[0x10..0x20]);
			var x2 = MemoryMarshal.Read<Vector128<byte>>(x[0x20..0x30]);
			var x3 = MemoryMarshal.Read<Vector128<byte>>(x[0x30..0x40]);

			var y0 = MemoryMarshal.Read<Vector128<byte>>(z[..0x10]);
			var y1 = MemoryMarshal.Read<Vector128<byte>>(z[0x10..0x20]);
			var y2 = MemoryMarshal.Read<Vector128<byte>>(z[0x20..0x30]);
			var y3 = MemoryMarshal.Read<Vector128<byte>>(z[0x30..0x40]);

			var z0 = Sse2.Xor(x0, y0);
			var z1 = Sse2.Xor(x1, y1);
			var z2 = Sse2.Xor(x2, y2);
			var z3 = Sse2.Xor(x3, y3);

			MemoryMarshal.Write(z[..0x10],     ref z0);
			MemoryMarshal.Write(z[0x10..0x20], ref z1);
			MemoryMarshal.Write(z[0x20..0x30], ref z2);
			MemoryMarshal.Write(z[0x30..0x40], ref z3);
			return;
		}

		for (var i = 0; i < 8; i += 4)
		{
			zIn[i + 0] ^= xIn[i + 0];
			zIn[i + 1] ^= xIn[i + 1];
			zIn[i + 2] ^= xIn[i + 2];
			zIn[i + 3] ^= xIn[i + 3];
		}
	}
}
