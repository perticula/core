namespace core;

/// <summary>
/// Class ComponentOptionsBase.
/// Implements the <see cref="IComponentOptions" />
/// </summary>
/// <seealso cref="IComponentOptions" />
public abstract class ComponentOptionsBase : IComponentOptions
{
  /// <summary>
  /// Holds the settings values
  /// </summary>
  protected Dictionary<string, string> CurrentSettings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

  /// <summary>
  /// Subscribe to this event to receive options as they change
  /// </summary>
  public event OptionChangedEventHandler? OnOptionChanged;

  public string GetOption(string name) => CurrentSettings.ContainsKey(name) ? CurrentSettings[name] : throw new KeyNotFoundException(nameof(name));

  public T GetOption<T>(string name)
  {
    if (!CurrentSettings.ContainsKey(name)) throw new KeyNotFoundException(nameof(name));
    try
    {
      return Serialize.FromString<T>(CurrentSettings[name]);
    }
    catch
    {
      throw new ApplicationException($"Unable to serialize option {name} to type {typeof(T).Name}");
    }
  }

  public void SetOption(string name, string value)
  {
    if (CurrentSettings.ContainsKey(name))
      CurrentSettings[name] = value;
    else
      CurrentSettings.Add(name, value);
    OnOptionChanged?.Invoke(name, this);
  }

  public void SetOption<T>(string name, T value) => SetOption(name, Serialize.ToString(value));

  public void CopyOption(string name, IComponentOptions other)
  {
    if (string.IsNullOrEmpty(name)) return;
    var val = other?.GetOption(name);
    if (val == null) return;
    SetOption(name, val);
  }
}
