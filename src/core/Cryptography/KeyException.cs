// perticula - core - KeyException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.Serialization;
using core.Security;

namespace core.Cryptography;

/// <summary>
///   Class KeyException.
///   Implements the <see cref="Security.GeneralSecurityException" />
/// </summary>
/// <seealso cref="Security.GeneralSecurityException" />
[Serializable]
public class KeyException : GeneralSecurityException
{
	/// <summary>
	///   Initializes a new instance of the <see cref="KeyException" /> class.
	/// </summary>
	public KeyException() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public KeyException(string message) : base(message) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyException" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="inner">The inner.</param>
	public KeyException(string message, Exception inner) : base(message, inner) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="KeyException" /> class.
	/// </summary>
	/// <param name="info">
	///   The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
	///   data about the exception being thrown.
	/// </param>
	/// <param name="context">
	///   The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
	///   information about the source or destination.
	/// </param>
	protected KeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
