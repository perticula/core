// perticula - core - DerExternal.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.ber;

namespace core.Protocol.asn1.der;

public class DerExternal : Asn1Object
{
	private readonly Asn1Object? _externalContent;
	public DerExternal(Asn1EncodableVector vector) : this(new BerSequence(vector)) { }

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

		BaseEencoding   = CheckEncoding(o.TagNo);
		ExternalContent = GetExternalContent(o);
	}

	public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1ObjectDescriptor dataValueDescriptor, Asn1TaggedObject externalData)
	{
		DirectReference     = directReference;
		IndirectReference   = indirectReference;
		DataValueDescriptor = dataValueDescriptor;
		BaseEencoding       = CheckEncoding(externalData.TagNo);
		ExternalContent     = GetExternalContent(externalData);
	}

	public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1ObjectDescriptor dataValueDescriptor, int encoding, Asn1Object externalData)
	{
		DirectReference     = directReference;
		IndirectReference   = indirectReference;
		DataValueDescriptor = dataValueDescriptor;
		BaseEencoding       = CheckEncoding(encoding);
		_externalContent    = CheckExternalContent(encoding, externalData);
	}

	public Asn1ObjectDescriptor? DataValueDescriptor { get; }

	public DerObjectIdentifier? DirectReference { get; }

	public int BaseEencoding { get; }

	public Asn1Object? ExternalContent
	{
		get => _externalContent;
		private init => _externalContent = value ?? new DerNull();
	}

	public DerInteger? IndirectReference { get; }

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

	public static DerExternal GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerExternal) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	internal virtual Asn1Sequence BuildSequence()
	{
		var v = new Asn1EncodableVector(4);
		v.AddOptional(DirectReference, IndirectReference, DataValueDescriptor);
		v.Add(new DerTaggedObject(0 == BaseEencoding, BaseEencoding, _externalContent));
		return new DerSequence(v);
	}

	internal override IAsn1Encoding GetEncoding(int encoding) => BuildSequence().GetEncodingImplicit(Asn1OutputStream.EncodingDer, Asn1Tags.Universal, Asn1Tags.External);

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => BuildSequence().GetEncodingImplicit(Asn1OutputStream.EncodingDer, tagClass, tagNo);

	internal sealed override DerEncoding GetEncodingDer() => BuildSequence().GetEncodingDerImplicit(Asn1Tags.Universal, Asn1Tags.External);

	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => BuildSequence().GetEncodingDerImplicit(tagClass, tagNo);

	protected override int Asn1GetHashCode()
		=> Objects.GetHashCode(DirectReference)
		   ^ Objects.GetHashCode(IndirectReference)
		   ^ Objects.GetHashCode(DataValueDescriptor)
		   ^ BaseEencoding
		   ^ _externalContent!.GetHashCode();

	protected override bool Asn1Equals(Asn1Object asn1Object)
		=> asn1Object is DerExternal that
		   && Equals(DirectReference,     that.DirectReference)
		   && Equals(IndirectReference,   that.IndirectReference)
		   && Equals(DataValueDescriptor, that.DataValueDescriptor)
		   && BaseEencoding == that.BaseEencoding
		   && ExternalContent!.Equals(that.ExternalContent!);

	private static Asn1ObjectDescriptor CheckDataValueDescriptor(Asn1Object dataValueDescriptor)
	{
		if (dataValueDescriptor is Asn1ObjectDescriptor descriptor)
			return descriptor;
		if (dataValueDescriptor is DerGraphicString graphicString)
			return new Asn1ObjectDescriptor(graphicString);

		throw new ArgumentException("incompatable type for data-value-descriptor", nameof(dataValueDescriptor));
	}

	private static int CheckEncoding(int encoding) => encoding is < 0 or > 2 ? throw new InvalidOperationException($"invalid encoding value: {encoding}") : encoding;

	private static Asn1Object CheckExternalContent(int tagNo, Asn1Object externalContent)
	{
		return tagNo switch
		       {
			       1 => Asn1OctetString.Meta.Instance.CheckedCast(externalContent),
			       2 => DerBitString.Meta.Instance.CheckedCast(externalContent),
			       _ => externalContent
		       };
	}

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

	private static Asn1Object GetObjFromSequence(Asn1Sequence sequence, int index) => sequence.Count <= index ? throw new ArgumentException("too few objects in input sequence", nameof(sequence)) : sequence[index].ToAsn1Object();

	internal class Meta : Asn1UniversalType
	{
		internal static readonly Asn1UniversalType Instance = new Meta();

		private Meta() : base(typeof(DerExternal), Asn1Tags.External) { }

		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence.ToAsn1External();
	}
}
