// perticula - core - StringExtensions.Formatting.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using core.Matching;

namespace core;

public partial class StringExtensions
{
	/// <summary>
	///   Removes single and double quotes from the string
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string? StripQuotes(this string? value) => value == null ? null : string.IsNullOrEmpty(value) ? "" : value.Replace("'", "").Replace("\"", "");

	/// <summary>
	///   Converts a string to pascal case.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string? ToPascalCase(this string? value) => value == null ? null : CaptureUnderscores().Replace(value, match => match.Groups[1].Value.ToUpper());

	/// <summary>
	///   Converts a string to kebab case.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string? ToKebabCase(this string? value) => value?.ToUnderscoreCase()?.Replace('_', '-') ?? default;

	/// <summary>
	///   Converts a string to underscore case.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string? ToUnderscoreCase(this string? value) =>
		value == null
			? null
			: CaptureHyphens()
			  .Replace(
				  CaptureUnicodeLetter()
					  .Replace(
						  CaptureUnicodeWord()
							  .Replace(value, "$1_$2"), "$1_$2"), "_")
			  .ToLower();

	/// <summary>
	///   Converts a string value to CamelCase.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string? ToCamelCase(this string? value)
	{
		if (value == null) return null;
		var word = value.ToPascalCase();
		return word?.Length > 0 ? word[..1].ToLower(CultureInfo.CurrentCulture) + word[1..] : word;
	}

	/// <summary>
	///   Performs a trim of a string from the end of the source string
	/// </summary>
	/// <param name="source"></param>
	/// <param name="value"></param>
	/// <param name="comparison"></param>
	/// <returns></returns>
	public static string TrimEnd(this string source, string value, StringComparison comparison = StringComparison.CurrentCulture)
	{
		if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value)) return source;

		var endAt = source.Length;

		while (source.LastIndexOf(value, endAt, comparison) == endAt - value.Length) endAt -= value.Length;

		return endAt < 0 ? "" : source[..endAt];
	}

	/// <summary>
	///   Performs a trim of a string from the start of the source string
	/// </summary>
	/// <param name="source"></param>
	/// <param name="value"></param>
	/// <param name="comparison"></param>
	/// <returns></returns>
	public static string TrimStart(this string source, string value, StringComparison comparison = StringComparison.CurrentCulture)
	{
		if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value)) return source;

		var startAt = 0;

		while (source.IndexOf(value, startAt, comparison) == startAt) startAt += value.Length;

		return startAt >= source.Length ? "" : source[startAt..];
	}

	/// <summary>
	///   Truncates the specified value to a maximum length.
	///   if specified attaches an indicator value (e.g. "...")
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="maxLength">The maximum length; must be at least one.</param>
	/// <param name="indicator">The indicator.</param>
	/// <returns>System.String.</returns>
	public static string Truncate(this string value, int maxLength, string indicator = "…")
	{
		if (string.IsNullOrEmpty(value)) return "";
		if (maxLength    < 1) return value;
		if (value.Length <= maxLength) return value;
		var truncated = value[..maxLength];

		if (!string.IsNullOrEmpty(indicator)) truncated += indicator;

		return truncated;
	}

	/// <summary>
	///   Wraps the specified source value with the specified wrapper value.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="wrapper">The wrapper.</param>
	/// <returns>System.String.</returns>
	public static string Wrap(this string source, string wrapper) => $"{wrapper}{source}{wrapper}";

	/// <summary>
	///   Wraps the specified wrapper.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="wrapper">The wrapper.</param>
	/// <returns>IEnumerable&lt;System.String&gt;.</returns>
	public static IEnumerable<string> Wrap(this IEnumerable<string> source, string wrapper) => source.Select(item => $"{wrapper}{item}{wrapper}");

	/// <summary>
	///   Wraps the last word.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="prefix">The prefix.</param>
	/// <param name="suffix">The suffix.</param>
	/// <returns>System.String.</returns>
	public static string WrapLastWord(this string value, string prefix, string suffix)
	{
		var regex      = CaptureLastWord();
		var strReplace = prefix + @"${last}" + suffix;
		return regex.Replace(value, strReplace);
	}

	/// <summary>
	///   Strips HTML from the specified text.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string StripHtml(this string value)
	{
		if (string.IsNullOrEmpty(value)) return string.Empty;

		// convert <br> and <p> to \n
		value = value.Replace("<br>", "\r\n").Replace("<br />", "\r\n").Replace("<br/>", "\r\n");
		value = value.Replace("<p>",  "\r\n").Replace("</p>", "\r\n");

		// add space before and after table cells
		value = value.Replace("<td>", "<td> ").Replace("</td>", " </td>");

		// strip out all html tags and condense multiple spaces / lines
		value = Alter.Html.StripTags(value);
		value = CaptureMultipleSpaces().Replace(value, " ");

		//  Dont use .HtmlDecode() below as this will cause rendering of encodded HTML
		// value = value.HtmlDecode()

		// replace some html entities (not all)
		value = value
		        .Replace("&nbsp;",  " ")
		        .Replace("&amp;",   "&")
		        .Replace("&quot;",  "\"")
		        .Replace("&tilde;", "~");

		return value
		       .Trim('\r', '\n')
		       .Trim();
	}

	/// <summary>
	///   Hilights elements within the text by wrapping word or phrases
	/// </summary>
	/// <param name="text">The text.</param>
	/// <param name="prefix">The prefix.</param>
	/// <param name="find">The find.</param>
	/// <param name="suffix">The suffix.</param>
	/// <param name="merge">if set to <c>true</c> [merge].</param>
	/// <returns>System.String.</returns>
	public static string Highlight(this string? text, string prefix, IEnumerable<string> find, string? suffix = null, bool merge = false)
	{
		if (string.IsNullOrEmpty(prefix)) throw new ArgumentNullException(nameof(prefix));
		var findArray = find as string[] ?? find.ToArray();
		text ??= "";
		// ReSharper disable once LoopCanBeConvertedToQuery
		foreach (var word in findArray)
		{
			if (string.IsNullOrEmpty(word)) continue;

			var pattern = $@"\b{Regex.Escape(word)}\w*?\b"; // words starting with
			text = Regex.Replace(text, pattern, match => $"{prefix}{match.Value}{suffix}", RegexOptions.IgnoreCase);
		}

		// ReSharper disable once InvertIf
		if (merge && !string.IsNullOrEmpty(suffix))
		{
			var pattern = $@"\w{Regex.Escape(suffix)}\value*?{Regex.Escape(prefix)}\w";
			text = Regex.Replace(text, pattern, match => match.Value.Replace(suffix, "").Replace(prefix, ""), RegexOptions.IgnoreCase);
		}

		return text;
	}

	/// <summary>
	///   Decodes a string of HTML to text.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string HtmlDecode(this string? value) => string.IsNullOrEmpty(value) ? "" : HttpUtility.HtmlDecode(value);

	/// <summary>
	///   Encodes a string of text as HTML.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string HtmlEncode(this string? value) => string.IsNullOrEmpty(value) ? "" : HttpUtility.HtmlEncode(value);
}
