// perticula - core - IAsn1BitStringParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Interface IAsn1BitStringParser
/// </summary>
public interface IAsn1BitStringParser : IAsn1Convertable
{
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
	int PadBits { get; }

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	/// <exception cref="IOException"></exception>
	Stream GetBitStream();

	/// <summary>
	///   Gets the bit stream.
	///   Return a <see cref="Stream" /> representing the contents of the BIT STRING. The final byte, if any,
	///   may include pad bits. See <see cref="PadBits" />.
	/// </summary>
	/// <returns>Stream.</returns>
	/// <exception cref="IOException"></exception>
	Stream GetOctetStream();
}
