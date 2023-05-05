// perticula - core - Asn1.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Extensions for the Asn1 family
/// </summary>
public static class Asn1
{
	/// <summary>
	///   Checks the tag class.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	/// <exception cref="InvalidOperationException">$"Expected {expected} tag but found {found}</exception>
	internal static Asn1TaggedObject CheckTagClass(this Asn1TaggedObject taggedObject, int tagClass)
	{
		if (taggedObject.HasTagClass(tagClass)) return taggedObject;
		var expected = GetTagClassText(tagClass);
		var found    = GetTagClassText(taggedObject);
		throw new InvalidOperationException($"Expected {expected} tag but found {found}");
	}

	/// <summary>
	///   Checks the tag class.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="InvalidOperationException">$"Expected {expected} tag but found {found}</exception>
	internal static IAsn1TaggedObjectParser CheckTagClass(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass)
	{
		if (taggedObjectParser.TagClass == tagClass) return taggedObjectParser;
		var expected = GetTagClassText(tagClass);
		var found    = GetTagClassText(taggedObjectParser);
		throw new InvalidOperationException($"Expected {expected} tag but found {found}");
	}

	/// <summary>
	///   Checks the tag.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	/// <exception cref="InvalidOperationException">$"Expected {expected} tag but found {found}</exception>
	internal static Asn1TaggedObject CheckTag(this Asn1TaggedObject taggedObject, int tagClass, int tagNo)
	{
		if (taggedObject.HasTag(tagClass, tagNo)) return taggedObject;
		var expected = GetTagText(tagClass, tagNo);
		var found    = GetTagText(taggedObject);
		throw new InvalidOperationException($"Expected {expected} tag but found {found}");
	}

	/// <summary>
	///   Checks the tag.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="InvalidOperationException">$"Expected {expected} tag but found {found}</exception>
	internal static IAsn1TaggedObjectParser CheckTag(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo)
	{
		if (taggedObjectParser.HasTag(tagClass, tagNo)) return taggedObjectParser;
		var expected = GetTagText(tagClass, tagNo);
		var found    = GetTagText(taggedObjectParser);
		throw new InvalidOperationException($"Expected {expected} tag but found {found}");
	}

	/// <summary>
	///   Gets the instance from choice.
	/// </summary>
	/// <typeparam name="TChoice">The type of the t choice.</typeparam>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="constructor">The constructor.</param>
	/// <returns>TChoice.</returns>
	/// <exception cref="ArgumentException">message, nameof(declaredExplicit)</exception>
	/// <exception cref="ArgumentNullException">nameof(taggedObject)</exception>
	internal static TChoice GetInstanceFromChoice<TChoice>(this Asn1TaggedObject taggedObject, bool declaredExplicit, Func<object, TChoice> constructor)
		where TChoice : Asn1Encodable, IAsn1Choice
	{
		if (!declaredExplicit)
		{
			var message = $"Implicit tagging cannot be used with untagged choice type {typeof(TChoice).GetTypeName()} (X.680 30.6, 30.8).";

			throw new ArgumentException(message, nameof(declaredExplicit));
		}

		if (taggedObject == null) throw new ArgumentNullException(nameof(taggedObject));

		return constructor(taggedObject.GetExplicitBaseObject());
	}

	/// <summary>
	///   Gets the tag class text.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <returns>string.</returns>
	public static string GetTagClassText(int tagClass)
		=> tagClass switch
		   {
			   Asn1Tags.Application     => "APPLICATION",
			   Asn1Tags.ContextSpecific => "CONTEXT",
			   Asn1Tags.Private         => "PRIVATE",
			   _                        => "UNIVERSAL"
		   };

	/// <summary>
	///   Gets the tag class text.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <returns>string.</returns>
	public static string GetTagClassText(this Asn1TaggedObject taggedObject) => GetTagClassText(taggedObject.TagClass);

	/// <summary>
	///   Gets the tag class text.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <returns>string.</returns>
	public static string GetTagClassText(this IAsn1TaggedObjectParser taggedObjectParser) => GetTagClassText(taggedObjectParser.TagClass);

	/// <summary>
	///   Gets the tag text.
	/// </summary>
	/// <param name="tag">The tag.</param>
	/// <returns>string.</returns>
	internal static string GetTagText(this Asn1Tag tag) => GetTagText(tag.TagClass, tag.TagNo);

	/// <summary>
	///   Gets the tag text.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <returns>string.</returns>
	public static string GetTagText(this Asn1TaggedObject taggedObject) => GetTagText(taggedObject.TagClass, taggedObject.TagNo);

