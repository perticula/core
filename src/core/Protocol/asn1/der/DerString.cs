// perticula - core - DerString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerString.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
///   Implements the <see cref="core.Protocol.asn1.IAsn1String" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
/// <seealso cref="core.Protocol.asn1.IAsn1String" />
public abstract class DerString : Asn1Object, IAsn1String
{
	/// <summary>
	///   Gets the current string value.
	/// </summary>
	/// <returns>System.String.</returns>
	public abstract string GetString();

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => GetString();

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => GetString().GetHashCode();
}
