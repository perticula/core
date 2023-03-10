// perticula - core - FilterSyntaxException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Runtime.Serialization;

namespace core.Filtering;

[Serializable]
public class FilterSyntaxException : Exception
{
	/// <summary>
	///   The position of the sytax error
	/// </summary>
	public readonly int? At;

	/// <summary>
	///   The origina input string
	/// </summary>
	public readonly string? Input;

	/// <summary>
	///   Initializes a new instance of the <see cref="FilterSyntaxException" /> class.
	/// </summary>
	public FilterSyntaxException() { }

	/// <summary>
	///   Initializes a new instance of the <see cref="FilterSyntaxException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public FilterSyntaxException(string message) : base(message) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="FilterSyntaxException" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="inner">The inner.</param>
	public FilterSyntaxException(string message, Exception inner) : base(message, inner) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="FilterSyntaxException" /> class.
	/// </summary>
	/// <param name="msg">The MSG.</param>
	/// <param name="input">The input.</param>
	/// <param name="at">At.</param>
	public FilterSyntaxException(string msg, string input, int at = 0)
		: base(msg)
	{
		Input = input;
		At    = at;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="FilterSyntaxException" /> class.
	/// </summary>
	/// <param name="info">
	///   The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
	///   data about the exception being thrown.
	/// </param>
	/// <param name="context">
	///   The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual
	///   information about the source or destination.
	/// </param>
	protected FilterSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
