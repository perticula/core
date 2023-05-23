// perticula - core - CryptographicApiRandomGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Security.Cryptography;

namespace core.Random;

/// <summary>
///   Class CryptographicApiRandomGenerator.
///   Implements the <see cref="core.Random.IRandomGenerator" />
///   Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="core.Random.IRandomGenerator" />
/// <seealso cref="System.IDisposable" />
public class CryptographicApiRandomGenerator : IRandomGenerator, IDisposable
{
	/// <summary>
	///   The random number generator
	/// </summary>
	private readonly RandomNumberGenerator _randomNumberGenerator;

	/// <summary>
	///   Initializes a new instance of the <see cref="CryptographicApiRandomGenerator" /> class.
	/// </summary>
	public CryptographicApiRandomGenerator() : this(RandomNumberGenerator.Create()) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="CryptographicApiRandomGenerator" /> class.
	/// </summary>
	/// <param name="randomNumberGenerator">The random number generator.</param>
	/// <exception cref="System.ArgumentNullException">randomNumberGenerator</exception>
	public CryptographicApiRandomGenerator(RandomNumberGenerator randomNumberGenerator) => _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));

	/// <summary>
	///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		_randomNumberGenerator.Dispose();
		GC.SuppressFinalize(this);
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public void AddSeedMaterial(byte[] seed)
	{
		// We don't care about the seed
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="inSeed">The in seed.</param>
	public void AddSeedMaterial(ReadOnlySpan<byte> inSeed)
	{
		// We don't care about the seed
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public void AddSeedMaterial(long seed)
	{
		// We don't care about the seed
	}

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public void NextBytes(byte[] bytes) => _randomNumberGenerator.GetBytes(bytes);

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="start">The start.</param>
	/// <param name="len">The length.</param>
	/// <exception cref="System.ArgumentException">Start offset cannot be negative - start</exception>
	/// <exception cref="System.ArgumentException">Byte array too small for requested offset and length</exception>
	public void NextBytes(byte[] bytes, int start, int len)
	{
		_randomNumberGenerator.GetBytes(bytes, start, len);
		if (start < 0) throw new ArgumentException("Start offset cannot be negative", nameof(start));
		if (start > bytes.Length - len) throw new ArgumentException("Byte array too small for requested offset and length");

		if (bytes.Length == len && start == 0)
		{
			NextBytes(bytes);
		}
		else
		{
			var tmp = new byte[len];
			NextBytes(tmp);
			tmp.CopyTo(bytes, start);
		}
	}

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public void NextBytes(Span<byte> bytes) => _randomNumberGenerator.GetBytes(bytes);
}
