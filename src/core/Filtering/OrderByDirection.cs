// perticula - core - OrderByDirection.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Attributes;

namespace core.Filtering;

/// <summary>
///   Enum OrderByDirection
/// </summary>
public enum OrderByDirection
{
	/// <summary>
	///   The invalid
	/// </summary>
	Invalid = 0x00, // all enums should define the default case as an invalid value.

	/// <summary>
	///   The ascending
	/// </summary>
	[DisplayText("Ascending")] Ascending = 0x01,

	/// <summary>
	///   The descending
	/// </summary>
	[DisplayText("Descending")] Descending = -0x01
}
