// perticula - core - TestableAppSettings.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Config;

/// <summary>
///   Class TestableAppSettings.
///   Used explicitly in unit tests
///   Implements the <see cref="IAppSettings" />
/// </summary>
/// <seealso cref="IAppSettings" />
[Obsolete("This method should only be used for unit tests;  otherwise prefer the injected IAppSettings class")]
public class TestableAppSettings : IAppSettings
{
	/// <summary>
	///   The internal
	/// </summary>
	private readonly IAppSettings _internal = new AppSettings();

	/// <summary>
	///   Indicates whether a setting exists. Supports machine specific
	///   setting names.
	/// </summary>
	/// <param name="name">The name of the setting</param>
	/// <returns><c>true</c> if the specified name has setting; otherwise, <c>false</c>.</returns>
	public bool HasSetting(string name) => _internal.HasSetting(name);

	/// <summary>
	///   Returns the setting by name. Supports machine specific setting names.
	/// </summary>
	/// <param name="name">The name of the setting</param>
	/// <returns>System.String.</returns>
	public string GetSetting(string name) => _internal.GetSetting(name);

	/// <summary>
	///   Returns the setting by name. Supports machine specific setting names.
	///   The returned value will be decrypted.  Encryption should happen out of process and is not directly supported
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>System.String.</returns>
	public string GetEncryptedSetting(string name) => _internal.GetEncryptedSetting(name);

	/// <summary>
	///   Returns the setting by name and type. Supports machine specific
	///   setting names. Throws an exception if the value cannot be converted
	///   to the requested type.
	/// </summary>
	/// <typeparam name="T">The type of the value to expect</typeparam>
	/// <param name="name">The name of the setting</param>
	/// <returns>T.</returns>
	public T GetSetting<T>(string name) => _internal.GetSetting<T>(name);

	/// <summary>
	///   Gets the cached setting.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="intervalMs">The interval ms.</param>
	/// <param name="name">The name.</param>
	/// <returns>ICachedAppSettings&lt;T&gt;.</returns>
	public ICachedAppSettings<T> GetCachedSetting<T>(int intervalMs, string name) =>
		_internal.GetCachedSetting<T>(intervalMs, name);

	/// <summary>
	///   Returns a connection string from the settings by name. Returns null
	///   if the connection string is not found. Supports machine specific
	///   setting names.
	/// </summary>
	/// <param name="name">The connection string name</param>
	/// <returns>System.String.</returns>
	public string ConnectionString(string name) => _internal.ConnectionString(name);

	/// <summary>
	///   Indicates whether a connection string exists. Supports machine specific
	///   setting names.
	/// </summary>
	/// <param name="name">The name of the setting</param>
	/// <returns><c>true</c> if [has connection string] [the specified name]; otherwise, <c>false</c>.</returns>
	public bool HasConnectionString(string name) => _internal.HasConnectionString(name);
}
