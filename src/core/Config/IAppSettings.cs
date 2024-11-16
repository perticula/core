// perticula - core - IAppSettings.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Config;

/// <summary>
///   Interface IAppSettings
/// </summary>
public interface IAppSettings
{
	/// <summary>
	///   Indicates whether a setting exists. Supports machine specific
	///   setting names.
	/// </summary>
	/// <param name="name">The name of the setting</param>
	/// <returns><c>true</c> if the specified name has setting; otherwise, <c>false</c>.</returns>
	bool HasSetting(string name);

	/// <summary>
	///   Returns the setting by name. Supports machine specific setting names.
	/// </summary>
	/// <param name="name">The name of the setting</param>
	/// <returns>System.String.</returns>
	string GetSetting(string name);

	/// <summary>
	///   Returns the setting by name and type. Supports machine specific
	///   setting names. Throws an exception if the value cannot be converted
	///   to the requested type.
	/// </summary>
	/// <typeparam name="T">The type of the value to expect</typeparam>
	/// <param name="name">The name of the setting</param>
	/// <returns>T.</returns>
	T GetSetting<T>(string name);

	/// <summary>
	///   Returns the setting by name. Supports machine specific setting names.
	///   The returned value will be decrypted.  Encryption should happen out of process and is not directly supported
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>System.String.</returns>
	string GetEncryptedSetting(string name);

	/// <summary>
	///   Gets the cached setting.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="intervalMs">The interval ms.</param>
	/// <param name="name">The name.</param>
	/// <returns>ICachedAppSettings&lt;T&gt;.</returns>
	ICachedAppSettings<T> GetCachedSetting<T>(int intervalMs, string name);

	/// <summary>
	///   Returns a connection string from the settings by name. Returns null
	///   if the connection string is not found. Supports machine specific
	///   setting names.
	/// </summary>
	/// <param name="name">The connection string name</param>
	/// <returns>System.String.</returns>
	string ConnectionString(string name);

	/// <summary>
	///   Indicates whether a connection string exists. Supports machine specific
	///   setting names.
	/// </summary>
	/// <param name="name">The name of the setting</param>
	/// <returns><c>true</c> if [has connection string] [the specified name]; otherwise, <c>false</c>.</returns>
	bool HasConnectionString(string name);
}
