using Microsoft.Win32;

namespace core;
/// <summary>
/// Class RegistrySettings.
/// Implements the <see cref="IRegistrySettings" />
/// </summary>
/// <seealso cref="IRegistrySettings" />
internal class RegistrySettings : IRegistrySettings
{
  // NOTE: This component assumes the application is running with enough
  //       permission to read and write to the HKEY_LOCAL_MACHINE registry
  //       hive and should only be used by components such as windows services.

  /// <summary>
  /// Returns the value of the registry setting by path and name.
  /// </summary>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <returns>System.String.</returns>
  public string GetSetting(string path, string name)
  {
    if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
    try
    {
      using (var key = Registry.LocalMachine.OpenSubKey(path, false))
      {
        if (key == null) throw new KeyNotFoundException($"Unable to find registry key {path}");
        var val = key.GetValue(name) as string;
        if (val != null) return val;
        throw new KeyNotFoundException($"Unable to find registry value {name} in {path}");
      }
    }
    catch
    {
      throw new ApplicationException($"Unable to serialize option {name}");
    }
  }

  /// <summary>
  /// Returns the  value of the registry setting by path and name and type.
  /// </summary>
  /// <typeparam name="T">The type of the value to expect</typeparam>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <returns>T.</returns>
  public T GetSetting<T>(string path, string name)
  {
    var value = GetSetting(path, name);
    try
    {
      return Serialize.FromString<T>(value);
    }
    catch
    {
      throw new ApplicationException($"Unable to serialize option {name}");
    }
  }

  /// <summary>
  /// Indicates whether a registry setting exists.
  /// </summary>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <returns><c>true</c> if the specified path has setting; otherwise, <c>false</c>.</returns>
  public bool HasSetting(string path, string name)
  {
    var value = GetSetting(path, name);
    return value != null; // Only way without enumerating all keys
  }

  /// <summary>
  /// Saves the value to the registry setting by path and name.
  /// </summary>
  /// <param name="path">The path of the setting (under HKEY_LOCAL_MACHINE)</param>
  /// <param name="name">The name of the setting</param>
  /// <param name="value">The value to set</param>
  public void SetSetting(string path, string name, string value)
  {
    if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
    using (var key = Registry.LocalMachine.CreateSubKey(path, true)) key?.SetValue(name, value, RegistryValueKind.String);
  }

  /// <summary>
  /// Sets the setting.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="path">The path.</param>
  /// <param name="name">The name.</param>
  /// <param name="value">The value.</param>
  public void SetSetting<T>(string path, string name, T value) => SetSetting(path, name, Serialize.ToString(value));
}
