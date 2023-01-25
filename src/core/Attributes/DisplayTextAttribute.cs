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
