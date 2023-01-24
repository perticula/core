// perticula - core - DisplayTextAttribute.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Attributes;

/// <summary>
///   Class DisplayTextAttribute. This class cannot be inherited.
///   Allows the attachment of a string value to an enumeration.
/// </summary>
/// <seealso cref="System.Attribute" />
/// <inheritdoc />
/// <seealso cref="T:System.Attribute" />
[AttributeUsage(AttributeTargets.Field)]
public sealed class DisplayTextAttribute : Attribute
{
  /// <summary>
  ///   Specifies the default value for the <see cref="T:DisplayTextAttribute" />,
  ///   which is an empty string ("").
  ///   This static field is read-only.
  /// </summary>
  public static readonly DisplayTextAttribute Default = new();

  /// <summary>
  ///   The display text
  /// </summary>
  private readonly string _displayText;

  /// <summary>
  ///   Initializes a new instance of the <see cref="T:Attributes.DisplayTextAttribute" /> class.
  /// </summary>
  /// <param name="text">The text.</param>
  /// <inheritdoc />
  public DisplayTextAttribute(string text) => _displayText = text;

  /// <summary>
  ///   Initializes a new instance of the <see cref="DisplayTextAttribute" /> class.
  /// </summary>
  /// <param name="text">The text.</param>
  /// <param name="default">The default.</param>
  /// <inheritdoc />
  public DisplayTextAttribute(string text, string @default)
  {
    _displayText = text;
    DefaultText  = @default;
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="T:Attributes.DisplayTextAttribute" /> class.
  /// </summary>
  /// <inheritdoc />
  public DisplayTextAttribute() : this(string.Empty) { }

  /// <summary>
  ///   Gets the default text if the DisplayText attribute is empty.
  /// </summary>
  /// <value>The default text.</value>
  public string DefaultText { get; } = string.Empty;

  /// <summary>
  ///   Gets or sets the display text.
  /// </summary>
  /// <value>The display text.</value>
  public string DisplayText => string.IsNullOrEmpty(_displayText) ? DefaultText : _displayText;

  /// <summary>
  ///   Returns a value that indicates whether this instance is equal to a specified object.
  /// </summary>
  /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance or null.</param>
  /// <returns>true if <paramref name="obj" /> equals the type and value of this instance; otherwise, false.</returns>
  /// <inheritdoc />
  public override bool Equals(object? obj)
  {
    if (obj == this) return true;
    if (obj is DisplayTextAttribute da) return string.Equals(da.DisplayText, DisplayText, StringComparison.OrdinalIgnoreCase);
    return false;
  }

  /// <summary>
  ///   Returns the hash code for this instance.
  /// </summary>
  /// <returns>A 32-bit signed integer hash code.</returns>
  public override int GetHashCode() => DisplayText.GetHashCode();

  /// <summary>
  ///   Returns a value indicating whether this is the default <see cref="T:DisplayTextAttribute" /> instance.
  /// </summary>
  /// <returns>true, if this is the default <see cref="T:DisplayTextAttribute" /> instance; otherwise, false.</returns>
  /// <inheritdoc />
  public override bool IsDefaultAttribute() => Equals(Default);
}

/// <summary>
///   Class DisplayTextAttributeExtensions.
///   Methods for selecting and operating on text values attached to enumerations
/// </summary>
public static class DisplayTextAttributeExtensions
{
  /// <summary>
  ///   Returns the display text associated with this enum (must be decorated
  ///   with [DisplayText( ... )] )
  /// </summary>
  /// <param name="value">The value.</param>
  /// <returns>System.String.</returns>
  /// <exception cref="System.ArgumentNullException">value</exception>
  public static string DisplayText(this Enum value)
  {
    if (value == null) throw new ArgumentNullException(nameof(value));

    return value
           .GetType()
           .GetField(value.ToString())
           ?.GetCustomAttributes(typeof(DisplayTextAttribute), false)
           .Cast<DisplayTextAttribute>()
           .Take(1)
           .Select(attr => attr.DisplayText)
           .FirstOrDefault() ?? "";
  }

  /// <summary>
  ///   Returns the display text associated with this enum (must be decorated
  ///   with [DisplayText( ... )] )
  ///   and formats the found value with the supplied params
  /// </summary>
  /// <param name="value">The value.</param>
  /// <param name="arguments">The format arguments.</param>
  /// <returns>System.String.</returns>
  /// <exception cref="System.ArgumentNullException">value</exception>
  /// <exception cref="FormatException">value cannot be null</exception>
  public static string DisplayText(this Enum value, params object[] arguments)
  {
    if (value == null) throw new ArgumentNullException(nameof(value));

    var attribute = value
                    .GetType()
                    .GetField(value.ToString())
                    ?.GetCustomAttributes(typeof(DisplayTextAttribute), false)
                    .Cast<DisplayTextAttribute>()
                    .Take(1)
                    .Select(attr => attr)
                    .FirstOrDefault();

    if (string.IsNullOrEmpty(attribute?.DisplayText)) return attribute?.DefaultText ?? "";

    var formatted = string.Format(attribute.DisplayText, arguments);
    return string.IsNullOrEmpty(formatted) ? attribute.DefaultText : formatted;
  }

  /// <summary>
  ///   Finds the (first) enum value matching the display text.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value">The value.</param>
  /// <returns>T.</returns>
  /// <exception cref="System.InvalidCastException"></exception>
  public static T FromDisplayText<T>(this string value)
    where T : struct, IConvertible
  {
    foreach (Enum e in Enum.GetValues(typeof(T)))
    {
      //check to see if entire string matches
      var dt = e.DisplayText() ?? "";
      if (string.Equals(dt, value, StringComparison.OrdinalIgnoreCase)) return (T) Enum.Parse(typeof(T), e.ToString(), true);

      //check to see if multiple values match (e.g. 'Hotels/Lodging' matches either 'hotels' or 'lodging')
      if (!dt.Contains('/')) continue;
      var splitValues = dt.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
      if (splitValues.Any(split => string.Equals(split, value, StringComparison.OrdinalIgnoreCase))) return (T) Enum.Parse(typeof(T), e.ToString(), true);
    }

    //check to see if we can parse to enum directly
    if (Enum.TryParse(value, true, out T test)) return test;

    //nothing matches
    throw new InvalidCastException();
  }
}
