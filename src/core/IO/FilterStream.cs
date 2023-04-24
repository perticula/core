// perticula - core - FilterStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO;

/// <summary>
///   Class FilterStream.
///   Implements the <see cref="System.IO.Stream" />
/// </summary>
/// <seealso cref="System.IO.Stream" />
public class FilterStream : Stream
{
	/// <summary>
	///   The base stream
	/// </summary>
	protected readonly Stream BaseStream;

	/// <summary>
	///   Initializes a new instance of the <see cref="FilterStream" /> class.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <exception cref="System.ArgumentNullException">s</exception>
	public FilterStream(Stream s) => BaseStream = s ?? throw new ArgumentNullException(nameof(s));

	/// <summary>
	///   When overridden in a derived class, gets a value indicating whether the current stream supports reading.
	/// </summary>
	/// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
	public override bool CanRead => BaseStream.CanRead;

	/// <summary>
	///   When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
	/// </summary>
	/// <value><c>true</c> if this instance can seek; otherwise, <c>false</c>.</value>
	public override bool CanSeek => BaseStream.CanSeek;

	/// <summary>
	///   When overridden in a derived class, gets a value indicating whether the current stream supports writing.
	/// </summary>
	/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
	public override bool CanWrite => BaseStream.CanWrite;

	/// <summary>
	///   When overridden in a derived class, gets the length in bytes of the stream.
	/// </summary>
	/// <value>The length.</value>
	public override long Length => BaseStream.Length;

	/// <summary>
	///   When overridden in a derived class, gets or sets the position within the current stream.
	/// </summary>
	/// <value>The position.</value>
	public override long Position
	{
		get => BaseStream.Position;
		set => BaseStream.Position = value;
	}

	/// <summary>
	///   Reads the bytes from the current stream and writes them to another stream, using a specified buffer size. Both
	///   streams positions are advanced by the number of bytes copied.
	/// </summary>
	/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
	/// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
	public override void CopyTo(Stream destination, int bufferSize) => Streams.CopyTo(BaseStream, destination, bufferSize);

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
	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) => Streams.CopyToAsync(BaseStream, destination, bufferSize, cancellationToken);

	/// <summary>
	///   When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to
	///   the underlying device.
	/// </summary>
	public override void Flush() => BaseStream.Flush();

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
	public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, offset, count);

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
	public override int Read(Span<byte> buffer) => BaseStream.Read(buffer);

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
	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => Streams.ReadAsync(BaseStream, buffer, cancellationToken);

	/// <summary>
	///   Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of
	///   the stream.
	/// </summary>
	/// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
	public override int ReadByte() => BaseStream.ReadByte();

	/// <summary>
	///   When overridden in a derived class, sets the position within the current stream.
	/// </summary>
	/// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
	/// <param name="origin">
	///   A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to
	///   obtain the new position.
	/// </param>
	/// <returns>The new position within the current stream.</returns>
	public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

	/// <summary>
	///   When overridden in a derived class, sets the length of the current stream.
	/// </summary>
	/// <param name="value">The desired length of the current stream in bytes.</param>
	public override void SetLength(long value) => BaseStream.SetLength(value);

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
	public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

	/// <summary>
	///   Writes a byte to the current position in the stream and advances the position within the stream by one byte.
	/// </summary>
	/// <param name="value">The byte to write to the stream.</param>
	public override void WriteByte(byte value) => BaseStream.WriteByte(value);

	/// <summary>
	///   When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
	///   position within this stream by the number of bytes written.
	/// </summary>
	/// <param name="buffer">A region of memory. This method copies the contents of this region to the current stream.</param>
	public override void Write(ReadOnlySpan<byte> buffer) => BaseStream.Write(buffer);

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
	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => Streams.WriteAsync(BaseStream, buffer, cancellationToken);

	/// <summary>
	///   Detaches the specified disposing.
	/// </summary>
	/// <param name="disposing">if set to <c>true</c> [disposing].</param>
	protected void Detach(bool disposing) => base.Dispose(disposing);

	/// <summary>
	///   Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed
	///   resources.
	/// </summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources;
	///   <see langword="false" /> to release only unmanaged resources.
	/// </param>
	protected override void Dispose(bool disposing)
	{
		if (disposing) BaseStream.Dispose();

		base.Dispose(disposing);
	}
}
