// perticula - core - HexEncoder.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Encoding;

/// <summary>
///   Class HexEncoder.
///   Implements the <see cref="core.Encoding.IEncoder" />
/// </summary>
/// <seealso cref="core.Encoding.IEncoder" />
public class HexEncoder : IEncoder
{
	/// <summary>
	///   The chars lower
	/// </summary>
	private static readonly char[] CharsLower = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};

	/// <summary>
	///   The chars upper
	/// </summary>
	private static readonly char[] CharsUpper = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

	/// <summary>
	///   The decoding table
	/// </summary>
	protected readonly byte[] DecodingTable = new byte[128];

	/// <summary>
	///   The encoding table
	/// </summary>
	protected readonly byte[] EncodingTable = "0123456789abcdef"u8.ToArray();

	/// <summary>
	///   Initializes a new instance of the <see cref="HexEncoder" /> class.
	/// </summary>
	public HexEncoder() => InitialiseDecodingTable();

	/// <summary>
	///   Encodes the specified buf.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>int.</returns>
	public int Encode(byte[] buf, int off, int len, Stream outStream) => Encode(buf.AsSpan(off, len), outStream);

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>int.</returns>
	public int Encode(ReadOnlySpan<byte> data, Stream outStream)
	{
		Span<byte> tmp    = stackalloc byte[72];
		var        result = data.Length * 2;
		while (!data.IsEmpty)
		{
			var inLen  = System.Math.Min(36, data.Length);
			var outLen = Encode(data[..inLen], tmp);
			outStream.Write(tmp[..outLen]);
			data = data[inLen..];
		}

		return result;
	}

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>int.</returns>
	public int Decode(byte[] data, int off, int length, Stream outStream) => Decode(data.AsSpan(off, length), outStream);

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>int.</returns>
	/// <exception cref="IOException">invalid characters encountered in Hex data</exception>
	public int Decode(ReadOnlySpan<byte> data, Stream outStream)
	{
		var        outLen = 0;
		Span<byte> buf    = stackalloc byte[36];
		var        bufOff = 0;
		var        end    = data.Length;

		while (end > 0)
		{
			if (!Ignore((char) data[end - 1]))
				break;

			end--;
		}

		var i = 0;
		while (i < end)
		{
			while (i < end && Ignore((char) data[i])) i++;

			var b1 = DecodingTable[data[i++]];

			while (i < end && Ignore((char) data[i])) i++;

			var b2 = DecodingTable[data[i++]];

			if ((b1 | b2) >= 0x80)
				throw new IOException("invalid characters encountered in Hex data");

			buf[bufOff++] = (byte) ((b1 << 4) | b2);

			if (bufOff == buf.Length)
			{
				outStream.Write(buf);
				bufOff = 0;
			}

			outLen++;
		}

		if (bufOff > 0) outStream.Write(buf[..bufOff]);

		return outLen;
	}


	/// <summary>
	///   Decodes the string.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>int.</returns>
	/// <exception cref="IOException">invalid characters encountered in Hex data</exception>
	public int DecodeString(string data, Stream outStream)
	{
		var        length = 0;
		Span<byte> buf    = stackalloc byte[36];

		var bufOff = 0;
		var end    = data.Length;

		while (end > 0)
		{
			if (!Ignore(data[end - 1]))
				break;

			end--;
		}

		var i = 0;
		while (i < end)
		{
			while (i < end && Ignore(data[i])) i++;

			var b1 = DecodingTable[data[i++]];

			while (i < end && Ignore(data[i])) i++;

			var b2 = DecodingTable[data[i++]];

			if ((b1 | b2) >= 0x80) throw new IOException("invalid characters encountered in Hex data");

			buf[bufOff++] = (byte) ((b1 << 4) | b2);

			if (bufOff == buf.Length)
			{
				outStream.Write(buf);

				bufOff = 0;
			}

			length++;
		}

		if (bufOff > 0) outStream.Write(buf[..bufOff]);

		return length;
	}

	/// <summary>
	///   Encodes the lower.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="output">The output.</param>
	/// <returns>int.</returns>
	public int EncodeLower(ReadOnlySpan<byte> input, Span<char> output)
	{
		var inPos  = 0;
		var inEnd  = input.Length;
		var outPos = 0;

		while (inPos < inEnd)
		{
			uint b = input[inPos++];

			output[outPos++] = CharsLower[b >> 4];
			output[outPos++] = CharsLower[b & 0xF];
		}

		return outPos;
	}

	/// <summary>
	///   Encodes the upper.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="output">The output.</param>
	/// <returns>int.</returns>
	public int EncodeUpper(ReadOnlySpan<byte> input, Span<char> output)
	{
		var inPos  = 0;
		var inEnd  = input.Length;
		var outPos = 0;

		while (inPos < inEnd)
		{
			uint b = input[inPos++];

			output[outPos++] = CharsUpper[b >> 4];
			output[outPos++] = CharsUpper[b & 0xF];
		}

		return outPos;
	}

	/// <summary>
	///   Initialises the decoding table.
	/// </summary>
	protected void InitialiseDecodingTable()
	{
		Arrays.Fill(DecodingTable, (byte) 0xff);

		for (var i = 0; i < EncodingTable.Length; i++) DecodingTable[EncodingTable[i]] = (byte) i;

		DecodingTable['A'] = DecodingTable['a'];
		DecodingTable['B'] = DecodingTable['b'];
		DecodingTable['C'] = DecodingTable['c'];
		DecodingTable['D'] = DecodingTable['d'];
		DecodingTable['E'] = DecodingTable['e'];
		DecodingTable['F'] = DecodingTable['f'];
	}

	/// <summary>
	///   Encodes the specified in buf.
	/// </summary>
	/// <param name="inBuf">The in buf.</param>
	/// <param name="inOff">The in off.</param>
	/// <param name="inLen">Length of the in.</param>
	/// <param name="outBuf">The out buf.</param>
	/// <param name="outOff">The out off.</param>
	/// <returns>int.</returns>
	public int Encode(byte[] inBuf, int inOff, int inLen, byte[] outBuf, int outOff) => Encode(inBuf.AsSpan(inOff, inLen), outBuf.AsSpan(outOff));

	/// <summary>
	///   Encodes the specified input.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="output">The output.</param>
	/// <returns>int.</returns>
	public int Encode(ReadOnlySpan<byte> input, Span<byte> output)
	{
		var inPos  = 0;
		var inEnd  = input.Length;
		var outPos = 0;

		while (inPos < inEnd)
		{
			uint b = input[inPos++];

			output[outPos++] = EncodingTable[b >> 4];
			output[outPos++] = EncodingTable[b & 0xF];
		}

		return outPos;
	}

	/// <summary>
	///   Ignores the specified c.
	/// </summary>
	/// <param name="c">The c.</param>
	/// <returns>bool.</returns>
	private static bool Ignore(char c) => c == '\n' || c == '\r' || c == '\t' || c == ' ';

	/// <summary>
	///   Decodes the strict.
	/// </summary>
	/// <param name="str">The string.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>byte[].</returns>
	/// <exception cref="ArgumentNullException">nameof(str)</exception>
	/// <exception cref="IndexOutOfRangeException">invalid offset and/or length specified</exception>
	/// <exception cref="ArgumentException">a hexadecimal encoding must have an even number of characters, nameof(len)</exception>
	/// <exception cref="IOException">invalid characters encountered in Hex data</exception>
	internal byte[] DecodeStrict(string str, int off, int len)
	{
		if (null == str) throw new ArgumentNullException(nameof(str));
		if (off < 0 || len < 0 || off > str.Length - len) throw new IndexOutOfRangeException("invalid offset and/or length specified");
		if (0 != (len & 1)) throw new ArgumentException("a hexadecimal encoding must have an even number of characters", nameof(len));

		var resultLen = len >> 1;
		var result    = new byte[resultLen];

		var strPos = off;
		for (var i = 0; i < resultLen; ++i)
		{
			var b1 = DecodingTable[str[strPos++]];
			var b2 = DecodingTable[str[strPos++]];

			if ((b1 | b2) >= 0x80)
				throw new IOException("invalid characters encountered in Hex data");

			result[i] = (byte) ((b1 << 4) | b2);
		}

		return result;
	}
}
