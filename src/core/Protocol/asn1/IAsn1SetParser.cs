// perticula - core - IAsn1SetParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Interface IAsn1SetParser
///   Implements the <see cref="core.Protocol.asn1.IAsn1Convertable" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1Convertable" />
public interface IAsn1SetParser : IAsn1Convertable
{
	/// <summary>
	///   Reads the object.
	/// </summary>
	/// <returns>IAsn1Convertable.</returns>
	IAsn1Convertable? ReadObject();
}
