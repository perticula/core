// perticula - core - TokenGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using core.Config;
using Newtonsoft.Json;

namespace core.Security;

/// <summary>
///   Class TokenGenerator.
///   Implements the <see cref="Security.ITokenGenerator" />
/// </summary>
/// <seealso cref="Security.ITokenGenerator" />
internal class TokenGenerator : ITokenGenerator
{
	/// <summary>
	///   The salt
	/// </summary>
	private static int _salt = 1;

	/// <summary>
	///   The security iv
	/// </summary>
	private readonly byte[] _securityIv;

	/// <summary>
	///   The security key
	/// </summary>
	private readonly byte[] _securityKey;

	/// <summary>
	///   The does nothing
	/// </summary>
	private int _doesNothing = 0xD0;

	/// <summary>
	///   Initializes a new instance of the <see cref="TokenGenerator" /> class.
	/// </summary>
	/// <param name="settings">The settings.</param>
	/// <param name="settingKey">The setting key.</param>
	/// <exception cref="System.ArgumentNullException">settings</exception>
	/// <exception cref="System.ArgumentNullException">settingKey</exception>
	/// <exception cref="System.Configuration.ConfigurationErrorsException">
	///   Configuration is missing a 'security key' setting
	///   called '{settingKey}'
	/// </exception>
	/// <exception cref="System.Configuration.ConfigurationErrorsException">
	///   'Security key' setting '{settingKey}' is null or
	///   empty
	/// </exception>
	/// <exception cref="System.Configuration.ConfigurationErrorsException">
	///   'Security key' setting '{settingKey}' has an
	///   invalid value (expected key|iv)
	/// </exception>
	/// <exception cref="System.Configuration.ConfigurationErrorsException"></exception>
	public TokenGenerator(IAppSettings settings, string settingKey = "SsoAuthorizationKey")
	{
				ArgumentNullException.ThrowIfNull(settings);
				if (string.IsNullOrWhiteSpace(settingKey)) throw new ArgumentNullException(nameof(settingKey));

		if (!settings.HasSetting(settingKey)) throw new ConfigurationErrorsException($"Configuration is missing a 'security key' setting called '{settingKey}'");

		var keyBoth = settings.GetSetting(settingKey);
		if (string.IsNullOrEmpty(keyBoth)) throw new ConfigurationErrorsException($"'Security key' setting '{settingKey}' is null or empty");

		var key = keyBoth.Split('|');
		if (key.Length < 2)
			throw new ConfigurationErrorsException($"'Security key' setting '{settingKey}' has an invalid value (expected key|iv)");
		_securityKey = Convert.FromBase64String(key[0].Trim());
		_securityIv  = Convert.FromBase64String(key[1].Trim());

		if (_securityKey == null) throw new ConfigurationErrorsException();
	}

	/// <summary>
	///   Security authorization tokens contain an embedded guid that can
	///   be used to convey credentials. The token however expires shortly
	///   after being created.
	/// </summary>
	/// <param name="securityGuid">The security guid</param>
	/// <param name="duration">The duration of this token in seconds (1 - 9999)</param>
	/// <param name="starting">The time on which to base the expiration</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">duration - duration must be between 1 and 9999</exception>
	/// <exception cref="System.ArgumentException">starting time may not be in the future - starting</exception>
	/// <exception cref="System.NullReferenceException">Encryption failed: Unable to create AesCryptoServiceProvider</exception>
	public string GenerateAuthorizationToken(Guid securityGuid, int duration, DateTime? starting = null)
	{
		if (duration is < 1 or >= 9999) throw new ArgumentOutOfRangeException(nameof(duration), "duration must be between 1 and 9999");

		var startOn = starting ?? DateTime.UtcNow;
		if (startOn.ToUniversalTime() > DateTime.UtcNow.AddMinutes(30)) throw new ArgumentException("starting time may not be in the future", nameof(starting));

		using var aes = Aes.Create() ?? throw new NullReferenceException("Encryption failed: Unable to create AesCryptoServiceProvider");

		aes.Key     = _securityKey;
		aes.IV      = _securityIv;
		aes.Mode    = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;

		using var encrypt      = aes.CreateEncryptor(aes.Key, aes.IV);
		using var stream       = new MemoryStream();
		using var cryptoStream = new CryptoStream(stream, encrypt, CryptoStreamMode.Write);
		using (var streamWriter = new StreamWriter(cryptoStream))
		{
			streamWriter.Write(
				JsonConvert.SerializeObject(
					// Anonymous class payload
					new
					{
						expires  = startOn.AddSeconds(duration),
						security = securityGuid,
						salt     = _salt++ % 999
					}
				)
			);
		}

		return ToUrlFriendlyBase64(stream.ToArray());
	}

	/// <summary>
	///   Creates a new set of encryption keys that can be stored in the app settings.
	/// </summary>
	/// <returns>System.String.</returns>
	public string GenerateNewEncryptionKeys()
	{
		using var aes = Aes.Create();
		return Convert.ToBase64String(aes.Key) + "|" + Convert.ToBase64String(aes.IV);
	}

