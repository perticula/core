// perticula - core - ISupportsFiltering.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Filtering;

public interface ISupportsFiltering
{
	/// <summary>
	///   Gets a value indicating whether this object also supports filter attribute reflection.
	/// </summary>
	/// <value><c>true</c> if the object supports filter attributes; otherwise, <c>false</c>.</value>
	bool SupportsFilterAttributes { get; }

	/// <summary>
	///   Determines whether this object has a filter value with the specified name.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns><c>true</c> if the object has a filter with the specified name; otherwise, <c>false</c>.</returns>
	bool HasFilterValue(string name);

	/// <summary>
	///   Resolves the value of a filtered object by name.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>System.Object.</returns>
	object ResolveFilterValue(string name);
}