	/// <summary>
	///   Gets the tag text.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <returns>string.</returns>
	public static string GetTagText(this IAsn1TaggedObjectParser taggedObjectParser) => GetTagText(taggedObjectParser.TagClass, taggedObjectParser.TagNo);

	/// <summary>
	///   Gets the tag text.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>string.</returns>
	public static string GetTagText(int tagClass, int tagNo)
		=> tagClass switch
		   {
			   Asn1Tags.Application     => $"[APPLICATION {tagNo}]",
			   Asn1Tags.ContextSpecific => $"[CONTEXT {tagNo}]",
			   Asn1Tags.Private         => $"[PRIVATE {tagNo}]",
			   _                        => $"[UNIVERSAL {tagNo}]"
		   };


	/// <summary>
	///   Gets the explicit base object.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1Encodable.</returns>
	public static Asn1Encodable GetExplicitBaseObject(this Asn1TaggedObject taggedObject, int tagClass, int tagNo) => CheckTag(taggedObject, tagClass, tagNo).GetExplicitBaseObject();

	/// <summary>
	///   Gets the explicit context base object.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1Encodable.</returns>
	public static Asn1Encodable GetExplicitContextBaseObject(this Asn1TaggedObject taggedObject, int tagNo) => GetExplicitBaseObject(taggedObject, Asn1Tags.ContextSpecific, tagNo);

	/// <summary>
	///   Tries the get explicit base object.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseObject">The base object.</param>
	/// <returns>bool.</returns>
	public static bool TryGetExplicitBaseObject(this Asn1TaggedObject taggedObject, int tagClass, int tagNo, out Asn1Encodable? baseObject)
	{
		var result = taggedObject.HasTag(tagClass, tagNo);
		baseObject = result ? taggedObject.GetExplicitBaseObject() : null;
		return result;
	}

	/// <summary>
	///   Tries the get explicit context base object.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseObject">The base object.</param>
	/// <returns>bool.</returns>
	public static bool TryGetExplicitContextBaseObject(this Asn1TaggedObject taggedObject, int tagNo, out Asn1Encodable? baseObject) => TryGetExplicitBaseObject(taggedObject, Asn1Tags.ContextSpecific, tagNo, out baseObject);


	/// <summary>
	///   Gets the explicit base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetExplicitBaseTagged(this Asn1TaggedObject taggedObject, int tagClass) => CheckTagClass(taggedObject, tagClass).GetExplicitBaseTagged();

	/// <summary>
	///   Gets the explicit base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetExplicitBaseTagged(this Asn1TaggedObject taggedObject, int tagClass, int tagNo) => CheckTag(taggedObject, tagClass, tagNo).GetExplicitBaseTagged();

	/// <summary>
	///   Gets the explicit context base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetExplicitContextBaseTagged(this Asn1TaggedObject taggedObject) => GetExplicitBaseTagged(taggedObject, Asn1Tags.ContextSpecific);

	/// <summary>
	///   Gets the explicit context base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetExplicitContextBaseTagged(this Asn1TaggedObject taggedObject, int tagNo) => GetExplicitBaseTagged(taggedObject, Asn1Tags.ContextSpecific, tagNo);


	/// <summary>
	///   Tries the get explicit base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	public static bool TryGetExplicitBaseTagged(this Asn1TaggedObject taggedObject, int tagClass, out Asn1TaggedObject? baseTagged)
	{
		var result = taggedObject.HasTagClass(tagClass);
		baseTagged = result ? taggedObject.GetExplicitBaseTagged() : null;
		return result;
	}

	/// <summary>
	///   Tries the get explicit base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	public static bool TryGetExplicitBaseTagged(this Asn1TaggedObject taggedObject, int tagClass, int tagNo, out Asn1TaggedObject? baseTagged)
	{
		var result = taggedObject.HasTag(tagClass, tagNo);
		baseTagged = result ? taggedObject.GetExplicitBaseTagged() : null;
		return result;
	}

	/// <summary>
	///   Tries the get explicit context base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	public static bool TryGetExplicitContextBaseTagged(this Asn1TaggedObject taggedObject, out Asn1TaggedObject? baseTagged) => TryGetExplicitBaseTagged(taggedObject, Asn1Tags.ContextSpecific, out baseTagged);

	/// <summary>
	///   Tries the get explicit context base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	public static bool TryGetExplicitContextBaseTagged(this Asn1TaggedObject taggedObject, int tagNo, out Asn1TaggedObject? baseTagged) => TryGetExplicitBaseTagged(taggedObject, Asn1Tags.ContextSpecific, tagNo, out baseTagged);

