// perticula - core - BaseInputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO;

/// <summary>
///   Class BaseInputStream.
///   Implements the <see cref="System.IO.Stream" />
/// </summary>
/// <seealso cref="System.IO.Stream" />
public abstract class BaseInputStream : Stream
{
	/// <summary>
	///   When overridden in a derived class, gets a value indicating whether the current stream supports reading.
	/// </summary>
	/// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
	public sealed override bool CanRead => true;

	/// <summary>
	///   When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
	/// </summary>
	/// <value><c>true</c> if this instance can seek; otherwise, <c>false</c>.</value>
	public sealed override bool CanSeek => false;

	/// <summary>
	///   When overridden in a derived class, gets a value indicating whether the current stream supports writing.
	/// </summary>
	/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
	public sealed override bool CanWrite => false;

	/// <summary>
	///   When overridden in a derived class, gets the length in bytes of the stream.
	/// </summary>
	/// <value>The length.</value>
	/// <exception cref="System.NotSupportedException"></exception>
	public sealed override long Length => throw new NotSupportedException();

	/// <summary>
	///   When overridden in a derived class, gets or sets the position within the current stream.
	/// </summary>
	/// <value>The position.</value>
	/// <exception cref="System.NotSupportedException"></exception>
	public sealed override long Position
	{
		get => throw new NotSupportedException();
		set => throw new NotSupportedException();
	}

	/// <summary>
	///   When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
	///   position within this stream by the number of bytes written.
	/// </summary>
	/// <param name="buffer">A region of memory. This method copies the contents of this region to the current stream.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public sealed override void Write(ReadOnlySpan<byte> buffer) => throw new NotSupportedException();

	/// <summary>
	///   Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by
	///   the number of bytes written, and monitors cancellation requests.
	/// </summary>
	/// <param name="buffer">The region of memory to write data from.</param>
	/// <param name="cancellationToken">
	///   The token to monitor for cancellation requests. The default value is
	///   <see cref="P:System.Threading.CancellationToken.None" />.
	/// </param>
	/// <returns>A task that represents the asynchronous write operation.</returns>
	/// <exception cref="System.NotSupportedException"></exception>
	public sealed override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => throw new NotSupportedException();

	/// <summary>
	///   When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to
	///   the underlying device.
	/// </summary>
	public sealed override void Flush() { }

	/// <summary>
	///   When overridden in a derived class, sets the position within the current stream.
	/// </summary>
	/// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
	/// <param name="origin">
	///   A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to
	///   obtain the new position.
	/// </param>
	/// <returns>The new position within the current stream.</returns>
	/// <exception cref="System.NotSupportedException"></exception>
	public sealed override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

	/// <summary>
	///   When overridden in a derived class, sets the length of the current stream.
	/// </summary>
	/// <param name="value">The desired length of the current stream in bytes.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public sealed override void SetLength(long value) => throw new NotSupportedException();

	/// <summary>
	///   When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
	///   position within this stream by the number of bytes written.
	/// </summary>
	/// <param name="buffer">
	///   An array of bytes. This method copies <paramref name="count" /> bytes from
	///   <paramref name="buffer" /> to the current stream.
	/// </param>
	/// <param name="offset">
	///   The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the
	///   current stream.
	/// </param>
	/// <param name="count">The number of bytes to be written to the current stream.</param>
	/// <exception cref="System.NotSupportedException"></exception>
	public sealed override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

	/// <summary>
	///   Reads the bytes from the current stream and writes them to another stream, using a specified buffer size. Both
	///   streams positions are advanced by the number of bytes copied.
	/// </summary>
	/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
	/// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
	public override void CopyTo(Stream destination, int bufferSize) => Streams.CopyTo(this, destination, bufferSize);

	/// <summary>
	///   Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer
	///   size and cancellation token. Both streams positions are advanced by the number of bytes copied.
	/// </summary>
	/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
	/// <param name="bufferSize">
	///   The size, in bytes, of the buffer. This value must be greater than zero. The default size is
	///   81920.
	/// </param>
	/// <param name="cancellationToken">
	///   The token to monitor for cancellation requests. The default value is
	///   <see cref="P:System.Threading.CancellationToken.None" />.
	/// </param>
	/// <returns>A task that represents the asynchronous copy operation.</returns>
	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) => Streams.CopyToAsync(this, destination, bufferSize, cancellationToken);

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

		var pos = 0;
		try
		{
			while (pos < count)
			{
				var b = ReadByte();
				if (b < 0)
					break;

				buffer[offset + pos++] = (byte) b;
			}
		}
		catch (IOException)
		{
			if (pos == 0)
				throw;
		}

		return pos;
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
		int count = buffer.Length, pos = 0;
		try
		{
			while (pos < count)
			{
				var b = ReadByte();
				if (b < 0)
					break;

				buffer[pos++] = (byte) b;
			}
		}
		catch (IOException)
		{
			if (pos == 0)
				throw;
		}

		return pos;
	}

	/// <summary>
	///   Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the
	///   number of bytes read, and monitors cancellation requests.
	/// </summary>
	/// <param name="buffer">The region of memory to write the data into.</param>
	/// <param name="cancellationToken">
	///   The token to monitor for cancellation requests. The default value is
	///   <see cref="P:System.Threading.CancellationToken.None" />.
	/// </param>
	/// <returns>
	///   A task that represents the asynchronous read operation. The value of its
	///   <see cref="P:System.Threading.Tasks.ValueTask`1.Result" /> property contains the total number of bytes read into the
	///   buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not
	///   currently available, or it can be 0 (zero) if the end of the stream has been reached.
	/// </returns>
	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => Streams.ReadAsync(this, buffer, cancellationToken);
}
