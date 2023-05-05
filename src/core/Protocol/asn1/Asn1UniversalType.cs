// perticula - core - Asn1UniversalType.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1UniversalType.
///   Implements the <see cref="core.Protocol.asn1.Asn1Type" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Type" />
public class Asn1UniversalType : Asn1Type
{
	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1UniversalType" /> class.
	/// </summary>
	/// <param name="platformType">Type of the platform.</param>
	/// <param name="tagNo">The tag no.</param>
	internal Asn1UniversalType(Type platformType, int tagNo) : base(platformType) => Tag = Asn1Tag.Create(Asn1Tags.Universal, tagNo);

	/// <summary>
	///   Gets the tag.
	/// </summary>
	/// <value>The tag.</value>
	internal Asn1Tag Tag { get; }

	/// <summary>
	///   Checkeds the cast.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.InvalidOperationException">unexpected object: {asn1Object.GetTypeName()}</exception>
	internal Asn1Object CheckedCast(Asn1Object asn1Object)
	{
		if (PlatformType.IsInstanceOfType(asn1Object)) return asn1Object;

		throw new InvalidOperationException($"unexpected object: {asn1Object.GetTypeName()}");
	}

	/// <summary>
	///   Froms the implicit primitive.
	/// </summary>
	/// <param name="octetString">The octet string.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.InvalidOperationException">unexpected implicit primitive encoding</exception>
	internal virtual Asn1Object FromImplicitPrimitive(DerOctetString octetString) => throw new InvalidOperationException("unexpected implicit primitive encoding");

	/// <summary>
	///   Froms the implicit constructed.
	/// </summary>
	/// <param name="sequence">The sequence.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.InvalidOperationException">unexpected implicit constructed encoding</exception>
	internal virtual Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => throw new InvalidOperationException("unexpected implicit constructed encoding");

	/// <summary>
	///   Froms the byte array.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="IOException"></exception>
	internal Asn1Object FromByteArray(byte[] bytes) => CheckedCast(Asn1Object.FromByteArray(bytes)!);

	/// <summary>
	///   Gets the context instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.InvalidOperationException">this method only valid for CONTEXT_SPECIFIC tags</exception>
	internal Asn1Object GetContextInstance(Asn1TaggedObject taggedObject, bool declaredExplicit)
	{
		if (Asn1Tags.ContextSpecific != taggedObject.TagClass) throw new InvalidOperationException("this method only valid for CONTEXT_SPECIFIC tags");
		return CheckedCast(taggedObject.GetBaseUniversal(declaredExplicit, this));
	}
}