	/// <summary>
	///   Gets the implicit base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetImplicitBaseTagged(this Asn1TaggedObject taggedObject, int tagClass, int tagNo, int baseTagClass, int baseTagNo) => CheckTag(taggedObject, tagClass, tagNo).GetImplicitBaseTagged(baseTagClass, baseTagNo);

	/// <summary>
	///   Gets the implicit context base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetImplicitContextBaseTagged(this Asn1TaggedObject taggedObject, int tagNo, int baseTagClass, int baseTagNo) => GetImplicitBaseTagged(taggedObject, Asn1Tags.ContextSpecific, tagNo, baseTagClass, baseTagNo);


	/// <summary>
	///   Tries the get implicit base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	public static bool TryGetImplicitBaseTagged(this Asn1TaggedObject taggedObject, int tagClass, int tagNo, int baseTagClass, int baseTagNo, out Asn1TaggedObject? baseTagged)
	{
		var result = taggedObject.HasTag(tagClass, tagNo);
		baseTagged = result ? taggedObject.GetImplicitBaseTagged(baseTagClass, baseTagNo) : null;
		return result;
	}

	/// <summary>
	///   Tries the get implicit context base tagged.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	public static bool TryGetImplicitContextBaseTagged(this Asn1TaggedObject taggedObject, int tagNo, int baseTagClass, int baseTagNo, out Asn1TaggedObject? baseTagged) => TryGetImplicitBaseTagged(taggedObject, Asn1Tags.ContextSpecific, tagNo, baseTagClass, baseTagNo, out baseTagged);

	/// <summary>
	///   Gets the base universal.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1Object.</returns>
	public static Asn1Object GetBaseUniversal(this Asn1TaggedObject taggedObject, int tagClass, int tagNo, bool declaredExplicit, int baseTagNo) => CheckTag(taggedObject, tagClass, tagNo).GetBaseUniversal(declaredExplicit, baseTagNo);

	/// <summary>
	///   Gets the context base universal.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.Asn1Object.</returns>
	public static Asn1Object GetContextBaseUniversal(this Asn1TaggedObject taggedObject, int tagNo, bool declaredExplicit, int baseTagNo) => GetBaseUniversal(taggedObject, Asn1Tags.ContextSpecific, tagNo, declaredExplicit, baseTagNo);


	/// <summary>
	///   Tries the get base universal.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseUniversal">The base universal.</param>
	/// <returns>bool.</returns>
	public static bool TryGetBaseUniversal(this Asn1TaggedObject taggedObject, int tagClass, int tagNo, bool declaredExplicit, int baseTagNo, out Asn1Object? baseUniversal)
	{
		var result = taggedObject.HasTag(tagClass, tagNo);
		baseUniversal = result ? taggedObject.GetBaseUniversal(declaredExplicit, baseTagNo) : null;
		return result;
	}

	/// <summary>
	///   Tries the get context base universal.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseUniversal">The base universal.</param>
	/// <returns>bool.</returns>
	public static bool TryGetContextBaseUniversal(this Asn1TaggedObject taggedObject, int tagNo, bool declaredExplicit, int baseTagNo, out Asn1Object? baseUniversal) => TryGetBaseUniversal(taggedObject, Asn1Tags.ContextSpecific, tagNo, declaredExplicit, baseTagNo, out baseUniversal);

	/// <summary>
	///   Parses the explicit base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1TaggedObjectParser ParseExplicitBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass) => CheckTagClass(taggedObjectParser, tagClass).ParseExplicitBaseTagged();

	/// <summary>
	///   Parses the explicit base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1TaggedObjectParser ParseExplicitBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo) => CheckTag(taggedObjectParser, tagClass, tagNo).ParseExplicitBaseTagged();

	/// <summary>
	///   Parses the explicit context base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1TaggedObjectParser ParseExplicitContextBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser) => ParseExplicitBaseTagged(taggedObjectParser, Asn1Tags.ContextSpecific);

	/// <summary>
	///   Parses the explicit context base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1TaggedObjectParser ParseExplicitContextBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo) => ParseExplicitBaseTagged(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo);

	/// <summary>
	///   Tries the parse explicit base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseExplicitBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, out IAsn1TaggedObjectParser? baseTagged)
	{
		var result = taggedObjectParser.TagClass == tagClass;
		baseTagged = result ? taggedObjectParser.ParseExplicitBaseTagged() : null;
		return result;
	}

	/// <summary>
	///   Tries the parse explicit base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseExplicitBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo, out IAsn1TaggedObjectParser? baseTagged)
	{
		var result = taggedObjectParser.HasTag(tagClass, tagNo);
		baseTagged = result ? taggedObjectParser.ParseExplicitBaseTagged() : null;
		return result;
	}

