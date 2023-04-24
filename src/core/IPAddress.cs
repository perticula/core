// perticula - core - IPAddress.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class IpAddress.
/// </summary>
public static class IpAddress
{
	/// <summary>
	///   Returns true if ... is valid.
	/// </summary>
	/// <param name="address">The address.</param>
	/// <returns><c>true</c> if the specified address is valid; otherwise, <c>false</c>.</returns>
	public static bool IsValid(string address) => IsValidIPv4(address) || IsValidIPv6(address);

	/// <summary>
	///   Determines whether [is valid with net mask] [the specified address].
	/// </summary>
	/// <param name="address">The address.</param>
	/// <returns><c>true</c> if [is valid with net mask] [the specified address]; otherwise, <c>false</c>.</returns>
	public static bool IsValidWithNetMask(string address) => IsValidIPv4WithNetmask(address) || IsValidIPv6WithNetmask(address);

	/// <summary>
	///   Determines whether [is valid i PV4] [the specified address].
	/// </summary>
	/// <param name="address">The address.</param>
	/// <returns><c>true</c> if [is valid i PV4] [the specified address]; otherwise, <c>false</c>.</returns>
	public static bool IsValidIPv4(string address)
	{
		var length = address.Length;
		if (length is < 7 or > 15) return false;

		var pos = 0;
		for (var octetIndex = 0; octetIndex < 3; ++octetIndex)
		{
			var end = address.IndexOf('.', pos);

			if (!IsParseableIPv4Octet(address, pos, end))
				return false;

			pos = end + 1;
		}

		return IsParseableIPv4Octet(address, pos, length);
	}

	/// <summary>
	///   Determines whether [is valid i PV4 with netmask] [the specified address].
	/// </summary>
	/// <param name="address">The address.</param>
	/// <returns><c>true</c> if [is valid i PV4 with netmask] [the specified address]; otherwise, <c>false</c>.</returns>
	public static bool IsValidIPv4WithNetmask(string address)
	{
		var index = address.IndexOf('/');
		if (index < 1) return false;

		var before = address[..index];
		var after  = address[(index + 1)..];

		return IsValidIPv4(before) && (IsValidIPv4(after) || IsParseableIPv4Mask(after));
	}

	/// <summary>
	///   Determines whether [is valid i PV6] [the specified address].
	/// </summary>
	/// <param name="address">The address.</param>
	/// <returns><c>true</c> if [is valid i PV6] [the specified address]; otherwise, <c>false</c>.</returns>
	public static bool IsValidIPv6(string address)
	{
		if (address.Length == 0) return false;

		if (address[0] != ':' && GetDigitHexadecimal(address, 0) < 0) return false;

		var segmentCount     = 0;
		var temp             = address + ":";
		var doubleColonFound = false;

		int pos = 0, end;
		while (pos < temp.Length && (end = temp.IndexOf(':', pos)) >= pos)
		{
			if (segmentCount == 8) return false;

			if (pos != end)
			{
				var value = temp[pos..end];

				if (end == temp.Length - 1 && value.IndexOf('.') > 0)
				{
					// add an extra one as address covers 2 words.
					if (++segmentCount == 8) return false;

					if (!IsValidIPv4(value)) return false;
				}
				else if (!IsParseableIPv6Segment(temp, pos, end))
				{
					return false;
				}
			}
			else
			{
				if (end != 1 && end != temp.Length - 1 && doubleColonFound) return false;

				doubleColonFound = true;
			}

			pos = end + 1;
			++segmentCount;
		}

		return segmentCount == 8 || doubleColonFound;
	}

	/// <summary>
	///   Determines whether [is valid i PV6 with netmask] [the specified address].
	/// </summary>
	/// <param name="address">The address.</param>
	/// <returns><c>true</c> if [is valid i PV6 with netmask] [the specified address]; otherwise, <c>false</c>.</returns>
	public static bool IsValidIPv6WithNetmask(string address)
	{
		var index = address.IndexOf('/');
		if (index < 1) return false;

		var before = address[..index];
		var after  = address[(index + 1)..];

		return IsValidIPv6(before) && (IsValidIPv6(after) || IsParseableIPv6Mask(after));
	}

