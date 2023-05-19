// perticula - core - DefiniteLengthTaggedObjectParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.ber;

namespace core.Protocol.asn1.dl;

/// <summary>
///   Class DefiniteLengthTaggedObjectParser.
///   Implements the <see cref="BerTaggedObjectParser" />
/// </summary>
/// <seealso cref="BerTaggedObjectParser" />
public class DefiniteLengthTaggedObjectParser : BerTaggedObjectParser
{
	/// <summary>
	///   The constructed
	/// </summary>
	private readonly bool _constructed;

	/// <summary>
	///   Initializes a new instance of the <see cref="DefiniteLengthTaggedObjectParser" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="constructed">if set to <c>true</c> [constructed].</param>
	/// <param name="parser">The parser.</param>
	internal DefiniteLengthTaggedObjectParser(int tagClass, int tagNo, bool constructed, Asn1StreamParser parser) : base(tagClass, tagNo, parser) => _constructed = constructed;

	/// <summary>
	///   Gets a value indicating whether this instance is constructed.
	/// </summary>
	/// <value><c>true</c> if this instance is constructed; otherwise, <c>false</c>.</value>
	public override bool IsConstructed => _constructed;

	/// <summary>
	///   Parses the base universal.
	/// </summary>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>IAsn1Convertable.</returns>
	public override IAsn1Convertable ParseBaseUniversal(bool declaredExplicit, int baseTagNo)
		=> declaredExplicit switch
		   {
			   true when !_constructed => throw new IOException("Explicit tags must be constructed (see X.690 8.14.2)"),
			   true                    => Parser.ParseObject(baseTagNo)!,
			   _                       => _constructed ? Parser.ParseImplicitConstructedDefiniteLength(baseTagNo) : Parser.ParseImplicitPrimitive(baseTagNo)
		   };

	/// <summary>
	///   Needed for open types, until we have better type-guided parsing support.
	/// </summary>
	/// <returns>IAsn1Convertable.</returns>
	/// <exception cref="System.IO.IOException">Explicit tags must be constructed (see X.690 8.14.2)</exception>
	/// <remarks>
	///   Use sparingly for other purposes, and prefer <see cref="ParseExplicitBaseTagged" /> or
	///   <see cref="ParseBaseUniversal(bool, int)" /> where possible. Before using, check for matching tag
	///   <see cref="TagClass">class</see> and <see cref="TagNo">number</see>.
	/// </remarks>
	public override IAsn1Convertable ParseExplicitBaseObject() => !_constructed ? throw new IOException("Explicit tags must be constructed (see X.690 8.14.2)") : Parser.ReadObject()!;

	/// <summary>
	///   Parses the explicit base tagged.
	/// </summary>
	/// <returns>Asn1TaggedObjectParser.</returns>
	/// <exception cref="System.IO.IOException">Explicit tags must be constructed (see X.690 8.14.2)</exception>
	public override IAsn1TaggedObjectParser ParseExplicitBaseTagged() => !_constructed ? throw new IOException("Explicit tags must be constructed (see X.690 8.14.2)") : Parser.ParseTaggedObject()!;

	/// <summary>
	///   Parses the implicit base tagged.
	/// </summary>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>Asn1TaggedObjectParser.</returns>
	public override IAsn1TaggedObjectParser ParseImplicitBaseTagged(int baseTagClass, int baseTagNo) => new DefiniteLengthTaggedObjectParser(baseTagClass, baseTagNo, _constructed, Parser);

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="core.Protocol.asn1.Asn1ParseException"></exception>
	public override Asn1Object ToAsn1Object()
	{
		try
		{
			return Parser.LoadTaggedDefiniteLength(TagClass, TagNo, _constructed);
		}
		catch (IOException e)
		{
			throw new Asn1ParseException(e.Message);
		}
	}
}
