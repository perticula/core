// perticula - core - GroupNameAttribute.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Attributes;

/// <summary>
///   Class GroupNameAttribute. This class cannot be inherited.
///   Implements the <see cref="System.Attribute" />
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Field)]
public sealed class GroupNameAttribute : Attribute
{
  /// <summary>
  ///   Initializes a new instance of the <see cref="T:Attributes.DisplayTextAttribute" /> class.
  /// </summary>
  /// <param name="text">The text.</param>
  /// <inheritdoc />
  public GroupNameAttribute(string text) => GroupName = text;

  /// <summary>
  ///   Gets or sets the display text.
  /// </summary>
  /// <value>The name of the group.</value>
  public string GroupName { get; }
}

/// <summary>
///   Class GroupNameAttributeExtensions.
/// </summary>
public static class GroupNameAttributeExtensions
{
  /// <summary>
  ///   Finds the (first) enum value matching the display text.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value">The value.</param>
  /// <returns>IEnumerable&lt;T&gt;.</returns>
  /// <exception cref="System.InvalidCastException"></exception>
  public static IEnumerable<T> FromGroupName<T>(this string value)
    where T : struct, IConvertible
  {
    foreach (Enum e in Enum.GetValues(typeof(T)))
    {
      if (e.GroupName() != value) continue;
      if (Enum.TryParse(e.ToString(), true, out T t))
        yield return t;
    }
  }

  /// <summary>
  ///   Returns the display text associated with this enum (must be decorated
  ///   with [DisplayText( ... )] )
  /// </summary>
  /// <param name="value">The value.</param>
  /// <returns>System.String.</returns>
  /// <exception cref="System.ArgumentNullException">value</exception>
  public static string GroupName(this Enum value)
  {
    if (value == null) throw new ArgumentNullException(nameof(value));
    return value
           .GetType()
           .GetField(value.ToString())
           ?.GetCustomAttributes(typeof(GroupNameAttribute), false)
           .Cast<GroupNameAttribute>()
           .Take(1)
           .Select(attr => attr.GroupName)
           .FirstOrDefault() ?? "";
  }
}
