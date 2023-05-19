// perticula - core - DerExternal.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.ber;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerExternal.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class DerExternal : Asn1Object
{
	/// <summary>
	///   The external content
	/// </summary>
	private readonly Asn1Object? _externalContent;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerExternal" /> class.
	/// </summary>
	/// <param name="vector">The vector.</param>
	public DerExternal(Asn1EncodableVector vector) : this(new BerSequence(vector)) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerExternal" /> class.
	/// </summary>
	/// <param name="sequence">The sequence.</param>
	/// <exception cref="System.ArgumentException">input sequence too large - sequence</exception>
	/// <exception cref="System.ArgumentException">
	///   No tagged object found in sequence. Structure doesn't seem to be of type
	///   External - sequence
	/// </exception>
	public DerExternal(Asn1Sequence sequence)
	{
		var offset = 0;

		var asn1 = GetObjFromSequence(sequence, offset);
		switch (asn1)
		{
			case DerObjectIdentifier identifier:
				DirectReference = identifier;
				asn1            = GetObjFromSequence(sequence, ++offset);
				break;

			case DerInteger integer:
				IndirectReference = integer;
				asn1              = GetObjFromSequence(sequence, ++offset);
				break;
		}

		if (asn1 is not Asn1TaggedObject)
		{
			DataValueDescriptor = (Asn1ObjectDescriptor) asn1;
			asn1                = GetObjFromSequence(sequence, ++offset);
		}

		if (sequence.Count != offset + 1)
			throw new ArgumentException("input sequence too large", nameof(sequence));

		if (asn1 is not Asn1TaggedObject o)
			throw new ArgumentException("No tagged object found in sequence. Structure doesn't seem to be of type External", nameof(sequence));

		BaseEncoding   = CheckEncoding(o.TagNo);
		ExternalContent = GetExternalContent(o);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerExternal" /> class.
	/// </summary>
	/// <param name="directReference">The direct reference.</param>
	/// <param name="indirectReference">The indirect reference.</param>
	/// <param name="dataValueDescriptor">The data value descriptor.</param>
	/// <param name="externalData">The external data.</param>
	public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1ObjectDescriptor dataValueDescriptor, Asn1TaggedObject externalData)
	{
		DirectReference     = directReference;
		IndirectReference   = indirectReference;
		DataValueDescriptor = dataValueDescriptor;
		BaseEncoding       = CheckEncoding(externalData.TagNo);
		ExternalContent     = GetExternalContent(externalData);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerExternal" /> class.
	/// </summary>
	/// <param name="directReference">The direct reference.</param>
	/// <param name="indirectReference">The indirect reference.</param>
	/// <param name="dataValueDescriptor">The data value descriptor.</param>
	/// <param name="encoding">The encoding.</param>
	/// <param name="externalData">The external data.</param>
	public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1ObjectDescriptor dataValueDescriptor, int encoding, Asn1Object externalData)
	{
		DirectReference     = directReference;
		IndirectReference   = indirectReference;
		DataValueDescriptor = dataValueDescriptor;
		BaseEncoding       = CheckEncoding(encoding);
		_externalContent    = CheckExternalContent(encoding, externalData);
	}

	/// <summary>
	///   Gets the data value descriptor.
	/// </summary>
	/// <value>The data value descriptor.</value>
	public Asn1ObjectDescriptor? DataValueDescriptor { get; }

	/// <summary>
	///   Gets the direct reference.
	/// </summary>
	/// <value>The direct reference.</value>
	public DerObjectIdentifier? DirectReference { get; }

	/// <summary>
	///   Gets the base eencoding.
	/// </summary>
	/// <value>The base eencoding.</value>
	public int BaseEncoding { get; }

	/// <summary>
	///   Gets the content of the external.
	/// </summary>
	/// <value>The content of the external.</value>
	public Asn1Object? ExternalContent
	{
		get => _externalContent;
		private init => _externalContent = value ?? new DerNull();
	}

	/// <summary>
	///   Gets the indirect reference.
	/// </summary>
	/// <value>The indirect reference.</value>
	public DerInteger? IndirectReference { get; }

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>DerExternal.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">failed to construct external from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in {nameof(GetInstance)}: " + obj.GetTypeName() - obj</exception>
	public static DerExternal GetInstance(object obj)
	{
		switch (obj)
		{
			case null:
				throw new ArgumentNullException(nameof(obj));
			case DerExternal derExternal:
				return derExternal;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerExternal converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerExternal) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct external from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in {nameof(GetInstance)}: " + obj.GetTypeName(), nameof(obj));
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>DerExternal.</returns>
	public static DerExternal GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerExternal) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Builds the sequence.
	/// </summary>
	/// <returns>Asn1Sequence.</returns>
	internal virtual Asn1Sequence BuildSequence()
	{
		var v = new Asn1EncodableVector(4);
		v.AddOptional(DirectReference, IndirectReference, DataValueDescriptor);
		v.Add(new DerTaggedObject(0 == BaseEncoding, BaseEncoding, _externalContent!));
		return new DerSequence(v);
	}

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => BuildSequence().GetEncodingImplicit(Asn1OutputStream.EncodingDer, Asn1Tags.Universal, Asn1Tags.External);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => BuildSequence().GetEncodingImplicit(Asn1OutputStream.EncodingDer, tagClass, tagNo);

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => BuildSequence().GetEncodingDerImplicit(Asn1Tags.Universal, Asn1Tags.External);

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => BuildSequence().GetEncodingDerImplicit(tagClass, tagNo);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode()
		=> Objects.GetHashCode(DirectReference)
		   ^ Objects.GetHashCode(IndirectReference)
		   ^ Objects.GetHashCode(DataValueDescriptor)
		   ^ BaseEncoding
		   ^ _externalContent!.GetHashCode();

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object)
		=> asn1Object is DerExternal that
		   && Equals(DirectReference,     that.DirectReference)
		   && Equals(IndirectReference,   that.IndirectReference)
		   && Equals(DataValueDescriptor, that.DataValueDescriptor)
		   && BaseEncoding == that.BaseEncoding
		   && ExternalContent!.Equals(that.ExternalContent!);

	/// <summary>
	///   Checks the data value descriptor.
	/// </summary>
	/// <param name="dataValueDescriptor">The data value descriptor.</param>
	/// <returns>Asn1ObjectDescriptor.</returns>
	private static Asn1ObjectDescriptor CheckDataValueDescriptor(Asn1Object dataValueDescriptor)
		=> dataValueDescriptor switch
		   {
			   Asn1ObjectDescriptor descriptor => descriptor,
			   DerGraphicString graphicString  => new Asn1ObjectDescriptor(graphicString),
			   _                               => throw new ArgumentException("incompatable type for data-value-descriptor", nameof(dataValueDescriptor))
		   };

	/// <summary>
	///   Checks the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Int32.</returns>
	/// <exception cref="System.InvalidOperationException">invalid encoding value: {encoding}</exception>
	private static int CheckEncoding(int encoding) => encoding is < 0 or > 2 ? throw new InvalidOperationException($"invalid encoding value: {encoding}") : encoding;

	/// <summary>
	///   Checks the content of the external.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="externalContent">Content of the external.</param>
	/// <returns>Asn1Object.</returns>
	private static Asn1Object CheckExternalContent(int tagNo, Asn1Object externalContent)
		=> tagNo switch
		   {
			   1 => Asn1OctetString.Meta.Instance.CheckedCast(externalContent),
			   2 => DerBitString.Meta.Instance.CheckedCast(externalContent),
			   _ => externalContent
		   };

	/// <summary>
	///   Gets the content of the external.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Nullable&lt;Asn1Object&gt;.</returns>
	/// <exception cref="System.ArgumentException">invalid tag: {Asn1.GetTagText(tagClass, tagNo)} - encoding</exception>
	private static Asn1Object? GetExternalContent(Asn1TaggedObject encoding)
	{
		int tagClass = encoding.TagClass, tagNo = encoding.TagNo;
		if (Asn1Tags.ContextSpecific != tagClass) throw new ArgumentException($"invalid tag: {Asn1.GetTagText(tagClass, tagNo)}", nameof(encoding));

		return tagNo switch
		       {
			       0 => encoding.GetExplicitBaseObject().ToAsn1Object(),
			       1 => Asn1OctetString.GetInstance(encoding, false),
			       2 => DerBitString.GetInstance(encoding, false),
			       _ => throw new ArgumentException($"invalid tag: {Asn1.GetTagText(tagClass, tagNo)}", nameof(encoding))
		       };
	}

	/// <summary>
	///   Gets the object from sequence.
	/// </summary>
	/// <param name="sequence">The sequence.</param>
	/// <param name="index">The index.</param>
	/// <returns>Asn1Object.</returns>
	/// <exception cref="System.ArgumentException">too few objects in input sequence - sequence</exception>
	private static Asn1Object GetObjFromSequence(Asn1Sequence sequence, int index) => sequence.Count <= index ? throw new ArgumentException("too few objects in input sequence", nameof(sequence)) : sequence[index].ToAsn1Object();

	/// <summary>
	///   Class Meta.
	///   Implements the <see cref="core.Protocol.asn1.Asn1UniversalType" />
	/// </summary>
	/// <seealso cref="core.Protocol.asn1.Asn1UniversalType" />
	internal class Meta : Asn1UniversalType
	{
		/// <summary>
		///   The instance
		/// </summary>
		internal static readonly Asn1UniversalType Instance = new Meta();

		/// <summary>
		///   Prevents a default instance of the <see cref="Meta" /> class from being created.
		/// </summary>
		private Meta() : base(typeof(DerExternal), Asn1Tags.External) { }

		/// <summary>
		///   Froms the implicit constructed.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence.ToAsn1External();
	}
}
