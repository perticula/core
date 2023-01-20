namespace core;

/// <summary>
/// Interface IRegistrySettings
/// </summary>
public interface IRegistrySettings
{
  /// <summary>
  /// Indicates whether a registry setting exists.
  /// </summary>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <returns><c>true</c> if the specified path has setting; otherwise, <c>false</c>.</returns>
  bool HasSetting(string path, string name);

  /// <summary>
  /// Returns the value of the registry setting by path and name.
  /// </summary>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <param name="def">A default value if the setting is not set (or can be accessed)</param>
  /// <returns>System.String.</returns>
  string GetSetting(string path, string name, string def = null);

  /// <summary>
  /// Returns the  value of the registry setting by path and name and type.
  /// </summary>
  /// <typeparam name="T">The type of the value to expect</typeparam>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <param name="def">A default value if the setting is not set</param>
  /// <returns>T.</returns>
  T GetSetting<T>(string path, string name, T def = default);

  /// <summary>
  /// Saves the value to the registry setting by path and name.
  /// </summary>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <param name="value">The value to set</param>
  void SetSetting(string path, string name, string value);

  /// <summary>
  /// Saves the value to the registry setting by path and name.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <param name="value">The value to set</param>
  void SetSetting<T>(string path, string name, T value);
}
