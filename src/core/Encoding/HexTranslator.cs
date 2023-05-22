// perticula - core - HexTranslator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Encoding;

/// <summary>
///   Class HexTranslator.
///   Implements the <see cref="core.Encoding.IEncodingTranslator" />
/// </summary>
/// <seealso cref="core.Encoding.IEncodingTranslator" />
public class HexTranslator : IEncodingTranslator
{
	/// <summary>
	///   The hexadecimal table
	/// </summary>
	private static readonly byte[] HexTable = "0123456789abcdef"u8.ToArray();

	/// <summary>
	///   Return encoded block size.
	/// </summary>
	/// <returns>2</returns>
	public int GetEncodedBlockSize() => 2;

	/// <summary>
	///   Encode some data.
	/// </summary>
	/// <param name="input">Input data array.</param>
	/// <param name="inOff">Start position within input data array.</param>
	/// <param name="length">The amount of data to process.</param>
	/// <param name="outBytes">The output data array.</param>
	/// <param name="outOff">The offset within the output data array to start writing from.</param>
	/// <returns>Amount of data encoded.</returns>
	public int Encode(byte[] input, int inOff, int length, byte[] outBytes, int outOff)
	{
		for (int i = 0, j = 0; i < length; i++, j += 2)
		{
			outBytes[outOff + j]     = HexTable[(input[inOff] >> 4) & 0x0f];
			outBytes[outOff + j + 1] = HexTable[input[inOff]        & 0x0f];

			inOff++;
		}

		return length * 2;
	}

	/// <summary>
	///   Returns the decoded block size.
	/// </summary>
	/// <returns>1</returns>
	public int GetDecodedBlockSize() => 1;

	/// <summary>
	///   Decode data from a byte array.
	/// </summary>
	/// <param name="input">The input data array.</param>
	/// <param name="inOff">Start position within input data array.</param>
	/// <param name="length">The amounty of data to process.</param>
	/// <param name="outBytes">The output data array.</param>
	/// <param name="outOff">The position within the output data array to start writing from.</param>
	/// <returns>The amount of data written.</returns>
	public int Decode(byte[] input, int inOff, int length, byte[] outBytes, int outOff)
	{
		var halfLength = length / 2;
		for (var i = 0; i < halfLength; i++)
		{
			var left  = input[inOff + i * 2];
			var right = input[inOff + i * 2 + 1];

			if (left < (byte) 'a')
				outBytes[outOff] = (byte) ((left - '0') << 4);
			else
				outBytes[outOff] = (byte) ((left - 'a' + 10) << 4);
			if (right < (byte) 'a')
				outBytes[outOff] += (byte) (right - '0');
			else
				outBytes[outOff] += (byte) (right - 'a' + 10);

			outOff++;
		}

		return halfLength;
	}
}
