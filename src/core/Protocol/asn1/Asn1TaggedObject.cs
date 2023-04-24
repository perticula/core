// perticula - core - Asn1TaggedObject.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

public abstract class Asn1TaggedObject : Asn1Object, IAsn1TaggedObjectParser
{
	private const     int           DeclaredExplicit = 1;
	private const     int           DeclaredImplicit = 2;
	private const     int           ParsedExplicit   = 3;
	private const     int           ParsedImplicit   = 4;
	internal readonly Asn1Encodable BaseTaggedObject;

	internal readonly int Explicitness;

	protected Asn1TaggedObject(bool isExplicit, int tagNo,    Asn1Encodable obj) : this(isExplicit, Asn1Tags.ContextSpecific, tagNo, obj) { }
	protected Asn1TaggedObject(bool isExplicit, int tagClass, int           tagNo, Asn1Encodable obj) : this(isExplicit ? DeclaredExplicit : DeclaredImplicit, tagClass, tagNo, obj) { }

	internal Asn1TaggedObject(int explicitness, int tagClass, int tagNo, Asn1Encodable obj)
	{
		if (null == obj)
			throw new ArgumentNullException(nameof(obj));
		if (Asn1Tags.Universal == tagClass || (tagClass & Asn1Tags.Private) != tagClass)
			throw new ArgumentException("invalid tag class: " + tagClass, nameof(tagClass));

		Explicitness     = obj is IAsn1Choice ? DeclaredExplicit : explicitness;
		TagClass         = tagClass;
		TagNo            = tagNo;
		BaseTaggedObject = obj;
	}

	internal abstract string Asn1Encoding { get; }

	public int TagClass { get; }
	public int TagNo    { get; }

	public bool HasContextTag()          => TagClass == Asn1Tags.ContextSpecific;
	public bool HasContextTag(int tagNo) => TagClass == Asn1Tags.ContextSpecific && TagNo == tagNo;

	public bool HasTag(int      tagClass, int tagNo) => TagClass == tagClass && TagNo == tagNo;
	public bool HasTagClass(int tagClass) => TagClass == tagClass;

	public IAsn1Convertable ParseExplicitBaseObject() => GetExplicitBaseObject();

	public IAsn1Convertable ParseBaseUniversal(bool declaredExplicit, int baseTagNo)
	{
		var asn1Object = GetBaseUniversal(declaredExplicit, baseTagNo);

		return baseTagNo switch
		       {
			       Asn1Tags.BitString   => ((DerBitString) asn1Object).Parser,
			       Asn1Tags.OctetString => ((Asn1OctetString) asn1Object).Parser,
			       Asn1Tags.Sequence    => ((Asn1Sequence) asn1Object).Parser,
			       Asn1Tags.Set         => ((Asn1Set) asn1Object).Parser,
			       _                    => asn1Object
		       };
	}

