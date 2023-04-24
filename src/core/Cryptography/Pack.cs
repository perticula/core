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

public static class Pack
{
	internal static void UInt16_To_BigEndian(ushort n, byte[] bs) => BinaryPrimitives.WriteUInt16BigEndian(bs, n);

	internal static void UInt16_To_BigEndian(ushort n, byte[] bs, int off) => BinaryPrimitives.WriteUInt16BigEndian(bs.AsSpan(off), n);

	internal static void UInt16_To_BigEndian(ushort[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt16_To_BigEndian(t, bs, off);
			off += 2;
		}
	}

	internal static void UInt16_To_BigEndian(ushort[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt16_To_BigEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 2;
		}
	}

	internal static byte[] UInt16_To_BigEndian(ushort n)
	{
		var bs = new byte[2];
		UInt16_To_BigEndian(n, bs, 0);
		return bs;
	}

	internal static byte[] UInt16_To_BigEndian(ushort[] ns) => UInt16_To_BigEndian(ns, 0, ns.Length);

	internal static byte[] UInt16_To_BigEndian(ushort[] ns, int nsOff, int nsLen)
	{
		var bs = new byte[2 * nsLen];
		UInt16_To_BigEndian(ns, nsOff, nsLen, bs, 0);
		return bs;
	}

	internal static ushort BigEndian_To_UInt16(byte[] bs, int off) => BinaryPrimitives.ReadUInt16BigEndian(bs.AsSpan(off));

	internal static void BigEndian_To_UInt16(byte[] bs, int bsOff, ushort[] ns, int nsOff) => ns[nsOff] = BigEndian_To_UInt16(bs, bsOff);

	internal static ushort[] BigEndian_To_UInt16(byte[] bs) => BigEndian_To_UInt16(bs, 0, bs.Length);

	internal static ushort[] BigEndian_To_UInt16(byte[] bs, int off, int len)
	{
		if ((len & 1) != 0)
			throw new ArgumentException("must be a multiple of 2", nameof(len));

		var ns = new ushort[len / 2];
		for (var i = 0; i < len; i += 2) BigEndian_To_UInt16(bs, off + i, ns, i >> 1);
		return ns;
	}

	internal static void UInt24_To_BigEndian(uint n, byte[] bs)
	{
		bs[0] = (byte) (n >> 16);
		bs[1] = (byte) (n >> 8);
		bs[2] = (byte) n;
	}

	internal static void UInt24_To_BigEndian(uint n, byte[] bs, int off)
	{
		bs[off + 0] = (byte) (n >> 16);
		bs[off + 1] = (byte) (n >> 8);
		bs[off + 2] = (byte) n;
	}

	internal static uint BigEndian_To_UInt24(byte[] bs) =>
		((uint) bs[0]   << 16)
		| ((uint) bs[1] << 8)
		| bs[2];

	internal static uint BigEndian_To_UInt24(byte[] bs, int off) =>
		((uint) bs[off]       << 16)
		| ((uint) bs[off + 1] << 8)
		| bs[off + 2];

	internal static void UInt32_To_BigEndian(uint n, byte[] bs) => BinaryPrimitives.WriteUInt32BigEndian(bs, n);

	internal static void UInt32_To_BigEndian(uint n, byte[] bs, int off) => BinaryPrimitives.WriteUInt32BigEndian(bs.AsSpan(off), n);

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

	internal static void UInt32_To_BigEndian_Low(uint n, byte[] bs, int off, int len) => UInt32_To_BigEndian_High(n << ((4 - len) << 3), bs, off, len);

	internal static void UInt32_To_BigEndian(uint[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt32_To_BigEndian(t, bs, off);
			off += 4;
		}
	}

	internal static void UInt32_To_BigEndian(uint[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt32_To_BigEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 4;
		}
	}

	internal static byte[] UInt32_To_BigEndian(uint n)
	{
		var bs = new byte[4];
		UInt32_To_BigEndian(n, bs, 0);
		return bs;
	}

	internal static byte[] UInt32_To_BigEndian(uint[] ns)
	{
		var bs = new byte[4 * ns.Length];
		UInt32_To_BigEndian(ns, bs, 0);
		return bs;
	}

	internal static uint BigEndian_To_UInt32(byte[] bs) => BinaryPrimitives.ReadUInt32BigEndian(bs);

	internal static uint BigEndian_To_UInt32(byte[] bs, int off) => BinaryPrimitives.ReadUInt32BigEndian(bs.AsSpan(off));

