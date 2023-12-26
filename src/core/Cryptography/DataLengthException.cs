// perticula - core - DataLengthException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.Serialization;

namespace core.Cryptography;

/// <summary>
///   Class DataLengthException.
///   This exception is thrown if a buffer that is meant to have output copied into it turns out to be too
///   short, or if we've been given insufficient input.
/// </summary>
/// <remarks>
///   Generally this is similar to an IndexOutOfRangeException, but it's more specific and it's a semantically more
///   precise.
/// </remarks>
public class DataLengthException : CryptoException
{
	/// <summary>
	///   Initializes a new instance of the <see cref="DataLengthException" /> class.
	/// </summary>
	public DataLengthException() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DataLengthException" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	public DataLengthException(string message) : base(message) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DataLengthException" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="innerException">The inner exception.</param>
	public DataLengthException(string message, Exception innerException) : base(message, innerException) { }
}
