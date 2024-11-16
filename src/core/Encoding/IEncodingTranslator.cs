// perticula - core - IEncodingTranslator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Encoding;

/// <summary>
///   Interface IEncodingTranslator
/// </summary>
public interface IEncodingTranslator
{
	/// <summary>
	///   Gets the size of the encoded block.
	/// </summary>
	/// <returns>System.Int32.</returns>
	int GetEncodedBlockSize();

	/// <summary>
	///   Encodes the specified input.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="inOff">The in off.</param>
	/// <param name="length">The length.</param>
	/// <param name="outBytes">The out bytes.</param>
	/// <param name="outOff">The out off.</param>
	/// <returns>System.Int32.</returns>
	int Encode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);

	/// <summary>
	///   Gets the size of the decoded block.
	/// </summary>
	/// <returns>System.Int32.</returns>
	int GetDecodedBlockSize();

	/// <summary>
	///   Decodes the specified input.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <param name="inOff">The in off.</param>
	/// <param name="length">The length.</param>
	/// <param name="outBytes">The out bytes.</param>
	/// <param name="outOff">The out off.</param>
	/// <returns>System.Int32.</returns>
	int Decode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);
}
