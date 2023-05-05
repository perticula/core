// perticula - core - Asn1Type.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Type.
/// </summary>
public abstract class Asn1Type
{
	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Type" /> class.
	/// </summary>
	/// <param name="platformType">Type of the platform.</param>
	internal Asn1Type(Type platformType) => PlatformType = platformType;

	/// <summary>
	///   Gets the type of the platform.
	/// </summary>
	/// <value>The type of the platform.</value>
	internal Type PlatformType { get; }

	/// <summary>
	///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
	/// </summary>
	/// <param name="that">The <see cref="System.Object" /> to compare with this instance.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
	public sealed override bool Equals(object? that) => this == that;

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public sealed override int GetHashCode() => PlatformType.GetHashCode();
}
