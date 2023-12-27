// perticula - core - BooleanExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Class BooleanExtensions.
/// </summary>
public static class BooleanExtensions
{
	/// <summary>
	///   Determines whether the any of specified values are true.
	/// </summary>
	/// <param name="values">The values.</param>
	/// <returns><c>true</c> if any of the specified values is true; otherwise, <c>false</c>.</returns>
	public static bool AreAnyTrue(params bool[]? values) => values != null && values.Any(value => value);
}
