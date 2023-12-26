// perticula - core - KeyPair.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using Org.BouncyCastle.Crypto;

namespace core.Cryptography;

/// <summary>
///   Class KeyPair.
/// </summary>
public class KeyPair
{
	/// <summary>
	///   The key pair
	/// </summary>
	private readonly AsymmetricCipherKeyPair _keyPair;

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyPair" /> class.
	/// </summary>
	/// <param name="keyPair">The key pair.</param>
	internal KeyPair(AsymmetricCipherKeyPair keyPair) => _keyPair = keyPair;

	/// <summary>
	///   Converts to encryptedprivatekeystring.
	/// </summary>
	/// <param name="passPhrase">The pass phrase.</param>
	/// <returns>System.String.</returns>
	public string ToEncryptedPrivateKeyString(string passPhrase) =>
		KeyFactory.ToEncryptedPrivateKeyString(_keyPair.Private, passPhrase);

	/// <summary>
	///   Converts to publickeystring.
	/// </summary>
	/// <returns>System.String.</returns>
	public string ToPublicKeyString() => KeyFactory.ToPublicKeyString(_keyPair.Public);
}
