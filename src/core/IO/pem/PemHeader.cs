// perticula - core - PemHeader.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO.pem;

/// <summary>
///   Class PemHeader.
/// </summary>
public class PemHeader
{
	/// <summary>
	///   The name
	/// </summary>
	private readonly string _name;

	/// <summary>
	///   The value
	/// </summary>
	private readonly string _val;

	/// <summary>
	///   Initializes a new instance of the <see cref="PemHeader" /> class.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="val">The value.</param>
	public PemHeader(string name, string val)
	{
		_name = name;
		_val  = val;
	}

	/// <summary>
	///   Gets the name.
	/// </summary>
	/// <value>The name.</value>
	public virtual string Name => _name;

	/// <summary>
	///   Gets the value.
	/// </summary>
	/// <value>The value.</value>
	public virtual string Value => _val;

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public override int GetHashCode() => GetHashCode(_name) + 31 * GetHashCode(_val);

	/// <summary>
	///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
	/// </summary>
	/// <param name="obj">The object to compare with the current object.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
	public override bool Equals(object? obj) => obj == this || (obj is PemHeader pemHeader && Equals(_name, pemHeader._name) && Equals(_val, pemHeader._val));

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	private static int GetHashCode(string? s) => s == null ? 1 : s.GetHashCode();

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => _name + ":" + _val;
}