	/// <summary>
	///   Determines whether [is parseable i PV4 mask] [the specified s].
	/// </summary>
	/// <param name="s">The s.</param>
	/// <returns><c>true</c> if [is parseable i PV4 mask] [the specified s]; otherwise, <c>false</c>.</returns>
	private static bool IsParseableIPv4Mask(string s) => IsParseableDecimal(s, 0, s.Length, 2, false, 0, 32);

	/// <summary>
	///   Determines whether [is parseable i PV4 octet] [the specified s].
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="pos">The position.</param>
	/// <param name="end">The end.</param>
	/// <returns><c>true</c> if [is parseable i PV4 octet] [the specified s]; otherwise, <c>false</c>.</returns>
	private static bool IsParseableIPv4Octet(string s, int pos, int end) => IsParseableDecimal(s, pos, end, 3, true, 0, 255);

	/// <summary>
	///   Determines whether [is parseable i PV6 mask] [the specified s].
	/// </summary>
	/// <param name="s">The s.</param>
	/// <returns><c>true</c> if [is parseable i PV6 mask] [the specified s]; otherwise, <c>false</c>.</returns>
	private static bool IsParseableIPv6Mask(string s) => IsParseableDecimal(s, 0, s.Length, 3, false, 1, 128);

	/// <summary>
	///   Determines whether [is parseable i PV6 segment] [the specified s].
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="pos">The position.</param>
	/// <param name="end">The end.</param>
	/// <returns><c>true</c> if [is parseable i PV6 segment] [the specified s]; otherwise, <c>false</c>.</returns>
	private static bool IsParseableIPv6Segment(string s, int pos, int end) => IsParseableHexadecimal(s, pos, end, 4, true, 0x0000, 0xFFFF);

	/// <summary>
	///   Determines whether [is parseable decimal] [the specified s].
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="pos">The position.</param>
	/// <param name="end">The end.</param>
	/// <param name="maxLength">The maximum length.</param>
	/// <param name="allowLeadingZero">if set to <c>true</c> [allow leading zero].</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns><c>true</c> if [is parseable decimal] [the specified s]; otherwise, <c>false</c>.</returns>
	private static bool IsParseableDecimal(string s, int pos, int end, int maxLength, bool allowLeadingZero, int minValue, int maxValue)
	{
		var length = end - pos;
		if ((length < 1) | (length > maxLength)) return false;

		var checkLeadingZero = (length > 1) & !allowLeadingZero;
		if (checkLeadingZero && s[pos] == '0') return false;

		var value = 0;
		while (pos < end)
		{
			var d = GetDigitDecimal(s, pos++);
			if (d < 0) return false;

			value *= 10;
			value += d;
		}

		return (value >= minValue) & (value <= maxValue);
	}

	/// <summary>
	///   Determines whether [is parseable hexadecimal] [the specified s].
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="pos">The position.</param>
	/// <param name="end">The end.</param>
	/// <param name="maxLength">The maximum length.</param>
	/// <param name="allowLeadingZero">if set to <c>true</c> [allow leading zero].</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns><c>true</c> if [is parseable hexadecimal] [the specified s]; otherwise, <c>false</c>.</returns>
	private static bool IsParseableHexadecimal(string s, int pos, int end, int maxLength, bool allowLeadingZero, int minValue, int maxValue)
	{
		var length = end - pos;
		if ((length < 1) | (length > maxLength)) return false;

		var checkLeadingZero = (length > 1) & !allowLeadingZero;
		if (checkLeadingZero && s[pos] == '0') return false;

		var value = 0;
		while (pos < end)
		{
			var d = GetDigitHexadecimal(s, pos++);
			if (d < 0) return false;

			value *= 16;
			value += d;
		}

		return (value >= minValue) & (value <= maxValue);
	}

	/// <summary>
	///   Gets the digit decimal.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="pos">The position.</param>
	/// <returns>System.Int32.</returns>
	private static int GetDigitDecimal(string s, int pos)
	{
		var c = s[pos];
		var d = (uint) (c - '0');
		return d <= 9 ? (int) d : -1;
	}

	/// <summary>
	///   Gets the digit hexadecimal.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="pos">The position.</param>
	/// <returns>System.Int32.</returns>
	private static int GetDigitHexadecimal(string s, int pos)
	{
		var c = s[pos];
		var d = c | 0x20U;
		d -= d >= 'a' ? (uint) 'a' - 10 : '0';
		return d <= 16 ? (int) d : -1;
	}
}
