// perticula - core - LimitedInputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO;

/// <summary>
///   Class LimitedInputStream.
///   Implements the <see cref="core.IO.BaseInputStream" />
/// </summary>
/// <seealso cref="core.IO.BaseInputStream" />
public class LimitedInputStream : BaseInputStream
{
	/// <summary>
	///   The stream
	/// </summary>
	protected readonly Stream Stream;

	/// <summary>
	///   Initializes a new instance of the <see cref="LimitedInputStream" /> class.
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <param name="limit">The limit.</param>
	internal LimitedInputStream(Stream stream, int limit)
	{
		Stream       = stream;
		CurrentLimit = limit;
	}

	/// <summary>
	///   Gets the current limit.
	/// </summary>
	/// <value>The current limit.</value>
	internal int CurrentLimit { get; private set; }

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
	/// <exception cref="core.IO.StreamOverflowException">Data Overflow</exception>
	public override int Read(byte[] buffer, int offset, int count)
	{
		var numRead = Stream.Read(buffer, offset, count);
		return numRead switch
		       {
			       > 0 when (CurrentLimit -= numRead) < 0 => throw new StreamOverflowException("Data Overflow"),
			       _                                      => numRead
		       };
	}

	/// <summary>
	///   Sets the parent EOF detect.
	/// </summary>
	protected void SetParentEofDetect()
	{
		if (Stream is IndefiniteLengthInputStream stream) stream.SetEofOn00(true);
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
	/// <exception cref="core.IO.StreamOverflowException">Data Overflow</exception>
	public override int Read(Span<byte> buffer)
	{
		var numRead = Stream.Read(buffer);
		return numRead switch
		       {
			       > 0 when (CurrentLimit -= numRead) < 0 => throw new StreamOverflowException("Data Overflow"),
			       _                                      => numRead
		       };
	}

	/// <summary>
	///   Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of
	///   the stream.
	/// </summary>
	/// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
	/// <exception cref="core.IO.StreamOverflowException">Data Overflow</exception>
	public override int ReadByte()
	{
		var b = Stream.ReadByte();
		if (b              < 0) return b;
		if (--CurrentLimit < 0) throw new StreamOverflowException("Data Overflow");
		return b;
	}
}
