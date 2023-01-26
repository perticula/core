// perticula - core - IStrongPasswordGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Random;

namespace core.Security;

/// <summary>
///   Interface IStrongPasswordGenerator
/// </summary>
public interface IStrongPasswordGenerator
{
	/// <summary>
	///   Generates a password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="allowed">The set of characters the password will be randomly made of</param>
	/// <returns>System.String.</returns>
	string Generate(int length = 10, string allowed = CommonCharSets.Alphanumerics);

	/// <summary>
	///   Generates a password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="allowed">The set of characters the password will be randomly made of</param>
	/// <returns>System.String.</returns>
	string Generate(int length, HashSet<char> allowed);

	/// <summary>
	///   Generates a password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="allowed">The set of characters the password will be randomly made of</param>
	/// <param name="random">The random.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">length - length cannot be less than zero.</exception>
	/// <exception cref="System.ArgumentException">allowed cannot be empty.</exception>
	string Generate(int length, HashSet<char> allowed, CryptographicRandom random);

	/// <summary>
	///   Generates a complex password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="requiredSets">The sets characters that the password must contain</param>
	/// <param name="predicate">An arbitrary function that the password must match</param>
	/// <returns>System.String.</returns>
	string GenerateComplex(int length = 10, IEnumerable<string>? requiredSets = null, Func<string, bool>? predicate = null);

	/// <summary>
	///   Generates a complex password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="requiredSets">The sets characters that the password must contain</param>
	/// <param name="predicate">An arbitrary function that the password must match</param>
	/// <returns>System.String.</returns>
	string GenerateComplex(int length, IReadOnlyCollection<HashSet<char>> requiredSets, Func<string, bool> predicate);

	/// <summary>
	///   Generates a complex password of the specified length.
	/// </summary>
	/// <param name="length">The length.</param>
	/// <param name="requiredSets">The sets characters that the password must contain</param>
	/// <param name="predicate">An arbitrary function that the password must match</param>
	/// <param name="random">The random.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">length - length cannot be less than the number of requiredSets.</exception>
	string GenerateComplex(int length, IReadOnlyCollection<HashSet<char>> requiredSets, Func<string, bool> predicate, CryptographicRandom random);
}
