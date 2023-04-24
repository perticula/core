// perticula - core - Asn1UniversalType.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

public class Asn1UniversalType : Asn1Type
{
	internal Asn1UniversalType(Type platformType, int tagNo) : base(platformType) => Tag = Asn1Tag.Create(Asn1Tags.Universal, tagNo);

	internal Asn1Tag Tag { get; }

	internal Asn1Object CheckedCast(Asn1Object asn1Object)
	{
		if (PlatformType.IsInstanceOfType(asn1Object)) return asn1Object;

		throw new InvalidOperationException($"unexpected object: {asn1Object.GetTypeName()}");
	}

	internal virtual Asn1Object FromImplicitPrimitive(DerOctetString octetString) => throw new InvalidOperationException("unexpected implicit primitive encoding");

	internal virtual Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => throw new InvalidOperationException("unexpected implicit constructed encoding");

	/// <exception cref="IOException" />
	internal Asn1Object FromByteArray(byte[] bytes) => CheckedCast(Asn1Object.FromByteArray(bytes));

	internal Asn1Object GetContextInstance(Asn1TaggedObject taggedObject, bool declaredExplicit)
	{
		if (Asn1Tags.ContextSpecific != taggedObject.TagClass) throw new InvalidOperationException("this method only valid for CONTEXT_SPECIFIC tags");
		return CheckedCast(taggedObject.GetBaseUniversal(declaredExplicit, this));
	}
}
