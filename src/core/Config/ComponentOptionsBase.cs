// perticula - core - ComponentOptionsBase.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.Serialization;
using core.Serialization;

namespace core.Config;

/// <summary>
///   Class ComponentOptionsBase.
///   Implements the <see cref="IComponentOptions" />
/// </summary>
/// <seealso cref="IComponentOptions" />
public abstract class ComponentOptionsBase : IComponentOptions
{
  /// <summary>
  ///   Holds the settings values
  /// </summary>
  protected Dictionary<string, string> CurrentSettings = new(StringComparer.OrdinalIgnoreCase);

  /// <summary>
  ///   Subscribe to this event to receive options as they change
  /// </summary>
  public event OptionChangedEventHandler? OnOptionChanged;

  /// <summary>
  ///   Returns the value of an option by name
  /// </summary>
  /// <param name="name">The name of the option</param>
  /// <returns>System.String.</returns>
  /// <exception cref="System.Collections.Generic.KeyNotFoundException">name</exception>
  public string GetOption(string name) => CurrentSettings.ContainsKey(name) ? CurrentSettings[name] : throw new KeyNotFoundException(nameof(name));

  /// <summary>
  ///   Returns the value of an option by name and type.
  /// </summary>
  /// <typeparam name="T">The type of the value to expect</typeparam>
  /// <param name="name">The name of the option</param>
  /// <returns>T.</returns>
  /// <exception cref="System.Collections.Generic.KeyNotFoundException">name</exception>
  /// <exception cref="System.Runtime.Serialization.SerializationException">
  ///   Unable to serialize option {name} to type
  ///   {typeof(T).Name}
  /// </exception>
  public T GetOption<T>(string name)
  {
    if (!CurrentSettings.ContainsKey(name)) throw new KeyNotFoundException(nameof(name));
    return Serialize.FromString<T>(CurrentSettings[name]) ?? throw new SerializationException($"Unable to serialize option {name} to type {typeof(T).Name}");
  }

  /// <summary>
  ///   Sets the option value by name.
  /// </summary>
  /// <param name="name">The name of the option</param>
  /// <param name="value">The value to set</param>
  public void SetOption(string name, string value)
  {
    if (CurrentSettings.ContainsKey(name))
      CurrentSettings[name] = value;
    else
      CurrentSettings.Add(name, value);
    OnOptionChanged?.Invoke(name, this);
  }

  /// <summary>
  ///   Sets the option value by name and type.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="name">The name of the option</param>
  /// <param name="value">The value to set</param>
  public void SetOption<T>(string name, T value) => SetOption(name, Serialize.ToString(value));

  /// <summary>
  ///   Copies an option from another options set by name
  /// </summary>
  /// <param name="name">The name of the option</param>
  /// <param name="other">The other set</param>
  public void CopyOption(string name, IComponentOptions other)
  {
    if (string.IsNullOrEmpty(name)) return;
    var val = other.GetOption(name);
    SetOption(name, val);
  }
}
