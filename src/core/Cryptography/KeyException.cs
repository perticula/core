// perticula - core - KeyException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Security;

namespace core.Cryptography;

/// <summary>
///   Class KeyException.
///   Implements the <see cref="Security.GeneralSecurityException" />
/// </summary>
/// <seealso cref="Security.GeneralSecurityException" />
public class KeyException : GeneralSecurityException
{
	/// <summary>
	///   Initializes a new instance of the <see cref="KeyException" /> class.
	/// </summary>
	public KeyException()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public KeyException(string message) : base(message)
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyException" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="inner">The inner.</param>
	public KeyException(string message, Exception inner) : base(message, inner)
	{
	}
}
