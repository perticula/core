// perticula - core - DerOctetStringParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerOctetStringParser.
///   Implements the <see cref="core.Protocol.asn1.IAsn1OctetStringParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1OctetStringParser" />
public class DerOctetStringParser : IAsn1OctetStringParser
{
	/// <summary>
	///   The stream
	/// </summary>
	private readonly DefiniteLengthInputStream _stream;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerOctetStringParser" /> class.
	/// </summary>
	/// <param name="stream">The stream.</param>
	internal DerOctetStringParser(DefiniteLengthInputStream stream) => _stream = stream;

	/// <summary>
	///   Gets the current content of the octet string as a stream.
	/// </summary>
	/// <returns>Stream containing the current OCTET.</returns>
	public Stream GetOctetStream() => _stream;

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.InvalidOperationException">IOException converting stream to byte array: {e.Message}</exception>
	public Asn1Object ToAsn1Object()
	{
		try
		{
			return new DerOctetString(_stream.ToArray());
		}
		catch (IOException e)
		{
			throw new InvalidOperationException($"IOException converting stream to byte array: {e.Message}", e);
		}
	}
}
