// perticula - core - IAsn1TaggedObjectParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

public interface IAsn1TaggedObjectParser : IAsn1Convertable
{
	int TagClass { get; }
	int TagNo    { get; }

	bool HasContextTag();
	bool HasContextTag(int tagNo);

	bool HasTag(int      tagClass, int tagNo);
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
