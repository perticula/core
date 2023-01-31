// perticula - core - StringExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using core.IO;
using core.Random;
using core.Security;
using core.Text;

namespace core;

public static partial class StringExtensions
{
	/// <summary>
	///   Attempts to convert the string to a boolean using on of the approved truthy values
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Boolean.</returns>
	public static bool AsBool(this string? value)
	{
		if (string.IsNullOrEmpty(value)) return false;

		var t = value.ToUpperInvariant();
		return t is "TRUE" or "T" or "1" or "YES" or "Y";
	}

	/// <summary>
	///   Replaces all characters in the specified string value with a password character. (default *)
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="passwordChar">The password character.</param>
	/// <returns>System.String.</returns>
	public static string AsPassword(this string value, string? passwordChar = "*")
	{
		if (string.IsNullOrEmpty(value)) return "";
		var randomizer = Randomizer.Get(RandomSeeds.Random);
		var count      = value.Length;
		var newLen     = randomizer.Next(8, count * 2);

		if (newLen == count)
			newLen += 4; //ensure we don't return a value that is equal in length to the original value.

		return passwordChar.Repeat(newLen);
	}

	/// <summary>
	///   Repeats the specified count.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="count">The count.</param>
	/// <returns>System.String.</returns>
	public static string Repeat(this string? value, int count)
	{
		var output = "";
		if (value                 == null) return "";
		if (count                 <= 0) count       =  1;
		for (var i = count - 1; i >= 0; i--) output += value;
		return output;
	}

	/// <summary>
	///   Trim text to a specific length with an elpisis added at the end
	/// </summary>
	/// <param name="text">The text.</param>
	/// <param name="len">The length.</param>
	/// <param name="ellipsis">The ellipsis.</param>
	/// <returns>System.String.</returns>
	public static string ClipText(this string? text, int len, string? ellipsis = " …")
	{
		ArgumentException.ThrowIfNullOrEmpty(ellipsis, nameof(ellipsis));
		if (len < 1) len = 1;

		if (text == null || text.Length <= len) return "";

		var breaks = text.Split(new[] {"<br>"}, StringSplitOptions.None);
		var output = new StringBuilder(len);
		foreach (var p in breaks)
		{
			if (p.Length > len)
			{
				output.Append(p[..len]);
				output.Append(p.ToCharArray().Skip(len).TakeUntil(ch => !char.IsLetterOrDigit(ch)).ToArray());
				break;
			}

			output.Append(p);
			output.Append("<br>");
			len -= p.Length;
		}

		output.Append(ellipsis);
		return output.ToString();
	}

	/// <summary>
	///   Provides a string contains method with comparison options
	/// </summary>
	/// <param name="source">The string to test</param>
	/// <param name="value">The value to find</param>
	/// <param name="comparison">The comparison type</param>
	/// <returns></returns>
	public static bool Contains(this string source, string value, StringComparison comparison) => source.IndexOf(value, comparison) >= 0;

	/// <summary>
	///   Indicates whether this string contains any one of these substrings
	/// </summary>
	/// <param name="value">The string to test</param>
	/// <param name="subStrings">The list of substrings to check for</param>
	/// <returns><c>true</c> if the specified sub strings contains any; otherwise, <c>false</c>.</returns>
	public static bool ContainsAny(this string? value, params string[]? subStrings) => value != null && subStrings != null && subStrings.Where(sz => !string.IsNullOrEmpty(sz)).Any(value.Contains);

	/// <summary>
	///   Converts 'Carrige Return' characters to Html 'Break Tags'
	/// </summary>
	/// <param name="text">The text.</param>
	/// <returns>System.String.</returns>
	public static string ConvertCrToBr(this string? text) => text?.Replace("\r", "").Replace("\n", "<br>") ?? "";

	/// <summary>
	///   Counts the number of times a character appears within a string.
	/// </summary>
	/// <param name="s">The value.</param>
	/// <param name="charValue">The character value.</param>
	/// <returns>System.Int32.</returns>
	public static int CountChars(this string s, char charValue)
	{
		if (string.IsNullOrEmpty(s)) return 0;

		return s.IndexOf(charValue) != -1 ? s.ToCharArray().Count(c => c == charValue) : 0;
	}

	/// <summary>
	///   Decrypts the value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string DecryptValue(this string value) => SimpleEncrypt.Decrypt(value);

