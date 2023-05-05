// perticula - core - Asn1ParseException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.Serialization;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1ParseException.
///   Implements the <see cref="System.InvalidOperationException" />
/// </summary>
/// <seealso cref="System.InvalidOperationException" />
[Serializable]
public class Asn1ParseException : InvalidOperationException
{
	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1ParseException" /> class.
	/// </summary>
	public Asn1ParseException() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1ParseException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public Asn1ParseException(string message) : base(message) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1ParseException" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	///   The exception that is the cause of the current exception. If the
	///   <paramref name="innerException" /> parameter is not a null reference (<see langword="Nothing" /> in Visual Basic),
	///   the current exception is raised in a <see langword="catch" /> block that handles the inner exception.
	/// </param>
	public Asn1ParseException(string message, Exception innerException) : base(message, innerException) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1ParseException" /> class.
	/// </summary>
	/// <param name="info">The object that holds the serialized object data.</param>
	/// <param name="context">The contextual information about the source or destination.</param>
	protected Asn1ParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
