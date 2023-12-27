// perticula - core - SimpleEncryption.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace core.Security;

/// <summary>
///   Class SimpleEncrypt.
/// </summary>
public static class SimpleEncrypt
{
	/// <summary>
	///   The aes key
	/// </summary>
	private static readonly string AesKey;

	/// <summary>
	///   Initializes static members of the <see cref="SimpleEncrypt" /> class.
	/// </summary>
	/// <exception cref="System.Configuration.ConfigurationErrorsException">Missing User Secret for aesKey</exception>
	static SimpleEncrypt()
	{
		var config = new ConfigurationManager()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddUserSecrets(Assembly.GetExecutingAssembly())
			.AddJsonFile("appsettings.json")
			.Build();

		var fromEnv = Environment.GetEnvironmentVariable("aesKey");
		if (!string.IsNullOrEmpty(fromEnv))
		{
			AesKey = fromEnv;
			return;
		}

		var fromUserSecret = config["aesKey"];
		if (!string.IsNullOrEmpty(fromUserSecret))
		{
			AesKey = fromUserSecret;
			return;
		}


		throw new ConfigurationErrorsException("Missing User Secret for aesKey");
	}

	/// <summary>
	///   Adds an extension method to decrypt the given value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string DecryptValue(this string value) => Decrypt(value);

	/// <summary>
	///   Adds an extension method to encrypts the given value
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	public static string EncryptValue(this string value) => Encrypt(value);

	/// <summary>
	///   Decrypts the specified encrypted.
	/// </summary>
	/// <param name="value">The encrypted value.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentNullException">value</exception>
	/// <exception cref="System.NullReferenceException">Encryption failed: Unable to create AesCryptoServiceProvider</exception>
	public static string Decrypt(string value)
	{
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
		var payload = Convert.FromBase64String(value);

		using var aes = Aes.Create() ??
		                throw new NullReferenceException("Encryption failed: Unable to create AesCryptoServiceProvider");

		aes.Key     = Convert.FromBase64String(AesKey);
		aes.Mode    = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;

		//get first 16 bytes of payload and use it as the IV for decryption
		var iv = new byte[16];
		Array.Copy(payload, 0, iv, 0, iv.Length);

		using var ms = new MemoryStream();
		using (var cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, iv), CryptoStreamMode.Write))
		using (var binaryWriter = new BinaryWriter(cs))
		{
			binaryWriter.Write(
				payload,
				iv.Length,
				payload.Length - iv.Length
			);
		}

		return System.Text.Encoding.Default.GetString(ms.ToArray());
	}

	/// <summary>
	///   Encrypts the specified unencrypted value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentNullException">value</exception>
	/// <exception cref="System.NullReferenceException">Encryption failed: Unable to create AesCryptoServiceProvider</exception>
	public static string Encrypt(string value)
	{
		if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
		var payload = System.Text.Encoding.UTF8.GetBytes(value);

		using var aes = Aes.Create() ??
		                throw new NullReferenceException("Encryption failed: Unable to create AesCryptoServiceProvider");

		aes.Key     = Convert.FromBase64String(AesKey);
		aes.Mode    = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;

		aes.GenerateIV();
		var       iv           = aes.IV;
		using var encryptor    = aes.CreateEncryptor(aes.Key, iv);
		using var cipherStream = new MemoryStream();
		using (var tCryptoStream = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write))
		using (var tbinaryWriter = new BinaryWriter(tCryptoStream))
		{
			//prepend IV to payload
			cipherStream.Write(iv);
			tbinaryWriter.Write(payload);
			tCryptoStream.FlushFinalBlock();
		}

		return Convert.ToBase64String(cipherStream.ToArray());
	}
}
