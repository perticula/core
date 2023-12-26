// perticula - core - Streams.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.InteropServices;

namespace core.IO;

/// <summary>
///   Class Streams.
/// </summary>
public static class Streams
{
	/// <summary>
	///   Gets the default size of the buffer.
	/// </summary>
	/// <value>The default size of the buffer.</value>
	public static int DefaultBufferSize { get; } = Platform.Is64BitProcess ? 4096 : 1024;

	/// <summary>
	///   Copies to.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	public static void CopyTo(Stream source, Stream destination) => CopyTo(source, destination, DefaultBufferSize);

	/// <summary>
	///   Copies to asynchronous.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	/// <returns>Task.</returns>
	public static Task CopyToAsync(Stream source, Stream destination) => CopyToAsync(source, destination, DefaultBufferSize);

	/// <summary>
	///   Copies to asynchronous.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="bufferSize">Size of the buffer.</param>
	/// <returns>Task.</returns>
	public static Task CopyToAsync(Stream source, Stream destination, int bufferSize) => CopyToAsync(source, destination, bufferSize, CancellationToken.None);

	/// <summary>
	///   Copies to asynchronous.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>Task.</returns>
	public static Task CopyToAsync(Stream source, Stream destination, CancellationToken cancellationToken) => CopyToAsync(source, destination, DefaultBufferSize, cancellationToken);

	/// <summary>
	///   Copy to as an asynchronous operation.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="bufferSize">Size of the buffer.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	public static async Task CopyToAsync(Stream source, Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		int bytesRead;
		var buffer = new byte[bufferSize];
		while ((bytesRead = await ReadAsync(source, new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false)) != 0)
			await WriteAsync(destination, new ReadOnlyMemory<byte>(buffer, 0, bytesRead), cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	///   Copies to.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="bufferSize">Size of the buffer.</param>
	public static void CopyTo(Stream source, Stream destination, int bufferSize)
	{
		int bytesRead;
		var buffer = bufferSize <= DefaultBufferSize
			             ? stackalloc byte[bufferSize]
			             : new byte[bufferSize];
		while ((bytesRead = source.Read(buffer)) != 0) destination.Write(buffer[..bytesRead]);
	}

	/// <summary>
	///   Drains the specified in string.
	/// </summary>
	/// <param name="inStr">The in string.</param>
	public static void Drain(Stream inStr) => CopyTo(inStr, Stream.Null, DefaultBufferSize);

	/// <summary>
	///   Write the full contents of inStr to the destination stream outStr.
	/// </summary>
	/// <param name="inStr">Source stream.</param>
	/// <param name="outStr">Destination stream.</param>
	/// <exception cref="IOException">In case of IO failure.</exception>
	public static void PipeAll(Stream inStr, Stream outStr) => PipeAll(inStr, outStr, DefaultBufferSize);

	/// <summary>
	///   Write the full contents of inStr to the destination stream outStr.
	/// </summary>
	/// <param name="inStr">Source stream.</param>
	/// <param name="outStr">Destination stream.</param>
	/// <param name="bufferSize">The size of temporary buffer to use.</param>
	/// <exception cref="IOException">In case of IO failure.</exception>
	public static void PipeAll(Stream inStr, Stream outStr, int bufferSize) => CopyTo(inStr, outStr, bufferSize);

	/// <summary>
	///   Reads all.
	/// </summary>
	/// <param name="inStr">The in string.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] ReadAll(Stream inStr)
	{
		var buf = new MemoryStream();
		PipeAll(inStr, buf);
		return buf.ToArray();
	}

	/// <summary>
	///   Reads all.
	/// </summary>
	/// <param name="inStr">The in string.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] ReadAll(MemoryStream inStr) => inStr.ToArray();

	/// <summary>
	///   Reads the asynchronous.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="buffer">The buffer.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>ValueTask&lt;System.Int32&gt;.</returns>
	public static ValueTask<int> ReadAsync(Stream source, Memory<byte> buffer, CancellationToken cancellationToken = default)
	{
		if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> array))
			return new ValueTask<int>(source.ReadAsync(array.Array!, array.Offset, array.Count, cancellationToken));

