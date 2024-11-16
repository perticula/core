// perticula - core - Randomizer.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

/// <summary>
///   Class Randomizer.
/// </summary>
public static class Randomizer
{
	/// <summary>
	///   The random any
	/// </summary>
	private static System.Random? _randomAny;

	/// <summary>
	///   Returns a random number generator based on the provided seed
	/// </summary>
	/// <param name="seed">The seed.</param>
	/// <returns>System.Random.</returns>
	/// <exception cref="System.NotSupportedException">unknown random seed: {seed}</exception>
	public static System.Random Get(RandomSeeds seed)
	{
		var now = DateTime.Now;
		switch (seed)
		{
			case RandomSeeds.Random:
				// Reuse the same randomizer for better results
				return _randomAny ??= new System.Random();
			case RandomSeeds.PerMinute:
				// Set the same seed per minute
				var perMin = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
				return new System.Random((int)perMin.Ticks % int.MaxValue);
			case RandomSeeds.PerHour:
				// Set the same seed per hour
				var perHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
				return new System.Random((int)perHour.Ticks % int.MaxValue);
			case RandomSeeds.PerDay:
				// Set the same seed per day
				var perDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
				return new System.Random((int)perDay.Ticks % int.MaxValue);
			default:
				throw new NotSupportedException($"unknown random seed: {seed}");
		}
	}

	/// <summary>
	///   Returns the list in a random order based on the selected seed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">This list to randomize</param>
	/// <param name="seed">The seed used to generate the random order</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> list, RandomSeeds seed = RandomSeeds.Random)
	{
		var random = Get(seed);
		return list.OrderBy(_ => random.Next());
	}

	/// <summary>
	///   Returns the list in a random order based on the selected seed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">This list to randomize</param>
	/// <param name="where">Filter the list before selecting a random item</param>
	/// <param name="seed">The seed used to generate the random order</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> list, Func<T, bool> where,
		RandomSeeds                                                 seed = RandomSeeds.Random)
	{
		var random = Get(seed);
		return list.Where(where).OrderBy(_ => random.Next());
	}

	/// <summary>
	///   Returns the list in a random order based on the selected seed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">This list to randomize</param>
	/// <param name="where">Filter the list before selecting a random item</param>
	/// <param name="seed">The seed used to generate the random order</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	public static IEnumerable<T> Randomize<T>(this IEnumerable<T> list, Func<T, bool?> where,
		RandomSeeds                                                 seed = RandomSeeds.Random)
	{
		var random = Get(seed);
		return list.Where(i => where(i) ?? false).OrderBy(_ => random.Next());
	}

	/// <summary>
	///   Returns a random item from the list based on the selected seed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">This list to randomize</param>
	/// <param name="seed">The seed used to generate the random order</param>
	/// <returns>T.</returns>
	public static T? RandomItem<T>(this IEnumerable<T> list, RandomSeeds seed = RandomSeeds.Random) =>
		list.Randomize(seed).FirstOrDefault();

	/// <summary>
	///   Returns a random item from the list based on the selected seed
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">This list to randomize</param>
	/// <param name="where">Filter the list before selecting a random item</param>
	/// <param name="seed">The seed used to generate the random order</param>
	/// <returns>T.</returns>
	public static T?
		RandomItem<T>(this IEnumerable<T> list, Func<T, bool> where, RandomSeeds seed = RandomSeeds.Random) =>
		list.Randomize(where, seed).FirstOrDefault();
}
