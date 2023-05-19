// perticula - core - ConstructedDefiniteLengthEncoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Class ConstructedDefiniteLengthEncoding.
///   Implements the <see cref="core.Protocol.asn1.IAsn1Encoding" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1Encoding" />
public class ConstructedDefiniteLengthEncoding : IAsn1Encoding
{
	/// <summary>
	///   The contents elements
	/// </summary>
	private readonly IAsn1Encoding[] _contentsElements;

	/// <summary>
	///   The contents length
	/// </summary>
	private readonly int _contentsLength;

	/// <summary>
	///   The tag class
	/// </summary>
	private readonly int _tagClass;

	/// <summary>
	///   The tag no
	/// </summary>
	private readonly int _tagNo;

	/// <summary>
	///   Initializes a new instance of the <see cref="ConstructedDefiniteLengthEncoding" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsElements">The contents elements.</param>
	internal ConstructedDefiniteLengthEncoding(int tagClass, int tagNo, IAsn1Encoding[] contentsElements)
	{
		_tagClass         = tagClass;
		_tagNo            = tagNo;
		_contentsElements = contentsElements;
		_contentsLength   = Asn1OutputStream.GetLengthOfContents(contentsElements);
	}

	/// <summary>
	///   Encodes the specified buffer.
	/// </summary>
	/// <param name="asn1Out">The asn1 out.</param>
	public void Encode(Asn1OutputStream asn1Out)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Constructed | _tagClass, _tagNo);
		asn1Out.WriteDL(_contentsLength);
		asn1Out.EncodeContents(_contentsElements);
	}

	/// <summary>
	///   Gets the length of the value to encode.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public int GetLength() => Asn1OutputStream.GetLengthOfEncodingDL(_tagNo, _contentsLength);
}
