// perticula - core - ObjectStore.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Matching;

/// <summary>
///   Class ObjectStore. This class cannot be inherited.
///   Implements the <see cref="core.Matching.IObjectStore{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="core.Matching.IObjectStore{T}" />
public sealed class ObjectStore<T> : IObjectStore<T>
{
	/// <summary>
	///   The contents
	/// </summary>
	private readonly IList<T> _contents;

	/// <summary>
	///   Initializes a new instance of the <see cref="ObjectStore{T}" /> class.
	/// </summary>
	/// <param name="baseEnumerable">The base enumerable.</param>
	public ObjectStore(IEnumerable<T> baseEnumerable) => _contents = new List<T>(baseEnumerable);

	/// <summary>
	///   Enumerates the matches.
	/// </summary>
	/// <param name="selector">The selector.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	public IEnumerable<T> EnumerateMatches(ISelector<T> selector) => _contents.Where(selector.Match);
}
