// perticula - core - ICryptographicRandom.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

public interface ICryptographicRandom
{
	/// <summary>
	///   Returns a random number between 0 and <c>uint.MaxValue</c> inclusive
	/// </summary>
	uint GenerateNum();

	/// <summary>
	///   Returns a random number between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	uint GenerateNum(uint maxInclusive);

	/// <summary>
	///   Returns a random number between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	int GenerateNum(int maxInclusive);

	/// <summary>
	///   Returns an infinite stream of random numbers between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	IEnumerable<int> GenerateNumStream(int maxInclusive);

	/// <summary>
	///   Returns one item from <paramref name="items" /> chosen randomly
	/// </summary>
	T Choose<T>(IEnumerable<T> items);

	/// <summary>
	///   Returns one item from <paramref name="set" /> chosen randomly
	/// </summary>
	T Choose<T>(ISet<T> set);

	/// <summary>
	///   Returns an infinite stream of randomly chosen items from a given <paramref name="set" />
	/// </summary>
	IEnumerable<T> GetChoiceStream<T>(ISet<T> set);

	/// <summary>
	///   Returns <paramref name="items" /> in a random order
	/// </summary>
	IEnumerable<T> Shuffle<T>(IEnumerable<T> items);
}
