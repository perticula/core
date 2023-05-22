// perticula - core - StrongPasswordGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Random;

namespace core.Security;

/// <summary>
///   Class StrongPasswordGenerator.
///   Implements the <see cref="core.Security.IStrongPasswordGenerator" />
/// </summary>
/// <seealso cref="core.Security.IStrongPasswordGenerator" />
internal class StrongPasswordGenerator : IStrongPasswordGenerator
{
	/// <summary>
	///   Generates a password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="allowed">The set of characters the password will be randomly made of</param>
	/// <returns>System.String.</returns>
	public string Generate(int length = 10, string allowed = CommonCharSets.Alphanumerics) => Generate(length, new HashSet<char>(allowed));

	/// <summary>
	///   Generates a password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="allowed">The set of characters the password will be randomly made of</param>
	/// <returns>System.String.</returns>
	public string Generate(int length, HashSet<char> allowed)
	{
		using var random = new CryptographicRandomizer();
		return Generate(length, allowed, random);
	}

	/// <summary>
	///   Generates the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="allowed">The set of characters the password will be randomly made of</param>
	/// <param name="random">The random.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">length - length cannot be less than zero.</exception>
	/// <exception cref="System.ArgumentException">allowed cannot be empty.</exception>
	public string Generate(int length, HashSet<char> allowed, CryptographicRandom random)
	{
		if (length < 0)
			throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");

		if (allowed.Count == 0)
			throw new ArgumentException("allowed cannot be empty.");

		return random.GetChoiceStream(allowed).Take(length).AsString();
	}

	/// <summary>
	///   Generates a complex password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="requiredSets">The sets characters that the password must contain</param>
	/// <param name="predicate">An arbitrary function that the password must match</param>
	/// <returns>System.String.</returns>
	public string GenerateComplex(int length = 10, IEnumerable<string>? requiredSets = null, Func<string, bool>? predicate = null)
	{
		requiredSets ??= CommonCharSets.AlphanumericGroups;
		predicate    ??= _ => true;

		var asSets = requiredSets.Select(s => new HashSet<char>(s));
		return GenerateComplex(length, asSets.ToList(), predicate);
	}

	/// <summary>
	///   Generates a complex password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="requiredSets">The sets characters that the password must contain</param>
	/// <param name="predicate">An arbitrary function that the password must match</param>
	/// <returns>System.String.</returns>
	public string GenerateComplex(int length, IReadOnlyCollection<HashSet<char>> requiredSets, Func<string, bool> predicate)
	{
		using var random = new CryptographicRandomizer();
		return GenerateComplex(length, requiredSets, predicate, random);
	}

	/// <summary>
	///   Generates a complex password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="requiredSets">The sets characters that the password must contain</param>
	/// <param name="predicate">An arbitrary function that the password must match</param>
	/// <param name="random">The random.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">length - length cannot be less than the number of requiredSets.</exception>
	public string GenerateComplex(int length, IReadOnlyCollection<HashSet<char>> requiredSets, Func<string, bool> predicate, CryptographicRandom random)
	{
		if (length < requiredSets.Count)
			throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than the number of requiredSets.");

		var allowed = UnionAll(requiredSets);

		while (true)
		{
			var password         = Generate(length, allowed, random);
			var allRequiredMatch = requiredSets.All(s => s.Overlaps(password));
			if (allRequiredMatch && predicate(password))
				return password;
		}
	}

	/// <summary>
	///   Unions all specified hash sets.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="sets">The sets.</param>
	/// <returns>HashSet&lt;T&gt;.</returns>
	private static HashSet<T> UnionAll<T>(IEnumerable<HashSet<T>> sets)
	{
		var result = new HashSet<T>();
		foreach (var s in sets)
			result.UnionWith(s);
		return result;
	}
}
