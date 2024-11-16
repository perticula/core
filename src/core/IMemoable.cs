// perticula - core - IMemoable.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Interface IMemoable
/// </summary>
public interface IMemoable
{
	/// <summary>
	///   Produces a copy of this object with is configuration and in its current state.
	/// </summary>
	/// <returns>IMemoable.</returns>
	IMemoable Copy();

	/// <summary>
	///   Resets a copied object's state into this object's state.
	/// </summary>
	/// <param name="other">The other.</param>
	void Reset(IMemoable other);
}

/// <summary>
///   Interface IMemoable
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMemoable<T> where T : IMemoable
{
	/// <summary>
	///   Produces a copy of this object with is configuration and in its current state.
	/// </summary>
	/// <returns>IMemoable.</returns>
	IMemoable<T> Copy();

	/// <summary>
	///   Resets a copied object's state into this object's state.
	/// </summary>
	/// <param name="other">The other.</param>
	void Reset(IMemoable<T> other);
}
