// perticula - core - BerTaggedObject.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerTaggedObject.
///   Implements the <see cref="DerTaggedObject" />
/// </summary>
/// <seealso cref="DerTaggedObject" />
public class BerTaggedObject : DerTaggedObject
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BerTaggedObject" /> class.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public BerTaggedObject(int tagNo, Asn1Encodable obj) : base(true, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerTaggedObject" /> class.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public BerTaggedObject(int tagClass, int tagNo, Asn1Encodable obj) : base(true, tagClass, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerTaggedObject" /> class.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public BerTaggedObject(bool isExplicit, int tagNo, Asn1Encodable obj) : base(isExplicit, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerTaggedObject" /> class.
	/// </summary>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	public BerTaggedObject(bool isExplicit, int tagClass, int tagNo, Asn1Encodable obj) : base(isExplicit, tagClass, tagNo, obj) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="BerTaggedObject" /> class.
	/// </summary>
	/// <param name="explicitness">The explicitness.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="obj">The object.</param>
	internal BerTaggedObject(int explicitness, int tagClass, int tagNo, Asn1Encodable obj) : base(explicitness, tagClass, tagNo, obj) { }

	/// <summary>
	///   Gets the asn1 encoding.
	/// </summary>
	/// <value>The asn1 encoding.</value>
	internal override string Asn1Encoding => Ber;

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding)
	{
		if (Asn1OutputStream.EncodingBer != encoding)
			return base.GetEncoding(encoding);

		var baseObject = GetBaseObject().ToAsn1Object();

		return !IsExplicit()
			       ? baseObject.GetEncodingImplicit(encoding, TagClass, TagNo)
			       : new ConstructedIndefiniteLengthEncoding(TagClass, TagNo, new[] {baseObject.GetEncoding(encoding)});
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
		if (Asn1OutputStream.EncodingBer != encoding)
			return base.GetEncodingImplicit(encoding, tagClass, tagNo);

		var baseObject = GetBaseObject().ToAsn1Object();

		return !IsExplicit()
			       ? baseObject.GetEncodingImplicit(encoding, tagClass, tagNo)
			       : new ConstructedIndefiniteLengthEncoding(tagClass, tagNo, new[] {baseObject.GetEncoding(encoding)});
	}

	/// <summary>
	///   Rebuilds the constructed.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns>Asn1Sequence.</returns>
	internal override Asn1Sequence RebuildConstructed(Asn1Object asn1Object) => new BerSequence(asn1Object);

	/// <summary>
	///   Replaces the tag.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>Asn1TaggedObject.</returns>
	internal override Asn1TaggedObject ReplaceTag(int tagClass, int tagNo) => new BerTaggedObject(Explicitness, tagClass, tagNo, BaseTaggedObject);
}
