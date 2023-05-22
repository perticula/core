// perticula - core - IEncodable.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Encoding;

/// <summary>
///   Interface IEncodable
/// </summary>
public interface IEncodable
{
	/// <summary>
	///   Gets the implementing instance as a byte array.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	byte[] GetEncoded();
}
