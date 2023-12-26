// perticula - core - CryptoException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Cryptography;

/// <summary>
///   Class CryptoException.
///   Implements the <see cref="System.Exception" />
/// </summary>
/// <seealso cref="System.Exception" />
public class CryptoException : Exception
{
	/// <summary>
	///   Initializes a new instance of the <see cref="CryptoException" /> class.
	/// </summary>
	public CryptoException()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="CryptoException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public CryptoException(string message) : base(message)
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="CryptoException" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	///   The exception that is the cause of the current exception, or a null reference (
	///   <see langword="Nothing" /> in Visual Basic) if no inner exception is specified.
	/// </param>
	public CryptoException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
