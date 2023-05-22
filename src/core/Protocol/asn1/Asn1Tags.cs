// perticula - core - Asn1Tags.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Tags.
/// </summary>
public static class Asn1Tags
{
	// 0x00: Reserved for use by the encoding rules
	/// <summary>
	///   The boolean
	/// </summary>
	public const int Boolean = 0x01;

	/// <summary>
	///   The integer
	/// </summary>
	public const int Integer = 0x02;

	/// <summary>
	///   The bit string
	/// </summary>
	public const int BitString = 0x03;

	/// <summary>
	///   The octet string
	/// </summary>
	public const int OctetString = 0x04;

	/// <summary>
	///   The null
	/// </summary>
	public const int Null = 0x05;

	/// <summary>
	///   The object identifier
	/// </summary>
	public const int ObjectIdentifier = 0x06;

	/// <summary>
	///   The object descriptor
	/// </summary>
	public const int ObjectDescriptor = 0x07;

	/// <summary>
	///   The external
	/// </summary>
	public const int External = 0x08;

	/// <summary>
	///   The real
	/// </summary>
	public const int Real = 0x09;

	/// <summary>
	///   The enumerated
	/// </summary>
	public const int Enumerated = 0x0a;

	/// <summary>
	///   The embedded PDV
	/// </summary>
	public const int EmbeddedPdv = 0x0b;

	/// <summary>
	///   The UTF8 string
	/// </summary>
	public const int Utf8String = 0x0c;

	/// <summary>
	///   The relative oid
	/// </summary>
	public const int RelativeOid = 0x0d;

	/// <summary>
	///   The time
	/// </summary>
	public const int Time = 0x0e;

	// 0x0f: Reserved for future editions of this Recommendation | International Standard
	/// <summary>
	///   The sequence
	/// </summary>
	public const int Sequence = 0x10;

	/// <summary>
	///   The sequence of
	/// </summary>
	public const int SequenceOf = 0x10; // for completeness

	/// <summary>
	///   The set
	/// </summary>
	public const int Set = 0x11;

	/// <summary>
	///   The set of
	/// </summary>
	public const int SetOf = 0x11; // for completeness

	/// <summary>
	///   The numeric string
	/// </summary>
	public const int NumericString = 0x12;

	/// <summary>
	///   The printable string
	/// </summary>
	public const int PrintableString = 0x13;

	/// <summary>
	///   The T61 string
	/// </summary>
	public const int T61String = 0x14;

	/// <summary>
	///   The video tex string
	/// </summary>
	public const int VideoTexString = 0x15;

	/// <summary>
	///   The ia5 string
	/// </summary>
	public const int Ia5String = 0x16;

	/// <summary>
	///   The UTC time
	/// </summary>
	public const int UtcTime = 0x17;

	/// <summary>
	///   The generalized time
	/// </summary>
	public const int GeneralizedTime = 0x18;

	/// <summary>
	///   The graphic string
	/// </summary>
	public const int GraphicString = 0x19;

	/// <summary>
	///   The visible string
	/// </summary>
	public const int VisibleString = 0x1a;

	/// <summary>
	///   The general string
	/// </summary>
	public const int GeneralString = 0x1b;

	/// <summary>
	///   The universal string
	/// </summary>
	public const int UniversalString = 0x1c;

	/// <summary>
	///   The unrestricted string
	/// </summary>
	public const int UnrestrictedString = 0x1d;

	/// <summary>
	///   The BMP string
	/// </summary>
	public const int BmpString = 0x1e;

	/// <summary>
	///   The date
	/// </summary>
	public const int Date = 0x1f;

	/// <summary>
	///   The time of day
	/// </summary>
	public const int TimeOfDay = 0x20;

	/// <summary>
	///   The date time
	/// </summary>
	public const int DateTime = 0x21;

	/// <summary>
	///   The duration
	/// </summary>
	public const int Duration = 0x22;

	/// <summary>
	///   The object identifier iri
	/// </summary>
	public const int ObjectIdentifierIri = 0x23;

	/// <summary>
	///   The relative oid iri
	/// </summary>
	public const int RelativeOidIri = 0x24;

	// 0x25..: Reserved for addenda to this Recommendation | International Standard

	/// <summary>
	///   The constructed
	/// </summary>
	public const int Constructed = 0x20;

	/// <summary>
	///   The universal
	/// </summary>
	public const int Universal = 0x00;

	/// <summary>
	///   The application
	/// </summary>
	public const int Application = 0x40;

	/// <summary>
	///   The context specific
	/// </summary>
	public const int ContextSpecific = 0x80;

	/// <summary>
	///   The private
	/// </summary>
	public const int Private = 0xC0;

	/// <summary>
	///   The flags
	/// </summary>
	public const int Flags = 0xE0;
}
