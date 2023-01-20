namespace core;

public static class SerializableUtilities
{
  /// <summary>
  /// Deserializes from.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value">The value.</param>
  /// <param name="delimiter">The delimiter.</param>
  /// <returns>IEnumerable&lt;T&gt;.</returns>
  public static IEnumerable<T?> DeserializeFrom<T>(this string value, string delimiter = ",") => string.IsNullOrEmpty(value)
             ? Enumerable.Empty<T>()
             : value.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(Serialize.FromString<T>);

  /// <summary>
  /// Serializeds the join.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="values">The values.</param>
  /// <param name="delimiter">The delimiter.</param>
  /// <returns>System.String.</returns>
  public static string SerializedJoin<T>(this IEnumerable<T> values, string delimiter = ", ")
  {
    values = (values ?? Enumerable.Empty<T>()).ToArray();
    return string.Join(delimiter, values.Select(Serialize.ToString));
  }
}
