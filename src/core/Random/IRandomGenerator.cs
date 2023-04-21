// perticula - core - IRandomGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

/// <summary>
///   Interface IRandomGenerator
///   A generic interface for random number generators that create random bytes.
/// </summary>
public interface IRandomGenerator
{
	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	void AddSeedMaterial(byte[] seed);

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	void AddSeedMaterial(ReadOnlySpan<byte> seed);

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	void AddSeedMaterial(long seed);

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	void NextBytes(byte[] bytes);

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="start">The start.</param>
	/// <param name="len">The length.</param>
	void NextBytes(byte[] bytes, int start, int len);

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	void NextBytes(Span<byte> bytes);
}
