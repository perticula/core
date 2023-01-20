using System.Diagnostics;
using System.Globalization;

/// <summary>
/// Class EnumJson.
/// Implements the <see cref="Json.EnumJson" />
/// </summary>
/// <typeparam name="TEnum">The type of the t enum.</typeparam>
/// <seealso cref="Json.EnumJson" />
[DebuggerDisplay("{DataType,nq}.{Name,nq}")]
public class EnumJson<TEnum> : EnumJson
    where TEnum : struct, IConvertible
{
  /// <summary>
  /// Converts from this enum json object back the original enum value
  /// </summary>
  /// <returns>TEnum.</returns>
  public TEnum ToEnum() => (TEnum)Enum.Parse(typeof(TEnum), IntValue.ToString(), true);

  /// <summary>
  /// Converts from an enum value to this standardize enum json object
  /// </summary>
  /// <param name="e">The e.</param>
  /// <returns>EnumJson&lt;TEnum&gt;.</returns>
  public static EnumJson<TEnum> FromEnum(TEnum e)
      => new EnumJson<TEnum>
      {
        Name = e.ToString(CultureInfo.InvariantCulture),
        DataType = typeof(TEnum).Name,
        DisplayText = (e as Enum).DisplayText(),
        GroupName = (e as Enum).GroupName(),
        IntValue = (int)(object)e
      };
}

/// <summary>
/// Class EnumJson.
/// Implements the <see cref="Json.EnumJson" />
/// </summary>
/// <seealso cref="Json.EnumJson" />
[DebuggerDisplay("{DataType,nq}.{Name,nq}")]
public class EnumJson
{
  /// <summary>
  /// Gets or sets the name.
  /// </summary>
  /// <value>The name.</value>
  public string Name { get; set; }

  /// <summary>
  /// Gets or sets the int value.
  /// </summary>
  /// <value>The int value.</value>
  public int IntValue { get; set; }

  /// <summary>
  /// Gets or sets the display text.
  /// </summary>
  /// <value>The display text.</value>
  public string DisplayText { get; set; }

  /// <summary>
  /// Gets or sets the type of the data.
  /// </summary>
  /// <value>The type of the data.</value>
  public string DataType { get; set; }

  /// <summary>
  /// Gets or sets the name of the group.
  /// </summary>
  /// <value>The name of the group.</value>
  public string GroupName { get; set; }
}
