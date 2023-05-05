// perticula - core - IAsn1TaggedObjectParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Interface IAsn1TaggedObjectParser
///   Implements the <see cref="core.Protocol.asn1.IAsn1Convertable" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1Convertable" />
public interface IAsn1TaggedObjectParser : IAsn1Convertable
{
	/// <summary>
	///   Gets the tag class.
	/// </summary>
	/// <value>The tag class.</value>
	int TagClass { get; }

	/// <summary>
	///   Gets the tag no.
	/// </summary>
	/// <value>The tag no.</value>
	int TagNo { get; }

	/// <summary>
	///   Determines whether [has context tag].
	/// </summary>
	/// <returns><c>true</c> if [has context tag]; otherwise, <c>false</c>.</returns>
	bool HasContextTag();

	/// <summary>
	///   Determines whether [has context tag] [the specified tag no].
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <returns><c>true</c> if [has context tag] [the specified tag no]; otherwise, <c>false</c>.</returns>
	bool HasContextTag(int tagNo);

	/// <summary>
	///   Determines whether the specified tag class has tag.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns><c>true</c> if the specified tag class has tag; otherwise, <c>false</c>.</returns>
	bool HasTag(int tagClass, int tagNo);

	/// <summary>
	///   Determines whether [has tag class] [the specified tag class].
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <returns><c>true</c> if [has tag class] [the specified tag class]; otherwise, <c>false</c>.</returns>
	bool HasTagClass(int tagClass);

	/// <summary>
	///   Parses the base universal.
	/// </summary>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>IAsn1Convertable.</returns>
	/// <exception cref="IOException"></exception>
	IAsn1Convertable ParseBaseUniversal(bool declaredExplicit, int baseTagNo);

	/// <summary>
	///   Needed for open types, until we have better type-guided parsing support.
	/// </summary>
	/// <returns>IAsn1Convertable.</returns>
	/// <exception cref="IOException"></exception>
	/// <remarks>
	///   Use sparingly for other purposes, and prefer <see cref="ParseExplicitBaseTagged" /> or
	///   <see cref="ParseBaseUniversal(bool, int)" /> where possible. Before using, check for matching tag
	///   <see cref="TagClass">class</see> and <see cref="TagNo">number</see>.
	/// </remarks>
	IAsn1Convertable ParseExplicitBaseObject();

	/// <summary>
	///   Parses the explicit base tagged.
	/// </summary>
	/// <returns>Asn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	IAsn1TaggedObjectParser ParseExplicitBaseTagged();

	/// <summary>
	///   Parses the implicit base tagged.
	/// </summary>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>Asn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	IAsn1TaggedObjectParser ParseImplicitBaseTagged(int baseTagClass, int baseTagNo);
}
