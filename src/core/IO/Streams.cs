// perticula - core - Streams.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.InteropServices;

namespace core.IO;

public static class Streams
{
	public static int DefaultBufferSize { get; } = Platform.Is64BitProcess ? 4096 : 1024;

	public static void CopyTo(Stream source, Stream destination)
	{
		CopyTo(source, destination, DefaultBufferSize);
	}

	public static Task CopyToAsync(Stream source, Stream destination) => CopyToAsync(source, destination, DefaultBufferSize);

	public static Task CopyToAsync(Stream source, Stream destination, int bufferSize) => CopyToAsync(source, destination, bufferSize, CancellationToken.None);

	public static Task CopyToAsync(Stream source, Stream destination, CancellationToken cancellationToken) => CopyToAsync(source, destination, DefaultBufferSize, cancellationToken);

	public static async Task CopyToAsync(Stream source, Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		int bytesRead;
		var buffer = new byte[bufferSize];
		while ((bytesRead = await ReadAsync(source, new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false)) != 0)
			await WriteAsync(destination, new ReadOnlyMemory<byte>(buffer, 0, bytesRead), cancellationToken).ConfigureAwait(false);
	}

	public static void CopyTo(Stream source, Stream destination, int bufferSize)
	{
		int bytesRead;
		var buffer = bufferSize <= DefaultBufferSize
			             ? stackalloc byte[bufferSize]
			             : new byte[bufferSize];
		while ((bytesRead = source.Read(buffer)) != 0) destination.Write(buffer[..bytesRead]);
	}

	public static void Drain(Stream inStr) => CopyTo(inStr, Stream.Null, DefaultBufferSize);

	/// <summary>Write the full contents of inStr to the destination stream outStr.</summary>
	/// <param name="inStr">Source stream.</param>
	/// <param name="outStr">Destination stream.</param>
	/// <exception cref="IOException">In case of IO failure.</exception>
	public static void PipeAll(Stream inStr, Stream outStr) => PipeAll(inStr, outStr, DefaultBufferSize);

	/// <summary>Write the full contents of inStr to the destination stream outStr.</summary>
	/// <param name="inStr">Source stream.</param>
	/// <param name="outStr">Destination stream.</param>
	/// <param name="bufferSize">The size of temporary buffer to use.</param>
	/// <exception cref="IOException">In case of IO failure.</exception>
	public static void PipeAll(Stream inStr, Stream outStr, int bufferSize) => CopyTo(inStr, outStr, bufferSize);

	/// <summary>
	///   Pipe all bytes from <c>inStr</c> to <c>outStr</c>, throwing <c>StreamFlowException</c> if greater
	///   than <c>limit</c> bytes in <c>inStr</c>.
	/// </summary>
	/// <param name="inStr">
	///   A <see cref="Stream" />
	/// </param>
	/// <param name="limit">
	///   A <see cref="System.Int64" />
	/// </param>
	/// <param name="outStr">
	///   A <see cref="Stream" />
	/// </param>
	/// <returns>The number of bytes actually transferred, if not greater than <c>limit</c></returns>
	/// <exception cref="IOException"></exception>
	public static long PipeAllLimited(Stream inStr, long limit, Stream outStr) => PipeAllLimited(inStr, limit, outStr, DefaultBufferSize);

	public static long PipeAllLimited(Stream inStr, long limit, Stream outStr, int bufferSize)
	{
		var limited = new LimitedInputStream(inStr, limit);
		CopyTo(limited, outStr, bufferSize);
		return limit - limited.CurrentLimit;
	}

	public static byte[] ReadAll(Stream inStr)
	{
		var buf = new MemoryStream();
		PipeAll(inStr, buf);
		return buf.ToArray();
	}

	public static byte[] ReadAll(MemoryStream inStr) => inStr.ToArray();

	public static byte[] ReadAllLimited(Stream inStr, int limit)
	{
		var buf = new MemoryStream();
		PipeAllLimited(inStr, limit, buf);
		return buf.ToArray();
	}

	public static ValueTask<int> ReadAsync(Stream source, Memory<byte> buffer, CancellationToken cancellationToken = default)
	{
		if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> array))
			return new ValueTask<int>(source.ReadAsync(array.Array!, array.Offset, array.Count, cancellationToken));

		var sharedBuffer = new byte[buffer.Length];
		var readTask     = source.ReadAsync(sharedBuffer, 0, buffer.Length, cancellationToken);
		return FinishReadAsync(readTask, sharedBuffer, buffer);
	}

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

	public static int ReadFully(Stream inStr, byte[] buf) => ReadFully(inStr, buf, 0, buf.Length);

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

	public static ValueTask WriteAsync(Stream destination, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
	{
		if (MemoryMarshal.TryGetArray(buffer, out var array))
			return new ValueTask(destination.WriteAsync(array.Array!, array.Offset, array.Count, cancellationToken));

		var sharedBuffer = buffer.ToArray();
		var writeTask    = destination.WriteAsync(sharedBuffer, 0, buffer.Length, cancellationToken);
		return new ValueTask(FinishWriteAsync(writeTask, sharedBuffer));
	}

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

	public static void ValidateBufferArguments(byte[] buffer, int offset, int count)
	{
		if (buffer == null) throw new ArgumentNullException(nameof(buffer));
		var available = buffer.Length - offset;
		if ((offset | available) < 0) throw new ArgumentOutOfRangeException(nameof(offset));

		var remaining = available - count;
		if ((count | remaining) < 0) throw new ArgumentOutOfRangeException(nameof(count));
	}
}
