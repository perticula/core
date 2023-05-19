// perticula - core - BerSetParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerSetParser.
///   Implements the <see cref="core.Protocol.asn1.Asn1SetParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1SetParser" />
public class BerSetParser : IAsn1SetParser
{
	/// <summary>
	///   The parser
	/// </summary>
	private readonly Asn1StreamParser _parser;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSetParser" /> class.
	/// </summary>
	/// <param name="parser">The parser.</param>
	internal BerSetParser(Asn1StreamParser parser) => _parser = parser;

	/// <summary>
	///   Reads the object.
	/// </summary>
	/// <returns>IAsn1Convertable.</returns>
	public IAsn1Convertable? ReadObject() => _parser.ReadObject();

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	public Asn1Object ToAsn1Object() => Parse(_parser);

	/// <summary>
	///   Parses the specified sp.
	/// </summary>
	/// <param name="sp">The sp.</param>
	/// <returns>BerSet.</returns>
	internal static BerSet Parse(Asn1StreamParser sp) => new(sp.LoadVector());
}
