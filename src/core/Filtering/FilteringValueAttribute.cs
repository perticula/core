// perticula - core - FilteringValueAttribute.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Filtering;

/// <summary>
///   Class FilteringValueAttribute Implements the <see cref="System.Attribute" />
///   There may be only one filtering value on an object.
///   This can be inherited via a parent class
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FilteringValueAttribute : Attribute
{
	/// <summary>
	///   Prevents a default instance of the <see cref="FilteringValueAttribute" /> class from being created.
	/// </summary>
	/// <exception cref="System.InvalidOperationException">Missing Name Property: [FilteringValueAttribute(string name)]</exception>
	private FilteringValueAttribute() =>
		throw new InvalidOperationException("Missing Name Property: [FilteringValueAttribute(string name)]");

	/// <summary>
	///   Initializes a new instance of the <see cref="FilteringValueAttribute" /> class.
	/// </summary>
	/// <param name="name">The name.</param>
	public FilteringValueAttribute(string name)
	{
		ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
		Name = name;
	}

	/// <summary>
	///   Gets the name of the field/property to use as the objects filtering value.
	/// </summary>
	/// <value>The name.</value>
	public string Name { get; }
}
