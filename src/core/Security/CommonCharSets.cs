// perticula - core - CommonCharSets.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Security;

/// <summary>
///   Class CommonCharSets.
/// </summary>
public static class CommonCharSets
{
	/// <summary>
	///   The lowercase en
	/// </summary>
	public const string LowercaseEn = @"abcdefghijklmnopqrstuvwxyz";

	/// <summary>
	///   The uppercase en
	/// </summary>
	public const string UppercaseEn = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	/// <summary>
	///   The digits
	/// </summary>
	public const string Digits = @"0123456789";

	/// <summary>
	///   The symbols
	/// </summary>
	public const string Symbols = @"!@#$%^&*()";

	/// <summary>
	///   The full symbols
	/// </summary>
	public const string FullSymbols = @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";

	/// <summary>
	///   The alphanumerics
	/// </summary>
	public const string Alphanumerics = LowercaseEn + UppercaseEn + Digits;

	/// <summary>
	///   The alpha numeric symbolic
	/// </summary>
	public const string AlphaNumericSymbolic = Alphanumerics + Symbols;

	/// <summary>
	///   The alpha numeric symbolic full
	/// </summary>
	public const string AlphaNumericSymbolicFull = Alphanumerics + FullSymbols;

	/// <summary>
	///   Gets the alphanumeric groups.
	/// </summary>
	/// <value>The alphanumeric groups.</value>
	public static IEnumerable<string> AlphanumericGroups => new[] { LowercaseEn, UppercaseEn, Digits };
}
