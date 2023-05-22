// perticula - core - Asn1TaggedObject.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.ber;
using core.Protocol.asn1.der;
using core.Protocol.asn1.dl;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1TaggedObject.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
///   Implements the <see cref="core.Protocol.asn1.IAsn1TaggedObjectParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
/// <seealso cref="core.Protocol.asn1.IAsn1TaggedObjectParser" />
public abstract class Asn1TaggedObject : Asn1Object, IAsn1TaggedObjectParser
{
	/// <summary>
	///   The declared explicit
	/// </summary>
	private const int DeclaredExplicit = 1;

	/// <summary>
	///   The declared implicit
	/// </summary>
	private const int DeclaredImplicit = 2;

	/// <summary>
	///   The parsed explicit
	/// </summary>
	private const int ParsedExplicit = 3;

	/// <summary>
	///   The parsed implicit
	/// </summary>
	private const int ParsedImplicit = 4;

	/// <summary>
	///   The base tagged object
	/// </summary>
	internal readonly Asn1Encodable BaseTaggedObject;

	/// <summary>
	///   The explicitness
	/// </summary>
	internal readonly int Explicitness;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1TaggedObject" /> class.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	protected Asn1TaggedObject(bool isExplicit, int tagNo, Asn1Encodable obj) : this(isExplicit, Asn1Tags.ContextSpecific, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1TaggedObject" /> class.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	protected Asn1TaggedObject(bool isExplicit, int tagClass, int tagNo, Asn1Encodable obj) : this(isExplicit ? DeclaredExplicit : DeclaredImplicit, tagClass, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1TaggedObject" /> class.
	/// </summary>
	/// <param name="explicitness">The explicitness.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">invalid tag class: {tagClass} - tagClass</exception>
	internal Asn1TaggedObject(int explicitness, int tagClass, int tagNo, Asn1Encodable obj)
	{
		if (null == obj) throw new ArgumentNullException(nameof(obj));
		if (Asn1Tags.Universal == tagClass || (tagClass & Asn1Tags.Private) != tagClass) throw new ArgumentException($"invalid tag class: {tagClass}", nameof(tagClass));

		// ReSharper disable once SuspiciousTypeConversion.Global
		Explicitness     = obj is IAsn1Choice ? DeclaredExplicit : explicitness;
		TagClass         = tagClass;
		TagNo            = tagNo;
		BaseTaggedObject = obj;
	}

	/// <summary>
	///   Gets the asn1 encoding.
	/// </summary>
	/// <value>The asn1 encoding.</value>
	internal abstract string Asn1Encoding { get; }

	/// <summary>
	///   Gets the tag class.
	/// </summary>
	/// <value>The tag class.</value>
	public int TagClass { get; }

	/// <summary>
	///   Gets the tag no.
	/// </summary>
	/// <value>The tag no.</value>
	public int TagNo { get; }

	/// <summary>
	///   Determines whether [has context tag].
	/// </summary>
	/// <returns><c>true</c> if [has context tag]; otherwise, <c>false</c>.</returns>
	public bool HasContextTag() => TagClass == Asn1Tags.ContextSpecific;

	/// <summary>
	///   Determines whether [has context tag] [the specified tag no].
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <returns><c>true</c> if [has context tag] [the specified tag no]; otherwise, <c>false</c>.</returns>
	public bool HasContextTag(int tagNo) => TagClass == Asn1Tags.ContextSpecific && TagNo == tagNo;

	/// <summary>
	///   Determines whether the specified tag class has tag.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns><c>true</c> if the specified tag class has tag; otherwise, <c>false</c>.</returns>
	public bool HasTag(int tagClass, int tagNo) => TagClass == tagClass && TagNo == tagNo;

	/// <summary>
	///   Determines whether [has tag class] [the specified tag class].
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <returns><c>true</c> if [has tag class] [the specified tag class]; otherwise, <c>false</c>.</returns>
	public bool HasTagClass(int tagClass) => TagClass == tagClass;

	/// <summary>
	///   Needed for open types, until we have better type-guided parsing support.
	/// </summary>
	/// <returns>IAsn1Convertable.</returns>
	/// <remarks>
	///   Use sparingly for other purposes, and prefer <see cref="ParseExplicitBaseTagged" /> or
	///   <see cref="ParseBaseUniversal(bool, int)" /> where possible. Before using, check for matching tag
	///   <see cref="TagClass">class</see> and <see cref="TagNo">number</see>.
	/// </remarks>
	public IAsn1Convertable ParseExplicitBaseObject() => GetExplicitBaseObject();

	/// <summary>
	///   Parses the base universal.
	/// </summary>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>IAsn1Convertable.</returns>
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

	/// <summary>
	///   Parses the explicit base tagged.
	/// </summary>
	/// <returns>Asn1TaggedObjectParser.</returns>
	public IAsn1TaggedObjectParser ParseExplicitBaseTagged() => GetExplicitBaseTagged();

	/// <summary>
	///   Parses the implicit base tagged.
	/// </summary>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>Asn1TaggedObjectParser.</returns>
	public IAsn1TaggedObjectParser ParseImplicitBaseTagged(int baseTagClass, int baseTagNo) => GetImplicitBaseTagged(baseTagClass, baseTagNo);

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>Asn1TaggedObject.</returns>
	/// <exception cref="System.ArgumentException">failed to construct tagged object from byte[] - obj</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
	public static Asn1TaggedObject GetInstance(object obj)
	{
		switch (obj)
		{
			case Asn1TaggedObject asn1TaggedObject: return asn1TaggedObject;
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

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <returns>Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetInstance(object obj, int tagClass) => CheckInstance(obj).CheckTagClass(tagClass);

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetInstance(object obj, int tagClass, int tagNo) => CheckInstance(obj).CheckTag(tagClass, tagNo);

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => CheckInstance(taggedObject, declaredExplicit).GetExplicitContextBaseTagged();

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetInstance(Asn1TaggedObject taggedObject, int tagClass, bool declaredExplicit) => CheckInstance(taggedObject, declaredExplicit).GetExplicitBaseTagged(tagClass);

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1TaggedObject.</returns>
	public static Asn1TaggedObject GetInstance(Asn1TaggedObject taggedObject, int tagClass, int tagNo, bool declaredExplicit) => CheckInstance(taggedObject, declaredExplicit).GetExplicitBaseTagged(tagClass, tagNo);

	/// <summary>
	///   Checks the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>Asn1TaggedObject.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	private static Asn1TaggedObject CheckInstance(object obj) => GetInstance(obj ?? throw new ArgumentNullException(nameof(obj)));

	/// <summary>
	///   Checks the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1TaggedObject.</returns>
	/// <exception cref="System.ArgumentException">this method not valid for implicitly tagged tagged objects</exception>
	/// <exception cref="System.ArgumentNullException">taggedObject</exception>
	private static Asn1TaggedObject CheckInstance(Asn1TaggedObject taggedObject, bool declaredExplicit)
	{
		if (!declaredExplicit)
			throw new ArgumentException("this method not valid for implicitly tagged tagged objects");

		return taggedObject ?? throw new ArgumentNullException(nameof(taggedObject));
	}

	/// <summary>
	///   Determines whether this instance is explicit.
	/// </summary>
	/// <returns><c>true</c> if this instance is explicit; otherwise, <c>false</c>.</returns>
	public bool IsExplicit() => Explicitness switch
	                            {
		                            DeclaredExplicit => true,
		                            ParsedExplicit   => true,
		                            _                => false
	                            };

	/// <summary>
	///   Determines whether this instance is parsed.
	/// </summary>
	/// <returns><c>true</c> if this instance is parsed; otherwise, <c>false</c>.</returns>
	public bool IsParsed() => Explicitness switch
	                          {
		                          ParsedExplicit => true,
		                          ParsedImplicit => true,
		                          _              => false
	                          };

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

	/// <summary>
	///   Gets the object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	public Asn1Object GetObject()
	{
		this.CheckTagClass(Asn1Tags.ContextSpecific);

		return BaseTaggedObject.ToAsn1Object();
	}

	/// <summary>
	///   Gets the base object.
	/// </summary>
	/// <returns>Asn1Encodable.</returns>
	public Asn1Encodable GetBaseObject() => BaseTaggedObject;

	/// <summary>
	///   Gets the explicit base object.
	/// </summary>
	/// <returns>Asn1Encodable.</returns>
	/// <exception cref="System.InvalidOperationException">object implicit - explicit expected.</exception>
	public Asn1Encodable GetExplicitBaseObject()
	{
		if (!IsExplicit())
			throw new InvalidOperationException("object implicit - explicit expected.");

		return BaseTaggedObject;
	}

	/// <summary>
	///   Gets the explicit base tagged.
	/// </summary>
	/// <returns>Asn1TaggedObject.</returns>
	/// <exception cref="System.InvalidOperationException">object implicit - explicit expected.</exception>
	public Asn1TaggedObject GetExplicitBaseTagged()
	{
		if (!IsExplicit())
			throw new InvalidOperationException("object implicit - explicit expected.");

		return CheckedCast(BaseTaggedObject.ToAsn1Object());
	}

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => Asn1.GetTagText(TagClass, TagNo) + BaseTaggedObject;

	/// <summary>
	///   Rebuilds the constructed.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns>Asn1Sequence.</returns>
	internal abstract Asn1Sequence RebuildConstructed(Asn1Object asn1Object);

	/// <summary>
	///   Replaces the tag.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>Asn1TaggedObject.</returns>
	internal abstract Asn1TaggedObject ReplaceTag(int tagClass, int tagNo);

	/// <summary>
	///   Creates the constructed dl.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsElements">The contents elements.</param>
	/// <returns>Asn1Object.</returns>
	internal static Asn1Object CreateConstructedDefinteLength(int tagClass, int tagNo, Asn1EncodableVector contentsElements)
	{
		var maybeExplicit = contentsElements.Count == 1;

		return maybeExplicit
			       ? new DefiniteLengthTaggedObject(ParsedExplicit, tagClass, tagNo, contentsElements[0])
			       : new DefiniteLengthTaggedObject(ParsedImplicit, tagClass, tagNo, DefinteLengthSequence.FromVector(contentsElements));
	}

	/// <summary>
	///   Creates the constructed il.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsElements">The contents elements.</param>
	/// <returns>Asn1Object.</returns>
	internal static Asn1Object CreateConstructedIndefinteLength(int tagClass, int tagNo, Asn1EncodableVector contentsElements)
	{
		var maybeExplicit = contentsElements.Count == 1;

		return maybeExplicit
			       ? new BerTaggedObject(ParsedExplicit, tagClass, tagNo, contentsElements[0])
			       : new BerTaggedObject(ParsedImplicit, tagClass, tagNo, BerSequence.FromVector(contentsElements));
	}

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsOctets">The contents octets.</param>
	/// <returns>Asn1Object.</returns>
	internal static Asn1Object CreatePrimitive(int tagClass, int tagNo, byte[] contentsOctets) => new DefiniteLengthTaggedObject(ParsedImplicit, tagClass, tagNo, new DerOctetString(contentsOctets));

	/// <summary>
	///   Checkeds the cast.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns>Asn1TaggedObject.</returns>
	/// <exception cref="System.InvalidOperationException">unexpected object: {asn1Object?.GetTypeName()}</exception>
	private static Asn1TaggedObject CheckedCast(Asn1Object? asn1Object)
	{
		if (asn1Object is Asn1TaggedObject taggedObject) return taggedObject;

		throw new InvalidOperationException($"unexpected object: {asn1Object?.GetTypeName()}");
	}

	/// <summary>
	///   Gets the implicit base tagged.
	/// </summary>
	/// <param name="baseTagClass">The base tag class.</param>
	/// <param name="baseTagNo">The base tag no.</param>
	/// <returns>Asn1TaggedObject.</returns>
	/// <exception cref="System.ArgumentException">invalid base tag class: {baseTagClass} - baseTagClass</exception>
	/// <exception cref="System.InvalidOperationException">object explicit - implicit expected.</exception>
	public Asn1TaggedObject GetImplicitBaseTagged(int baseTagClass, int baseTagNo)
	{
		if (Asn1Tags.Universal == baseTagClass || (baseTagClass & Asn1Tags.Private) != baseTagClass)
			throw new ArgumentException($"invalid base tag class: {baseTagClass}", nameof(baseTagClass));

		switch (Explicitness)
		{
			case DeclaredExplicit: throw new InvalidOperationException("object explicit - implicit expected.");

			case DeclaredImplicit:
			{
				var declared = CheckedCast(BaseTaggedObject.ToAsn1Object());
				return declared.CheckTag(baseTagClass, baseTagNo);
			}

			// Parsed; return a virtual tag (i.e. that couldn't have been present in the encoding)
			default:
				return ReplaceTag(baseTagClass, baseTagNo);
		}
	}

	/// <summary>
	///   Gets the base universal.
	/// </summary>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.ArgumentException">unsupported UNIVERSAL tag number: {tagNo} - tagNo</exception>
	public Asn1Object GetBaseUniversal(bool declaredExplicit, int tagNo)
	{
		var universalType = Asn1UniversalTypes.Get(tagNo) ?? throw new ArgumentException($"unsupported UNIVERSAL tag number: {tagNo}", nameof(tagNo));

		return GetBaseUniversal(declaredExplicit, universalType);
	}

	/// <summary>
	///   Gets the base universal.
	/// </summary>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <param name="universalType">Type of the universal.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.InvalidOperationException">object explicit - implicit expected.</exception>
	internal Asn1Object GetBaseUniversal(bool declaredExplicit, Asn1UniversalType universalType)
	{
		if (declaredExplicit)
		{
			if (!IsExplicit()) throw new InvalidOperationException("object explicit - implicit expected.");

			return universalType.CheckedCast(BaseTaggedObject.ToAsn1Object());
		}

		if (DeclaredExplicit == Explicitness) throw new InvalidOperationException("object explicit - implicit expected.");

		var baseObject = BaseTaggedObject.ToAsn1Object();
		switch (Explicitness)
		{
			case ParsedExplicit: return universalType.FromImplicitConstructed(RebuildConstructed(baseObject));
			case ParsedImplicit:
			{
				if (baseObject is Asn1Sequence asn1Sequence) return universalType.FromImplicitConstructed(asn1Sequence);

				return universalType.FromImplicitPrimitive((DerOctetString) baseObject);
			}
			default: return universalType.CheckedCast(baseObject);
		}
	}

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => (TagClass * 7919) ^ TagNo ^ (IsExplicit() ? 0x0F : 0xF0) ^ BaseTaggedObject.ToAsn1Object().CallAsn1GetHashCode();
}
