namespace core;

/// <summary>
/// Interface ICachedAppSettings
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICachedAppSettings<out T>
{
  /// <summary>
  /// The interval with which the value will be reloaded in milliseconds
  /// </summary>
  /// <value>The update interval ms.</value>
  int UpdateIntervalMs { get; set; }

  /// <summary>
  /// The elapsed time since the value was last loaded in milliseconds
  /// </summary>
  /// <value>The setting age ms.</value>
  double SettingAgeMs { get; }

  /// <summary>
  /// The defalut value to be used if no setting value is available
  /// </summary>
  /// <value>The default value.</value>
  T? DefaultValue { get; }

  /// <summary>
  /// The stored setting value as it was last loaded (or the default if none was available)
  /// Does NOT throw an exception if the value cannot be converted to the requested type.
  /// Instead the default value will be returned.
  /// This value will change at the timeout interval if the stored setting value changes.
  /// </summary>
  /// <value>The setting value.</value>
  T SettingValue { get; }

  /// <summary>
  /// The stored setting value as it was last loaded or an exception if no value
  /// is set in the configuration for this settings.
  /// This value will change at the timeout interval if the stored setting value changes.
  /// </summary>
  /// <value>The expect value.</value>
  T ExpectValue { get; }

  /// <summary>
  /// The setting name
  /// </summary>
  /// <value>The name of the setting.</value>
  string SettingName { get; }

  /// <summary>
  /// Indicates wither there was a setting value available
  /// This value will change at the timeout interval if the stored setting value changes.
  /// </summary>
  /// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
  bool HasValue { get; }

  /// <summary>
  /// Forces the setting value to be updated
  /// </summary>
  /// <returns>ICachedAppSettings&lt;T&gt;.</returns>
  ICachedAppSettings<T> ForceUpdate();
}
