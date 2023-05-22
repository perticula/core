// perticula - core - ConstructedDerEncoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class ConstructedDerEncoding.
///   Implements the <see cref="core.Protocol.asn1.der.DerEncoding" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.der.DerEncoding" />
public class ConstructedDerEncoding : DerEncoding
{
	/// <summary>
	///   The contents elements
	/// </summary>
	private readonly DerEncoding[] _contentsElements;

	/// <summary>
	///   The contents length
	/// </summary>
	private readonly int _contentsLength;

	/// <summary>
	///   Initializes a new instance of the <see cref="ConstructedDerEncoding" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsElements">The contents elements.</param>
	internal ConstructedDerEncoding(int tagClass, int tagNo, DerEncoding[] contentsElements) : base(tagClass, tagNo)
	{
		Debug.Assert(contentsElements != null);
		_contentsElements = contentsElements;
		_contentsLength   = Asn1OutputStream.GetLengthOfContents(contentsElements);
	}

	/// <summary>
	///   Compares the length and contents.
	/// </summary>
	/// <param name="other">The other.</param>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.InvalidOperationException"></exception>
	protected override int CompareLengthAndContents(DerEncoding other)
	{
		if (other is not ConstructedDerEncoding that)
			throw new InvalidOperationException();

		if (_contentsLength != that._contentsLength)
			return _contentsLength - that._contentsLength;

		var length = System.Math.Min(_contentsElements.Length, that._contentsElements.Length);
		for (var i = 0; i < length; i++)
		{
			var c = _contentsElements[i].CompareTo(that._contentsElements[i]);
			if (c != 0)
				return c;
		}

		Debug.Assert(_contentsElements.Length == that._contentsElements.Length);
		return _contentsElements.Length - that._contentsElements.Length;
	}

	/// <summary>
	///   Encodes the specified buffer.
	/// </summary>
	/// <param name="asn1Out">The asn1 out.</param>
	public override void Encode(Asn1OutputStream asn1Out)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Constructed | TagClass, TagNo);
		asn1Out.WriteDefiniteLength(_contentsLength);
		asn1Out.EncodeContents(_contentsElements);
	}

	/// <summary>
	///   Gets the length of the value to encode.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public override int GetLength() => Asn1OutputStream.GetLengthOfEncodingDefiniteLength(TagNo, _contentsLength);
}
