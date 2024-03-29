// perticula - core - IStore.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Matching;

/// <summary>
///   Interface IObjectStore
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IObjectStore<out T>
{
	/// <summary>
	///   Enumerates the matches.
	/// </summary>
	/// <param name="selector">The selector.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<T> EnumerateMatches(ISelector<T> selector);
}
