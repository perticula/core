// perticula - core - Integers.cs
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
///   Class Integers.
/// </summary>
public static class Integers
{
	/// <summary>
	///   The number bits
	/// </summary>
	public const int NumBits = 32;

	/// <summary>
	///   The number bytes
	/// </summary>
	public const int NumBytes = 4;

	/// <summary>
	///   Highests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int HighestOneBit(int i) => (int) HighestOneBit((uint) i);

	/// <summary>
	///   Highests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt32.</returns>
	public static uint HighestOneBit(uint i)
	{
		i |= i >> 1;
		i |= i >> 2;
		i |= i >> 4;
		i |= i >> 8;
		i |= i >> 16;
		return i - (i >> 1);
	}


	/// <summary>
	///   Lowests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int LowestOneBit(int i) => i & -i;

	/// <summary>
	///   Lowests the one bit.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt32.</returns>
	public static uint LowestOneBit(uint i) => (uint) LowestOneBit((int) i);

	/// <summary>
	///   Numbers the of leading zeros.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int NumberOfLeadingZeros(int i) => BitOperations.LeadingZeroCount((uint) i);

	/// <summary>
	///   Numbers the of trailing zeros.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int NumberOfTrailingZeros(int i) => BitOperations.TrailingZeroCount(i);

	/// <summary>
	///   Pops the count.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int PopCount(int i) => PopCount((uint) i);

	/// <summary>
	///   Pops the count.
	/// </summary>
	/// <param name="u">The u.</param>
	/// <returns>System.Int32.</returns>
	public static int PopCount(uint u) => BitOperations.PopCount(u);

	/// <summary>
	///   Reverses the specified i.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int Reverse(int i) => (int) Reverse((uint) i);

	/// <summary>
	///   Reverses the specified i.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt32.</returns>
	public static uint Reverse(uint i)
	{
		i = Bits.BitPermuteStepSimple(i, 0x55555555U, 1);
		i = Bits.BitPermuteStepSimple(i, 0x33333333U, 2);
		i = Bits.BitPermuteStepSimple(i, 0x0F0F0F0FU, 4);
		return ReverseBytes(i);
	}

	/// <summary>
	///   Reverses the bytes.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int32.</returns>
	public static int ReverseBytes(int i) => BinaryPrimitives.ReverseEndianness(i);

	/// <summary>
	///   Reverses the bytes.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt32.</returns>
	public static uint ReverseBytes(uint i) => BinaryPrimitives.ReverseEndianness(i);

	/// <summary>
	///   Rotates the left.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.Int32.</returns>
	public static int RotateLeft(int i, int distance) => (int) BitOperations.RotateLeft((uint) i, distance);

	/// <summary>
	///   Rotates the left.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.UInt32.</returns>
	public static uint RotateLeft(uint i, int distance) => BitOperations.RotateLeft(i, distance);

	/// <summary>
	///   Rotates the right.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.Int32.</returns>
	public static int RotateRight(int i, int distance) => (int) BitOperations.RotateRight((uint) i, distance);

	/// <summary>
	///   Rotates the right.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.UInt32.</returns>
	public static uint RotateRight(uint i, int distance) => BitOperations.RotateRight(i, distance);
}
