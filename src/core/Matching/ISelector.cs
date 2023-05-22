// perticula - core - ISelector.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Matching;

/// <summary>
///   Interface ISelector
///   Implements the <see cref="System.ICloneable" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.ICloneable" />
public interface ISelector<in T> : ICloneable
{
	/// <summary>
	///   Matches the specified candidate object, returns true if the canidate would be matched by the selector.
	/// </summary>
	/// <param name="candidate">The candidate.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	bool Match(T candidate);
}
