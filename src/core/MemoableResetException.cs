// perticula - core - MemoableResetException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.Serialization;

namespace core;

/// <summary>
///   Class MemoableResetException.
///   Implements the <see cref="System.InvalidCastException" />
/// </summary>
/// <seealso cref="System.InvalidCastException" />
public class MemoableResetException : InvalidCastException
{
	/// <summary>
	///   Initializes a new instance of the <see cref="MemoableResetException" /> class.
	/// </summary>
	public MemoableResetException() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="MemoableResetException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public MemoableResetException(string message) : base(message) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="MemoableResetException" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	///   The exception that is the cause of the current exception. If the
	///   <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a
	///   <see langword="catch" /> block that handles the inner exception.
	/// </param>
	public MemoableResetException(string message, Exception innerException) : base(message, innerException) { }
}
