// perticula - core - ConstructedBitStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1;

/// <summary>
///   Class ConstructedBitStream.
///   Implements the <see cref="BaseInputStream" />
/// </summary>
/// <seealso cref="BaseInputStream" />
public class ConstructedBitStream : BaseInputStream
{
	/// <summary>
	///   The octet aligned
	/// </summary>
	private readonly bool _octetAligned;

	/// <summary>
	///   The parser
	/// </summary>
	private readonly Asn1StreamParser _parser;

	/// <summary>
	///   The current parser
	/// </summary>
	private IAsn1BitStringParser? _currentParser;

	/// <summary>
	///   The current stream
	/// </summary>
	private Stream? _currentStream;

	/// <summary>
	///   The first
	/// </summary>
	private bool _first = true;

	/// <summary>
	///   Initializes a new instance of the <see cref="ConstructedBitStream" /> class.
	/// </summary>
	/// <param name="parser">The parser.</param>
	/// <param name="octetAligned">if set to <c>true</c> [octet aligned].</param>
	internal ConstructedBitStream(Asn1StreamParser parser, bool octetAligned)
	{
		_parser       = parser;
		_octetAligned = octetAligned;
	}

	/// <summary>
	///   Gets the pad bits.
	/// </summary>
	/// <value>The pad bits.</value>
	internal int PadBits { get; private set; }

	/// <summary>
	///   When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position
	///   within the stream by the number of bytes read.
	/// </summary>
	/// <param name="buffer">
	///   An array of bytes. When this method returns, the buffer contains the specified byte array with the
	///   values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced
	///   by the bytes read from the current source.
	/// </param>
	/// <param name="offset">
	///   The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read
	///   from the current stream.
	/// </param>
	/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
	/// <returns>
	///   The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
	///   many bytes are not currently available, or zero (0) if the end of the stream has been reached.
	/// </returns>
	public override int Read(byte[] buffer, int offset, int count)
	{
		Streams.ValidateBufferArguments(buffer, offset, count);

		return Read(buffer.AsSpan(offset, count));
	}

	/// <summary>
	///   When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position
	///   within the stream by the number of bytes read.
	/// </summary>
	/// <param name="buffer">
	///   A region of memory. When this method returns, the contents of this region are replaced by the
	///   bytes read from the current source.
	/// </param>
	/// <returns>
	///   The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the
	///   buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
	/// </returns>
	public override int Read(Span<byte> buffer)
	{
		if (buffer.IsEmpty)
			return 0;

		if (_currentStream == null)
		{
			if (!_first)
				return 0;

			_currentParser = GetNextParser();
			if (_currentParser == null)
				return 0;

			_first         = false;
			_currentStream = _currentParser.GetBitStream();
		}

		var totalRead = 0;

		for (;;)
		{
			var numRead = _currentStream.Read(buffer[totalRead..]);

			if (numRead > 0)
			{
				totalRead += numRead;

				if (totalRead == buffer.Length)
					return totalRead;
			}
			else
			{
				PadBits        = _currentParser!.PadBits;
				_currentParser = GetNextParser();
				if (_currentParser == null)
				{
					_currentStream = null;
					return totalRead;
				}

				_currentStream = _currentParser.GetBitStream();
			}
		}
	}

	/// <summary>
	///   Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of
	///   the stream.
	/// </summary>
	/// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
	public override int ReadByte()
	{
		if (_currentStream == null)
		{
			if (!_first)
				return -1;

			_currentParser = GetNextParser();
			if (_currentParser == null)
				return -1;

			_first         = false;
			_currentStream = _currentParser.GetBitStream();
		}

		for (;;)
		{
			var b = _currentStream.ReadByte();

			if (b >= 0)
				return b;

			PadBits        = _currentParser!.PadBits;
			_currentParser = GetNextParser();
			if (_currentParser == null)
			{
				_currentStream = null;
				return -1;
			}

			_currentStream = _currentParser.GetBitStream();
		}
	}

	/// <summary>
	///   Gets the next parser.
	/// </summary>
	/// <returns>System.Nullable&lt;IAsn1BitStringParser&gt;.</returns>
	/// <exception cref="System.IO.IOException">expected octet-aligned bitstring, but found padBits: {PadBits}</exception>
	/// <exception cref="System.IO.IOException">only the last nested bitstring can have padding</exception>
	private IAsn1BitStringParser? GetNextParser()
	{
		var asn1Obj = _parser.ReadObject();
		return asn1Obj switch
		       {
			       null when _octetAligned && PadBits != 0 => throw new IOException($"expected octet-aligned bitstring, but found padBits: {PadBits}"),
			       null                                    => null,
			       IAsn1BitStringParser when PadBits != 0  => throw new IOException("only the last nested bitstring can have padding"),
			       IAsn1BitStringParser obj                => obj,
			       _                                       => throw new IOException($"unknown object encountered: {asn1Obj.GetTypeName()}")
		       };
	}
}
