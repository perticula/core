// perticula - core - Base64.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Encoding;

/// <summary>
///   Class Base64.
/// </summary>
public static class Base64
{
	/// <summary>
	///   Converts to base64string.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.String.</returns>
	public static string ToBase64String(byte[] data) => Convert.ToBase64String(data, 0, data.Length);

	/// <summary>
	///   Converts to base64string.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <returns>System.String.</returns>
	public static string ToBase64String(byte[] data, int off, int length) => Convert.ToBase64String(data, off, length);

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] Encode(byte[] data) => Encode(data, 0, data.Length);

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] Encode(byte[] data, int off, int length)
	{
		var s = Convert.ToBase64String(data, off, length);
		return s.ToAsciiByteArray();
	}

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	public static int Encode(byte[] data, Stream outStream)
	{
		var encoded = Encode(data);
		outStream.Write(encoded, 0, encoded.Length);
		return encoded.Length;
	}

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	public static int Encode(byte[] data, int off, int length, Stream outStream)
	{
		var encoded = Encode(data, off, length);
		outStream.Write(encoded, 0, encoded.Length);
		return encoded.Length;
	}

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] Decode(byte[] data)
	{
		var s = data.FromAsciiByteArray();
		return Convert.FromBase64String(s);
	}

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] Decode(string data) => Convert.FromBase64String(data);

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	public static int Decode(string data, Stream outStream)
	{
		var decoded = Decode(data);
		outStream.Write(decoded, 0, decoded.Length);
		return decoded.Length;
	}
}
