// perticula - core - CryptographicRandom.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

public abstract class CryptographicRandom : ICryptographicRandom
{
	/// <summary>
	///   Returns a random number between 0 and <c>uint.MaxValue</c> inclusive
	/// </summary>
	public abstract uint GenerateNum();

	/// <summary>
	///   Returns a random number between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	public virtual uint GenerateNum(uint maxInclusive)
	{
		if (maxInclusive == 0) return 0;

		const long baseSize     = (long) uint.MaxValue + 1;
		var        maxExclusive = (long) maxInclusive + 1;

		var cutoff = baseSize - baseSize % maxExclusive;

		uint choice;
		do
			choice = GenerateNum();
		while (choice >= cutoff);

		return (uint) (choice % maxExclusive);
	}

	/// <summary>
	///   Returns a random number between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	public virtual int GenerateNum(int maxInclusive)
	{
		if (maxInclusive < 0) throw new ArgumentOutOfRangeException(nameof(maxInclusive), "maxInclusive may not be negative.");
		return (int) GenerateNum((uint) maxInclusive);
	}

	/// <summary>
	///   Returns an infinite stream of random numbers between 0 and <paramref name="maxInclusive" /> inclusive
	/// </summary>
	public virtual IEnumerable<int> GenerateNumStream(int maxInclusive)
	{
		while (true)
			yield return GenerateNum(maxInclusive);
		// ReSharper disable once IteratorNeverReturns
	}

	/// <summary>
	///   Returns one item from <paramref name="items" /> chosen randomly
	/// </summary>
	public virtual T Choose<T>(IEnumerable<T> items) => Choose(new HashSet<T>(items));

	/// <summary>
	///   Returns one item from <paramref name="set" /> chosen randomly
	/// </summary>
	public virtual T Choose<T>(ISet<T> set)
	{
		if (set.Count == 0) throw new ArgumentException("set is empty.");
		return set.ElementAt(GenerateNum(set.Count - 1));
	}

	/// <summary>
	///   Returns an infinite stream of randomly chosen items from <paramref name="set" />
	/// </summary>
	public virtual IEnumerable<T> GetChoiceStream<T>(ISet<T> set)
	{
		if (set.Count == 0) throw new ArgumentException("set is empty.");
		return GenerateNumStream(set.Count - 1).Select(set.ElementAt);
	}

	/// <summary>
	///   Returns <paramref name="items" /> in a random order
	/// </summary>
	public virtual IEnumerable<T> Shuffle<T>(IEnumerable<T> items)
	{
		// The Fisher-Yates shuffle algorithm
		// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm
		var result = items.ToArray();
		for (var i = result.Length - 1; i >= 1; i--)
			ArraySwap(result, i, GenerateNum(i));
		return result;

		static void ArraySwap(IList<T> items, int i, int j) => (items[i], items[j]) = (items[j], items[i]);
	}
}
