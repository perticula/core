// perticula - core - Asn1Encoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Encoding.
///   Implements the <see cref="core.Protocol.asn1.IAsn1Encoding" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1Encoding" />
public class Asn1Encoding : IAsn1Encoding
{
	/// <summary>
	///   The contents octets
	/// </summary>
	private readonly byte[] _contentsOctets;

	/// <summary>
	///   The tag class
	/// </summary>
	private readonly int _tagClass;

	/// <summary>
	///   The tag no
	/// </summary>
	private readonly int _tagNo;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Encoding" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsOctets">The contents octets.</param>
	internal Asn1Encoding(int tagClass, int tagNo, byte[] contentsOctets)
	{
		_tagClass       = tagClass;
		_tagNo          = tagNo;
		_contentsOctets = contentsOctets;
	}

	/// <summary>
	///   Encodes the specified buffer.
	/// </summary>
	/// <param name="asn1Out">The asn1 out.</param>
	public virtual void Encode(Asn1OutputStream asn1Out)
	{
		asn1Out.WriteIdentifier(_tagClass, _tagNo);
		asn1Out.WriteDefiniteLength(_contentsOctets.Length);
		asn1Out.Write(_contentsOctets, 0, _contentsOctets.Length);
	}

	/// <summary>
	///   Gets the length of the value to encode.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public virtual int GetLength() => Asn1OutputStream.GetLengthOfEncodingDefiniteLength(_tagNo, _contentsOctets.Length);
}
