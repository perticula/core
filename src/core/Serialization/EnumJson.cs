// perticula - core - EnumJson.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Globalization;
using core.Attributes;

namespace core.Serialization
{
  /// <summary>
  ///   Class EnumJson.
  ///   Implements the <see cref="core.Serialization.EnumJson" />
  /// </summary>
  /// <typeparam name="TEnum">The type of the t enum.</typeparam>
  /// <seealso cref="core.Serialization.EnumJson" />
  [DebuggerDisplay("{DataType,nq}.{Name,nq}")]
  public record EnumJson<TEnum> : EnumJson
    where TEnum : struct, IConvertible
  {
    /// <summary>
    ///   Converts from this enum json object back the original enum value
    /// </summary>
    /// <returns>TEnum.</returns>
    public TEnum ToEnum() => (TEnum) Enum.Parse(typeof(TEnum), IntValue.ToString(), true);

    /// <summary>
    ///   Converts from an enum value to this standardize enum json object
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>EnumJson&lt;TEnum&gt;.</returns>
    public static EnumJson<TEnum> FromEnum(TEnum e)
      => new()
      {
        Name        = e.ToString(CultureInfo.InvariantCulture),
        DataType    = typeof(TEnum).Name,
        DisplayText = (e as Enum)?.DisplayText(),
        GroupName   = (e as Enum)?.GroupName(),
        IntValue    = (int) (object) e
      };
  }

  /// <summary>
  ///   Class EnumJson.
  /// </summary>
  [DebuggerDisplay("{DataType,nq}.{Name,nq}")]
  public record EnumJson
  {
    /// <summary>
    ///   Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string? Name { get; set; }

    /// <summary>
    ///   Gets or sets the int value.
    /// </summary>
    /// <value>The int value.</value>
    public int IntValue { get; set; }

    /// <summary>
    ///   Gets or sets the display text.
    /// </summary>
    /// <value>The display text.</value>
    public string? DisplayText { get; set; }

    /// <summary>
    ///   Gets or sets the type of the data.
    /// </summary>
    /// <value>The type of the data.</value>
    public string? DataType { get; set; }

    /// <summary>
    ///   Gets or sets the name of the group.
    /// </summary>
    /// <value>The name of the group.</value>
    public string? GroupName { get; set; }
  }
}
