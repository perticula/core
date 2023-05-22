// perticula - core - Alter.Html.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text.RegularExpressions;

namespace core.Matching;

/// <summary>
///   Class Alter.
/// </summary>
public static partial class Alter
{
	/// <summary>
	///   Class Html.
	/// </summary>
	public static partial class Html
	{
		/// <summary>
		///   The find HTML tag
		/// </summary>
		private static readonly Regex FindHtmlTag = MatchWellFormedXmlTag();

		/// <summary>
		///   Replaces all HTML tags in the provided string with an alternative
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="with">The with.</param>
		/// <returns>System.String.</returns>
		public static string ReplaceTags(string? value, string? with) => FindHtmlTag.Replace(value ?? "", with ?? "");

		/// <summary>
		///   Removes all HTML tags from the provided string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string StripTags(string? value) => ReplaceTags(value, "");

		/// <summary>
		///   Matches the well formed XML tag.
		/// </summary>
		/// <returns>Regex.</returns>
		[GeneratedRegex("<[^>]*>", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
		private static partial Regex MatchWellFormedXmlTag();
	}
}