	/// <summary>
	///   Tries the parse explicit context base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseExplicitContextBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, out IAsn1TaggedObjectParser? baseTagged) => TryParseExplicitBaseTagged(taggedObjectParser, Asn1Tags.ContextSpecific, out baseTagged);

	/// <summary>
	///   Tries the parse explicit context base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseExplicitContextBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo, out IAsn1TaggedObjectParser? baseTagged) => TryParseExplicitBaseTagged(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo, out baseTagged);

	/// <summary>
	///   Parses the implicit base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1TaggedObjectParser ParseImplicitBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo, int baseTagClass, int baseTagNo) => CheckTag(taggedObjectParser, tagClass, tagNo).ParseImplicitBaseTagged(baseTagClass, baseTagNo);

	/// <summary>
	///   Parses the implicit context base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1TaggedObjectParser.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1TaggedObjectParser ParseImplicitContextBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo, int baseTagClass, int baseTagNo) => ParseImplicitBaseTagged(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo, baseTagClass, baseTagNo);

	/// <summary>
	///   Tries the parse implicit base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseImplicitBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo, int baseTagClass, int baseTagNo, out IAsn1TaggedObjectParser? baseTagged)
	{
		var result = taggedObjectParser.HasTag(tagClass, tagNo);
		baseTagged = result ? taggedObjectParser.ParseImplicitBaseTagged(baseTagClass, baseTagNo) : null;
		return result;
	}

	/// <summary>
	///   Tries the parse implicit context base tagged.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseTagged">The base tagged.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseImplicitContextBaseTagged(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo, int baseTagClass, int baseTagNo, out IAsn1TaggedObjectParser? baseTagged) => TryParseImplicitBaseTagged(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo, baseTagClass, baseTagNo, out baseTagged);

	/// <summary>
	///   Parses the base universal.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1Convertable ParseBaseUniversal(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo, bool declaredExplicit, int baseTagNo) => CheckTag(taggedObjectParser, tagClass, tagNo).ParseBaseUniversal(declaredExplicit, baseTagNo);

	/// <summary>
	///   Parses the context base universal.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1Convertable ParseContextBaseUniversal(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo, bool declaredExplicit, int baseTagNo) => ParseBaseUniversal(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo, declaredExplicit, baseTagNo);

	/// <summary>
	///   Tries the parse base universal.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseUniversal">The base universal.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseBaseUniversal(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo, bool declaredExplicit, int baseTagNo, out IAsn1Convertable? baseUniversal)
	{
		var result = taggedObjectParser.HasTag(tagClass, tagNo);
		baseUniversal = result ? taggedObjectParser.ParseBaseUniversal(declaredExplicit, baseTagNo) : null;
		return result;
	}

	/// <summary>
	///   Tries the parse context base universal.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">The declared explicit.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <param name="baseUniversal">The base universal.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseContextBaseUniversal(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo, bool declaredExplicit, int baseTagNo, out IAsn1Convertable? baseUniversal) => TryParseBaseUniversal(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo, declaredExplicit, baseTagNo, out baseUniversal);

	/// <summary>
	///   Parses the explicit base object.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1Convertable ParseExplicitBaseObject(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo) => CheckTag(taggedObjectParser, tagClass, tagNo).ParseExplicitBaseObject();

	/// <summary>
	///   Parses the explicit context base object.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Convertable.</returns>
	/// <exception cref="IOException"></exception>
	public static IAsn1Convertable ParseExplicitContextBaseObject(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo) => ParseExplicitBaseObject(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo);

	/// <summary>
	///   Tries the parse explicit base object.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseObject">The base object.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseExplicitBaseObject(this IAsn1TaggedObjectParser taggedObjectParser, int tagClass, int tagNo, out IAsn1Convertable? baseObject)
	{
		var result = taggedObjectParser.HasTag(tagClass, tagNo);
		baseObject = result ? taggedObjectParser.ParseExplicitBaseObject() : null;
		return result;
	}

	/// <summary>
	///   Tries the parse explicit context base object.
	/// </summary>
	/// <param name="taggedObjectParser">The tagged object parser.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="baseObject">The base object.</param>
	/// <returns>bool.</returns>
	/// <exception cref="IOException"></exception>
	public static bool TryParseExplicitContextBaseObject(this IAsn1TaggedObjectParser taggedObjectParser, int tagNo, out IAsn1Convertable? baseObject) => TryParseExplicitBaseObject(taggedObjectParser, Asn1Tags.ContextSpecific, tagNo, out baseObject);
}