	/// <summary>
	///   Encodes foramtted basic authentication credentials from a username and password.
	///   used for calling rest services that won't accept this any other way for some stupid reason.
	///   yes. iDSS.  that means you.  you stupid idiots.
	/// </summary>
	/// <param name="username">The username.</param>
	/// <param name="password">The password.</param>
	/// <returns>System.String.</returns>
	public static string EncodeBasicAuthenticationCredentials(string username, string password)
	{
		var formatted = $"{username}:{password}";
		var encoded   = Convert.ToBase64String(Encoding.UTF8.GetBytes(formatted));
		return $"Basic {encoded}";
	}

	/// <summary>
	///   Encodes a string so it can be saftely used in a url
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string EncodeForUrl(this string? value)
	{
		if (string.IsNullOrEmpty(value)) return string.Empty;

		value = WebUtility.UrlEncode(value);
		value = value.Replace("+", "%20");
		return value;
	}

	/// <summary>
	///   Encrypts the value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string EncryptValue(this string value) => SimpleEncrypt.Encrypt(value);

	/// <summary>
	///   Filters the characters.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="filters">The filters.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="ArgumentNullException">
	///   value
	///   or
	///   filters
	/// </exception>
	public static string FilterCharacters(this string value, params string[] filters)
	{
		ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
		if (filters == null || !filters.Any()) throw new ArgumentNullException(nameof(filters));
		return filters.Aggregate(value, (current, filter) => !string.IsNullOrEmpty(filter) ? current.Replace(filter, string.Empty) : current);
	}

	/// <summary>
	///   Filters non alpha-numeric chars from  URL string.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string FilterUrlString(this string value)
	{
		if (string.IsNullOrEmpty(value)) return value;

		var filtered = ValidUrlChars().Replace(value, "");
		return filtered.Replace(' ', '-');
	}

	/// <summary>
	///   Formats the name of the private queue.
	/// </summary>
	/// <param name="machineName">Name of the machine.</param>
	/// <param name="name">The name.</param>
	/// <returns>System.String.</returns>
	public static string FormatPrivateQueueName(string machineName, string name) => $"FormatName:DIRECT=TCP:{machineName}\\private$\\{name}";

	/// <summary>
	///   Gets the article preceding the role display text, (e.g. an author vs. a business partner.
	///   note: this will not hold up well upon translation
	/// </summary>
	/// <param name="value">The role.</param>
	/// <returns>System.String.</returns>
	public static string GetArticle(this string? value) => string.IsNullOrEmpty(value) ? "" : MatchGrammarArticle().IsMatch(value) ? "an" : "a";

	/// <summary>
	///   Parses the path and returns the file extension.
	/// </summary>
	/// <param name="path">The path.</param>
	/// <returns>System.String.</returns>
	public static string GetFileExtension(this string? path) => Path.GetExtension(path ?? "");

	/// <summary>
	///   Parses the file name from the path.
	/// </summary>
	/// <param name="file">The file path.</param>
	/// <returns>System.String.</returns>
	public static string GetFileName(this string? file) => Path.GetFileName(file ?? "");

	/// <summary>
	///   create a simple hash from the specified value
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="ArgumentNullException">value - Value for hashing cannot be null</exception>
	public static string Hash(this string value) => Sha256Hash(value);

	/// <summary>
	///   Determines whether the specified string is null, whitespace, lorem ipsum, or an editor placeholder value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Boolean.</returns>
	public static bool IsNullOrDefault(this string? value) => string.IsNullOrWhiteSpace(value) || value.IsLoremIpsum() || value.IsPlaceholderText();

	/// <summary>
	///   Indicates whether the text is placeholder as in not set by the user but
	///   rather part of the initialization. For example: (title) (add address) (add contact)
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns><c>true</c> if [is placeholder text] [the specified value]; otherwise, <c>false</c>.</returns>
	public static bool IsPlaceholderText(this string? value) => string.IsNullOrWhiteSpace(value) || (value.Length > 4 && value.StartsWith("(", StringComparison.OrdinalIgnoreCase) && value.EndsWith(")", StringComparison.OrdinalIgnoreCase));

