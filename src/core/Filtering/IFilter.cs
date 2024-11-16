// perticula - core - IFilter.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Filtering;

/// <summary>
///   Interface IFilter
/// </summary>
public interface IFilter
{
	/// <summary>
	///   Gets the original statement used to create this filter.
	/// </summary>
	/// <value>The original statement.</value>
	string OriginalStatement { get; }

	/// <summary>
	///   Gets the parsed statement as interpreted by the filter parser.
	/// </summary>
	/// <value>The parsed statement.</value>
	string ParsedStatement { get; }

	/// <summary>
	///   Converts this filter to json.
	/// </summary>
	/// <returns>System.String.</returns>
	string ToJson();

	/// <summary>
	///   Filters a list of objects based on this filter
	/// </summary>
	/// <typeparam name="TISupportsFiltering">The type of the ti supports filtering.</typeparam>
	/// <param name="list">The list.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<TISupportsFiltering> FilterList<TISupportsFiltering>(IEnumerable<TISupportsFiltering> list)
		where TISupportsFiltering : ISupportsFiltering;

	/// <summary>
	///   sorts the list based on the orderby clause (if present).
	/// </summary>
	/// <typeparam name="TISupportsFiltering">The type of the ti supports filtering.</typeparam>
	/// <param name="list">The list.</param>
	/// <param name="comparer">The comparer.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<TISupportsFiltering> OrderList<TISupportsFiltering>(IEnumerable<TISupportsFiltering> list,
		Func<TISupportsFiltering, TISupportsFiltering, int> comparer) where TISupportsFiltering : ISupportsFiltering;

	/// <summary>
	///   Filters a list of objects based on this filter and then sorts the list based on the orderby clause (if present).
	/// </summary>
	/// <typeparam name="TISupportsFiltering">The type of the ti supports filtering.</typeparam>
	/// <param name="list">The list.</param>
	/// <param name="comparer">The comparer.</param>
	/// <returns>IEnumerable&lt;TISupportsFiltering&gt;.</returns>
	IEnumerable<TISupportsFiltering> FilterAndOrderList<TISupportsFiltering>(IEnumerable<TISupportsFiltering> list,
		Func<TISupportsFiltering, TISupportsFiltering, int> comparer) where TISupportsFiltering : ISupportsFiltering;
}
