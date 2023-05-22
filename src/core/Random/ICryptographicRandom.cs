// perticula - core - ICryptographicRandom.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

/// <summary>
///   Interface ICryptographicRandom
/// </summary>
public interface ICryptographicRandom
{
	/// <summary>
	///   Returns a random number between 0 and <c>uint.MaxValue</c> inclusive
	/// </summary>
	/// <returns>System.UInt32.</returns>
	uint GenerateNum();

	/// <summary>
	///   Returns a random number between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	/// <param name="maxInclusive">The maximum inclusive.</param>
	/// <returns>System.UInt32.</returns>
	uint GenerateNum(uint maxInclusive);

	/// <summary>
	///   Returns a random number between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	/// <param name="maxInclusive">The maximum inclusive.</param>
	/// <returns>System.Int32.</returns>
	int GenerateNum(int maxInclusive);

	/// <summary>
	///   Returns an infinite stream of random numbers between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	/// <param name="maxInclusive">The maximum inclusive.</param>
	/// <returns>IEnumerable&lt;System.Int32&gt;.</returns>
	IEnumerable<int> GenerateNumStream(int maxInclusive);

	/// <summary>
	///   Returns one item from <paramref name="items" /> chosen randomly
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="items">The items.</param>
	/// <returns>T.</returns>
	T Choose<T>(IEnumerable<T> items);

	/// <summary>
	///   Returns one item from <paramref name="set" /> chosen randomly
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="set">The set.</param>
	/// <returns>T.</returns>
	T Choose<T>(ISet<T> set);

	/// <summary>
	///   Returns an infinite stream of randomly chosen items from a given <paramref name="set" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="set">The set.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<T> GetChoiceStream<T>(ISet<T> set);

	/// <summary>
	///   Returns <paramref name="items" /> in a random order
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="items">The items.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	IEnumerable<T> Shuffle<T>(IEnumerable<T> items);
}
