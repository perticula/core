// perticula - core - InvalidLicenseException.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Licensing;

/// <summary>
///   Class InvalidLicenseException.
///   Implements the <see cref="System.Exception" />
/// </summary>
/// <seealso cref="System.Exception" />
public class InvalidLicenseException : Exception
{
	/// <summary>
	///   Initializes a new instance of the <see cref="InvalidLicenseException" /> class.
	/// </summary>
	public InvalidLicenseException()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="InvalidLicenseException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public InvalidLicenseException(string message) : base(message)
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="InvalidLicenseException" /> class.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="inner">The inner.</param>
	public InvalidLicenseException(string message, Exception inner) : base(message, inner)
	{
	}
}
