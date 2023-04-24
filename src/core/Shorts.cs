// perticula - core - Shorts.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers.Binary;

namespace core;

public class Shorts
{
	public const int NumBits  = 16;
	public const int NumBytes = 2;

	public static short ReverseBytes(short i) => BinaryPrimitives.ReverseEndianness(i);

	public static ushort ReverseBytes(ushort i) => BinaryPrimitives.ReverseEndianness(i);

	public static short RotateLeft(short i, int distance) => (short) RotateLeft((ushort) i, distance);

	public static ushort RotateLeft(ushort i, int distance) => (ushort) ((i << distance) | (i >> (16 - distance)));

	public static short RotateRight(short i, int distance) => (short) RotateRight((ushort) i, distance);

	public static ushort RotateRight(ushort i, int distance) => (ushort) ((i >> distance) | (i << (16 - distance)));
}
