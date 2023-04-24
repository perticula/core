// perticula - core - Integers.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers.Binary;
using System.Numerics;

namespace core;

public static class Integers
{
	public const int NumBits  = 32;
	public const int NumBytes = 4;

	public static int HighestOneBit(int i) => (int) HighestOneBit((uint) i);

	public static uint HighestOneBit(uint i)
	{
		i |= i >> 1;
		i |= i >> 2;
		i |= i >> 4;
		i |= i >> 8;
		i |= i >> 16;
		return i - (i >> 1);
	}


	public static int LowestOneBit(int i) => i & -i;

	public static uint LowestOneBit(uint i) => (uint) LowestOneBit((int) i);

	public static int NumberOfLeadingZeros(int  i) => BitOperations.LeadingZeroCount((uint) i);
	public static int NumberOfTrailingZeros(int i) => BitOperations.TrailingZeroCount(i);

	public static int PopCount(int  i) => PopCount((uint) i);
	public static int PopCount(uint u) => BitOperations.PopCount(u);

	public static int Reverse(int i) => (int) Reverse((uint) i);

	public static uint Reverse(uint i)
	{
		i = Bits.BitPermuteStepSimple(i, 0x55555555U, 1);
		i = Bits.BitPermuteStepSimple(i, 0x33333333U, 2);
		i = Bits.BitPermuteStepSimple(i, 0x0F0F0F0FU, 4);
		return ReverseBytes(i);
	}

	public static int  ReverseBytes(int  i) => BinaryPrimitives.ReverseEndianness(i);
	public static uint ReverseBytes(uint i) => BinaryPrimitives.ReverseEndianness(i);

	public static int  RotateLeft(int   i, int distance) => (int) BitOperations.RotateLeft((uint) i, distance);
	public static uint RotateLeft(uint  i, int distance) => BitOperations.RotateLeft(i, distance);
	public static int  RotateRight(int  i, int distance) => (int) BitOperations.RotateRight((uint) i, distance);
	public static uint RotateRight(uint i, int distance) => BitOperations.RotateRight(i, distance);
}
