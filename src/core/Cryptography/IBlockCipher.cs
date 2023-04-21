// perticula - core - IBlockCipher.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Cryptography;

public interface IBlockCipher
{
	/// <summary>
	///   Gets the name of the algorithm used by this cipher.
	/// </summary>
	/// <value>The name of the algorithm.</value>
	string AlgorithmName { get; }

	/// <summary>
	///   Gets the size of the block.
	/// </summary>
	/// <value>The size (in bytes) of the block for this cyper.</value>
	int BlockSize { get; }

	/// <summary>
	///   Initializes the cipher specified for encryption.
	/// </summary>
	/// <param name="forEncryption">
	///   if set to <c>true</c> the ciper is initialized for encryption.  if set to <c>false</c> the
	///   ciper is initialized for decryption.
	/// </param>
	/// <param name="parameters">The key, or other data parameters, required to initilize the cipher.</param>
	void Init(bool forEncryption, ICipherParameters parameters);

	/// <summary>
	///   Processes a block.
	/// </summary>
	/// <param name="input">The input block (as a span).</param>
	/// <param name="output">The output block (as a span).</param>
	/// <exception cref="DataLengthException">If input block is wrong size, or output span too small.</exception>
	/// <returns>System.Int32: The number of bytes processed and produced.</returns>
	int ProcessBlock(ReadOnlySpan<byte> input, Span<byte> output);
}
