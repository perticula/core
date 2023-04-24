// perticula - core - Asn1Tag.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

public sealed class Asn1Tag
{
	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Tag" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	private Asn1Tag(int tagClass, int tagNo)
	{
		TagClass = tagClass;
		TagNo    = tagNo;
	}

	/// <summary>
	///   Prevents a default instance of the <see cref="Asn1Tag" /> class from being created.
	/// </summary>
	private Asn1Tag() { }

	public int TagClass { get; }
	public int TagNo    { get; }

	/// <summary>
	///   Creates the specified tag class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>Asn1Tag.</returns>
	public static Asn1Tag Create(int tagClass, int tagNo) => new(tagClass, tagNo);
}
