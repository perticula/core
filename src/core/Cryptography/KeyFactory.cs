// perticula - core - KeyFactory.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace core.Cryptography;

/// <summary>
///   Class KeyFactory.
/// </summary>
public class KeyFactory
{
	/// <summary>
	///   The key encryption algorithm
	/// </summary>
	private static readonly string KeyEncryptionAlgorithm = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id;

	/// <summary>
	///   Converts to encryptedprivatekeystring.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="passPhrase">The pass phrase.</param>
	/// <returns>System.String.</returns>
	public static string ToEncryptedPrivateKeyString(AsymmetricKeyParameter key, string passPhrase)
	{
		var salt         = new byte[16];
		var secureRandom = SecureRandom.GetInstance("SHA256PRNG");
		secureRandom.SetSeed(secureRandom.GenerateSeed(16));
		secureRandom.NextBytes(salt);

		return Convert.ToBase64String(PrivateKeyFactory.EncryptKey(KeyEncryptionAlgorithm, passPhrase.ToCharArray(), salt, 10, key));
	}

	/// <summary>
	///   Froms the encrypted private key string.
	/// </summary>
	/// <param name="privateKey">The private key.</param>
	/// <param name="passPhrase">The pass phrase.</param>
	/// <returns>AsymmetricKeyParameter.</returns>
	public static AsymmetricKeyParameter FromEncryptedPrivateKeyString(string privateKey, string passPhrase) => PrivateKeyFactory.DecryptKey(passPhrase.ToCharArray(), Convert.FromBase64String(privateKey));

	/// <summary>
	///   Converts to publickeystring.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>System.String.</returns>
	public static string ToPublicKeyString(AsymmetricKeyParameter key) => Convert.ToBase64String(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key).ToAsn1Object().GetDerEncoded());

	/// <summary>
	///   Froms the public key string.
	/// </summary>
	/// <param name="publicKey">The public key.</param>
	/// <returns>AsymmetricKeyParameter.</returns>
	public static AsymmetricKeyParameter FromPublicKeyString(string publicKey) => PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
}
