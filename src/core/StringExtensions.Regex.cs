// perticula - core - StringExtensions.Regex.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text.RegularExpressions;

namespace core
{
	public partial class StringExtensions
	{
		[GeneratedRegex("((^[a-z]+)|([0-9]+)|([A-Z]{1}[a-z]+)|([A-Z]+(?=([A-Z][a-z])|($)|([0-9]))))")]
		public static partial Regex DetectCamelCase();

		[GeneratedRegex("[ ]{2,}", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
		public static partial Regex CaptureMultipleSpaces();

		[GeneratedRegex("[^a-zA-z0-9_ -]", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline, "en-US")]
		public static partial Regex ValidUrlChars();

		[GeneratedRegex("(?<last>(\\w+)$)", RegexOptions.IgnoreCase | RegexOptions.Singleline, "en-US")]
		public static partial Regex CaptureLastWord();

		[GeneratedRegex("(?:\\r\\n|\\r(?!\\n)|(?<!\\r)\\n){2,}", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
		public static partial Regex DetectMultipleLines();

		[GeneratedRegex("[-\\s]")]
		public static partial Regex CaptureHyphens();

		[GeneratedRegex("([\\p{Ll}\\d])([\\p{Lu}])")]
		public static partial Regex CaptureUnicodeLetter();

		[GeneratedRegex("([\\p{Lu}]+)([\\p{Lu}][\\p{Ll}])")]
		public static partial Regex CaptureUnicodeWord();

		[GeneratedRegex("(?:^|_| +)(.)")]
		public static partial Regex CaptureUnderscores();

		[GeneratedRegex("^[a|e|i|o|u]")]
		public static partial Regex MatchGrammarArticle();

		[GeneratedRegex(".")]
		public static partial Regex MatchAllChars();
	}
}
