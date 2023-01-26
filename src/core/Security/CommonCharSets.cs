// perticula - core - CommonCharSets.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Security;

public static class CommonCharSets
{
	public const string LowercaseEn = @"abcdefghijklmnopqrstuvwxyz";
	public const string UppercaseEn = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	public const string Digits      = @"0123456789";
	public const string Symbols     = @"!@#$%^&*()";
	public const string FullSymbols = @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";

	public const string Alphanumerics            = LowercaseEn   + UppercaseEn + Digits;
	public const string AlphaNumericSymbolic     = Alphanumerics + Symbols;
	public const string AlphaNumericSymbolicFull = Alphanumerics + FullSymbols;

	public static IEnumerable<string> AlphanumericGroups => new[] {LowercaseEn, UppercaseEn, Digits};
}
