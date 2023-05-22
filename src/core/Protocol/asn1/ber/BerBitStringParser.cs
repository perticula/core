// perticula - core - BerBitStringParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerBitStringParser.
///   Implements the <see cref="core.Protocol.asn1.IAsn1BitStringParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1BitStringParser" />
public class BerBitStringParser : IAsn1BitStringParser
{
	/// <summary>
	///   The parser
	/// </summary>
	private readonly Asn1StreamParser _parser;

	/// <summary>
	///   The bit stream
	/// </summary>
	private ConstructedBitStream? _bitStream;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerBitStringParser" /> class.
	/// </summary>
	/// <param name="parser">The parser.</param>
	internal BerBitStringParser(Asn1StreamParser parser) => _parser = parser;

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	public Stream GetOctetStream()
	{
		return _bitStream = new ConstructedBitStream(_parser, true);
	}

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	public Stream GetBitStream()
	{
		return _bitStream = new ConstructedBitStream(_parser, false);
	}

	/// <summary>
	///   Gets the pad bits.
	///   Return the number of pad bits, if any, in the final byte, if any, read from <see cref="GetBitStream" />.
	/// </summary>
	/// <value>The number of pad bits. In the range zero to seven.</value>
	/// <exception cref="System.NullReferenceException"></exception>
	/// <remarks>
	///   This number is in the range zero to seven. That number of the least significant bits of the final byte, if
	///   any, are not part of the contents and should be ignored.
	///   NOTE: Must be called AFTER the stream has been fully processed.
	///   Does not need to be called if <see cref="GetOctetStream" /> was used instead of   <see cref="GetBitStream" />.
	/// </remarks>
	public int PadBits => _bitStream?.PadBits ?? throw new NullReferenceException();

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
	/// <returns>BerBitString.</returns>
	internal static BerBitString Parse(Asn1StreamParser sp)
	{
		var bitStream = new ConstructedBitStream(sp, false);
		var data      = Streams.ReadAll(bitStream);
		var padBits   = bitStream.PadBits;
		return new BerBitString(data, padBits);
	}
}
