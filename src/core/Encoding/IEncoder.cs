// perticula - core - IEncoder.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Encoding;

/// <summary>
///   Interface IEncoder
/// </summary>
public interface IEncoder
{
	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	int Encode(byte[] data, int off, int length, Stream outStream);

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	int Encode(ReadOnlySpan<byte> data, Stream outStream);

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	int Decode(byte[] data, int off, int length, Stream outStream);

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	int Decode(ReadOnlySpan<byte> data, Stream outStream);

	/// <summary>
	///   Decodes the string.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	int DecodeString(string data, Stream outStream);
}
