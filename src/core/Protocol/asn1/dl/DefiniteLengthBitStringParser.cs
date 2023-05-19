// perticula - core - DefiniteLengthBitStringParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1.dl;

/// <summary>
///   Class DefiniteLengthBitStringParser.
///   Implements the <see cref="core.Protocol.asn1.IAsn1BitStringParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1BitStringParser" />
public class DefiniteLengthBitStringParser : IAsn1BitStringParser
{
	/// <summary>
	///   The stream
	/// </summary>
	private readonly DefiniteLengthInputStream _stream;

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthBitStringParser" /> class.
	/// </summary>
	/// <param name="stream">The stream.</param>
	internal DefiniteLengthBitStringParser(DefiniteLengthInputStream stream) => _stream = stream;

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	public Stream GetBitStream() => GetBitStream(false);

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	public Stream GetOctetStream() => GetBitStream(true);

	/// <summary>
	///   Gets the pad bits.
	///   Return the number of pad bits, if any, in the final byte, if any, read from <see cref="GetBitStream" />.
	/// </summary>
	/// <value>The number of pad bits. In the range zero to seven.</value>
	/// <remarks>
	///   This number is in the range zero to seven. That number of the least significant bits of the final byte, if
	///   any, are not part of the contents and should be ignored.
	///   NOTE: Must be called AFTER the stream has been fully processed.
	///   Does not need to be called if <see cref="GetOctetStream" /> was used instead of   <see cref="GetBitStream" />.
	/// </remarks>
	public int PadBits { get; private set; }

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="core.Protocol.asn1.Asn1ParseException">IOException converting stream to byte array: {e.Message}</exception>
	public Asn1Object ToAsn1Object()
	{
		try
		{
			return DerBitString.CreatePrimitive(_stream.ToArray());
		}
		catch (IOException e)
		{
			throw new Asn1ParseException($"IOException converting stream to byte array: {e.Message}", e);
		}
	}

	/// <summary>
	///   Gets the bit stream.
	/// </summary>
	/// <param name="octetAligned">if set to <c>true</c> [octet aligned].</param>
	/// <returns>Stream.</returns>
	/// <exception cref="System.InvalidOperationException">content octets cannot be empty</exception>
	/// <exception cref="System.InvalidOperationException">zero length data with non-zero pad bits</exception>
	/// <exception cref="System.InvalidOperationException">pad bits cannot be greater than 7 or less than 0</exception>
	private Stream GetBitStream(bool octetAligned)
	{
		var length = _stream.Remaining;
		if (length < 1)
			throw new InvalidOperationException("content octets cannot be empty");

		PadBits = _stream.ReadByte();
		return PadBits switch
		       {
			       > 0 when length  < 2  => throw new InvalidOperationException("zero length data with non-zero pad bits"),
			       > 0 when PadBits > 7  => throw new InvalidOperationException("pad bits cannot be greater than 7 or less than 0"),
			       > 0 when octetAligned => throw new IOException($"expected octet-aligned bitstring, but found padBits: {PadBits}"),
			       _                     => _stream
		       };
	}
}
