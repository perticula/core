// perticula - core - ICipher.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Cryptography;

/// <summary>
///   Interface ICipher
///   Base interface for a ciphers that do not require data to be block aligned.
///   <para>
///     Note: In cases where the underlying algorithm is block based, these ciphers may add or remove padding as needed.
///   </para>
/// </summary>
public interface ICipher
{
	/// <summary>
	///   Gets the message stream for reading/writing data processed/to be processed.
	/// </summary>
	/// <value>The message stream associated with this cipher.</value>
	Stream Message { get; }

	/// <summary>
	///   Gets the maximum size of the output.
	///   Return the size of the output buffer required for a Write() plus a
	///   close() with the write() being passed `inputLength` bytes.
	///   <para>
	///     The returned size may be dependent on the initialisation of this cipher
	///     and may not be accurate once subsequent input data is processed as the cipher may
	///     add, add or remove padding, as it sees fit.
	///   </para>
	/// </summary>
	/// <param name="inputLength">Length of the expected input.</param>
	/// <returns>
	///   System.Int32: The space required to accommodate a call to processBytes and doFinal with inputLen bytes of
	///   input.
	/// </returns>
	int GetMaxOutputSize(int inputLength);

	/// <summary>
	///   Gets the size of the update output buffer required for a write()
	///   with the write() being passed `inputLength` bytes and just updating the cipher output.
	/// </summary>
	/// <param name="inputLength">Length of the input.</param>
	/// <returns>System.Int32.</returns>
	int GetUpdateOutputSize(int inputLength);
}