	/// <summary>
	///   using the specified delimiter returns a joined string list containing all values.
	///   optionally prepends a prefix to each item
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="values">The values.</param>
	/// <param name="delimiter">The delimiter.</param>
	/// <param name="prefix">The prefix.</param>
	/// <returns>System.String.</returns>
	public static string Join<T>(this IEnumerable<T>? values, string delimiter, string? prefix = null)
	{
		if (values == null) return string.Empty;

		var enumerable = values as T[] ?? values.ToArray();
		if (!enumerable.Any()) return string.Empty;

		var strings = enumerable.Select(v => v?.ToString());
		strings = strings.Where(sz => !string.IsNullOrEmpty(sz));

		return string.Join(delimiter,
		                   string.IsNullOrWhiteSpace(prefix)
			                   ? strings.Select(v => v?.ToString() ?? "")
			                   : strings.Select(v => $"{prefix}{v}"));
	}

	/// <summary>
	///   using the specified delimiter returns a joined string list containing all values.
	/// </summary>
	/// <param name="value">The first.</param>
	/// <param name="delimiter">The delimiter.</param>
	/// <param name="values">The list of values.</param>
	/// <returns>System.String.</returns>
	public static string JoinWith(this string value, string delimiter, params string[]? values) => string.Join(delimiter, new[] {value ?? ""}.Concat(values ?? Enumerable.Empty<string>()));

	/// <summary>
	///   using the specified delimiter returns a joined string list containing all values.
	/// </summary>
	/// <param name="value">The first.</param>
	/// <param name="delimiter">The delimiter.</param>
	/// <param name="values">The list of values.</param>
	/// <returns>System.String.</returns>
	public static string JoinWith(this string? value, string delimiter, IEnumerable<string>? values) => string.Join(delimiter, new[] {value ?? ""}.Concat(values ?? Enumerable.Empty<string>()));

	/// <summary>
	///   Makes a term singular or plural based on explicitly specified terms.
	/// </summary>
	/// <param name="singular">The singular term.</param>
	/// <param name="plural">The plural term.</param>
	/// <param name="count">The count.</param>
	/// <returns>System.String.</returns>
	public static string? MakePlural(this string? singular, string plural, int count) => count != 1 ? plural : singular;

