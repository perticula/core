// perticula - core - LoremIpsumGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text;

namespace core.Text;

public static class LoremIpsumGenerator
{
	/// <summary>
	///   Standard lorem ipsum text
	/// </summary>
	public const string Standard =
		"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam hendrerit nisi sed sollicitudin pellentesque. Nunc posuere purus rhoncus pulvinar aliquam. Ut aliquet tristique nisl vitae volutpat. Nulla aliquet porttitor venenatis. Donec a dui et dui fringilla consectetur id nec massa. Aliquam erat volutpat. Sed ut dui ut lacus dictum fermentum vel tincidunt neque. Sed sed lacinia lectus. Duis sit amet sodales felis. Duis nunc eros, mattis at dui ac, convallis semper risus. In adipiscing ultrices tellus, in suscipit massa vehicula eu.";

	/// <summary>
	///   a lazy static cache
	/// </summary>
	private static string[]? _loremWords;

	/// <summary>
	///   a lazy static cache
	/// </summary>
	private static string[]? _loremSplit;

	/// <summary>
	///   Standard lorem ipsum text as individual words
	/// </summary>
	/// <value>The words.</value>
	public static string[] Words => _loremWords ??= LoremSplit.Select(s => s.Trim(' ', ',', '.')).ToArray();

	/// <summary>
	///   Standard lorem ipsum text split on spaces,  may contain odd punctuation.
	/// </summary>
	/// <value>The lorem split.</value>
	private static string[] LoremSplit => _loremSplit ??= Standard.Split(' ');

	/// <summary>
	///   Determines whether the specified value starts with a LoremIpsum string.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Boolean.</returns>
	public static bool IsLoremIpsum(this string value) => value.StartsWith(@"Lorem ipsum", StringComparison.OrdinalIgnoreCase);

	/// <summary>
	///   Returns lorem ipsum text with the provided number of words
	/// </summary>
	/// <param name="words">The words.</param>
	/// <returns>System.String.</returns>
	public static string Text(int words)
	{
		if (words < 1) return "";
		var at     = 0;
		var result = new StringBuilder(words * 8);
		for (; words > 0; words--)
		{
			if (at >= LoremSplit.Length) at = 0;
			result.Append(LoremSplit[at]);
			at++;
			if (words > 1)
				result.Append(' ');
		}

		return result.Append('.')
		             .Replace(", .", ".")
		             .Replace(". .", ".")
		             .Replace(",.",  ".")
		             .Replace("..",  ".")
		             .Replace("  ",  " ")
		             .ToString();
	}
}
