// perticula - core - Shorts.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers.Binary;

namespace core;

/// <summary>
///   Class Shorts.
/// </summary>
public class Shorts
{
	/// <summary>
	///   The number bits
	/// </summary>
	public const int NumBits = 16;

	/// <summary>
	///   The number bytes
	/// </summary>
	public const int NumBytes = 2;

	/// <summary>
	///   Reverses the bytes.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.Int16.</returns>
	public static short ReverseBytes(short i) => BinaryPrimitives.ReverseEndianness(i);

	/// <summary>
	///   Reverses the bytes.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <returns>System.UInt16.</returns>
	public static ushort ReverseBytes(ushort i) => BinaryPrimitives.ReverseEndianness(i);

	/// <summary>
	///   Rotates the left.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.Int16.</returns>
	public static short RotateLeft(short i, int distance) => (short)RotateLeft((ushort)i, distance);

	/// <summary>
	///   Rotates the left.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.UInt16.</returns>
	public static ushort RotateLeft(ushort i, int distance) => (ushort)((i << distance) | (i >> (16 - distance)));

	/// <summary>
	///   Rotates the right.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.Int16.</returns>
	public static short RotateRight(short i, int distance) => (short)RotateRight((ushort)i, distance);

	/// <summary>
	///   Rotates the right.
	/// </summary>
	/// <param name="i">The i.</param>
	/// <param name="distance">The distance.</param>
	/// <returns>System.UInt16.</returns>
	public static ushort RotateRight(ushort i, int distance) => (ushort)((i >> distance) | (i << (16 - distance)));
}
