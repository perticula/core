// perticula - core - StringExtensions.Regex.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text.RegularExpressions;

namespace core;

/// <summary>
///   Class StringExtensions.
/// </summary>
public partial class StringExtensions
{
	/// <summary>
	///   Detects the camel case.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("((^[a-z]+)|([0-9]+)|([A-Z]{1}[a-z]+)|([A-Z]+(?=([A-Z][a-z])|($)|([0-9]))))")]
	public static partial Regex DetectCamelCase();

	/// <summary>
	///   Captures the multiple spaces.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("[ ]{2,}", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
	public static partial Regex CaptureMultipleSpaces();

	/// <summary>
	///   Valids the URL chars.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("[^a-zA-z0-9_ -]", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline, "en-US")]
	public static partial Regex ValidUrlChars();

	/// <summary>
	///   Captures the last word.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("(?<last>(\\w+)$)", RegexOptions.IgnoreCase | RegexOptions.Singleline, "en-US")]
	public static partial Regex CaptureLastWord();

	/// <summary>
	///   Detects the multiple lines.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("(?:\\r\\n|\\r(?!\\n)|(?<!\\r)\\n){2,}", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
	public static partial Regex DetectMultipleLines();

	/// <summary>
	///   Captures the hyphens.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("[-\\s]")]
	public static partial Regex CaptureHyphens();

	/// <summary>
	///   Captures the unicode letter.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("([\\p{Ll}\\d])([\\p{Lu}])")]
	public static partial Regex CaptureUnicodeLetter();

	/// <summary>
	///   Captures the unicode word.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("([\\p{Lu}]+)([\\p{Lu}][\\p{Ll}])")]
	public static partial Regex CaptureUnicodeWord();

	/// <summary>
	///   Captures the underscores.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("(?:^|_| +)(.)")]
	public static partial Regex CaptureUnderscores();

	/// <summary>
	///   Matches the grammar article.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex("^[a|e|i|o|u]")]
	public static partial Regex MatchGrammarArticle();

	/// <summary>
	///   Matches all chars.
	/// </summary>
	/// <returns>Regex.</returns>
	[GeneratedRegex(".")]
	public static partial Regex MatchAllChars();
}
