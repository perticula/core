// perticula - core - Pack.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace core.Cryptography;

/// <summary>
///   Class Pack.
/// </summary>
public static class Pack
{
	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt16_To_BigEndian(ushort n, byte[] bs) => BinaryPrimitives.WriteUInt16BigEndian(bs, n);

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt16_To_BigEndian(ushort n, byte[] bs, int off) => BinaryPrimitives.WriteUInt16BigEndian(bs.AsSpan(off), n);

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt16_To_BigEndian(ushort[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt16_To_BigEndian(t, bs, off);
			off += 2;
		}
	}

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	internal static void UInt16_To_BigEndian(ushort[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt16_To_BigEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 2;
		}
	}

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt16_To_BigEndian(ushort n)
	{
		var bs = new byte[2];
		UInt16_To_BigEndian(n, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt16_To_BigEndian(ushort[] ns) => UInt16_To_BigEndian(ns, 0, ns.Length);

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt16_To_BigEndian(ushort[] ns, int nsOff, int nsLen)
	{
		var bs = new byte[2 * nsLen];
		UInt16_To_BigEndian(ns, nsOff, nsLen, bs, 0);
		return bs;
	}

	/// <summary>
	///   Bigs the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt16.</returns>
	internal static ushort BigEndian_To_UInt16(byte[] bs, int off) => BinaryPrimitives.ReadUInt16BigEndian(bs.AsSpan(off));

	/// <summary>
	///   Bigs the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	internal static void BigEndian_To_UInt16(byte[] bs, int bsOff, ushort[] ns, int nsOff) => ns[nsOff] = BigEndian_To_UInt16(bs, bsOff);

	/// <summary>
	///   Bigs the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt16[].</returns>
	internal static ushort[] BigEndian_To_UInt16(byte[] bs) => BigEndian_To_UInt16(bs, 0, bs.Length);

	/// <summary>
	///   Bigs the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.UInt16[].</returns>
	/// <exception cref="System.ArgumentException">must be a multiple of 2 - len</exception>
	internal static ushort[] BigEndian_To_UInt16(byte[] bs, int off, int len)
	{
		if ((len & 1) != 0)
			throw new ArgumentException("must be a multiple of 2", nameof(len));

		var ns = new ushort[len / 2];
		for (var i = 0; i < len; i += 2) BigEndian_To_UInt16(bs, off + i, ns, i >> 1);
		return ns;
	}

	/// <summary>
	///   us the int24 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt24_To_BigEndian(uint n, byte[] bs)
	{
		bs[0] = (byte) (n >> 16);
		bs[1] = (byte) (n >> 8);
		bs[2] = (byte) n;
	}

	/// <summary>
	///   us the int24 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt24_To_BigEndian(uint n, byte[] bs, int off)
	{
		bs[off + 0] = (byte) (n >> 16);
		bs[off + 1] = (byte) (n >> 8);
		bs[off + 2] = (byte) n;
	}

	/// <summary>
	///   Bigs the endian to u int24.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint BigEndian_To_UInt24(byte[] bs) =>
		((uint) bs[0]   << 16)
		| ((uint) bs[1] << 8)
		| bs[2];

	/// <summary>
	///   Bigs the endian to u int24.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint BigEndian_To_UInt24(byte[] bs, int off) =>
		((uint) bs[off]       << 16)
		| ((uint) bs[off + 1] << 8)
		| bs[off + 2];

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt32_To_BigEndian(uint n, byte[] bs) => BinaryPrimitives.WriteUInt32BigEndian(bs, n);

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt32_To_BigEndian(uint n, byte[] bs, int off) => BinaryPrimitives.WriteUInt32BigEndian(bs.AsSpan(off), n);

	/// <summary>
	///   us the int32 to big endian high.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	internal static void UInt32_To_BigEndian_High(uint n, byte[] bs, int off, int len)
	{
		Debug.Assert(len is >= 1 and <= 4);

		var pos = 24;
		bs[off] = (byte) (n >> pos);
		for (var i = 1; i < len; ++i)
		{
			pos         -= 8;
			bs[off + i] =  (byte) (n >> pos);
		}
	}

	/// <summary>
	///   us the int32 to big endian low.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	internal static void UInt32_To_BigEndian_Low(uint n, byte[] bs, int off, int len) => UInt32_To_BigEndian_High(n << ((4 - len) << 3), bs, off, len);

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt32_To_BigEndian(uint[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt32_To_BigEndian(t, bs, off);
			off += 4;
		}
	}

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	internal static void UInt32_To_BigEndian(uint[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt32_To_BigEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 4;
		}
	}

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt32_To_BigEndian(uint n)
	{
		var bs = new byte[4];
		UInt32_To_BigEndian(n, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt32_To_BigEndian(uint[] ns)
	{
		var bs = new byte[4 * ns.Length];
		UInt32_To_BigEndian(ns, bs, 0);
		return bs;
	}

	/// <summary>
	///   Bigs the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint BigEndian_To_UInt32(byte[] bs) => BinaryPrimitives.ReadUInt32BigEndian(bs);

	/// <summary>
	///   Bigs the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint BigEndian_To_UInt32(byte[] bs, int off) => BinaryPrimitives.ReadUInt32BigEndian(bs.AsSpan(off));

	/// <summary>
	///   Bigs the endian to u int32 high.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint BigEndian_To_UInt32_High(byte[] bs, int off, int len) => BigEndian_To_UInt32_Low(bs, off, len) << ((4 - len) << 3);

	/// <summary>
	///   Bigs the endian to u int32 low.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint BigEndian_To_UInt32_Low(byte[] bs, int off, int len)
	{
		Debug.Assert(len is >= 1 and <= 4);

		uint result = bs[off];
		for (var i = 1; i < len; ++i)
		{
			result <<= 8;
			result |=  bs[off + i];
		}

		return result;
	}

	/// <summary>
	///   Bigs the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="ns">The ns.</param>
	internal static void BigEndian_To_UInt32(byte[] bs, int off, uint[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  BigEndian_To_UInt32(bs, off);
			off   += 4;
		}
	}

	/// <summary>
	///   Bigs the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	internal static void BigEndian_To_UInt32(byte[] bs, int bsOff, uint[] ns, int nsOff, int nsLen)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			ns[nsOff + i] =  BigEndian_To_UInt32(bs, bsOff);
			bsOff         += 4;
		}
	}

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt64_To_BigEndian(ulong n)
	{
		var bs = new byte[8];
		UInt64_To_BigEndian(n, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt64_To_BigEndian(ulong n, byte[] bs) => BinaryPrimitives.WriteUInt64BigEndian(bs, n);

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt64_To_BigEndian(ulong n, byte[] bs, int off) => BinaryPrimitives.WriteUInt64BigEndian(bs.AsSpan(off), n);

	/// <summary>
	///   us the int64 to big endian high.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	internal static void UInt64_To_BigEndian_High(ulong n, byte[] bs, int off, int len)
	{
		Debug.Assert(len is >= 1 and <= 8);

		var pos = 56;
		bs[off] = (byte) (n >> pos);
		for (var i = 1; i < len; ++i)
		{
			pos         -= 8;
			bs[off + i] =  (byte) (n >> pos);
		}
	}

	/// <summary>
	///   us the int64 to big endian low.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	internal static void UInt64_To_BigEndian_Low(ulong n, byte[] bs, int off, int len) => UInt64_To_BigEndian_High(n << ((8 - len) << 3), bs, off, len);

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt64_To_BigEndian(ulong[] ns)
	{
		var bs = new byte[8 * ns.Length];
		UInt64_To_BigEndian(ns, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt64_To_BigEndian(ulong[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt64_To_BigEndian(t, bs, off);
			off += 8;
		}
	}

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	internal static void UInt64_To_BigEndian(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt64_To_BigEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 8;
		}
	}

	/// <summary>
	///   Bigs the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt64.</returns>
	internal static ulong BigEndian_To_UInt64(byte[] bs) => BinaryPrimitives.ReadUInt64BigEndian(bs);

	/// <summary>
	///   Bigs the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt64.</returns>
	internal static ulong BigEndian_To_UInt64(byte[] bs, int off) => BinaryPrimitives.ReadUInt64BigEndian(bs.AsSpan(off));

	/// <summary>
	///   Bigs the endian to u int64 high.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.UInt64.</returns>
	internal static ulong BigEndian_To_UInt64_High(byte[] bs, int off, int len) => BigEndian_To_UInt64_Low(bs, off, len) << ((8 - len) << 3);

	/// <summary>
	///   Bigs the endian to u int64 low.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.UInt64.</returns>
	internal static ulong BigEndian_To_UInt64_Low(byte[] bs, int off, int len)
	{
		Debug.Assert(len is >= 1 and <= 8);

		ulong result = bs[off];
		for (var i = 1; i < len; ++i)
		{
			result <<= 8;
			result |=  bs[off + i];
		}

		return result;
	}

	/// <summary>
	///   Bigs the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="ns">The ns.</param>
	internal static void BigEndian_To_UInt64(byte[] bs, int off, ulong[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  BigEndian_To_UInt64(bs, off);
			off   += 8;
		}
	}

	/// <summary>
	///   Bigs the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	internal static void BigEndian_To_UInt64(byte[] bs, int bsOff, ulong[] ns, int nsOff, int nsLen)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			ns[nsOff + i] =  BigEndian_To_UInt64(bs, bsOff);
			bsOff         += 8;
		}
	}

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt16_To_LittleEndian(ushort n, byte[] bs) => BinaryPrimitives.WriteUInt16LittleEndian(bs, n);

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt16_To_LittleEndian(ushort n, byte[] bs, int off) => BinaryPrimitives.WriteUInt16LittleEndian(bs.AsSpan(off), n);

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt16_To_LittleEndian(ushort n)
	{
		var bs = new byte[2];
		UInt16_To_LittleEndian(n, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt16_To_LittleEndian(ushort[] ns)
	{
		var bs = new byte[2 * ns.Length];
		UInt16_To_LittleEndian(ns, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt16_To_LittleEndian(ushort[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt16_To_LittleEndian(t, bs, off);
			off += 2;
		}
	}

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	internal static void UInt16_To_LittleEndian(ushort[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt16_To_LittleEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 2;
		}
	}

	/// <summary>
	///   Littles the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt16.</returns>
	internal static ushort LittleEndian_To_UInt16(byte[] bs) => BinaryPrimitives.ReadUInt16LittleEndian(bs);

	/// <summary>
	///   Littles the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt16.</returns>
	internal static ushort LittleEndian_To_UInt16(byte[] bs, int off) => BinaryPrimitives.ReadUInt16LittleEndian(bs.AsSpan(off));

	/// <summary>
	///   Littles the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="ns">The ns.</param>
	internal static void LittleEndian_To_UInt16(byte[] bs, int off, ushort[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  LittleEndian_To_UInt16(bs, off);
			off   += 2;
		}
	}

	/// <summary>
	///   Littles the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="bOff">The b off.</param>
	/// <param name="ns">The ns.</param>
	/// <param name="nOff">The n off.</param>
	/// <param name="count">The count.</param>
	internal static void LittleEndian_To_UInt16(byte[] bs, int bOff, ushort[] ns, int nOff, int count)
	{
		for (var i = 0; i < count; ++i)
		{
			ns[nOff + i] =  LittleEndian_To_UInt16(bs, bOff);
			bOff         += 2;
		}
	}

	/// <summary>
	///   Littles the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="count">The count.</param>
	/// <returns>System.UInt16[].</returns>
	internal static ushort[] LittleEndian_To_UInt16(byte[] bs, int off, int count)
	{
		var ns = new ushort[count];
		LittleEndian_To_UInt16(bs, off, ns);
		return ns;
	}

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt32_To_LittleEndian(uint n)
	{
		var bs = new byte[4];
		UInt32_To_LittleEndian(n, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt32_To_LittleEndian(uint n, byte[] bs) => BinaryPrimitives.WriteUInt32LittleEndian(bs, n);

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt32_To_LittleEndian(uint n, byte[] bs, int off) => BinaryPrimitives.WriteUInt32LittleEndian(bs.AsSpan(off), n);

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt32_To_LittleEndian(uint[] ns)
	{
		var bs = new byte[4 * ns.Length];
		UInt32_To_LittleEndian(ns, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt32_To_LittleEndian(uint[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt32_To_LittleEndian(t, bs, off);
			off += 4;
		}
	}

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	internal static void UInt32_To_LittleEndian(uint[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt32_To_LittleEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 4;
		}
	}

	/// <summary>
	///   Littles the endian to u int24.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint LittleEndian_To_UInt24(byte[] bs, int off) =>
		bs[off]
		| ((uint) bs[off + 1] << 8)
		| ((uint) bs[off + 2] << 16);

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint LittleEndian_To_UInt32(byte[] bs) => BinaryPrimitives.ReadUInt32LittleEndian(bs);

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint LittleEndian_To_UInt32(byte[] bs, int off) => BinaryPrimitives.ReadUInt32LittleEndian(bs.AsSpan(off));

	/// <summary>
	///   Littles the endian to u int32 high.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint LittleEndian_To_UInt32_High(byte[] bs, int off, int len) => LittleEndian_To_UInt32_Low(bs, off, len) << ((4 - len) << 3);

	/// <summary>
	///   Littles the endian to u int32 low.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint LittleEndian_To_UInt32_Low(byte[] bs, int off, int len)
	{
		Debug.Assert(len is >= 1 and <= 4);

		uint result = bs[off];
		var  pos    = 0;
		for (var i = 1; i < len; ++i)
		{
			pos    += 8;
			result |= (uint) bs[off + i] << pos;
		}

		return result;
	}

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="ns">The ns.</param>
	internal static void LittleEndian_To_UInt32(byte[] bs, int off, uint[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  LittleEndian_To_UInt32(bs, off);
			off   += 4;
		}
	}

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="bOff">The b off.</param>
	/// <param name="ns">The ns.</param>
	/// <param name="nOff">The n off.</param>
	/// <param name="count">The count.</param>
	internal static void LittleEndian_To_UInt32(byte[] bs, int bOff, uint[] ns, int nOff, int count)
	{
		for (var i = 0; i < count; ++i)
		{
			ns[nOff + i] =  LittleEndian_To_UInt32(bs, bOff);
			bOff         += 4;
		}
	}

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="count">The count.</param>
	/// <returns>System.UInt32[].</returns>
	internal static uint[] LittleEndian_To_UInt32(byte[] bs, int off, int count)
	{
		var ns = new uint[count];
		LittleEndian_To_UInt32(bs, off, ns);
		return ns;
	}

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt64_To_LittleEndian(ulong n)
	{
		var bs = new byte[8];
		UInt64_To_LittleEndian(n, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt64_To_LittleEndian(ulong n, byte[] bs) => BinaryPrimitives.WriteUInt64LittleEndian(bs, n);

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt64_To_LittleEndian(ulong n, byte[] bs, int off) => BinaryPrimitives.WriteUInt64LittleEndian(bs.AsSpan(off), n);

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <returns>System.Byte[].</returns>
	internal static byte[] UInt64_To_LittleEndian(ulong[] ns)
	{
		var bs = new byte[8 * ns.Length];
		UInt64_To_LittleEndian(ns, bs, 0);
		return bs;
	}

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	internal static void UInt64_To_LittleEndian(ulong[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt64_To_LittleEndian(t, bs, off);
			off += 8;
		}
	}

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	internal static void UInt64_To_LittleEndian(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt64_To_LittleEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 8;
		}
	}

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt64.</returns>
	internal static ulong LittleEndian_To_UInt64(byte[] bs) => BinaryPrimitives.ReadUInt64LittleEndian(bs);

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt64.</returns>
	internal static ulong LittleEndian_To_UInt64(byte[] bs, int off) => BinaryPrimitives.ReadUInt64LittleEndian(bs.AsSpan(off));

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="ns">The ns.</param>
	internal static void LittleEndian_To_UInt64(byte[] bs, int off, ulong[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  LittleEndian_To_UInt64(bs, off);
			off   += 8;
		}
	}

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="bsOff">The bs off.</param>
	/// <param name="ns">The ns.</param>
	/// <param name="nsOff">The ns off.</param>
	/// <param name="nsLen">Length of the ns.</param>
	internal static void LittleEndian_To_UInt64(byte[] bs, int bsOff, ulong[] ns, int nsOff, int nsLen)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			ns[nsOff + i] =  LittleEndian_To_UInt64(bs, bsOff);
			bsOff         += 8;
		}
	}

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <param name="count">The count.</param>
	/// <returns>System.UInt64[].</returns>
	internal static ulong[] LittleEndian_To_UInt64(byte[] bs, int off, int count)
	{
		var ns = new ulong[count];
		LittleEndian_To_UInt64(bs, off, ns);
		return ns;
	}

	/// <summary>
	///   Bigs the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BigEndian_To_UInt32(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt32BigEndian(bs);

	/// <summary>
	///   Bigs the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BigEndian_To_UInt32(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt32BigEndian(bs[off..]);

	/// <summary>
	///   Bigs the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="ns">The ns.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void BigEndian_To_UInt32(ReadOnlySpan<byte> bs, Span<uint> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = BigEndian_To_UInt32(bs);
			bs    = bs[4..];
		}
	}

	/// <summary>
	///   Bigs the endian to u int32 high.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BigEndian_To_UInt32_High(ReadOnlySpan<byte> bs) => BigEndian_To_UInt32_Low(bs) << ((4 - bs.Length) << 3);

	/// <summary>
	///   Bigs the endian to u int32 low.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint BigEndian_To_UInt32_Low(ReadOnlySpan<byte> bs)
	{
		var len = bs.Length;
		Debug.Assert(len is >= 1 and <= 4);

		uint result = bs[0];
		for (var i = 1; i < len; ++i)
		{
			result <<= 8;
			result |=  bs[i];
		}

		return result;
	}

	/// <summary>
	///   Bigs the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt64.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BigEndian_To_UInt64(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt64BigEndian(bs);

	/// <summary>
	///   Bigs the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt64.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BigEndian_To_UInt64(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt64BigEndian(bs[off..]);

	/// <summary>
	///   Bigs the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="ns">The ns.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void BigEndian_To_UInt64(ReadOnlySpan<byte> bs, Span<ulong> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = BigEndian_To_UInt64(bs);
			bs    = bs[8..];
		}
	}

	/// <summary>
	///   Bigs the endian to u int64 high.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt64.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BigEndian_To_UInt64_High(ReadOnlySpan<byte> bs) => BigEndian_To_UInt64_Low(bs) << ((8 - bs.Length) << 3);

	/// <summary>
	///   Bigs the endian to u int64 low.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt64.</returns>
	internal static ulong BigEndian_To_UInt64_Low(ReadOnlySpan<byte> bs)
	{
		var len = bs.Length;
		Debug.Assert(len is >= 1 and <= 8);

		ulong result = bs[0];
		for (var i = 1; i < len; ++i)
		{
			result <<= 8;
			result |=  bs[i];
		}

		return result;
	}

	/// <summary>
	///   Littles the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt16.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ushort LittleEndian_To_UInt16(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt16LittleEndian(bs);

	/// <summary>
	///   Littles the endian to u int16.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="ns">The ns.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LittleEndian_To_UInt16(ReadOnlySpan<byte> bs, Span<ushort> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = LittleEndian_To_UInt16(bs);
			bs    = bs[2..];
		}
	}

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint LittleEndian_To_UInt32(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt32LittleEndian(bs);

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint LittleEndian_To_UInt32(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt32LittleEndian(bs[off..]);

	/// <summary>
	///   Littles the endian to u int32.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="ns">The ns.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LittleEndian_To_UInt32(ReadOnlySpan<byte> bs, Span<uint> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = LittleEndian_To_UInt32(bs);
			bs    = bs[4..];
		}
	}

	/// <summary>
	///   Littles the endian to u int32 high.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint LittleEndian_To_UInt32_High(ReadOnlySpan<byte> bs) => LittleEndian_To_UInt32_Low(bs) << ((4 - bs.Length) << 3);

	/// <summary>
	///   Littles the endian to u int32 low.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt32.</returns>
	internal static uint LittleEndian_To_UInt32_Low(ReadOnlySpan<byte> bs)
	{
		var len = bs.Length;
		Debug.Assert(len is >= 1 and <= 4);

		uint result = bs[0];
		var  pos    = 0;
		for (var i = 1; i < len; ++i)
		{
			pos    += 8;
			result |= (uint) bs[i] << pos;
		}

		return result;
	}

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <returns>System.UInt64.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong LittleEndian_To_UInt64(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt64LittleEndian(bs);

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="off">The off.</param>
	/// <returns>System.UInt64.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong LittleEndian_To_UInt64(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt64LittleEndian(bs[off..]);

	/// <summary>
	///   Littles the endian to u int64.
	/// </summary>
	/// <param name="bs">The bs.</param>
	/// <param name="ns">The ns.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LittleEndian_To_UInt64(ReadOnlySpan<byte> bs, Span<ulong> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = LittleEndian_To_UInt64(bs);
			bs    = bs[8..];
		}
	}

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_BigEndian(ushort n, Span<byte> bs)
	{
		BinaryPrimitives.WriteUInt16BigEndian(bs, n);
	}

	/// <summary>
	///   us the int16 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_BigEndian(ReadOnlySpan<ushort> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt16_To_BigEndian(t, bs);
			bs = bs[2..];
		}
	}

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_LittleEndian(ushort n, Span<byte> bs)
	{
		BinaryPrimitives.WriteUInt16LittleEndian(bs, n);
	}

	/// <summary>
	///   us the int16 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_LittleEndian(ReadOnlySpan<ushort> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt16_To_LittleEndian(t, bs);
			bs = bs[2..];
		}
	}

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_BigEndian(uint n, Span<byte> bs)
	{
		BinaryPrimitives.WriteUInt32BigEndian(bs, n);
	}

	/// <summary>
	///   us the int32 to big endian high.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt32_To_BigEndian_High(uint n, Span<byte> bs)
	{
		var len = bs.Length;
		Debug.Assert(len is >= 1 and <= 4);

		var pos = 24;
		bs[0] = (byte) (n >> pos);
		for (var i = 1; i < len; ++i)
		{
			pos   -= 8;
			bs[i] =  (byte) (n >> pos);
		}
	}

	/// <summary>
	///   us the int32 to big endian low.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_BigEndian_Low(uint n, Span<byte> bs)
	{
		UInt32_To_BigEndian_High(n << ((4 - bs.Length) << 3), bs);
	}

	/// <summary>
	///   us the int32 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_BigEndian(ReadOnlySpan<uint> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt32_To_BigEndian(t, bs);
			bs = bs[4..];
		}
	}

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_LittleEndian(uint n, Span<byte> bs) => BinaryPrimitives.WriteUInt32LittleEndian(bs, n);

	/// <summary>
	///   us the int32 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_LittleEndian(ReadOnlySpan<uint> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt32_To_LittleEndian(t, bs);
			bs = bs[4..];
		}
	}

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_BigEndian(ulong n, Span<byte> bs) => BinaryPrimitives.WriteUInt64BigEndian(bs, n);

	/// <summary>
	///   us the int64 to big endian high.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	internal static void UInt64_To_BigEndian_High(ulong n, Span<byte> bs)
	{
		var len = bs.Length;
		Debug.Assert(len is >= 1 and <= 8);

		var pos = 56;
		bs[0] = (byte) (n >> pos);
		for (var i = 1; i < len; ++i)
		{
			pos   -= 8;
			bs[i] =  (byte) (n >> pos);
		}
	}

	/// <summary>
	///   us the int64 to big endian low.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_BigEndian_Low(ulong n, Span<byte> bs) => UInt64_To_BigEndian_High(n << ((8 - bs.Length) << 3), bs);

	/// <summary>
	///   us the int64 to big endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_BigEndian(ReadOnlySpan<ulong> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt64_To_BigEndian(t, bs);
			bs = bs[8..];
		}
	}

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="n">The n.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_LittleEndian(ulong n, Span<byte> bs) => BinaryPrimitives.WriteUInt64LittleEndian(bs, n);

	/// <summary>
	///   us the int64 to little endian.
	/// </summary>
	/// <param name="ns">The ns.</param>
	/// <param name="bs">The bs.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_LittleEndian(ReadOnlySpan<ulong> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt64_To_LittleEndian(t, bs);
			bs = bs[8..];
		}
	}
}
