// perticula - core - Asn1UniversalTypes.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Cryptography;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1UniversalTypes.
/// </summary>
public static class Asn1UniversalTypes
{
	/// <summary>
	///   Gets the specified tag no.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>Asn1UniversalType.</returns>
	public static Asn1UniversalType Get(int tagNo)
		=> tagNo switch
		   {
			   Asn1Tags.Boolean             => DerBoolean.Meta.Instance,
			   Asn1Tags.Integer             => DerInteger.Meta.Instance,
			   Asn1Tags.BitString           => DerBitString.Meta.Instance,
			   Asn1Tags.OctetString         => Asn1OctetString.Meta.Instance,
			   Asn1Tags.Null                => Asn1Null.Meta.Instance,
			   Asn1Tags.ObjectIdentifier    => DerObjectIdentifier.Meta.Instance,
			   Asn1Tags.ObjectDescriptor    => Asn1ObjectDescriptor.Meta.Instance, // [UNIVERSAL 7] IMPLICIT GraphicString
			   Asn1Tags.External            => DerExternal.Meta.Instance,
			   Asn1Tags.Enumerated          => DerEnumerated.Meta.Instance,
			   Asn1Tags.Utf8String          => DerUtf8String.Meta.Instance, // [UNIVERSAL 12] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.RelativeOid         => Asn1RelativeOid.Meta.Instance,
			   Asn1Tags.Sequence            => Asn1Sequence.Meta.Instance,
			   Asn1Tags.Set                 => Asn1Set.Meta.Instance,
			   Asn1Tags.NumericString       => DerNumericString.Meta.Instance,    // [UNIVERSAL 18] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.PrintableString     => DerPrintableString.Meta.Instance,  // [UNIVERSAL 19] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.T61String           => DerT61String.Meta.Instance,        // [UNIVERSAL 20] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.VideotexString      => DerVideotexString.Meta.Instance,   // [UNIVERSAL 21] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.Ia5String           => DerIa5String.Meta.Instance,        // [UNIVERSAL 22] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.UtcTime             => Asn1UtcTime.Meta.Instance,         // [UNIVERSAL 23] IMPLICIT VisibleString (restricted values)
			   Asn1Tags.GeneralizedTime     => Asn1GeneralizedTime.Meta.Instance, // [UNIVERSAL 24] IMPLICIT VisibleString (restricted values)
			   Asn1Tags.GraphicString       => DerGraphicString.Meta.Instance,    // [UNIVERSAL 25] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.VisibleString       => DerVisibleString.Meta.Instance,    // [UNIVERSAL 26] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.GeneralString       => DerGeneralString.Meta.Instance,    // [UNIVERSAL 27] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.UniversalString     => DerUniversalString.Meta.Instance,  // [UNIVERSAL 28] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.BmpString           => DerBmpString.Meta.Instance,        // [UNIVERSAL 30] IMPLICIT OCTET STRING (encode as if)
			   Asn1Tags.Real                => null,
			   Asn1Tags.EmbeddedPdv         => null,
			   Asn1Tags.Time                => null,
			   Asn1Tags.UnrestrictedString  => null,
			   Asn1Tags.Date                => null,
			   Asn1Tags.TimeOfDay           => null,
			   Asn1Tags.DateTime            => null,
			   Asn1Tags.Duration            => null,
			   Asn1Tags.ObjectIdentifierIri => null,
			   Asn1Tags.RelativeOidIri      => null,
			   _                            => throw new CryptoException("Invalid tag specified")
		   };
}
