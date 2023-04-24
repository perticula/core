// perticula - core - Longs.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers.Binary;
using System.Numerics;
using core.Math.Raw;

namespace core;

public class Longs
{
	public const int NumBits  = 64;
	public const int NumBytes = 8;

	public static long HighestOneBit(long i) => (long) HighestOneBit((ulong) i);

	public static ulong HighestOneBit(ulong i)
	{
		i |= i >> 1;
		i |= i >> 2;
		i |= i >> 4;
		i |= i >> 8;
		i |= i >> 16;
		i |= i >> 32;
		return i - (i >> 1);
	}

	public static long LowestOneBit(long i) => i & -i;

	public static ulong LowestOneBit(ulong i) => (ulong) LowestOneBit((long) i);

	public static int NumberOfLeadingZeros(long i) => BitOperations.LeadingZeroCount((ulong) i);

	public static int NumberOfTrailingZeros(long i) => BitOperations.TrailingZeroCount((ulong) i);

	public static long Reverse(long i) => (long) Reverse((ulong) i);

	public static ulong Reverse(ulong i)
	{
		i = Bits.BitPermuteStepSimple(i, 0x5555555555555555UL, 1);
		i = Bits.BitPermuteStepSimple(i, 0x3333333333333333UL, 2);
		i = Bits.BitPermuteStepSimple(i, 0x0F0F0F0F0F0F0F0FUL, 4);
		return ReverseBytes(i);
	}

	public static long ReverseBytes(long i) => BinaryPrimitives.ReverseEndianness(i);

	public static ulong ReverseBytes(ulong i) => BinaryPrimitives.ReverseEndianness(i);

	public static long RotateLeft(long i, int distance) => (long) BitOperations.RotateLeft((ulong) i, distance);

	public static ulong RotateLeft(ulong i, int distance) => BitOperations.RotateLeft(i, distance);

	public static long RotateRight(long i, int distance) => (long) BitOperations.RotateRight((ulong) i, distance);

	public static ulong RotateRight(ulong i, int distance) => BitOperations.RotateRight(i, distance);
}
