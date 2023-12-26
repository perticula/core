// perticula - core - FilteringExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Reflection;

namespace core.Filtering;

/// <summary>
///   Class FilteringExtensions.
/// </summary>
public static class FilteringExtensions
{
	/// <summary>
	///   Determines whether the filtered object has a value attribute with the specified name.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="name">The name.</param>
	/// <returns><c>true</c> if the specified name is a valid filtering value otherwise, <c>false</c>.</returns>
	/// <exception cref="System.ArgumentNullException">source</exception>
	public static bool HasFilterValueAttribute(this ISupportsFiltering source, string name)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

		if (!source.SupportsFilterAttributes) return false;

		// find a matching property or field that has the specified name (ignoring case)
		var member = source
			.GetType()
			.GetMembers()
			.FirstOrDefault(m => m.GetCustomAttributes(typeof(FilteringValueAttribute), true)
				.Cast<FilteringValueAttribute>()
				.Any(a => string.Compare(a.Name, name, StringComparison.OrdinalIgnoreCase) == 0));
		if (member == null) return false;

		// if a matching member is found, determine if it can be used.
		return member switch
		       {
			       FieldInfo      => true,
			       PropertyInfo p => p.CanRead,
			       _              => false
		       };
	}

	/// <summary>
	///   Resolves the value of the filtered object with the specified attribute name.
	///   Supports providing a default if the member does not exist.
	///   returns a null object, if the member cannot be found and no default is specified
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="name">The name.</param>
	/// <param name="def">The definition.</param>
	/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
	/// <exception cref="System.ArgumentNullException">source</exception>
	public static object? ResolveFilterValueAttribute(this ISupportsFiltering source, string name,
		Func<ISupportsFiltering, object>?                                       def = null)
	{
		ArgumentNullException.ThrowIfNull(source);

		if (!source.SupportsFilterAttributes) return def?.Invoke(source) ?? null;

		// find a matching property or field that has the specified name (ignoring case)
		var member = source
			.GetType()
			.GetMembers()
			.FirstOrDefault(m => m.GetCustomAttributes(typeof(FilteringValueAttribute), true)
				.Cast<FilteringValueAttribute>()
				.Any(a => string.Compare(a.Name, name, StringComparison.OrdinalIgnoreCase) == 0));

		if (member == null) return def?.Invoke(source) ?? null;

		// Get the member value
		return member switch
		       {
			       FieldInfo field => field.GetValue(source),
			       PropertyInfo prop => prop.CanRead
				       ? prop.GetValue(source)
				       : def?.Invoke(source) ?? null, //If we can't read the prop, consider the member null
			       _ => def?.Invoke(source) ?? null
		       };
	}
}
