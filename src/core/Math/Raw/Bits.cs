// perticula - core - Bits.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace core.Math.Raw;

/// <summary>
///   Class Bits.
/// </summary>
public static class Bits
{
	/// <summary>
	///   Bits the permute step.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="m">The m.</param>
	/// <param name="s">The s.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BitPermuteStep(uint x, uint m, int s)
	{
		Debug.Assert((m & (m << s)) == 0U);
		Debug.Assert((m << s) >> s  == m);

		var t = (x ^ (x >> s)) & m;
		return t ^ (t << s) ^ x;
	}

	/// <summary>
	///   Bits the permute step.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="m">The m.</param>
	/// <param name="s">The s.</param>
	/// <returns>System.UInt64.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BitPermuteStep(ulong x, ulong m, int s)
	{
		Debug.Assert((m & (m << s)) == 0UL);
		Debug.Assert((m << s) >> s  == m);

		var t = (x ^ (x >> s)) & m;
		return t ^ (t << s) ^ x;
	}

	/// <summary>
	///   Bits the permute step2.
	/// </summary>
	/// <param name="hi">The hi.</param>
	/// <param name="lo">The lo.</param>
	/// <param name="m">The m.</param>
	/// <param name="s">The s.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void BitPermuteStep2(ref uint hi, ref uint lo, uint m, int s)
	{
		Debug.Assert(!Unsafe.AreSame(ref hi, ref lo) || (m & (m << s)) == 0U);
		Debug.Assert((m << s) >> s == m);

		var t = ((lo >> s) ^ hi) & m;
		lo ^= t << s;
		hi ^= t;
	}

	/// <summary>
	///   Bits the permute step2.
	/// </summary>
	/// <param name="hi">The hi.</param>
	/// <param name="lo">The lo.</param>
	/// <param name="m">The m.</param>
	/// <param name="s">The s.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void BitPermuteStep2(ref ulong hi, ref ulong lo, ulong m, int s)
	{
		Debug.Assert(!Unsafe.AreSame(ref hi, ref lo) || (m & (m << s)) == 0UL);
		Debug.Assert((m << s) >> s == m);

		var t = ((lo >> s) ^ hi) & m;
		lo ^= t << s;
		hi ^= t;
	}

	/// <summary>
	///   Bits the permute step simple.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="m">The m.</param>
	/// <param name="s">The s.</param>
	/// <returns>System.UInt32.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static uint BitPermuteStepSimple(uint x, uint m, int s)
	{
		Debug.Assert((m & (m << s)) == 0U);
		Debug.Assert((m << s) >> s  == m);

		return ((x & m) << s) | ((x >> s) & m);
	}

	/// <summary>
	///   Bits the permute step simple.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="m">The m.</param>
	/// <param name="s">The s.</param>
	/// <returns>System.UInt64.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ulong BitPermuteStepSimple(ulong x, ulong m, int s)
	{
		Debug.Assert((m & (m << s)) == 0UL);
		Debug.Assert((m << s) >> s  == m);

		return ((x & m) << s) | ((x >> s) & m);
	}
}
