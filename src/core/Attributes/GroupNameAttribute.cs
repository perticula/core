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
