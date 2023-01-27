// perticula - core - DateTimeExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;

namespace core;

public static class DateTimeExtensions
{
	/// <summary>
	///   Gets the value for yesterday.
	/// </summary>
	/// <value>The yesterday.</value>
	public static DateOnly Yesterday => DateOnly.FromDateTime(DateTime.Today.AddDays(-1d));

	/// <summary>
	///   Returns this date, but set the time to midnight 23:59:59
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>DateTime.</returns>
	public static DateTime AtEndOfDay(this DateTime date) => new(date.Year, date.Month, date.Day, 23, 59, 59);

	/// <summary>
	///   Returns this date, but set the time to midnight 23:59:59
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>System.Nullable&lt;DateTime&gt;.</returns>
	public static DateTime? AtEndOfDay(this DateTime? date) => date?.AtEndOfDay();

	/// <summary>
	///   Returns this date, but set the time to midnight 00:00
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>DateTime.</returns>
	public static DateTime AtStartOfDay(this DateTime date) => new(date.Year, date.Month, date.Day, 0, 0, 0);

	/// <summary>
	///   Returns this date, but set the time to midnight 00:00
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>System.Nullable&lt;DateTime&gt;.</returns>
	public static DateTime? AtStartOfDay(this DateTime? date) => date?.AtStartOfDay();

	/// <summary>
	///   Determines if the specified <see cref="DateTime" /> object are close the enough to one another to satisfy equality
	///   for the given operations.
	/// </summary>
	/// <param name="value1">The value1.</param>
	/// <param name="value2">The value2.</param>
	/// <param name="toleranceSeconds">The tolerance value (in seconds).</param>
	/// <returns><c>true</c> if the dates match within the allowed tolerance, <c>false</c> otherwise.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	///   toleranceSeconds - Tolerance must be a positive number
	///   or
	///   toleranceSeconds - Tolerance must be less than one minute
	/// </exception>
	/// <exception cref="System.ArgumentOutOfRangeException">
	///   Tolerance must be a positive number
	///   or
	///   Tolerance must be less than one minute
	/// </exception>
	/// <remarks>
	///   This is very useful for edge cases in unit testing where DateTime.now can differ by periods of 1 second during test
	///   execution.
	///   This would cause an otherwise successful and correct test to fail.
	/// </remarks>
	public static bool CloseEnoughToEqual(DateTime value1, DateTime value2, int toleranceSeconds = 2)
	{
		switch (toleranceSeconds)
		{
			case <= 0:  throw new ArgumentOutOfRangeException(nameof(toleranceSeconds), toleranceSeconds, "Tolerance must be a positive number");
			case >= 60: throw new ArgumentOutOfRangeException(nameof(toleranceSeconds), toleranceSeconds, "Tolerance must be less than one minute");
		}

		if (value1.Ticks == value2.Ticks) return true;
		if (value1.Ticks.WithinRange(GetTicks(value2, -toleranceSeconds), GetTicks(value2, toleranceSeconds))) return true;
		// ReSharper disable once ConvertIfStatementToReturnStatement
		if (value2.Ticks.WithinRange(GetTicks(value1, -toleranceSeconds), GetTicks(value1, toleranceSeconds))) return true;

		return false;
	}

	/// <summary>
	///   Enumerates over the days between a range.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <param name="end">The end.</param>
	/// <returns>System.Collections.Generic.IEnumerable&lt;System.DateTime&gt;.</returns>
	public static IEnumerable<DateTime> EachDay(this DateTime start, DateTime end)
	{
		for (var date = start; date.Date <= end.Date; date = date.AddDays(1))
			yield return date;
	}

	/// <summary>
	///   Gets the unix epoch.
	/// </summary>
	/// <returns>DateTime.</returns>
	public static DateTime GetUnixEpoch() => new(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);

	/// <summary>
	///   Gets the unix epoch time stamp.
	/// </summary>
	/// <param name="asOf">As of.</param>
	/// <returns>System.UInt64.</returns>
	public static ulong GetUnixEpochTimestamp(DateTime? asOf = null)
	{
		var time     = asOf ?? new DateTime();
		var timeSpan = time - GetUnixEpoch();
		return Convert.ToUInt64(timeSpan.TotalSeconds);
	}

	/// <summary>
	///   Gets a hashed value for the specified DateTime.
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>System.String.</returns>
	public static string HashedDate(this DateTime date)
	{
		var dateString = date.ToString("d", CultureInfo.InvariantCulture);
		var raw        = dateString.Hash();
		return raw.FilterCharacters("/", "=", "?", "&", "+", "@"); //strip out certain char values
	}

	/// <summary>
	///   Formats the DateTime value as an Iso8601 string.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string ToIso8601(this DateTime value) => value.ToString("O");

	/// <summary>
	///   formats the date with a SQL style string as date
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string ToSqlDate(this DateTime value) => value.ToString("yyyy-MM-dd");

	/// <summary>
	///   formats the date with a SQL style string as date/time
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string ToSqlDateTime(this DateTime value) => value.ToString("yyyy-MM-dd HH:mm:ss");

	/// <summary>
	///   Gets the unix epoch time stamp.
	/// </summary>
	/// <param name="asOf">As of.</param>
	/// <returns>System.UInt64.</returns>
	public static ulong ToUnixEpochTime(this DateTime asOf)
	{
		var timeSpan = asOf - GetUnixEpoch();
		return Convert.ToUInt64(timeSpan.TotalSeconds);
	}

	/// <summary>
	///   Gets the ticks.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="modifier">The modifier.</param>
	/// <returns>System.Int64.</returns>
	private static long GetTicks(DateTime value, int? modifier = 0) => value.AddSeconds(modifier ?? 0).Ticks;
}
