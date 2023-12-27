// perticula - core - DateTimeExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;

namespace core;

/// <summary>
///   Class DateTimeExtensions.
/// </summary>
public static class DateTimeExtensions
{
	/// <summary>
	///   The maximum unix ms
	/// </summary>
	public static readonly long MaxUnixMs =
		(DateTime.MaxValue.Ticks - GetUnixEpoch().Ticks) / TimeSpan.TicksPerMillisecond;

	/// <summary>
	///   The minimum unix ms
	/// </summary>
	public static readonly long MinUnixMs = 0L;

	/// <summary>
	///   Gets the value for yesterday.
	/// </summary>
	/// <value>The yesterday.</value>
	public static DateOnly Yesterday => DateOnly.FromDateTime(DateTime.Today.AddDays(-1d));

	/// <summary>
	///   Returns the list of time zones known to the OS
	///   NOTE: These are subject to change based on where the code runs
	/// </summary>
	/// <value>The known time zones.</value>
	public static IEnumerable<string> KnownTimeZones => TimeZoneInfo.GetSystemTimeZones().Select(tz => tz.Id);

	/// <summary>
	///   Returns the local time zome of the OS
	/// </summary>
	/// <value>The local time zone.</value>
	public static string LocalTimeZone => TimeZoneInfo.Local.Id;

	/// <summary>
	///   Converts a date into a human readable "Time elapsed since" format.
	///   (e.g.) This event happened 2 days ago, etc.
	/// </summary>
	/// <param name="when">The date value.</param>
	/// <param name="justNow">The message to display value an event happened within the past 2 minutes</param>
	/// <param name="preFix">an optional prefix value.</param>
	/// <param name="postFix">an optional post fix value.</param>
	/// <returns>System.String.</returns>
	public static string ToRelativeElapsed(DateTime when, string justNow = "", string preFix = "", string postFix = "") =>
		ToRelativeElapsed(DateTime.Now.Subtract(when), justNow, preFix, postFix);

