// perticula - core - DefiniteLengthInputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO;

/// <summary>
///   Class DefiniteLengthInputStream.
///   Implements the <see cref="core.IO.LimitedInputStream" />
/// </summary>
/// <seealso cref="core.IO.LimitedInputStream" />
public class DefiniteLengthInputStream : LimitedInputStream
{
	/// <summary>
	///   The empty bytes
	/// </summary>
	private static readonly byte[] EmptyBytes = Array.Empty<byte>();

	/// <summary>
	///   The original length
	/// </summary>
	private readonly long _originalLength;

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthInputStream" /> class.
	/// </summary>
	/// <param name="inStream">The in stream.</param>
	/// <param name="length">The length.</param>
	/// <param name="limit">The limit.</param>
	/// <exception cref="System.ArgumentException">negative lengths not allowed - length</exception>
	internal DefiniteLengthInputStream(Stream inStream, int length, int limit) : base(inStream, limit)
	{
		if (length <= 0)
		{
			if (length < 0) throw new ArgumentException("negative lengths not allowed", nameof(length));

			SetParentEofDetect();
		}

		_originalLength = length;
		Remaining       = length;
	}

	/// <summary>
	///   Gets the remaining.
	/// </summary>
	/// <value>The remaining.</value>
	internal int Remaining { get; private set; }

	/// <summary>
	///   Reads the byte.
	/// </summary>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.IO.EndOfStreamException">DEF length {_originalLength} object truncated by {Remaining}</exception>
	public override int ReadByte()
	{
		if (Remaining < 2)
		{
			if (Remaining == 0)
				return -1;

			var b = Stream.ReadByte();
			if (b < 0)
				throw new EndOfStreamException($"DEF length {_originalLength} object truncated by {Remaining}");

			Remaining = 0;
			SetParentEofDetect();

			return b;
		}
		else
		{
			var b = Stream.ReadByte();
			if (b < 0)
				throw new EndOfStreamException($"DEF length {_originalLength} object truncated by {Remaining}");

			--Remaining;
			return b;
		}
	}

	/// <summary>
	///   Reads the specified buf.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.IO.EndOfStreamException">DEF length {_originalLength} object truncated by {Remaining}</exception>
	public override int Read(byte[] buf, int off, int len)
	{
		if (Remaining == 0)
			return 0;

		var toRead  = System.Math.Min(len, Remaining);
		var numRead = Stream.Read(buf, off, toRead);

		if (numRead < 1)
			throw new EndOfStreamException($"DEF length {_originalLength} object truncated by {Remaining}");

		if ((Remaining -= numRead) == 0) SetParentEofDetect();

		return numRead;
	}

	/// <summary>
	///   Reads the specified buffer.
	/// </summary>
	/// <param name="buffer">The buffer.</param>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.IO.EndOfStreamException">DEF length {_originalLength} object truncated by {Remaining}</exception>
	public override int Read(Span<byte> buffer)
	{
		if (Remaining == 0) return 0;

		var toRead  = System.Math.Min(buffer.Length, Remaining);
		var numRead = Stream.Read(buffer[..toRead]);

		if (numRead < 1)
			throw new EndOfStreamException($"DEF length {_originalLength} object truncated by {Remaining}");

		if ((Remaining -= numRead) == 0) SetParentEofDetect();

		return numRead;
	}

	/// <summary>
	///   Reads all into byte array.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <exception cref="System.ArgumentException">buffer length not right for data</exception>
	/// <exception cref="System.IO.IOException">corrupted stream - out of bounds length found: {Remaining} >= {limit}</exception>
	/// <exception cref="System.IO.EndOfStreamException">DEF length {_originalLength} object truncated by {Remaining}</exception>
	internal void ReadAllIntoByteArray(byte[] buf)
	{
		if (Remaining != buf.Length)
			throw new ArgumentException("buffer length not right for data");

		if (Remaining == 0)
			return;

		// make sure it's safe to do this!
		var limit = CurrentLimit;
		if (Remaining >= limit)
			throw new IOException($"corrupted stream - out of bounds length found: {Remaining} >= {limit}");

		if ((Remaining -= Streams.ReadFully(Stream, buf, 0, buf.Length)) != 0)
			throw new EndOfStreamException($"DEF length {_originalLength} object truncated by {Remaining}");
		SetParentEofDetect();
	}

	/// <summary>
	///   Converts to array.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	/// <exception cref="System.IO.IOException">corrupted stream - out of bounds length found: {Remaining} >= {limit}</exception>
	/// <exception cref="System.IO.EndOfStreamException">DEF length {_originalLength} object truncated by {Remaining}</exception>
	internal byte[] ToArray()
	{
		if (Remaining == 0)
			return EmptyBytes;

		// make sure it's safe to do this!
		var limit = CurrentLimit;
		if (Remaining >= limit)
			throw new IOException($"corrupted stream - out of bounds length found: {Remaining} >= {limit}");

		var bytes = new byte[Remaining];
		if ((Remaining -= Streams.ReadFully(Stream, bytes, 0, bytes.Length)) != 0)
			throw new EndOfStreamException($"DEF length {_originalLength} object truncated by {Remaining}");
		SetParentEofDetect();
		return bytes;
	}
}
