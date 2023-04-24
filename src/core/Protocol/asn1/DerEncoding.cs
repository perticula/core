// perticula - core - DerEncoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;

namespace core.Protocol.asn1;

public abstract class DerEncoding : IAsn1Encoding, IComparable<DerEncoding>
{
	protected internal readonly int TagClass;
	protected internal readonly int TagNo;

	protected internal DerEncoding(int tagClass, int tagNo)
	{
		Debug.Assert((tagClass & Asn1Tags.Private) == tagClass);
		Debug.Assert(tagNo                         >= 0);
		TagClass = tagClass;
		TagNo    = tagNo;
	}

	public abstract void Encode(Asn1OutputStream buffer);

	public abstract int GetLength();

	public int CompareTo(DerEncoding? other)
	{
		Debug.Assert(other != null);

		if (TagClass != other.TagClass) return TagClass - other.TagClass;

		if (TagNo != other.TagNo) return TagNo - other.TagNo;

		return CompareLengthAndContents(other);
	}

	protected internal abstract int CompareLengthAndContents(DerEncoding other);
}
