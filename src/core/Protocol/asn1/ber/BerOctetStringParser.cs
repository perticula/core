// perticula - core - BerOctetStringParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerOctetStringParser.
///   Implements the <see cref="core.Protocol.asn1.IAsn1OctetStringParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1OctetStringParser" />
public class BerOctetStringParser : IAsn1OctetStringParser
{
	/// <summary>
	///   The parser
	/// </summary>
	private readonly Asn1StreamParser _parser;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerOctetStringParser" /> class.
	/// </summary>
	/// <param name="parser">The parser.</param>
	internal BerOctetStringParser(Asn1StreamParser parser) => _parser = parser;

	/// <summary>
	///   Gets the current content of the octet string as a stream.
	/// </summary>
	/// <returns>Stream containing the current OCTET.</returns>
	public Stream GetOctetStream() => new ConstructedOctetStream(_parser);

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="core.Protocol.asn1.Asn1ParseException">IOException converting stream to byte array: {e.Message}</exception>
	public Asn1Object ToAsn1Object()
	{
		try
		{
			return Parse(_parser);
		}
		catch (IOException e)
		{
			throw new Asn1ParseException($"IOException converting stream to byte array: {e.Message}", e);
		}
	}

	/// <summary>
	///   Parses the specified sp.
	/// </summary>
	/// <param name="sp">The sp.</param>
	/// <returns>BerOctetString.</returns>
	internal static BerOctetString Parse(Asn1StreamParser sp) => new(Streams.ReadAll(new ConstructedOctetStream(sp)));
}
