// perticula - core - InvalidParameterException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Cryptography;

/// <summary>
///   Class InvalidParameterException.
///   Implements the <see cref="System.Exception" />
/// </summary>
/// <seealso cref="System.Exception" />
public class InvalidParameterException : Exception
{
	/// <summary>
	///   Initializes a new instance of the <see cref="InvalidParameterException" /> class.
	/// </summary>
	public InvalidParameterException()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="InvalidParameterException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public InvalidParameterException(string message) : base(message)
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="InvalidParameterException" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="inner">The inner.</param>
	public InvalidParameterException(string message, Exception inner) : base(message, inner)
	{
	}
}
