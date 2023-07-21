// perticula - core - IFluentInterface.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.ComponentModel;

namespace core;

/// <summary>
///   Interface IFluentInterface.
///   Marks a class as a Fluent Interface, and hides standard object methods from Intellisense.
/// </summary>
public interface IFluentInterface
{
	/// <summary>
	///   Gets the type.
	/// </summary>
	/// <returns>Type.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	Type GetType();

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	int GetHashCode();

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	string? ToString();

	/// <summary>
	///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
	/// </summary>
	/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	bool Equals(object obj);
}
