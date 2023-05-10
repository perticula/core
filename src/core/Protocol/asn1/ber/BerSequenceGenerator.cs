// perticula - core - BerSequenceGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerSequenceGenerator.
///   Implements the <see cref="core.Protocol.asn1.ber.BerGenerator" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.ber.BerGenerator" />
public class BerSequenceGenerator : BerGenerator
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequenceGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	public BerSequenceGenerator(Stream outStream) : base(outStream) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.Sequence);

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSequenceGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="isExplicit">The is explicit.</param>
	public BerSequenceGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.Sequence);
}
