// perticula - core - ITokenGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Security;

/// <summary>
///   Creates security tokens for use by services
/// </summary>
public interface ITokenGenerator
{
	/// <summary>
	///   Generates a token that will automatically expire on the specified day.
	///   If no date is provided, "today's" date will be assumed. (e.g the token will expire at midnight UTC)
	/// </summary>
	/// <param name="today">The date on which to base to token</param>
	/// <returns>System.String.</returns>
	string GenerateAutoExpireToken(DateTime? today = null);

	/// <summary>
	///   Security authorization tokens contain an embedded guid that can
	///   be used to convey credentials. The token however expires shortly
	///   after being created.
	/// </summary>
	/// <param name="securityGuid">The security guid</param>
	/// <param name="duration">The duration of this token in seconds (1 - 9999)</param>
	/// <param name="starting">The time on which to base the expiration</param>
	/// <returns>System.String.</returns>
	string GenerateAuthorizationToken(Guid securityGuid, int duration, DateTime? starting = null);

	/// <summary>
	///   Decrypts a security authorization token and returns the security guid.
	///   Returns Guid.Empty if the token is invalid or expired.
	/// </summary>
	/// <param name="token">The security authorization token to decrypt</param>
	/// <returns>Guid.</returns>
	Guid ValidateAuthorizationToken(string token);

	/// <summary>
	///   Creates a new set of encryption keys that can be stored in the app settings.
	/// </summary>
	/// <returns>System.String.</returns>
	string GenerateNewEncryptionKeys();
}
