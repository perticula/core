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
  public event OptionChangedEventHandler OnOptionChanged;

  public string GetOption(string name, string def = null) => CurrentSettings.ContainsKey(name) ? CurrentSettings[name] : def;

  public T GetOption<T>(string name, T def = default)
  {
    if (!CurrentSettings.ContainsKey(name)) return def;
    try
    {
      return Serialize.FromString<T>(CurrentSettings[name]);
    }
    catch
    {
      return def;
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
    SetOption(name, other?.GetOption(name));
  }
}
