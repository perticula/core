namespace core;

/// <summary>
/// Class TestableAppSettings.
/// Used explicitly in unit tests
/// Implements the <see cref="IAppSettings" />
/// </summary>
/// <seealso cref="IAppSettings" />
[Obsolete("This method should only be used for unit tests;  otherwise prefer the injected IAppSettings class")]
public class TestableAppSettings : IAppSettings
{
  private readonly IAppSettings _internal = new AppSettings();

  /// <summary>
  /// Indicates whether a setting exists. Supports machine specific
  /// setting names.
  /// </summary>
  /// <param name="name">The name of the setting</param>
  /// <returns><c>true</c> if the specified name has setting; otherwise, <c>false</c>.</returns>
  public bool HasSetting(string name) => _internal.HasSetting(name);

  /// <summary>
  /// Returns the setting by name. Supports machine specific setting names.
  /// </summary>
  /// <param name="name">The name of the setting</param>
  /// <param name="def">A default value if the setting is not set</param>
  /// <returns>System.String.</returns>
  public string GetSetting(string name, string def = null) => _internal.GetSetting(name, def);

  /// <summary>
  /// Returns the setting by name. Supports machine specific setting names.
  /// The returned value will be decrypted.  Encryption should happen out of process and is not directly supported
  /// </summary>
  /// <param name="name">The name.</param>
  /// <param name="def">The definition.</param>
  /// <returns>System.String.</returns>
  public string GetEncryptedSetting(string name, string def = null) => _internal.GetEncryptedSetting(name, def);

  /// <summary>
  /// Returns the setting by name and type. Supports machine specific
  /// setting names. Throws an exception if the value cannot be converted
  /// to the requested type.
  /// </summary>
  /// <typeparam name="T">The type of the value to expect</typeparam>
  /// <param name="name">The name of the setting</param>
  /// <param name="def">A default value if the setting is not set</param>
  /// <returns>T.</returns>
  public T GetSetting<T>(string name, T def = default) => _internal.GetSetting(name, def);

  /// <summary>
  /// Gets the cached setting.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="intervalMs">The interval ms.</param>
  /// <param name="name">The name.</param>
  /// <param name="def">The definition.</param>
  /// <returns>ICachedAppSettings&lt;T&gt;.</returns>
  public ICachedAppSettings<T> GetCachedSetting<T>(int intervalMs, string name, T def = default) => _internal.GetCachedSetting(intervalMs, name, def);

  /// <summary>
  /// Returns a connection string from the settings by name. Returns null
  /// if the connection string is not found. Supports machine specific
  /// setting names.
  /// </summary>
  /// <param name="name">The connection string name</param>
  /// <returns>System.String.</returns>
  public string ConnectionString(string name) => _internal.ConnectionString(name);

  /// <summary>
  /// Indicates whether a connection string exists. Supports machine specific
  /// setting names.
  /// </summary>
  /// <param name="name">The name of the setting</param>
  /// <returns><c>true</c> if [has connection string] [the specified name]; otherwise, <c>false</c>.</returns>
  public bool HasConnectionString(string name) => _internal.HasConnectionString(name);
}
