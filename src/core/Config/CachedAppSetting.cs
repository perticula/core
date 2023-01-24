// perticula - core - CachedAppSetting.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Configuration;

namespace core.Config;

/// <summary>
///   Class CachedAppSettings.
///   Implements the <see cref="ICachedAppSettings{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="ICachedAppSettings{T}" />
internal class CachedAppSettings<T> : ICachedAppSettings<T>
{
  /// <summary>
  ///   The settings
  /// </summary>
  private readonly IAppSettings _settings;

  /// <summary>
  ///   The last has value
  /// </summary>
  private bool _lastHasValue;

  /// <summary>
  ///   The last known value
  /// </summary>
  private T? _lastKnownValue;

  /// <summary>
  ///   The last update
  /// </summary>
  private DateTime? _lastUpdate;

  /// <summary>
  ///   The update interval ms
  /// </summary>
  private int _updateIntervalMs;

  /// <summary>
  ///   Initializes a new instance of the <see cref="CachedAppSettings{T}" /> class.
  /// </summary>
  /// <param name="settings">The settings.</param>
  /// <param name="intervalMs">The interval ms.</param>
  /// <param name="name">The name.</param>
  /// <exception cref="System.ArgumentNullException">name</exception>
  /// <exception cref="System.ArgumentNullException">settings</exception>
  public CachedAppSettings(IAppSettings settings, int intervalMs, string name)
  {
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
    _settings        = settings ?? throw new ArgumentNullException(nameof(settings));
    SettingName      = name;
    UpdateIntervalMs = intervalMs;
  }

  /// <summary>
  ///   The interval with which the value will be reloaded in milliseconds
  /// </summary>
  /// <value>The update interval ms.</value>
  public int UpdateIntervalMs
  {
    get => _updateIntervalMs;
    set
    {
      if (value < 500) value = 500; // min update interval
      _updateIntervalMs = value;
    }
  }

  /// <summary>
  ///   The elapsed time since the value was last loaded in milliseconds
  /// </summary>
  /// <value>The setting age ms.</value>
  public double SettingAgeMs => _lastUpdate.HasValue
                                  ? DateTime.Now.Subtract(_lastUpdate.Value).TotalMilliseconds
                                  : double.MaxValue;

  /// <summary>
  ///   The defalut value to be used if no setting value is available
  /// </summary>
  /// <value>The default value.</value>
  public T? DefaultValue => default;

  /// <summary>
  ///   The stored setting value as it was last loaded (or the default if none was available)
  ///   Does NOT throw an exception if the value cannot be converted to the requested type.
  ///   Instead the default value will be returned.
  ///   This value will change at the timeout interval if the stored setting value changes.
  /// </summary>
  /// <value>The setting value.</value>
  /// <exception cref="System.Configuration.ConfigurationErrorsException">
  ///   Configuration is missing a setting called
  ///   '{SettingName}'
  /// </exception>
  public T SettingValue
  {
    get
    {
      // Inside the timeout window?
      if (SettingAgeMs < UpdateIntervalMs)
        if (_lastKnownValue != null)
          return _lastKnownValue;

      try
      {
        // Update the value
        _lastUpdate     = DateTime.Now;
        _lastHasValue   = _settings.HasSetting(SettingName);
        _lastKnownValue = _lastHasValue ? _settings.GetSetting<T>(SettingName) : DefaultValue;
      }
      catch (Exception)
      {
        // Use default on bad format
        _lastKnownValue = DefaultValue;
      }

      if (_lastKnownValue != null)
        return _lastKnownValue;

      throw new ConfigurationErrorsException($"Configuration is missing a setting called '{SettingName}'");
    }
  }

  /// <summary>
  ///   The stored setting value as it was last loaded or an exception if no value
  ///   is set in the configuration for this settings.
  ///   This value will change at the timeout interval if the stored setting value changes.
  /// </summary>
  /// <value>The expect value.</value>
  /// <exception cref="System.Configuration.ConfigurationErrorsException">
  ///   Configuration is missing a setting called
  ///   '{SettingName}'
  /// </exception>
  public T ExpectValue
  {
    get
    {
      if (!HasValue)
        throw new ConfigurationErrorsException($"Configuration is missing a setting called '{SettingName}'");
      return SettingValue;
    }
  }

  /// <summary>
  ///   The setting name
  /// </summary>
  /// <value>The name of the setting.</value>
  public string SettingName { get; }

  /// <summary>
  ///   Indicates wither there was a setting value available
  ///   This value will change at the timeout interval if the stored setting value changes.
  /// </summary>
  /// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
  public bool HasValue
  {
    get
    {
      // Inside the timeout window?
      if (SettingAgeMs < UpdateIntervalMs)
        return _lastHasValue;

      // Update and return the new value
      ForceUpdate();
      return _lastHasValue;
    }
  }

  /// <summary>
  ///   Forces the update.
  /// </summary>
  /// <returns>ICachedAppSettings&lt;T&gt;.</returns>
  public ICachedAppSettings<T> ForceUpdate()
  {
    _lastUpdate = null;
    var _ = SettingValue;
    return this;
  }
}
