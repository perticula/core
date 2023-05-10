// perticula - core - ConstructedOctetStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;

namespace core.Protocol.asn1;

/// <summary>
///   Class ConstructedOctetStream.
///   Implements the <see cref="BaseInputStream" />
/// </summary>
/// <seealso cref="BaseInputStream" />
public class ConstructedOctetStream : BaseInputStream
{
	/// <summary>
	///   The parser
	/// </summary>
	private readonly Asn1StreamParser? _parser;

	/// <summary>
	///   The current stream
	/// </summary>
	private Stream? _currentStream;

	/// <summary>
	///   The first
	/// </summary>
	private bool _first = true;

	/// <summary>
	///   Initializes a new instance of the <see cref="ConstructedOctetStream" /> class.
	/// </summary>
	/// <param name="parser">The parser.</param>
	internal ConstructedOctetStream(Asn1StreamParser parser) => _parser = parser;

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

			var next = GetNextParser();
			if (next == null)
				return 0;

			_first         = false;
			_currentStream = next.GetOctetStream();
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
				var next = GetNextParser();
				if (next == null)
				{
					_currentStream = null;
					return totalRead;
				}

				_currentStream = next.GetOctetStream();
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

			var next = GetNextParser();
			if (next == null)
				return -1;

			_first         = false;
			_currentStream = next.GetOctetStream();
		}

		for (;;)
		{
			var b = _currentStream.ReadByte();

			if (b >= 0)
				return b;

			var next = GetNextParser();
			if (next == null)
			{
				_currentStream = null;
				return -1;
			}

			_currentStream = next.GetOctetStream();
		}
	}

	/// <summary>
	///   Gets the next parser.
	/// </summary>
	/// <returns>System.Nullable&lt;IAsn1OctetStringParser&gt;.</returns>
	private IAsn1OctetStringParser? GetNextParser()
	{
		var asn1Obj = _parser?.ReadObject();
		return asn1Obj switch
		       {
			       null                       => null,
			       IAsn1OctetStringParser obj => obj,
			       _                          => throw new IOException($"unknown object encountered: {asn1Obj.GetTypeName()}")
		       };
	}
}
