// perticula - core - DerEncoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerEncoding.
///   Implements the <see cref="IAsn1Encoding" />
///   Implements the <see cref="IComparable{DerEncoding}" />
/// </summary>
/// <seealso cref="IAsn1Encoding" />
/// <seealso cref="IComparable{DerEncoding}" />
public class DerEncoding : Asn1Encoding, IComparable<DerEncoding>
{
	/// <summary>
	///   The contents octets
	/// </summary>
	protected internal readonly byte[] ContentsOctets;

	/// <summary>
	///   The tag class
	/// </summary>
	protected readonly int TagClass;

	/// <summary>
	///   The tag no
	/// </summary>
	protected readonly int TagNo;

	/// <summary>
	///   The contents octets length
	/// </summary>
	protected int ContentsOctetsLength;

	public DerEncoding(int tagClass, int tagNo) : this(tagClass, tagNo, null) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerEncoding" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsOctets">The contents octets.</param>
	public DerEncoding(int tagClass, int tagNo, byte[]? contentsOctets) : base(tagClass, tagNo, contentsOctets!)
	{
		Debug.Assert((tagClass & Asn1Tags.Private) == tagClass);
		Debug.Assert(tagNo                         >= 0);

		ContentsOctets = contentsOctets ?? Array.Empty<byte>();
		TagClass       = tagClass;
		TagNo          = tagNo;
	}

	/// <summary>
	///   Compares the current instance with another object of the same type and returns an integer that indicates whether the
	///   current instance precedes, follows, or occurs in the same position in the sort order as the other object.
	/// </summary>
	/// <param name="other">An object to compare with this instance.</param>
	/// <returns>
	///   A value that indicates the relative order of the objects being compared. The return value has these meanings:
	///   <list type="table">
	///     <listheader>
	///       <term> Value</term><description> Meaning</description>
	///     </listheader>
	///     <item>
	///       <term> Less than zero</term>
	///       <description> This instance precedes <paramref name="other" /> in the sort order.</description>
	///     </item>
	///     <item>
	///       <term> Zero</term>
	///       <description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description>
	///     </item>
	///     <item>
	///       <term> Greater than zero</term>
	///       <description> This instance follows <paramref name="other" /> in the sort order.</description>
	///     </item>
	///   </list>
	/// </returns>
	public int CompareTo(DerEncoding? other)
	{
		Debug.Assert(other != null);

		if (TagClass != other.TagClass) return TagClass - other.TagClass;

		if (TagNo != other.TagNo) return TagNo - other.TagNo;

		return CompareLengthAndContents(other);
	}

	/// <summary>
	///   Encodes the specified buffer.
	/// </summary>
	/// <param name="buffer">The buffer.</param>
	public override void Encode(Asn1OutputStream buffer)
	{
		buffer.WriteIdentifier(TagClass, TagNo);
		buffer.WriteDL(ContentsOctets?.Length ?? 0);
		buffer.Write(ContentsOctets           ?? Array.Empty<byte>(), 0, ContentsOctets?.Length ?? 0);
	}

	/// <summary>
	///   Gets the length of the value to encode.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public override int GetLength() => Asn1OutputStream.GetLengthOfEncodingDL(TagNo, ContentsOctets?.Length ?? 0);

	/// <summary>
	///   Compares the length and contents.
	/// </summary>
	/// <param name="other">The other.</param>
	/// <returns>System.Int32.</returns>
	/// <exception cref="InvalidOperationException"></exception>
	protected virtual int CompareLengthAndContents(DerEncoding other)
	{
		switch (other)
		{
			case DerEncodingSuffixed suffixed: return -suffixed.CompareLengthAndContents(this);
			case null:                         throw new InvalidOperationException();
		}

		ContentsOctetsLength = ContentsOctets?.Length                                                                        ?? 0;
		if (ContentsOctetsLength != other.ContentsOctets?.Length) return ContentsOctetsLength - other.ContentsOctets?.Length ?? 0;

		return ContentsOctets.AsSpan(0, ContentsOctetsLength).SequenceCompareTo(other.ContentsOctets.AsSpan(0, ContentsOctetsLength));
	}
}
