// perticula - core - BerTaggedObjectParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerTaggedObjectParser.
///   Implements the <see cref="core.Protocol.asn1.IAsn1TaggedObjectParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1TaggedObjectParser" />
public class BerTaggedObjectParser : IAsn1TaggedObjectParser
{
	/// <summary>
	///   The ber tag class
	/// </summary>
	internal readonly int BerTagClass;

	/// <summary>
	///   The ber tag no
	/// </summary>
	internal readonly int BerTagNo;

	/// <summary>
	///   The parser
	/// </summary>
	internal readonly Asn1StreamParser Parser;

	/// <summary>
	///   Initializes a new instance of the <see cref="BerTaggedObjectParser" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="berTagNo">The ber tag no.</param>
	/// <param name="parser">The parser.</param>
	internal BerTaggedObjectParser(int tagClass, int berTagNo, Asn1StreamParser parser)
	{
		BerTagClass = tagClass;
		BerTagNo    = berTagNo;
		Parser      = parser;
	}

	/// <summary>
	///   Gets a value indicating whether this instance is constructed.
	/// </summary>
	/// <value><c>true</c> if this instance is constructed; otherwise, <c>false</c>.</value>
	public virtual bool IsConstructed => true;

	/// <summary>
	///   Gets the tag class.
	/// </summary>
	/// <value>The tag class.</value>
	public int TagClass => BerTagClass;

	/// <summary>
	///   Gets the tag no.
	/// </summary>
	/// <value>The tag no.</value>
	public int TagNo => BerTagNo;

	/// <summary>
	///   Determines whether [has context tag].
	/// </summary>
	/// <returns><c>true</c> if [has context tag]; otherwise, <c>false</c>.</returns>
	public bool HasContextTag() => BerTagClass == Asn1Tags.ContextSpecific;

	/// <summary>
	///   Determines whether [has context tag] [the specified tag no].
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <returns><c>true</c> if [has context tag] [the specified tag no]; otherwise, <c>false</c>.</returns>
	public bool HasContextTag(int tagNo) => BerTagClass == Asn1Tags.ContextSpecific && BerTagNo == tagNo;

	/// <summary>
	///   Determines whether the specified tag class has tag.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns><c>true</c> if the specified tag class has tag; otherwise, <c>false</c>.</returns>
	public bool HasTag(int tagClass, int tagNo) => BerTagClass == tagClass && BerTagNo == tagNo;

	/// <summary>
	///   Determines whether [has tag class] [the specified tag class].
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <returns><c>true</c> if [has tag class] [the specified tag class]; otherwise, <c>false</c>.</returns>
	public bool HasTagClass(int tagClass) => BerTagClass == tagClass;

	/// <summary>
	///   Parses the base universal.
	/// </summary>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>IAsn1Convertable.</returns>
	public virtual IAsn1Convertable ParseBaseUniversal(bool declaredExplicit, int baseTagNo) => declaredExplicit ? Parser.ParseObject(baseTagNo)! : Parser.ParseImplicitConstructedIndefiniteLength(baseTagNo);

	/// <summary>
	///   Needed for open types, until we have better type-guided parsing support.
	/// </summary>
	/// <returns>IAsn1Convertable.</returns>
	/// <remarks>
	///   Use sparingly for other purposes, and prefer <see cref="ParseExplicitBaseTagged" /> or
	///   <see cref="ParseBaseUniversal(bool, int)" /> where possible. Before using, check for matching tag
	///   <see cref="TagClass">class</see> and <see cref="TagNo">number</see>.
	/// </remarks>
	public virtual IAsn1Convertable ParseExplicitBaseObject() => Parser.ReadObject()!;

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="core.Protocol.asn1.Asn1ParseException"></exception>
	public virtual Asn1Object ToAsn1Object()
	{
		try
		{
			return Parser.LoadTaggedIndefiniteLength(TagClass, TagNo);
		}
		catch (IOException e)
		{
			throw new Asn1ParseException(e.Message);
		}
	}

	/// <summary>
	///   Parses the explicit base tagged.
	/// </summary>
	/// <returns>Asn1TaggedObjectParser.</returns>
	public virtual IAsn1TaggedObjectParser ParseExplicitBaseTagged() => Parser.ParseTaggedObject()!;

	/// <summary>
	///   Parses the implicit base tagged.
	/// </summary>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>Asn1TaggedObjectParser.</returns>
	public virtual IAsn1TaggedObjectParser ParseImplicitBaseTagged(int baseTagClass, int baseTagNo) => new BerTaggedObjectParser(baseTagClass, baseTagNo, Parser);
}