		var sharedBuffer = new byte[buffer.Length];
		var readTask     = source.ReadAsync(sharedBuffer, 0, buffer.Length, cancellationToken);
		return FinishReadAsync(readTask, sharedBuffer, buffer);
	}

	/// <summary>
	///   Finish read as an asynchronous operation.
	/// </summary>
	/// <param name="readTask">The read task.</param>
	/// <param name="localBuffer">The local buffer.</param>
	/// <param name="localDestination">The local destination.</param>
	/// <returns>A Task&lt;System.Int32&gt; representing the asynchronous operation.</returns>
	private static async ValueTask<int> FinishReadAsync(Task<int> readTask, byte[] localBuffer, Memory<byte> localDestination)
	{
		try
		{
			var result = await readTask.ConfigureAwait(false);
			new ReadOnlySpan<byte>(localBuffer, 0, result).CopyTo(localDestination.Span);
			return result;
		}
		finally
		{
			Array.Fill<byte>(localBuffer, 0x00);
		}
	}

	/// <summary>
	///   Reads the fully.
	/// </summary>
	/// <param name="inStr">The in string.</param>
	/// <param name="buf">The buf.</param>
	/// <returns>System.Int32.</returns>
	public static int ReadFully(Stream inStr, byte[] buf) => ReadFully(inStr, buf, 0, buf.Length);

	/// <summary>
	///   Reads the fully.
	/// </summary>
	/// <param name="inStr">The in string.</param>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.Int32.</returns>
	public static int ReadFully(Stream inStr, byte[] buf, int off, int len)
	{
		var totalRead = 0;
		while (totalRead < len)
		{
			var numRead = inStr.Read(buf, off + totalRead, len - totalRead);
			if (numRead < 1)
				break;
			totalRead += numRead;
		}

		return totalRead;
	}

	/// <summary>
	///   Reads the fully.
	/// </summary>
	/// <param name="inStr">The in string.</param>
	/// <param name="buffer">The buffer.</param>
	/// <returns>System.Int32.</returns>
	public static int ReadFully(Stream inStr, Span<byte> buffer)
	{
		var totalRead = 0;
		while (totalRead < buffer.Length)
		{
			var numRead = inStr.Read(buffer[totalRead..]);
			if (numRead < 1)
				break;
			totalRead += numRead;
		}

		return totalRead;
	}

	/// <summary>
	///   Writes the asynchronous.
	/// </summary>
	/// <param name="destination">The destination.</param>
	/// <param name="buffer">The buffer.</param>
	/// <param name="cancellationToken">
	///   The cancellation token that can be used by other objects or threads to receive notice
	///   of cancellation.
	/// </param>
	/// <returns>ValueTask.</returns>
	public static ValueTask WriteAsync(Stream destination, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
	{
		if (MemoryMarshal.TryGetArray(buffer, out var array))
			return new ValueTask(destination.WriteAsync(array.Array!, array.Offset, array.Count, cancellationToken));

		var sharedBuffer = buffer.ToArray();
		var writeTask    = destination.WriteAsync(sharedBuffer, 0, buffer.Length, cancellationToken);
		return new ValueTask(FinishWriteAsync(writeTask, sharedBuffer));
	}

	/// <summary>
	///   Writes the buf to.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <param name="output">The output.</param>
	/// <param name="offset">The offset.</param>
	/// <returns>System.Int32.</returns>
	public static int WriteBufTo(MemoryStream buf, byte[] output, int offset)
	{
		if (buf.TryGetBuffer(out var buffer))
		{
			buffer.CopyTo(output, offset);
			return buffer.Count;
		}

		var size = Convert.ToInt32(buf.Length);
		buf.WriteTo(new MemoryStream(output, offset, size));
		return size;
	}

	/// <summary>
	///   Finish write as an asynchronous operation.
	/// </summary>
	/// <param name="writeTask">The write task.</param>
	/// <param name="localBuffer">The local buffer.</param>
	/// <returns>A Task representing the asynchronous operation.</returns>
	private static async Task FinishWriteAsync(Task writeTask, byte[] localBuffer)
	{
		try
		{
			await writeTask.ConfigureAwait(false);
		}
		finally
		{
			Array.Fill<byte>(localBuffer, 0x00);
		}
	}

	/// <summary>
	///   Validates the buffer arguments.
	/// </summary>
	/// <param name="buffer">The buffer.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="count">The count.</param>
	/// <exception cref="System.ArgumentNullException">buffer</exception>
	/// <exception cref="System.ArgumentOutOfRangeException">offset</exception>
	/// <exception cref="System.ArgumentOutOfRangeException">count</exception>
	public static void ValidateBufferArguments(byte[] buffer, int offset, int count)
	{
				ArgumentNullException.ThrowIfNull(buffer);
				var available = buffer.Length - offset;
		if ((offset | available) < 0) throw new ArgumentOutOfRangeException(nameof(offset));

		var remaining = available - count;
		if ((count | remaining) < 0) throw new ArgumentOutOfRangeException(nameof(count));
	}
}