	public static Asn1TaggedObject GetInstance(object obj)
	{
		switch (obj)
		{
			case Asn1TaggedObject asn1TaggedObject:
				return asn1TaggedObject;
			case IAsn1Convertable asn1Convertible:
			{
				var asn1Object = asn1Convertible.ToAsn1Object();
				if (asn1Object is Asn1TaggedObject converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return CheckedCast(FromByteArray(bytes));
				}
				catch (IOException e)
				{
					throw new ArgumentException("failed to construct tagged object from byte[]", nameof(obj), e);
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	public static Asn1TaggedObject GetInstance(object obj, int tagClass)            => Asn1Utilities.CheckTagClass(CheckInstance(obj), tagClass);
	public static Asn1TaggedObject GetInstance(object obj, int tagClass, int tagNo) => Asn1Utilities.CheckTag(CheckInstance(obj), tagClass, tagNo);

	public static Asn1TaggedObject GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit)                => Asn1Utilities.GetExplicitContextBaseTagged(CheckInstance(taggedObject, declaredExplicit));
	public static Asn1TaggedObject GetInstance(Asn1TaggedObject taggedObject, int  tagClass, bool declaredExplicit) => Asn1Utilities.GetExplicitBaseTagged(CheckInstance(taggedObject,        declaredExplicit), tagClass);

	public static Asn1TaggedObject GetInstance(Asn1TaggedObject taggedObject, int tagClass, int tagNo, bool declaredExplicit) => Asn1Utilities.GetExplicitBaseTagged(CheckInstance(taggedObject, declaredExplicit), tagClass, tagNo);

	private static Asn1TaggedObject CheckInstance(object obj) => GetInstance(obj ?? throw new ArgumentNullException(nameof(obj)));

	private static Asn1TaggedObject CheckInstance(Asn1TaggedObject taggedObject, bool declaredExplicit)
	{
		if (!declaredExplicit)
			throw new ArgumentException("this method not valid for implicitly tagged tagged objects");

		return taggedObject ?? throw new ArgumentNullException(nameof(taggedObject));
	}

	public bool IsExplicit() => Explicitness switch
	                            {
		                            DeclaredExplicit => true,
		                            ParsedExplicit   => true,
		                            _                => false
	                            };

	public bool IsParsed() => Explicitness switch
	                            {
		                            ParsedExplicit => true,
		                            ParsedImplicit => true,
		                            _              => false
	                            };

	protected override bool Asn1Equals(Asn1Object asn1Object)
	{
		if (asn1Object is not Asn1TaggedObject that || TagNo != that.TagNo || TagClass != that.TagClass)
			return false;

		if (Explicitness != that.Explicitness)
			if (IsExplicit() != that.IsExplicit())
				return false;

		var p1 = BaseTaggedObject.ToAsn1Object();
		var p2 = that.BaseTaggedObject.ToAsn1Object();

		if (p1.Equals(p2))
			return true;

		if (!IsExplicit())
			try
			{
				var d1 = GetEncoded();
				var d2 = that.GetEncoded();

				return Arrays.AreEqual(d1, d2);
			}
			catch (IOException)
			{
				return false;
			}

		return p1.CallAsn1Equals(p2);
	}

	public Asn1Object GetObject()
	{
		Asn1Utilities.CheckTagClass(this, Asn1Tags.ContextSpecific);

		return BaseTaggedObject.ToAsn1Object();
	}

	public Asn1Encodable GetBaseObject() => BaseTaggedObject;

	public Asn1Encodable GetExplicitBaseObject()
	{
		if (!IsExplicit())
			throw new InvalidOperationException("object implicit - explicit expected.");

		return BaseTaggedObject;
	}

	public Asn1TaggedObject GetExplicitBaseTagged()
	{
		if (!IsExplicit())
			throw new InvalidOperationException("object implicit - explicit expected.");

		return CheckedCast(BaseTaggedObject.ToAsn1Object());
	}

	public IAsn1TaggedObjectParser ParseExplicitBaseTagged() => GetExplicitBaseTagged();

	public IAsn1TaggedObjectParser ParseImplicitBaseTagged(int baseTagClass, int baseTagNo) => GetImplicitBaseTagged(baseTagClass, baseTagNo);

	public override string ToString() => Asn1Utilities.GetTagText(TagClass, TagNo) + BaseTaggedObject;

	internal abstract Asn1Sequence RebuildConstructed(Asn1Object asn1Object);

	internal abstract Asn1TaggedObject ReplaceTag(int tagClass, int tagNo);

	internal static Asn1Object CreateConstructedDL(int tagClass, int tagNo, Asn1EncodableVector contentsElements)
	{
		var maybeExplicit = contentsElements.Count == 1;

		return maybeExplicit
			       ? new DLTaggedObject(ParsedExplicit, tagClass, tagNo, contentsElements[0])
			       : new DLTaggedObject(ParsedImplicit, tagClass, tagNo, DLSequence.FromVector(contentsElements));
	}

	internal static Asn1Object CreateConstructedIL(int tagClass, int tagNo, Asn1EncodableVector contentsElements)
	{
		var maybeExplicit = contentsElements.Count == 1;

		return maybeExplicit
			       ? new BerTaggedObject(ParsedExplicit, tagClass, tagNo, contentsElements[0])
			       : new BerTaggedObject(ParsedImplicit, tagClass, tagNo, BerSequence.FromVector(contentsElements));
	}

	internal static Asn1Object CreatePrimitive(int tagClass, int tagNo, byte[] contentsOctets) => new DLTaggedObject(ParsedImplicit, tagClass, tagNo, new DerOctetString(contentsOctets));

	private static Asn1TaggedObject CheckedCast(Asn1Object asn1Object)
	{
		if (asn1Object is Asn1TaggedObject taggedObject) return taggedObject;

		throw new InvalidOperationException($"unexpected object: {asn1Object.GetTypeName()}");
	}

	public Asn1TaggedObject GetImplicitBaseTagged(int baseTagClass, int baseTagNo)
	{
		if (Asn1Tags.Universal == baseTagClass || (baseTagClass & Asn1Tags.Private) != baseTagClass)
			throw new ArgumentException($"invalid base tag class: {baseTagClass}", nameof(baseTagClass));

		switch (Explicitness)
		{
			case DeclaredExplicit:
				throw new InvalidOperationException("object explicit - implicit expected.");

			case DeclaredImplicit:
			{
				var declared = CheckedCast(BaseTaggedObject.ToAsn1Object());
				return Asn1Utilities.CheckTag(declared, baseTagClass, baseTagNo);
			}

			// Parsed; return a virtual tag (i.e. that couldn't have been present in the encoding)
			default:
				return ReplaceTag(baseTagClass, baseTagNo);
		}
	}

	public Asn1Object GetBaseUniversal(bool declaredExplicit, int tagNo)
	{
		var universalType = Asn1UniversalTypes.Get(tagNo) ?? throw new ArgumentException($"unsupported UNIVERSAL tag number: {tagNo}", nameof(tagNo));

		return GetBaseUniversal(declaredExplicit, universalType);
	}

	internal Asn1Object GetBaseUniversal(bool declaredExplicit, Asn1UniversalType universalType)
	{
		if (declaredExplicit)
		{
			if (!IsExplicit())
				throw new InvalidOperationException("object explicit - implicit expected.");

			return universalType.CheckedCast(BaseTaggedObject.ToAsn1Object());
		}

		if (DeclaredExplicit == Explicitness)
			throw new InvalidOperationException("object explicit - implicit expected.");

		var baseObject = BaseTaggedObject.ToAsn1Object();
		switch (Explicitness)
		{
			case ParsedExplicit:
				return universalType.FromImplicitConstructed(RebuildConstructed(baseObject));
			case ParsedImplicit:
			{
				if (baseObject is Asn1Sequence asn1Sequence)
					return universalType.FromImplicitConstructed(asn1Sequence);

				return universalType.FromImplicitPrimitive((DerOctetString) baseObject);
			}
			default:
				return universalType.CheckedCast(baseObject);
		}
	}

	protected override int Asn1GetHashCode() => (TagClass * 7919) ^ TagNo ^ (IsExplicit() ? 0x0F : 0xF0) ^ BaseTaggedObject.ToAsn1Object().CallAsn1GetHashCode();
}
