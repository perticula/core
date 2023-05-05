// perticula - core - IEncoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Interface IEncoding
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEncoding<in T>
{
	/// <summary>
	///   Encodes the specified buffer.
	/// </summary>
	/// <param name="buffer">The buffer.</param>
	void Encode(T buffer);

	/// <summary>
	///   Gets the length of the value to encode.
	/// </summary>
	/// <returns>System.Int32.</returns>
	int GetLength();
}
