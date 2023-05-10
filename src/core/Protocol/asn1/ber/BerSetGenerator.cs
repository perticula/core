// perticula - core - BetSetGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerSetGenerator.
///   Implements the <see cref="core.Protocol.asn1.ber.BerGenerator" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.ber.BerGenerator" />
public class BerSetGenerator : BerGenerator
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BerSetGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	public BerSetGenerator(Stream outStream) : base(outStream) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.Set);

	/// <summary>
	///   Initializes a new instance of the <see cref="BerSetGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="isExplicit">The is explicit.</param>
	public BerSetGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.Set);
}
