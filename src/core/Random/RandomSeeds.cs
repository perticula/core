// perticula - core - RandomSeeds.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

/// <summary>
///   Enum RandomSeeds
/// </summary>
public enum RandomSeeds
{
	/// <summary>
	///   This randomization will not persist
	/// </summary>
	Random,

	/// <summary>
	///   This randomization will change per minute
	/// </summary>
	PerMinute,

	/// <summary>
	///   This randomization will change per hour
	/// </summary>
	PerHour,

	/// <summary>
	///   This randomization will change per day
	/// </summary>
	PerDay
}