	/// <summary>
	///   Converts a timespan into a human readable "Time elapsed since" format.
	///   (e.g.) This event happened 2 days ago, etc.
	/// </summary>
	/// <param name="span">The span.</param>
	/// <param name="justNow">The message to display value an event happened within the past 2 minutes</param>
	/// <param name="preFix">an optional prefix value.</param>
	/// <param name="postFix">an optional post fix value.</param>
	/// <returns>System.String.</returns>
	public static string ToRelativeElapsed(TimeSpan span, string justNow = "", string preFix = "", string postFix = "")
	{
		if (span.TotalDays >= 2) return $"{preFix}{(int)span.TotalDays} days{postFix}";
		return span.TotalMinutes switch
		       {
			       > 90 => $"{preFix}{(int)span.TotalHours} hours{postFix}",
			       > 2  => $"{preFix}{(int)span.TotalMinutes} minutes{postFix}",
			       _    => justNow
		       };
	}

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
	/// <exception cref="System.ArgumentOutOfRangeException">toleranceSeconds - Tolerance must be a positive number</exception>
	/// <exception cref="System.ArgumentOutOfRangeException">toleranceSeconds - Tolerance must be less than one minute</exception>
	/// <remarks>
	///   This is very useful for edge cases in unit testing where DateTime.now can differ by periods of 1 second during test
	///   execution.
	///   This would cause an otherwise successful and correct test to fail.
	/// </remarks>
	public static bool CloseEnoughToEqual(DateTime value1, DateTime value2, int toleranceSeconds = 2)
	{
		switch (toleranceSeconds)
		{
			case <= 0:
				throw new ArgumentOutOfRangeException(nameof(toleranceSeconds), toleranceSeconds,
					"Tolerance must be a positive number");
			case >= 60:
				throw new ArgumentOutOfRangeException(nameof(toleranceSeconds), toleranceSeconds,
					"Tolerance must be less than one minute");
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
	///   Converts to unix epoch time in ms.
	/// </summary>
	/// <param name="asOf">As of.</param>
	/// <returns>System.Int64.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">asOf - DateTime value may not be before the epoch</exception>
	public static long ToUnixEpochTimeMs(this DateTime asOf)
	{
		var ue  = GetUnixEpoch();
		var utc = asOf.ToUniversalTime();
		if (utc.CompareTo(ue) < 0)
			throw new ArgumentOutOfRangeException(nameof(asOf), "DateTime value may not be before the epoch");

		return (utc.Ticks - ue.Ticks) / TimeSpan.TicksPerMillisecond;
	}

	/// <summary>
	///   Currents the unix ms.
	/// </summary>
	/// <returns>System.Int64.</returns>
	public static long CurrentUnixMs() => ToUnixEpochTimeMs(DateTime.UtcNow);

	/// <summary>
	///   Converts to date time from unix epoch time in ms.
	/// </summary>
	/// <param name="unixMs">The unix ms.</param>
	/// <returns>DateTime.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">unixMs</exception>
	public static DateTime FromUnixMs(long unixMs)
	{
		if (unixMs < MinUnixMs || unixMs > MaxUnixMs)
			throw new ArgumentOutOfRangeException(nameof(unixMs));

		return new DateTime(unixMs * TimeSpan.TicksPerMillisecond + GetUnixEpoch().Ticks, DateTimeKind.Utc);
	}

	/// <summary>
	///   Gets the ticks.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="modifier">The modifier.</param>
	/// <returns>System.Int64.</returns>
	private static long GetTicks(DateTime value, int? modifier = 0) => value.AddSeconds(modifier ?? 0).Ticks;

	/// <summary>
	///   Withes the precision centisecond.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>DateTime.</returns>
	public static DateTime WithPrecisionCentisecond(DateTime dateTime)
	{
		var ticks = dateTime.Ticks - dateTime.Ticks % (TimeSpan.TicksPerMillisecond * 10L);
		return new DateTime(ticks, dateTime.Kind);
	}

	/// <summary>
	///   Withes the precision decisecond.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>DateTime.</returns>
	public static DateTime WithPrecisionDecisecond(DateTime dateTime)
	{
		var ticks = dateTime.Ticks - dateTime.Ticks % (TimeSpan.TicksPerMillisecond * 100L);
		return new DateTime(ticks, dateTime.Kind);
	}

	/// <summary>
	///   Withes the precision millisecond.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>DateTime.</returns>
	public static DateTime WithPrecisionMillisecond(DateTime dateTime)
	{
		var ticks = dateTime.Ticks - dateTime.Ticks % TimeSpan.TicksPerMillisecond;
		return new DateTime(ticks, dateTime.Kind);
	}

	/// <summary>
	///   Withes the precision second.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>DateTime.</returns>
	public static DateTime WithPrecisionSecond(DateTime dateTime)
	{
		var ticks = dateTime.Ticks - dateTime.Ticks % TimeSpan.TicksPerSecond;
		return new DateTime(ticks, dateTime.Kind);
	}

	/// <summary>
	///   Returns the time zone information based on the provided id,
	///   or returns null if the id is not known
	/// </summary>
	/// <param name="tz">The tz.</param>
	/// <returns>TimeZoneInfo.</returns>
	public static TimeZoneInfo? FindTimeZone(string? tz)
	{
		if (string.IsNullOrEmpty(tz)) return null;
		try
		{
			return TimeZoneInfo.FindSystemTimeZoneById(tz);
		}
		catch (TimeZoneNotFoundException)
		{
			return null;
		}
	}

	/// <summary>
	///   Indicates whether this is a valid time zone id
	/// </summary>
	/// <param name="tz">The tz.</param>
	/// <returns><c>true</c> if [is value time zone] [the specified tz]; otherwise, <c>false</c>.</returns>
	public static bool IsValueTimeZone(string? tz) => FindTimeZone(tz ?? "") != null;

	/// <summary>
	///   Converts from the time in the provided time zone to local time. The caller
	///   should check the timezone provided to ensure it is valid otherwise an
	///   TimeZoneNotFoundException exception may be thrown.
	/// </summary>
	/// <param name="value">The time in that time zone</param>
	/// <param name="timeZone">The name of the time zone</param>
	/// <returns>DateTime.</returns>
	public static DateTime ToLocalTime(DateTime value, string timeZone) => TimeZoneInfo.ConvertTimeFromUtc(
		TimeZoneInfo.ConvertTimeToUtc(value, TimeZoneInfo.FindSystemTimeZoneById(timeZone)), TimeZoneInfo.Local);

	/// <summary>
	///   Converts from the time in the provided time zone to local time. The caller
	///   should check the timezone provided to ensure it is valid otherwise an
	///   TimeZoneNotFoundException exception may be thrown.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.DateTime.</returns>
	public static DateTime ToLocalTime(DateTime value) => TimeZoneInfo.ConvertTimeFromUtc(
		TimeZoneInfo.ConvertTimeToUtc(value, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id)),
		TimeZoneInfo.Local);
}
