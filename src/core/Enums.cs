// perticula - core - Enums.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class Enums.
/// </summary>
public static class Enums
{
	/// <summary>
	///   Gets a specific enum value by string.
	/// </summary>
	/// <typeparam name="TEnum">The type of the t enum.</typeparam>
	/// <param name="s">The s.</param>
	/// <returns>TEnum.</returns>
	/// <exception cref="System.ArgumentException">Could not find enum value {s} - s</exception>
	public static TEnum GetEnumValue<TEnum>(string s) where TEnum : struct, Enum
	{
		switch (s.Length)
		{
			// only parse single named constants
			case > 0 when char.IsLetter(s[0]) && s.IndexOf(',') < 0:
				s = s.Replace('-', '_');
				s = s.Replace('/', '_');

				return Enum.Parse<TEnum>(s, false);
			default:
				throw new ArgumentException($"Could not find enum value {s}", nameof(s));
		}
	}

	/// <summary>
	///   Gets all enum values.
	/// </summary>
	/// <typeparam name="TEnum">The type of the t enum.</typeparam>
	/// <returns>TEnum[].</returns>
	public static TEnum[] GetEnumValues<TEnum>() where TEnum : struct, Enum => Enum.GetValues<TEnum>();

	/// <summary>
	///   Gets an arbitrary value from the set of enums.
	/// </summary>
	/// <typeparam name="TEnum">The type of the t enum.</typeparam>
	/// <returns>TEnum.</returns>
	public static TEnum GetArbitraryValue<TEnum>() where TEnum : struct, Enum
	{
		var values = GetEnumValues<TEnum>();
		var pos    = (int)(DateTimeExtensions.GetUnixEpochTimestamp() & int.MaxValue) % values.Length;
		return values[pos];
	}
}
