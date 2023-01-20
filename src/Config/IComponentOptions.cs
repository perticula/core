namespace core;

/// <summary>
/// Defines a callback handler for when an option has been set or changed
/// </summary>
/// <param name="name">The name of the option</param>
/// <param name="source">The options source</param>
public delegate void OptionChangedEventHandler(string name, IComponentOptions source);

/// <summary>
/// Interface IComponentOptions
/// </summary>
public interface IComponentOptions
{
  /// <summary>
  /// Returns the value of an option by name
  /// </summary>
  /// <param name="name">The name of the option</param>
  /// <returns>System.String.</returns>
  string GetOption(string name);

  /// <summary>
  /// Returns the value of an option by name and type.
  /// </summary>
  /// <typeparam name="T">The type of the value to expect</typeparam>
  /// <param name="name">The name of the option</param>
  /// <returns>T.</returns>
  T GetOption<T>(string name);

  /// <summary>
  /// Sets the option value by name.
  /// </summary>
  /// <param name="name">The name of the option</param>
  /// <param name="value">The value to set</param>
  void SetOption(string name, string value);

  /// <summary>
  /// Sets the option value by name and type.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="name">The name of the option</param>
  /// <param name="value">The value to set</param>
  void SetOption<T>(string name, T value);

  /// <summary>
  /// Copies an option from another options set by name
  /// </summary>
  /// <param name="name">The name of the option</param>
  /// <param name="other">The other set</param>
  void CopyOption(string name, IComponentOptions other);

  /// <summary>
  /// Subscribe to this event to receive options as they change
  /// </summary>
  event OptionChangedEventHandler OnOptionChanged;
}
