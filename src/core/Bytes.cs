// perticula - core - Bytes.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Numerics;
using System.Runtime.InteropServices;

namespace core;

/// <summary>
///   Class Bytes.
/// </summary>
public static class Bytes
{
	/// <summary>
	///   The number bits
	/// </summary>
	public const int NumBits = 8;

	/// <summary>
	///   The number bytes
	/// </summary>
	public const int NumBytes = 1;

	/// <summary>
	///   Xors the specified length.
	/// </summary>
	/// <param name="len">The length.</param>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <param name="z">The z.</param>
	public static void Xor(int len, byte[] x, byte[] y, byte[] z) => Xor(len, x.AsSpan(0, len), y.AsSpan(0, len), z.AsSpan(0, len));

	/// <summary>
	///   Xors the specified length.
	/// </summary>
	/// <param name="len">The length.</param>
	/// <param name="x">The x.</param>
	/// <param name="xOff">The x off.</param>
	/// <param name="y">The y.</param>
	/// <param name="yOff">The y off.</param>
	/// <param name="z">The z.</param>
	/// <param name="zOff">The z off.</param>
	public static void Xor(int len, byte[] x, int xOff, byte[] y, int yOff, byte[] z, int zOff) => Xor(len, x.AsSpan(xOff, len), y.AsSpan(yOff, len), z.AsSpan(zOff, len));

	/// <summary>
	///   Xors the specified length.
	/// </summary>
	/// <param name="len">The length.</param>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <param name="z">The z.</param>
	public static void Xor(int len, ReadOnlySpan<byte> x, ReadOnlySpan<byte> y, Span<byte> z)
	{
		var i = 0;
		if (Vector.IsHardwareAccelerated)
		{
			var limit = len - Vector<byte>.Count;
			while (i <= limit)
			{
				var vx = new Vector<byte>(x[i..]);
				var vy = new Vector<byte>(y[i..]);
				(vx ^ vy).CopyTo(z[i..]);
				i += Vector<byte>.Count;
			}
		}

		{
			var limit = len - 8;
			while (i <= limit)
			{
				var x64 = MemoryMarshal.Read<ulong>(x[i..]);
				var y64 = MemoryMarshal.Read<ulong>(y[i..]);
				var z64 = x64 ^ y64;
				MemoryMarshal.Write(z[i..], in z64);
				i += 8;
			}
		}
		{
			while (i < len)
			{
				z[i] = (byte) (x[i] ^ y[i]);
				++i;
			}
		}
	}

	/// <summary>
	///   Xors to.
	/// </summary>
	/// <param name="len">The length.</param>
	/// <param name="x">The x.</param>
	/// <param name="z">The z.</param>
	public static void XorTo(int len, byte[] x, byte[] z) => XorTo(len, x.AsSpan(0, len), z.AsSpan(0, len));

	/// <summary>
	///   Xors to.
	/// </summary>
	/// <param name="len">The length.</param>
	/// <param name="x">The x.</param>
	/// <param name="xOff">The x off.</param>
	/// <param name="z">The z.</param>
	/// <param name="zOff">The z off.</param>
	public static void XorTo(int len, byte[] x, int xOff, byte[] z, int zOff) => XorTo(len, x.AsSpan(xOff, len), z.AsSpan(zOff, len));

	/// <summary>
	///   Xors to.
	/// </summary>
	/// <param name="len">The length.</param>
	/// <param name="x">The x.</param>
	/// <param name="z">The z.</param>
	public static void XorTo(int len, ReadOnlySpan<byte> x, Span<byte> z)
	{
		var i = 0;
		if (Vector.IsHardwareAccelerated)
		{
			var limit = len - Vector<byte>.Count;
			while (i <= limit)
			{
				var vx = new Vector<byte>(x[i..]);
				var vz = new Vector<byte>(z[i..]);
				(vx ^ vz).CopyTo(z[i..]);
				i += Vector<byte>.Count;
			}
		}

		{
			var limit = len - 8;
			while (i <= limit)
			{
				var x64 = MemoryMarshal.Read<ulong>(x[i..]);
				var z64 = MemoryMarshal.Read<ulong>(z[i..]);
				z64 ^= x64;
				MemoryMarshal.Write(z[i..], in z64);
				i += 8;
			}
		}

		{
			while (i < len)
			{
				z[i] ^= x[i];
				++i;
			}
		}
	}
}
