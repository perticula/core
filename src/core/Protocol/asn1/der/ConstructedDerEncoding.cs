// perticula - core - ConstructedDerEncoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;

namespace core.Protocol.asn1.der;

public class ConstructedDerEncoding : DerEncoding
{
	private readonly DerEncoding[] _contentsElements;
	private readonly int           _contentsLength;

	internal ConstructedDerEncoding(int tagClass, int tagNo, DerEncoding[] contentsElements) : base(tagClass, tagNo)
	{
		Debug.Assert(contentsElements != null);
		_contentsElements = contentsElements;
		_contentsLength   = Asn1OutputStream.GetLengthOfContents(contentsElements);
	}

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

	public override void Encode(Asn1OutputStream asn1Out)
	{
		asn1Out.WriteIdentifier(Asn1Tags.Constructed | TagClass, TagNo);
		asn1Out.WriteDL(_contentsLength);
		asn1Out.EncodeContents(_contentsElements);
	}

	public override int GetLength() => Asn1OutputStream.GetLengthOfEncodingDL(TagNo, _contentsLength);
}
