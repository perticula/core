using System.Configuration;

namespace core;

/// <summary>
/// Class AppSettings.
/// Implements the <see cref="IAppSettings" />
/// </summary>
/// <seealso cref="IAppSettings" />
internal class AppSettings : IAppSettings
{
  /// <summary>
  /// Indicates whether a setting exists. Supports machine specific
  /// setting names.
  /// </summary>
  /// <param name="name">The name of the setting</param>
  /// <returns><c>true</c> if the specified name has setting; otherwise, <c>false</c>.</returns>
  public bool HasSetting(string name)
  {
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
    var machineSettingKey = $"{Environment.MachineName}:{name}";
    if (ConfigurationManager.AppSettings[machineSettingKey] != null) return true;
    if (ConfigurationManager.AppSettings[name] != null) return true;
    return false;
  }

  /// <summary>
  /// Returns the setting by name.Supports machine specific
  /// setting names.
  /// </summary>
  /// <param name="name">The name of the setting</param>
  /// <param name="def">A default value if the setting is not set</param>
  /// <returns>System.String.</returns>
  /// <exception cref="ArgumentNullException">name</exception>
  public string GetSetting(string name, string def = null)
  {
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

    // Machine specific settings take priority
    var machineSettingKey = $"{Environment.MachineName}:{name}";
    if (ConfigurationManager.AppSettings[machineSettingKey] != null)
      name = machineSettingKey;
    if (ConfigurationManager.AppSettings[name] == null)
      return def;
    return ConfigurationManager.AppSettings[name];
  }

  /// <summary>
  /// Returns the setting by name. Supports machine specific setting names.
  /// The returned value will be decrypted.  Encryption should happen out of process and is not directly supported
  /// </summary>
  /// <param name="name">The name.</param>
  /// <param name="def">The definition.</param>
  /// <returns>System.String.</returns>
  public string GetEncryptedSetting(string name, string def = null) => GetSetting(name, def).DecryptValue();

  /// <summary>
  /// Returns the setting by name and type. Supports machine specific
  /// setting names. Throws an exception if the value cannot be converted
  /// to the requested type.
  /// </summary>
  /// <typeparam name="T">The type of the value to expect</typeparam>
  /// <param name="name">The name of the setting</param>
  /// <param name="def">A default value if the setting is not set</param>
  /// <returns>T.</returns>
  /// <exception cref="ArgumentNullException">name</exception>
  public T GetSetting<T>(string name, T def = default)
  {
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
    var value = GetSetting(name);
    if (value == null) return def;
    return Serialize.FromString<T>(value);
  }

  /// <summary>
  /// Returns a connection string from the settings by name. Returns null
  /// if the connection string is not found. Supports machine specific
  /// setting names.
  /// </summary>
  /// <param name="name">The connection string name</param>
  /// <returns>System.String.</returns>
  public string ConnectionString(string name)
  {
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

    // Machine specific settings take priority
    var machineSettingKey = $"{Environment.MachineName}:{name}";
    if (ConfigurationManager.ConnectionStrings[machineSettingKey] != null)
      name = machineSettingKey;
    if (ConfigurationManager.ConnectionStrings[name] == null)
      return null;
    return ConfigurationManager.ConnectionStrings[name].ConnectionString;
  }

  /// <summary>
  /// Indicates whether a connection string exists. Supports machine specific
  /// setting names.
  /// </summary>
  /// <param name="name">The name of the setting</param>
  /// <returns><c>true</c> if [has connection string] [the specified name]; otherwise, <c>false</c>.</returns>
  public bool HasConnectionString(string name)
  {
    if (string.IsNullOrEmpty(name)) return false;
    var machineSettingKey = $"{Environment.MachineName}:{name}";
    if (ConfigurationManager.ConnectionStrings[machineSettingKey] != null) return true;
    if (ConfigurationManager.ConnectionStrings[name] != null) return true;
    return false;
  }

  /// <summary>
  /// Gets the cached setting.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="intervalMs">The interval ms.</param>
  /// <param name="name">The name.</param>
  /// <param name="def">The definition.</param>
  /// <returns>ICachedAppSettings&lt;T&gt;.</returns>
  public ICachedAppSettings<T> GetCachedSetting<T>(int intervalMs, string name, T def = default) => new CachedAppSettings<T>(this, intervalMs, name, def);
}
