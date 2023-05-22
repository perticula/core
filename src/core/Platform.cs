// perticula - core - Platform.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;

namespace core;

/// <summary>
///   Class Platform.
/// </summary>
public static class Platform
{
	/// <summary>
	///   The invariant compare information
	/// </summary>
	public static readonly CompareInfo InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

	/// <summary>
	///   Gets a value indicating whether [is64 bit process].
	/// </summary>
	/// <value><c>true</c> if [is64 bit process]; otherwise, <c>false</c>.</value>
	public static bool Is64BitProcess => Environment.Is64BitProcess;

	/// <summary>
	///   Gets the name of the type.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.String.</returns>
	public static string GetTypeName(this object obj) => GetTypeName(obj.GetType());

	/// <summary>
	///   Gets the name of the type.
	/// </summary>
	/// <param name="t">The t.</param>
	/// <returns>System.String.</returns>
	public static string GetTypeName(this Type t) => t.FullName ?? t.ToString();
}