	/// <summary>
	///   ms the d5 hash.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="ArgumentNullException">value - Value for hashing cannot be null</exception>
	// ReSharper disable once InconsistentNaming
	public static string MD5Hash(this string value)
	{
		if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value), "Value for hashing cannot be null");

		var hash  = new StringBuilder();
		var bytes = MD5.HashData(new UTF8Encoding().GetBytes(value));

		foreach (var b in bytes) hash.Append(b.ToString("x2"));

		return hash.ToString();
	}

	public static string Sha256Hash(this string value)
	{
		if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value), "Value for hashing cannot be null");

		var hash  = new StringBuilder();
		var bytes = SHA256.HashData(new UTF8Encoding().GetBytes(value));

		foreach (var b in bytes) hash.Append(b.ToString("x2"));

		return hash.ToString();
	}

	/// <summary>
	///   Returns a fast generating hash for the specified string, not intended for security or uniqueness
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string QuickHash(this string value)
	{
		ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

		var hashedValue = 3074457345618258791ul;

		foreach (var ch in value)
		{
			hashedValue += ch;
			hashedValue *= 3074457345618258799ul;
		}

		return hashedValue.ToString();
	}

	/// <summary>
	///   Replaces the string value with a replacement string after the original value exceeds the specified number of
	///   characters.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="chars">The chars.</param>
	/// <param name="replacement">The replacement.</param>
	/// <returns>System.String.</returns>
	public static string ReplaceAfter(this string? value, int chars, string replacement)
	{
		if (string.IsNullOrEmpty(value)) return "";

		return value.Length > chars ? replacement : value;
	}

	/// <summary>
	///   Replaces all.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="charsToReplace">The chars to replace.</param>
	/// <param name="replacement">The replacement.</param>
	/// <returns>System.String.</returns>
	public static string ReplaceAll(this string value, char[] charsToReplace, char replacement) => charsToReplace.Aggregate(value, (current, c) => current.Replace(c, replacement));

	/// <summary>
	///   Replaces the last occurrence of a matching value within a string.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="find">The find.</param>
	/// <param name="replace">The replace.</param>
	/// <returns>System.String.</returns>
	public static string ReplaceLastOccurrence(this string? source, string find, string replace)
	{
		if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(find) || string.IsNullOrEmpty(replace)) return string.Empty;

		var place = source.LastIndexOf(find, StringComparison.Ordinal);

		if (place == -1) return source;

		var result = source.Remove(place, find.Length).Insert(place, replace);
		return result;
	}

	/// <summary>
	///   Returns the last occurrence.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="separator">The separator.</param>
	/// <returns>System.String.</returns>
	public static string ReturnLastOccurrence(this string? source, char separator)
	{
		if (string.IsNullOrEmpty(source)) return string.Empty;

		var relativePath = source.LastIndexOf(separator);
		return relativePath == -1
			       ? string.Empty
			       : source[(relativePath + 1)..];
	}

	/// <summary>
	///   Splits the value on ',' and ';' and parses the result using the specified expression.
	/// </summary>
	/// <typeparam name="TValue">The type of the t value.</typeparam>
	/// <param name="value">The value.</param>
	/// <param name="parseExpression">The parse expression.</param>
	/// <returns>IEnumerable&lt;TValue&gt;.</returns>
	public static IEnumerable<TValue>? SplitAndParse<TValue>(this string? value, Func<string, TValue> parseExpression)
	{
		if (string.IsNullOrEmpty(value)) return null;
		if (parseExpression == null) throw new ArgumentNullException(nameof(parseExpression));

		var chunks = value.Split(',', ';');
		return !chunks.Any() ? null : chunks.Select(parseExpression);
	}

	/// <summary>
	///   Splits a string on commas and returns the first result.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="delimiter">The delimiter.</param>
	/// <returns>System.String.</returns>
	public static string? SplitAndReturnFirst(this string? value, char delimiter = ',')
	{
		if (string.IsNullOrEmpty(value)) return value;

		var chunks = value.Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries);
		return !chunks.Any() ? string.Empty : chunks.First();
	}

	/// <summary>
	///   Splits the string on commas and wraps each value with the specified wrapping value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="splitOn">The split on.</param>
	/// <param name="wrapper">The wrapper.</param>
	/// <returns>System.String.</returns>
	public static string SplitAndWrap(this string value, char[]? splitOn = null, string? wrapper = "")
	{
		if (string.IsNullOrEmpty(value)) return string.Empty;

		splitOn ??= new[] {',', ';'};

		var chunks = value.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);
		var list   = chunks.Select(chunk => string.Format(CultureInfo.InvariantCulture, "{1}{0}{1}", chunk, wrapper)).ToList();
		return string.Join(",", list);
	}

	/// <summary>
	///   Splits the string on commas and wraps each value in single quotes.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string SplitAndWrapQuotes(this string value) => value.SplitAndWrap(wrapper: "'");

	/// <summary>
	///   Splits a string on camel case and return individual words.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string SplitCamelCase(this string value)
	{
		if (string.IsNullOrEmpty(value)) return value;

		var split    = DetectCamelCase().Replace(value, "  $0");
		var collapse = CaptureMultipleSpaces().Replace(split, " ");
		return collapse.Trim();
	}

	/// <summary>
	///   Splits a string on the newline (as in one per line)
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>IEnumerable&lt;System.String&gt;.</returns>
	public static IEnumerable<string> SplitOnNewLine(this string value) => value?.Split('\n').Select(sz => sz.Trim('\r')) ?? Enumerable.Empty<string>();

	/// <summary>
	///   Converts the string to a memory stream.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.IO.Stream.</returns>
	public static Stream? ToStream(this string? value)
	{
		if (string.IsNullOrEmpty(value)) return default;

		var stream = new MemoryStream();
		var writer = new StreamWriter(stream);
		writer.Write(value);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}

	/// <summary>
	///   Converts to validpathname.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string ToValidPathName(this string value)
	{
		ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
		return value.ReplaceAll(FileUtilities.InvalidFileNameChars, '-');
	}

	/// <summary>
	///   Returns from the list of strings, only those which are not null nor empty.
	///   Optionally trim the values before checking for empty.
	/// </summary>
	/// <param name="list">The list.</param>
	/// <param name="trim">Optionally trim the values before checking them</param>
	/// <returns>IEnumerable&lt;System.String&gt;.</returns>
	public static IEnumerable<string> WhereHasValue(this IEnumerable<string> list, bool trim = false) => list?.Where(sz => !string.IsNullOrEmpty(trim ? sz?.Trim() : sz)) ?? Enumerable.Empty<string>();

	/// <summary>
	///   Converts an enumerable char array to a string
	/// </summary>
	/// <param name="cs">The cs.</param>
	/// <returns>System.String.</returns>
	public static string AsString(this IEnumerable<char> cs) => new(cs.ToArray());
}
