// perticula - core - DerExternalParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.dl;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerExternalParser.
///   Implements the <see cref="core.Protocol.asn1.Asn1Encodable" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Encodable" />
public class DerExternalParser : Asn1Encodable
{
	/// <summary>
	///   The parser
	/// </summary>
	private readonly Asn1StreamParser _parser;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerExternalParser" /> class.
	/// </summary>
	/// <param name="parser">The parser.</param>
	internal DerExternalParser(Asn1StreamParser parser) => _parser = parser;

	/// <summary>
	///   Reads the object.
	/// </summary>
	/// <returns>System.Nullable&lt;IAsn1Convertable&gt;.</returns>
	public IAsn1Convertable? ReadObject() => _parser.ReadObject();

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	public override Asn1Object ToAsn1Object() => Parse(_parser);

	/// <summary>
	///   Parses the specified sp.
	/// </summary>
	/// <param name="sp">The sp.</param>
	/// <returns>DerExternal.</returns>
	internal static DerExternal Parse(Asn1StreamParser sp) => new DefiniteLengthExternal(sp.LoadVector());
}
