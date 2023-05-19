// perticula - core - DerUtcTime.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerUtcTime.
///   Implements the <see cref="core.Protocol.asn1.Asn1UtcTime" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1UtcTime" />
public class DerUtcTime : Asn1UtcTime
{
	/// <summary>
	///   Initializes a new instance of the <see cref="DerUtcTime" /> class.
	/// </summary>
	/// <param name="timeString">The time string.</param>
	public DerUtcTime(string timeString) : base(timeString) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerUtcTime" /> class.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <param name="twoDigitYearMax">The two digit year maximum.</param>
	public DerUtcTime(DateTime dateTime, int twoDigitYearMax) : base(dateTime, twoDigitYearMax) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerUtcTime" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	internal DerUtcTime(byte[] contents) : base(contents) { }

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.UtcTime, GetContents(Asn1OutputStream.EncodingDer));

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, GetContents(Asn1OutputStream.EncodingDer));
}
