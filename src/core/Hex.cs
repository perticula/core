// perticula - core - Hex.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using core.Encoding;

namespace core;

/// <summary>
///   Class Hex.
/// </summary>
public static class Hex
{
	/// <summary>
	///   The encoder
	/// </summary>
	private static readonly HexEncoder Encoder = new();

	/// <summary>
	///   Converts to hexstring.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.String.</returns>
	public static string ToHexString(byte[] data) => ToHexString(data, false);

	/// <summary>
	///   Converts to hexstring.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="upperCase">if set to <c>true</c> [upper case].</param>
	/// <returns>System.String.</returns>
	public static string ToHexString(byte[] data, bool upperCase) => ToHexString(data, 0, data.Length, upperCase);

	/// <summary>
	///   Converts to hexstring.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <returns>System.String.</returns>
	public static string ToHexString(byte[] data, int off, int length) => ToHexString(data, off, length, false);

	/// <summary>
	///   Converts to hexstring.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <param name="upperCase">if set to <c>true</c> [upper case].</param>
	/// <returns>System.String.</returns>
	public static string ToHexString(byte[] data, int off, int length, bool upperCase) =>
		ToHexString(data.AsMemory(off, length), upperCase);

	/// <summary>
	///   Converts to hexstring.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="upperCase">if set to <c>true</c> [upper case].</param>
	/// <returns>System.String.</returns>
	/// <exception cref="System.ArgumentOutOfRangeException">data</exception>
	public static string ToHexString(ReadOnlyMemory<byte> data, bool upperCase = false)
	{
		switch (data.Length)
		{
			case 0:
				return string.Empty;
			case > int.MaxValue / 2:
				throw new ArgumentOutOfRangeException(nameof(data));
		}

		if (upperCase)
			return string.Create(data.Length * 2, data, (chars, memory) =>
			{
				var length = Encoder.EncodeUpper(memory.Span, chars);
				Debug.Assert(chars.Length == length);
			});
		return string.Create(data.Length * 2, data, (chars, memory) =>
		{
			var length = Encoder.EncodeLower(memory.Span, chars);
			Debug.Assert(chars.Length == length);
		});
	}

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
		var bOut = new MemoryStream(length * 2);

		Encoder.Encode(data, off, length, bOut);

		return bOut.ToArray();
	}

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	public static int Encode(byte[] data, Stream outStream) => Encoder.Encode(data, 0, data.Length, outStream);

	/// <summary>
	///   Encodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="length">The length.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	public static int Encode(byte[] data, int off, int length, Stream outStream) =>
		Encoder.Encode(data, off, length, outStream);

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] Decode(byte[] data)
	{
		var bOut = new MemoryStream((data.Length + 1) / 2);

		Encoder.Decode(data, 0, data.Length, bOut);

		return bOut.ToArray();
	}

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] Decode(string data)
	{
		var bOut = new MemoryStream((data.Length + 1) / 2);

		Encoder.DecodeString(data, bOut);

		return bOut.ToArray();
	}

	/// <summary>
	///   Decodes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="outStream">The out stream.</param>
	/// <returns>System.Int32.</returns>
	public static int Decode(string data, Stream outStream) => Encoder.DecodeString(data, outStream);

	/// <summary>
	///   Decodes the strict.
	/// </summary>
	/// <param name="str">The string.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] DecodeStrict(string str) => Encoder.DecodeStrict(str, 0, str.Length);

	/// <summary>
	///   Decodes the strict.
	/// </summary>
	/// <param name="str">The string.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] DecodeStrict(string str, int off, int len) => Encoder.DecodeStrict(str, off, len);
}
