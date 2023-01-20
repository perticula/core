namespace core;

/// <summary>
/// Interface ISimpleCache
/// Implements the <see cref="ICacheCommon{ISimpleCache}" />
/// </summary>
/// <seealso cref="ICacheCommon{ISimpleCache}" />
public interface ISimpleCache : ICacheCommon<ISimpleCache>
{
  /// <summary>
  /// References a cache of values by name. Useful to isolate one group of values
  /// from another. Note, the name used here will collide with other values using
  /// the same name
  /// </summary>
  /// <param name="key">The key.</param>
  /// <returns>ISimpleCache.</returns>
  ISimpleCache ByName(string key);
}
