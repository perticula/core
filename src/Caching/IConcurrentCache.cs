namespace core;

/// <summary>
/// Interface IConcurrentCache
/// Implements the <see cref="ICacheCommon{IConcurrentCache}" />
/// </summary>
/// <seealso cref="ICacheCommon{IConcurrentCache}" />
public interface IConcurrentCache : ICacheCommon<IConcurrentCache>
{
  /// <summary>
  /// References a cache of values by name. Useful to isolate one group of values
  /// from another. Note, the name used here will collide with other values using
  /// the same name
  /// </summary>
  /// <param name="key">The key.</param>
  /// <returns>IConcurrentCache.</returns>
  IConcurrentCache ByName(string key);
}
