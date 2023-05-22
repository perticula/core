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

/// <summary>
///   Class Longs.
/// </summary>
public class Longs
{
	/// <summary>
	///   The number bits
	/// </summary>
	public const int NumBits = 64;

	/// <summary>
	///   The number bytes
	/// </summary>
	public const int NumBytes = 8;

	/// <summary>
	///   Highests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int64.</returns>
	public static long HighestOneBit(long i) => (long) HighestOneBit((ulong) i);

	/// <summary>
	///   Highests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt64.</returns>
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

	/// <summary>
	///   Lowests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int64.</returns>
	public static long LowestOneBit(long i) => i & -i;

	/// <summary>
	///   Lowests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt64.</returns>
	public static ulong LowestOneBit(ulong i) => (ulong) LowestOneBit((long) i);

	/// <summary>
	///   Numbers the of leading zeros.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int NumberOfLeadingZeros(long i) => BitOperations.LeadingZeroCount((ulong) i);

	/// <summary>
	///   Numbers the of trailing zeros.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int NumberOfTrailingZeros(long i) => BitOperations.TrailingZeroCount((ulong) i);

	/// <summary>
	///   Reverses the specified i.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int64.</returns>
	public static long Reverse(long i) => (long) Reverse((ulong) i);

	/// <summary>
	///   Reverses the specified i.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt64.</returns>
	public static ulong Reverse(ulong i)
	{
		i = Bits.BitPermuteStepSimple(i, 0x5555555555555555UL, 1);
		i = Bits.BitPermuteStepSimple(i, 0x3333333333333333UL, 2);
		i = Bits.BitPermuteStepSimple(i, 0x0F0F0F0F0F0F0F0FUL, 4);
		return ReverseBytes(i);
	}

	/// <summary>
	///   Reverses the bytes.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int64.</returns>
	public static long ReverseBytes(long i) => BinaryPrimitives.ReverseEndianness(i);

	/// <summary>
	///   Reverses the bytes.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt64.</returns>
	public static ulong ReverseBytes(ulong i) => BinaryPrimitives.ReverseEndianness(i);

	/// <summary>
	///   Rotates the left.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.Int64.</returns>
	public static long RotateLeft(long i, int distance) => (long) BitOperations.RotateLeft((ulong) i, distance);

	/// <summary>
	///   Rotates the left.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.UInt64.</returns>
	public static ulong RotateLeft(ulong i, int distance) => BitOperations.RotateLeft(i, distance);

	/// <summary>
	///   Rotates the right.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.Int64.</returns>
	public static long RotateRight(long i, int distance) => (long) BitOperations.RotateRight((ulong) i, distance);

	/// <summary>
	///   Rotates the right.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.UInt64.</returns>
	public static ulong RotateRight(ulong i, int distance) => BitOperations.RotateRight(i, distance);
}
