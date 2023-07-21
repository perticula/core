// perticula - core - KeyGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace core.Cryptography;

/// <summary>
///   Class KeyGenerator.
/// </summary>
public class KeyGenerator
{
	/// <summary>
	///   The key generator
	/// </summary>
	private readonly IAsymmetricCipherKeyPairGenerator _keyGenerator;

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyGenerator" /> class.
	/// </summary>
	public KeyGenerator() : this(256) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyGenerator" /> class.
	/// </summary>
	/// <param name="keySize">Size of the key.</param>
	public KeyGenerator(int keySize)
	{
		var secureRandom = SecureRandom.GetInstance("SHA256PRNG");
		secureRandom.SetSeed(secureRandom.GenerateSeed(keySize));

		var keyParams = new KeyGenerationParameters(secureRandom, keySize);
		_keyGenerator = new ECKeyPairGenerator();
		_keyGenerator.Init(keyParams);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyGenerator" /> class.
	/// </summary>
	/// <param name="keySize">Size of the key.</param>
	/// <param name="seed">The seed.</param>
	public KeyGenerator(int keySize, byte[] seed)
	{
		var secureRandom = SecureRandom.GetInstance("SHA256PRNG");
		secureRandom.SetSeed(seed);

		var keyParams = new KeyGenerationParameters(secureRandom, keySize);
		_keyGenerator = new ECKeyPairGenerator();
		_keyGenerator.Init(keyParams);
	}

	/// <summary>
	///   Creates this instance.
	/// </summary>
	/// <returns>KeyGenerator.</returns>
	public static KeyGenerator Create() => new();

	/// <summary>
	///   Generates the key pair.
	/// </summary>
	/// <returns>KeyPair.</returns>
	public KeyPair GenerateKeyPair() => new(_keyGenerator.GenerateKeyPair());
}