	/// <summary>
	///   Decrypts a security authorization token and returns the security guid.
	///   Returns Guid.Empty if the token is invalid or expired.
	/// </summary>
	/// <param name="token">The security authorization token to decrypt</param>
	/// <returns>Guid.</returns>
	/// <exception cref="System.NullReferenceException">Encryption failed: Unable to create AesCryptoServiceProvider</exception>
	public Guid ValidateAuthorizationToken(string token)
	{
		if (string.IsNullOrEmpty(token)) return Guid.Empty;
		try
		{
			var       encrypted = FromUrlFriendlyBase64(token);
			using var aes       = Aes.Create() ?? throw new NullReferenceException("Encryption failed: Unable to create AesCryptoServiceProvider");

			aes.Key     = _securityKey;
			aes.IV      = _securityIv;
			aes.Mode    = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			using var decrypt      = aes.CreateDecryptor(aes.Key, aes.IV);
			using var stream       = new MemoryStream(encrypted);
			using var cryptoStream = new CryptoStream(stream, decrypt, CryptoStreamMode.Read);
			using var streamReader = new StreamReader(cryptoStream);
			var       json         = streamReader.ReadToEnd();
			if (string.IsNullOrEmpty(json)) return Guid.Empty;
			var payload = JsonConvert.DeserializeAnonymousType(json, new
			{
				expires  = DateTime.MinValue,
				security = Guid.Empty,
				salt     = 0
			});
			if (payload != null)
				return DateTime.UtcNow > payload.expires
					       ? Guid.Empty
					       : payload.security;
			return Guid.Empty;
		}
		catch
		{
			// You didn't see anything!
			return Guid.Empty;
		}
	}

	/// <summary>
	///   Generates a token that will automatically expire on the specified day.
	///   If no date is provided, "today's" date will be assumed. (e.g the token will expire at midnight UTC)
	/// </summary>
	/// <param name="today">The date on which to base to token</param>
	/// <returns>System.String.</returns>
	public string GenerateAutoExpireToken(DateTime? today = null) => SaltedBase64Token(today ?? DateTime.UtcNow.Date);

	/// <summary>
	///   Salteds the base64 token.
	/// </summary>
	/// <param name="today">The today.</param>
	/// <returns>System.String.</returns>
	private string SaltedBase64Token(DateTime today)
	{
		Nop();
		var digest = new StringBuilder();
		digest.Append("-🔐🪪-");
		digest.Append(today.Day.ToString("00"));
		digest.Append(today.Month.ToString("00"));
		digest.Append("-🤐🤫-");
		digest.Append(today.Year.ToString("0000"));
		digest.Append("-🤝-");
		var bytes = System.Text.Encoding.UTF8.GetBytes(digest.ToString());
		return ToBase64Token(SHA256.HashData(bytes));

		void Nop() // This is just to make it harder to reverse engineer the obfuscated code, it does nothing useful.
		{
			switch (_doesNothing)
			{
				case >= 0xD0:
					_doesNothing = 0x00;
					break;
				default:
					_doesNothing++;
					break;
			}
		}
	}

	/// <summary>
	///   Converts to a base 64 string and strips out any non-alphanumeric
	///   characters.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>System.String.</returns>
	internal string ToBase64Token(byte[] bytes)
		=> Convert.ToBase64String(bytes)
		          .ToCharArray() // Append only letters and numbers to the string builder
		          .Aggregate(new StringBuilder(), (sb, ch) => sb.Append(char.IsLetterOrDigit(ch) ? ch.ToString() : ""))
		          .ToString();

	/// <summary>
	///   Converts the data into base 64 and replaces reserved url
	///   characters with acceptable ones.
	/// </summary>
	/// <param name="bytes">The data to be converted</param>
	/// <returns>System.String.</returns>
	internal string ToUrlFriendlyBase64(byte[] bytes)
		=> Convert.ToBase64String(bytes)
		          .ToCharArray() // Convert (/ to _) and (+ to -) and (= to ,)
		          .Aggregate(new StringBuilder(), (sb, ch) =>
		          {
			          return ch switch
			                 {
				                 '/' => sb.Append('_'),
				                 '+' => sb.Append('-'),
				                 '=' => sb.Append(','),
				                 'o' => sb.Append('~'),
				                 _   => sb.Append(ch)
			                 };
		          }).ToString();

	/// <summary>
	///   Reverses the process of used to create the url friendly base 64 data
	/// </summary>
	/// <param name="data">The data to be decoded</param>
	/// <returns>System.Byte[].</returns>
	internal byte[] FromUrlFriendlyBase64(string data)
		=> Convert.FromBase64String(data.ToCharArray()
		                                .Aggregate(new StringBuilder(), (sb, ch) =>
		                                {
			                                return ch switch
			                                       {
				                                       '~' => sb.Append('o'),
				                                       '_' => sb.Append('/'),
				                                       '-' => sb.Append('+'),
				                                       ',' => sb.Append('='),
				                                       _   => sb.Append(ch)
			                                       };
		                                }).ToString());
}
