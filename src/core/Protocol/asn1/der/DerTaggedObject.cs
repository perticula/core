// perticula - core - DerTaggedObject.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerTaggedObject.
///   Implements the <see cref="core.Protocol.asn1.Asn1TaggedObject" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1TaggedObject" />
public class DerTaggedObject : Asn1TaggedObject
{
	/// <summary>
	///   Initializes a new instance of the <see cref="DerTaggedObject" /> class.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public DerTaggedObject(int tagNo, Asn1Encodable obj) : base(true, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerTaggedObject" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public DerTaggedObject(int tagClass, int tagNo, Asn1Encodable obj) : base(true, tagClass, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerTaggedObject" /> class.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public DerTaggedObject(bool isExplicit, int tagNo, Asn1Encodable obj) : base(isExplicit, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerTaggedObject" /> class.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public DerTaggedObject(bool isExplicit, int tagClass, int tagNo, Asn1Encodable obj) : base(isExplicit, tagClass, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerTaggedObject" /> class.
	/// </summary>
	/// <param name="explicitness">The explicitness.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	internal DerTaggedObject(int explicitness, int tagClass, int tagNo, Asn1Encodable obj) : base(explicitness, tagClass, tagNo, obj) { }

	/// <summary>
	///   Gets the asn1 encoding.
	/// </summary>
	/// <value>The asn1 encoding.</value>
	internal override string Asn1Encoding => Der;

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding)
	{
		encoding = Asn1OutputStream.EncodingDer;

		var baseObject = GetBaseObject().ToAsn1Object();

		return !IsExplicit()
			       ? baseObject.GetEncodingImplicit(encoding, TagClass, TagNo)
			       : new ConstructedDefiniteLengthEncoding(TagClass, TagNo, new[] {baseObject.GetEncoding(encoding)});
	}

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
	{
		encoding = Asn1OutputStream.EncodingDer;

		var baseObject = GetBaseObject().ToAsn1Object();

		return !IsExplicit()
			       ? baseObject.GetEncodingImplicit(encoding, tagClass, tagNo)
			       : new ConstructedDefiniteLengthEncoding(tagClass, tagNo, new[] {baseObject.GetEncoding(encoding)});
	}

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer()
	{
		var baseObject = GetBaseObject().ToAsn1Object();

		return !IsExplicit()
			       ? baseObject.GetEncodingDerImplicit(TagClass, TagNo)
			       : new ConstructedDerEncoding(TagClass, TagNo, new[] {baseObject.GetEncodingDer()});
	}

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
	{
		var baseObject = GetBaseObject().ToAsn1Object();

		return !IsExplicit()
			       ? baseObject.GetEncodingDerImplicit(tagClass, tagNo)
			       : new ConstructedDerEncoding(tagClass, tagNo, new[] {baseObject.GetEncodingDer()});
	}

	/// <summary>
	///   Rebuilds the constructed.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns>Asn1Sequence.</returns>
	internal override Asn1Sequence RebuildConstructed(Asn1Object asn1Object) => new DerSequence(asn1Object);

	/// <summary>
	///   Replaces the tag.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>Asn1TaggedObject.</returns>
	internal override Asn1TaggedObject ReplaceTag(int tagClass, int tagNo) => new DerTaggedObject(Explicitness, tagClass, tagNo, BaseTaggedObject);
}
