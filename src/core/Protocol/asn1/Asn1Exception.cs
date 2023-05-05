// perticula - core - Asn1Exception.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.Serialization;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Exception.
///   Implements the <see cref="System.IO.IOException" />
/// </summary>
/// <seealso cref="System.IO.IOException" />
[Serializable]
public class Asn1Exception : IOException
{
	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Exception" /> class.
	/// </summary>
	public Asn1Exception() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Exception" /> class.
	/// </summary>
	/// <param name="message">
	///   A <see cref="T:System.String" /> that describes the error. The content of
	///   <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to
	///   ensure that this string has been localized for the current system culture.
	/// </param>
	public Asn1Exception(string message) : base(message) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Exception" /> class.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">
	///   The exception that is the cause of the current exception. If the
	///   <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a
	///   <see langword="catch" /> block that handles the inner exception.
	/// </param>
	public Asn1Exception(string message, Exception innerException) : base(message, innerException) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Exception" /> class.
	/// </summary>
	/// <param name="info">The data for serializing or deserializing the object.</param>
	/// <param name="context">The source and destination for the object.</param>
	protected Asn1Exception(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
