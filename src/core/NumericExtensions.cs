// perticula - core - NumericExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class NumericExtensions.
/// </summary>
public static class NumericExtensions
{
	/// <summary>
	///   Converts a string to a decimal.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
	public static decimal? ConvertDecimal(this string value) => decimal.TryParse(value, out var d) ? d : null;

	/// <summary>
	///   Converts a string to a  double.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Nullable&lt;System.Double&gt;.</returns>
	public static double? ConvertDouble(this string value) => double.TryParse(value, out var d) ? d : null;

	/// <summary>
	///   Converts a string to an  integer.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
	public static int? ConvertInteger(this string value) => int.TryParse(value, out var d) ? d : null;

	/// <summary>
	///   Compares this number to another number. Returns true if it is greater.
	/// </summary>
	/// <typeparam name="T">The type of number (technically any comparable type).</typeparam>
	/// <param name="me">Me.</param>
	/// <param name="other">The other.</param>
	/// <returns><c>true</c> if [is greater than] [the specified other]; otherwise, <c>false</c>.</returns>
	public static bool IsGreaterThan<T>(this T me, T other) where T : IComparable<T> => me.CompareTo(other) > 0;

	/// <summary>
	///   Compares this number to another number. Returns true if it is less than.
	/// </summary>
	/// <typeparam name="T">The type of number (technically any comparable type).</typeparam>
	/// <param name="me">Me.</param>
	/// <param name="other">The other.</param>
	/// <returns><c>true</c> if [is less than] [the specified other]; otherwise, <c>false</c>.</returns>
	public static bool IsLessThan<T>(this T me, T other) where T : IComparable<T> => me.CompareTo(other) < 0;

	/// <summary>
	///   Returns the number or a maximum value.
	/// </summary>
	/// <typeparam name="T">The type of number (technically any comparable type).</typeparam>
	/// <param name="me">Me.</param>
	/// <param name="max">The maximum.</param>
	/// <returns>T.</returns>
	public static T OrMaximumOf<T>(this T me, T max) where T : IComparable<T> => me.IsGreaterThan(max) ? max : me;

	/// <summary>
	///   Returns the number or a minimum value.
	/// </summary>
	/// <typeparam name="T">The type of number (technically any comparable type).</typeparam>
	/// <param name="me">Me.</param>
	/// <param name="min">The minimum.</param>
	/// <returns>T.</returns>
	public static T OrMinimumOf<T>(this T me, T min) where T : IComparable<T> => me.IsLessThan(min) ? min : me;

	/// <summary>
	///   Returns the number or a minimum or maximum value.
	/// </summary>
	/// <typeparam name="T">The type of number (technically any comparable type).</typeparam>
	/// <param name="me">Me.</param>
	/// <param name="min">The minimum.</param>
	/// <param name="max">The maximum.</param>
	/// <returns>T.</returns>
	public static T OrMinMaxOf<T>(this T me, T min, T max) where T : IComparable<T> =>
		me.OrMinimumOf(min).OrMaximumOf(max);

	/// <summary>
	///   Safes the division.
	/// </summary>
	/// <param name="numerator">The numerator.</param>
	/// <param name="denominator">The denominator.</param>
	/// <returns>System.Decimal.</returns>
	public static decimal SafeDivision(this decimal numerator, decimal denominator) =>
		denominator == 0 ? 0 : numerator / denominator;

	/// <summary>
	///   Determines if the specified int falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this int value, int floor, int ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified uint falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this uint value, uint floor, uint ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified long falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this long value, long floor, long ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified ulong falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this ulong value, ulong floor, ulong ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified short falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this short value, short floor, short ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified ushort falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this ushort value, ushort floor, ushort ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified char falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this char value, char floor, char ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified byte falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this byte value, byte floor, byte ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Determines if the specified sbyte falls between the floor and ceiling values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="floor">The floor.</param>
	/// <param name="ceiling">The ceiling.</param>
	/// <param name="includeBoundary">
	///   if set to <c>true</c> the output is inclusive with boundaries, otherwise boundaries are
	///   excluded.
	/// </param>
	/// <returns><c>true</c> if the number is within range, <c>false</c> otherwise.</returns>
	public static bool WithinRange(this sbyte value, sbyte floor, sbyte ceiling, bool includeBoundary = true) =>
		includeBoundary ? value <= ceiling && value >= floor : value < ceiling && value > floor;

	/// <summary>
	///   Converts a number to a size with the appropriate units
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>System.String.</returns>
	public static string ToSize(long bytes) =>
		bytes switch
		{
			< 1024               => ToSizeBytes(bytes),
			< 1024 * 1024        => ToSizeKib(bytes),
			< 1024 * 1024 * 1024 => ToSizeMib(bytes),
			_                    => ToSizeGib(bytes)
		};

	/// <summary>
	///   Converts a number to a size in bytes (ie 512 = "512 Bytes")
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="units">The units.</param>
	/// <returns>System.String.</returns>
	public static string ToSizeBytes(long bytes, string units = "Bytes") => $"{bytes:#,###,###,###,##0} {units}";

	/// <summary>
	///   Converts a number to a size in gigabytes (ie 1610612736 = "1.5 GB")
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="units">The units.</param>
	/// <returns>System.String.</returns>
	public static string ToSizeGib(long bytes, string units = "GiB") => ToSizeMib(bytes / 1024, units);

	/// <summary>
	///   Converts a number to a size in terrabytes (ie 1610612736 = "1.5 GB")
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="units">The units.</param>
	/// <returns>System.String.</returns>
	public static string ToSizeTib(long bytes, string units = "TiB") => ToSizeGib(bytes / 1024, units);

	/// <summary>
	///   Converts a number to a size in kilobytes (ie 1536 = "1.5 KB")
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="units">The units.</param>
	/// <returns>System.String.</returns>
	public static string ToSizeKib(long bytes, string units = "KiB") => $"{bytes / 1024.0:#,###,###,###,##0.#} {units}";

	/// <summary>
	///   Converts a number to a size in megabytes (ie 1572864 = "1.5 MB")
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="units">The units.</param>
	/// <returns>System.String.</returns>
	public static string ToSizeMib(long bytes, string units = "MiB") => ToSizeKib(bytes / 1024, units);
}
