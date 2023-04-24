// perticula - core - Spans.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.CompilerServices;

namespace core;

public static class Spans
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void CopyFrom<T>(this Span<T> output, ReadOnlySpan<T> input) => input[..output.Length].CopyTo(output);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Span<T> FromNullable<T>(T[]? array) => array == null ? Span<T>.Empty : array.AsSpan();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Span<T> FromNullable<T>(T[]? array, int start) => array == null ? Span<T>.Empty : array.AsSpan(start);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ReadOnlySpan<T> FromNullableReadOnly<T>(T[]? array) => array == null ? Span<T>.Empty : array.AsSpan();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ReadOnlySpan<T> FromNullableReadOnly<T>(T[]? array, int start) => array == null ? Span<T>.Empty : array.AsSpan(start);
}
