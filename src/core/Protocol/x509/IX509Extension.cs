// perticula - core - IX509Extension.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1;
using core.Protocol.asn1.der;

namespace core.Protocol.x509;

/// <summary>
///   Interface IX509Extension
/// </summary>
public interface IX509Extension
{
	/// <summary>
	///   Get all critical extension values, by oid
	/// </summary>
	/// <returns>ISet&lt;System.String&gt;.</returns>
	ISet<string> GetCriticalExtensionOids();

	/// <summary>
	///   Get all non-critical extension values, by oid
	/// </summary>
	/// <returns>ISet&lt;System.String&gt;.</returns>
	ISet<string> GetNonCriticalExtensionOids();

	/// <summary>
	///   Gets the extension value.
	/// </summary>
	/// <param name="oid">The oid.</param>
	/// <returns>Asn1OctetString.</returns>
	Asn1OctetString GetExtensionValue(DerObjectIdentifier oid);
}