	internal static uint BigEndian_To_UInt32_High(byte[] bs, int off, int len) => BigEndian_To_UInt32_Low(bs, off, len) << ((4 - len) << 3);

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

	internal static void BigEndian_To_UInt32(byte[] bs, int off, uint[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  BigEndian_To_UInt32(bs, off);
			off   += 4;
		}
	}

	internal static void BigEndian_To_UInt32(byte[] bs, int bsOff, uint[] ns, int nsOff, int nsLen)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			ns[nsOff + i] =  BigEndian_To_UInt32(bs, bsOff);
			bsOff         += 4;
		}
	}

	internal static byte[] UInt64_To_BigEndian(ulong n)
	{
		var bs = new byte[8];
		UInt64_To_BigEndian(n, bs, 0);
		return bs;
	}

	internal static void UInt64_To_BigEndian(ulong n, byte[] bs) => BinaryPrimitives.WriteUInt64BigEndian(bs, n);

	internal static void UInt64_To_BigEndian(ulong n, byte[] bs, int off) => BinaryPrimitives.WriteUInt64BigEndian(bs.AsSpan(off), n);

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

	internal static void UInt64_To_BigEndian_Low(ulong n, byte[] bs, int off, int len) => UInt64_To_BigEndian_High(n << ((8 - len) << 3), bs, off, len);

	internal static byte[] UInt64_To_BigEndian(ulong[] ns)
	{
		var bs = new byte[8 * ns.Length];
		UInt64_To_BigEndian(ns, bs, 0);
		return bs;
	}

	internal static void UInt64_To_BigEndian(ulong[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt64_To_BigEndian(t, bs, off);
			off += 8;
		}
	}

	internal static void UInt64_To_BigEndian(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt64_To_BigEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 8;
		}
	}

	internal static ulong BigEndian_To_UInt64(byte[] bs) => BinaryPrimitives.ReadUInt64BigEndian(bs);

	internal static ulong BigEndian_To_UInt64(byte[] bs, int off) => BinaryPrimitives.ReadUInt64BigEndian(bs.AsSpan(off));

	internal static ulong BigEndian_To_UInt64_High(byte[] bs, int off, int len) => BigEndian_To_UInt64_Low(bs, off, len) << ((8 - len) << 3);

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

	internal static void BigEndian_To_UInt64(byte[] bs, int off, ulong[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  BigEndian_To_UInt64(bs, off);
			off   += 8;
		}
	}

	internal static void BigEndian_To_UInt64(byte[] bs, int bsOff, ulong[] ns, int nsOff, int nsLen)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			ns[nsOff + i] =  BigEndian_To_UInt64(bs, bsOff);
			bsOff         += 8;
		}
	}

	internal static void UInt16_To_LittleEndian(ushort n, byte[] bs) => BinaryPrimitives.WriteUInt16LittleEndian(bs, n);

	internal static void UInt16_To_LittleEndian(ushort n, byte[] bs, int off) => BinaryPrimitives.WriteUInt16LittleEndian(bs.AsSpan(off), n);

	internal static byte[] UInt16_To_LittleEndian(ushort n)
	{
		var bs = new byte[2];
		UInt16_To_LittleEndian(n, bs, 0);
		return bs;
	}

	internal static byte[] UInt16_To_LittleEndian(ushort[] ns)
	{
		var bs = new byte[2 * ns.Length];
		UInt16_To_LittleEndian(ns, bs, 0);
		return bs;
	}

	internal static void UInt16_To_LittleEndian(ushort[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt16_To_LittleEndian(t, bs, off);
			off += 2;
		}
	}

	internal static void UInt16_To_LittleEndian(ushort[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt16_To_LittleEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 2;
		}
	}

	internal static ushort LittleEndian_To_UInt16(byte[] bs) => BinaryPrimitives.ReadUInt16LittleEndian(bs);

	internal static ushort LittleEndian_To_UInt16(byte[] bs, int off) => BinaryPrimitives.ReadUInt16LittleEndian(bs.AsSpan(off));

	internal static void LittleEndian_To_UInt16(byte[] bs, int off, ushort[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  LittleEndian_To_UInt16(bs, off);
			off   += 2;
		}
	}

	internal static void LittleEndian_To_UInt16(byte[] bs, int bOff, ushort[] ns, int nOff, int count)
	{
		for (var i = 0; i < count; ++i)
		{
			ns[nOff + i] =  LittleEndian_To_UInt16(bs, bOff);
			bOff         += 2;
		}
	}

	internal static ushort[] LittleEndian_To_UInt16(byte[] bs, int off, int count)
	{
		var ns = new ushort[count];
		LittleEndian_To_UInt16(bs, off, ns);
		return ns;
	}

	internal static byte[] UInt32_To_LittleEndian(uint n)
	{
		var bs = new byte[4];
		UInt32_To_LittleEndian(n, bs, 0);
		return bs;
	}

	internal static void UInt32_To_LittleEndian(uint n, byte[] bs) => BinaryPrimitives.WriteUInt32LittleEndian(bs, n);

	internal static void UInt32_To_LittleEndian(uint n, byte[] bs, int off) => BinaryPrimitives.WriteUInt32LittleEndian(bs.AsSpan(off), n);

	internal static byte[] UInt32_To_LittleEndian(uint[] ns)
	{
		var bs = new byte[4 * ns.Length];
		UInt32_To_LittleEndian(ns, bs, 0);
		return bs;
	}

	internal static void UInt32_To_LittleEndian(uint[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt32_To_LittleEndian(t, bs, off);
			off += 4;
		}
	}

	internal static void UInt32_To_LittleEndian(uint[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt32_To_LittleEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 4;
		}
	}

	internal static uint LittleEndian_To_UInt24(byte[] bs, int off) =>
		bs[off]
		| ((uint) bs[off + 1] << 8)
		| ((uint) bs[off + 2] << 16);

	internal static uint LittleEndian_To_UInt32(byte[] bs) => BinaryPrimitives.ReadUInt32LittleEndian(bs);

	internal static uint LittleEndian_To_UInt32(byte[] bs, int off) => BinaryPrimitives.ReadUInt32LittleEndian(bs.AsSpan(off));

	internal static uint LittleEndian_To_UInt32_High(byte[] bs, int off, int len) => LittleEndian_To_UInt32_Low(bs, off, len) << ((4 - len) << 3);

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

	internal static void LittleEndian_To_UInt32(byte[] bs, int off, uint[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  LittleEndian_To_UInt32(bs, off);
			off   += 4;
		}
	}

	internal static void LittleEndian_To_UInt32(byte[] bs, int bOff, uint[] ns, int nOff, int count)
	{
		for (var i = 0; i < count; ++i)
		{
			ns[nOff + i] =  LittleEndian_To_UInt32(bs, bOff);
			bOff         += 4;
		}
	}

	internal static uint[] LittleEndian_To_UInt32(byte[] bs, int off, int count)
	{
		var ns = new uint[count];
		LittleEndian_To_UInt32(bs, off, ns);
		return ns;
	}

	internal static byte[] UInt64_To_LittleEndian(ulong n)
	{
		var bs = new byte[8];
		UInt64_To_LittleEndian(n, bs, 0);
		return bs;
	}

	internal static void UInt64_To_LittleEndian(ulong n, byte[] bs) => BinaryPrimitives.WriteUInt64LittleEndian(bs, n);

	internal static void UInt64_To_LittleEndian(ulong n, byte[] bs, int off) => BinaryPrimitives.WriteUInt64LittleEndian(bs.AsSpan(off), n);

	internal static byte[] UInt64_To_LittleEndian(ulong[] ns)
	{
		var bs = new byte[8 * ns.Length];
		UInt64_To_LittleEndian(ns, bs, 0);
		return bs;
	}

	internal static void UInt64_To_LittleEndian(ulong[] ns, byte[] bs, int off)
	{
		foreach (var t in ns)
		{
			UInt64_To_LittleEndian(t, bs, off);
			off += 8;
		}
	}

	internal static void UInt64_To_LittleEndian(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			UInt64_To_LittleEndian(ns[nsOff + i], bs, bsOff);
			bsOff += 8;
		}
	}

	internal static ulong LittleEndian_To_UInt64(byte[] bs) => BinaryPrimitives.ReadUInt64LittleEndian(bs);

	internal static ulong LittleEndian_To_UInt64(byte[] bs, int off) => BinaryPrimitives.ReadUInt64LittleEndian(bs.AsSpan(off));

	internal static void LittleEndian_To_UInt64(byte[] bs, int off, ulong[] ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] =  LittleEndian_To_UInt64(bs, off);
			off   += 8;
		}
	}

	internal static void LittleEndian_To_UInt64(byte[] bs, int bsOff, ulong[] ns, int nsOff, int nsLen)
	{
		for (var i = 0; i < nsLen; ++i)
		{
			ns[nsOff + i] =  LittleEndian_To_UInt64(bs, bsOff);
			bsOff         += 8;
		}
	}

	internal static ulong[] LittleEndian_To_UInt64(byte[] bs, int off, int count)
	{
		var ns = new ulong[count];
		LittleEndian_To_UInt64(bs, off, ns);
		return ns;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BigEndian_To_UInt32(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt32BigEndian(bs);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BigEndian_To_UInt32(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt32BigEndian(bs[off..]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void BigEndian_To_UInt32(ReadOnlySpan<byte> bs, Span<uint> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = BigEndian_To_UInt32(bs);
			bs    = bs[4..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BigEndian_To_UInt32_High(ReadOnlySpan<byte> bs) => BigEndian_To_UInt32_Low(bs) << ((4 - bs.Length) << 3);

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BigEndian_To_UInt64(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt64BigEndian(bs);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BigEndian_To_UInt64(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt64BigEndian(bs[off..]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void BigEndian_To_UInt64(ReadOnlySpan<byte> bs, Span<ulong> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = BigEndian_To_UInt64(bs);
			bs    = bs[8..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BigEndian_To_UInt64_High(ReadOnlySpan<byte> bs) => BigEndian_To_UInt64_Low(bs) << ((8 - bs.Length) << 3);

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ushort LittleEndian_To_UInt16(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt16LittleEndian(bs);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LittleEndian_To_UInt16(ReadOnlySpan<byte> bs, Span<ushort> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = LittleEndian_To_UInt16(bs);
			bs    = bs[2..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint LittleEndian_To_UInt32(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt32LittleEndian(bs);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint LittleEndian_To_UInt32(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt32LittleEndian(bs[off..]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LittleEndian_To_UInt32(ReadOnlySpan<byte> bs, Span<uint> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = LittleEndian_To_UInt32(bs);
			bs    = bs[4..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint LittleEndian_To_UInt32_High(ReadOnlySpan<byte> bs) => LittleEndian_To_UInt32_Low(bs) << ((4 - bs.Length) << 3);

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong LittleEndian_To_UInt64(ReadOnlySpan<byte> bs) => BinaryPrimitives.ReadUInt64LittleEndian(bs);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong LittleEndian_To_UInt64(ReadOnlySpan<byte> bs, int off) => BinaryPrimitives.ReadUInt64LittleEndian(bs[off..]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void LittleEndian_To_UInt64(ReadOnlySpan<byte> bs, Span<ulong> ns)
	{
		for (var i = 0; i < ns.Length; ++i)
		{
			ns[i] = LittleEndian_To_UInt64(bs);
			bs    = bs[8..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_BigEndian(ushort n, Span<byte> bs)
	{
		BinaryPrimitives.WriteUInt16BigEndian(bs, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_BigEndian(ReadOnlySpan<ushort> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt16_To_BigEndian(t, bs);
			bs = bs[2..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_LittleEndian(ushort n, Span<byte> bs)
	{
		BinaryPrimitives.WriteUInt16LittleEndian(bs, n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt16_To_LittleEndian(ReadOnlySpan<ushort> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt16_To_LittleEndian(t, bs);
			bs = bs[2..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_BigEndian(uint n, Span<byte> bs)
	{
		BinaryPrimitives.WriteUInt32BigEndian(bs, n);
	}

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_BigEndian_Low(uint n, Span<byte> bs)
	{
		UInt32_To_BigEndian_High(n << ((4 - bs.Length) << 3), bs);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_BigEndian(ReadOnlySpan<uint> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt32_To_BigEndian(t, bs);
			bs = bs[4..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_LittleEndian(uint n, Span<byte> bs) => BinaryPrimitives.WriteUInt32LittleEndian(bs, n);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt32_To_LittleEndian(ReadOnlySpan<uint> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt32_To_LittleEndian(t, bs);
			bs = bs[4..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_BigEndian(ulong n, Span<byte> bs) => BinaryPrimitives.WriteUInt64BigEndian(bs, n);

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_BigEndian_Low(ulong n, Span<byte> bs) => UInt64_To_BigEndian_High(n << ((8 - bs.Length) << 3), bs);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_BigEndian(ReadOnlySpan<ulong> ns, Span<byte> bs)
	{
		foreach (var t in ns)
		{
			UInt64_To_BigEndian(t, bs);
			bs = bs[8..];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void UInt64_To_LittleEndian(ulong n, Span<byte> bs) => BinaryPrimitives.WriteUInt64LittleEndian(bs, n);

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
