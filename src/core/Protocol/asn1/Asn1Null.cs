// perticula - core - Asn1Null.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

public abstract class Asn1Null : Asn1Object
{
	internal Asn1Null() { }

	public static Asn1Null? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:              return null;
			case Asn1Null asn1Null: return asn1Null;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is Asn1Null converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1Null) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct NULL from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}");
	}

	public static Asn1Null GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1Null) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	public override string ToString() => "NULL";

	internal static Asn1Null CreatePrimitive(byte[] contents)
	{
		if (0 != contents.Length) throw new InvalidOperationException("malformed NULL encoding encountered");

		return DerNull.Instance;
	}

	internal class Meta : Asn1UniversalType
	{
		internal static readonly Asn1UniversalType Instance = new Meta();

		private Meta() : base(typeof(Asn1Null), Asn1Tags.Null) { }

		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
