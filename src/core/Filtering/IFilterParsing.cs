// perticula - core - IFilterParsing.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Filtering;

/// <summary>
///   Interface IFilterParsing
/// </summary>
public interface IFilterParsing
{
	/// <summary>
	///   Parse a filter statement from a string.
	/// </summary>
	/// <param name="source">The filter statement to parse</param>
	/// <returns>IFilter.</returns>
	/// <exception cref="FilterSyntaxException">Throws FilterSyntaxException if the filter cannot be parsed</exception>
	IFilter Parse(string source);
}
