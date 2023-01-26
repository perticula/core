// perticula - core - Alter.Text.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text.RegularExpressions;

namespace core.Matching;

public static partial class Alter
{
	public static partial class Text
	{
		/// <summary>
		///   The find non alpha
		/// </summary>
		private static readonly Regex FindNonAlpha = MatchNonAlpha();

		/// <summary>
		///   The find non alpha numeric
		/// </summary>
		private static readonly Regex FindNonAlphaNumeric = MatchNonAlphaNumeric();

		/// <summary>
		///   The find non numeric
		/// </summary>
		private static readonly Regex FindNonNumeric = MatchNonNumeric();

		/// <summary>
		///   Removes all non aphabet characters from the string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string StripNonAlpha(string? value) => FindNonAlpha.Replace(value ?? "", "");

		/// <summary>
		///   Removes all non aphanumeric characters from the string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string StripNonAlphaNumeric(string? value) => FindNonAlphaNumeric.Replace(value ?? "", "");

		/// <summary>
		///   Removes all non digit characters from the string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string StripNonNumeric(string? value) => FindNonNumeric.Replace(value ?? "", "");

		[GeneratedRegex("[^A-Za-z]", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
		private static partial Regex MatchNonAlpha();

		[GeneratedRegex("[^\\dA-Za-z]", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
		private static partial Regex MatchNonAlphaNumeric();

		[GeneratedRegex("[^\\d]", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
		private static partial Regex MatchNonNumeric();
	}
}
