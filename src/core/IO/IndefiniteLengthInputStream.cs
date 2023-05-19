// perticula - core - IndefiniteLengthInputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO;

/// <summary>
///   Class IndefiniteLengthInputStream.
///   Implements the <see cref="core.IO.LimitedInputStream" />
/// </summary>
/// <seealso cref="core.IO.LimitedInputStream" />
public class IndefiniteLengthInputStream : LimitedInputStream
{
	/// <summary>
	///   The EOF on00
	/// </summary>
	private bool _eofOn00 = true;

	/// <summary>
	///   The look ahead
	/// </summary>
	private int _lookAhead;

	/// <summary>
	///   Initializes a new instance of the <see cref="IndefiniteLengthInputStream" /> class.
	/// </summary>
	/// <param name="inStream">The in stream.</param>
	/// <param name="limit">The limit.</param>
	internal IndefiniteLengthInputStream(Stream inStream, int limit) : base(inStream, limit)
	{
		_lookAhead = RequireByte();

		if (0 == _lookAhead) CheckEndOfContents();
	}

	/// <summary>
	///   Sets the EOF on00.
	/// </summary>
	/// <param name="eofOn00">if set to <c>true</c> [EOF on00].</param>
	internal void SetEofOn00(bool eofOn00)
	{
		_eofOn00 = eofOn00;
		if (_eofOn00 && 0 == _lookAhead) CheckEndOfContents();
	}

	/// <summary>
	///   Checks the end of contents.
	/// </summary>
	/// <exception cref="System.IO.IOException">malformed end-of-contents marker</exception>
	private void CheckEndOfContents()
	{
		if (0 != RequireByte()) throw new IOException("malformed end-of-contents marker");

		_lookAhead = -1;
		SetParentEofDetect();
	}

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
	/// <exception cref="System.IO.EndOfStreamException"></exception>
	public override int Read(byte[] buffer, int offset, int count)
	{
		// Only use this optimisation if we aren't checking for 00
		if (_eofOn00 || count <= 1) return base.Read(buffer, offset, count);

		if (_lookAhead < 0) return 0;

		var numRead = Stream.Read(buffer, offset + 1, count - 1);
		if (numRead <= 0) throw new EndOfStreamException();

		buffer[offset] = (byte) _lookAhead;
		_lookAhead     = RequireByte();

		return numRead + 1;
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
	/// <exception cref="System.IO.EndOfStreamException"></exception>
	public override int Read(Span<byte> buffer)
	{
		// Only use this optimisation if we aren't checking for 00
		if (_eofOn00 || buffer.Length <= 1) return base.Read(buffer);
		if (_lookAhead < 0) return 0;

		var numRead = Stream.Read(buffer[1..]);
		if (numRead <= 0) throw new EndOfStreamException();

		buffer[0]  = (byte) _lookAhead;
		_lookAhead = RequireByte();

		return numRead + 1;
	}

	/// <summary>
	///   Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of
	///   the stream.
	/// </summary>
	/// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
	public override int ReadByte()
	{
		if (_eofOn00 && _lookAhead <= 0)
		{
			if (0 == _lookAhead) CheckEndOfContents();
			return -1;
		}

		var result = _lookAhead;
		_lookAhead = RequireByte();
		return result;
	}

	/// <summary>
	///   Requires the byte.
	/// </summary>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.IO.EndOfStreamException"></exception>
	private int RequireByte()
	{
		var b = Stream.ReadByte();
		if (b < 0) throw new EndOfStreamException();

		return b;
	}
}
