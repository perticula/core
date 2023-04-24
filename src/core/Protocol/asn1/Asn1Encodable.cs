// perticula - core - Asn1Encodable.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Encodable.
///   Implements the <see cref="IAsn1Convertable" />
///   Implements the <see cref="IEquatable{IAsn1Convertable}" />
///   Implements the <see cref="IEquatable{Asn1Encodable}" />
/// </summary>
/// <seealso cref="IAsn1Convertable" />
/// <seealso cref="IEquatable{IAsn1Convertable}" />
/// <seealso cref="IEquatable{Asn1Encodable}" />
public abstract class Asn1Encodable : IAsn1Convertable,
                                      IEquatable<IAsn1Convertable>,
                                      IEquatable<Asn1Encodable>
{
	/// <summary>
	///   Distinguished Encoding Rules
	/// </summary>
	public const string Der = "DER"; //https://en.wikipedia.org/wiki/X.690#DER_encoding

	/// <summary>
	///   Basic Encoding Rules
	/// </summary>
	public const string Ber = "BER"; //https://en.wikipedia.org/wiki/X.690#BER_encoding

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	public abstract Asn1Object ToAsn1Object();

	/// <summary>
	///   Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">An object to compare with this object.</param>
	/// <returns>
	///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public bool Equals(Asn1Encodable? other) => Equals((object?) other);

	/// <summary>
	///   Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">An object to compare with this object.</param>
	/// <returns>
	///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public bool Equals(IAsn1Convertable? other) => Equals((object?) other);

	/// <summary>
	///   Encodes to.
	/// </summary>
	/// <param name="output">The output.</param>
	public virtual void EncodeTo(Stream output) => ToAsn1Object().EncodeTo(output);

	/// <summary>
	///   Encodes to.
	/// </summary>
	/// <param name="output">The output.</param>
	/// <param name="encoding">The encoding.</param>
	public virtual void EncodeTo(Stream output, string encoding) => ToAsn1Object().EncodeTo(output, encoding);

	/// <summary>
	///   Gets the encoded.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	public virtual byte[] GetEncoded() => ToAsn1Object().InternalGetEncoded(Ber);

	/// <summary>
	///   Gets the encoded.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Byte[].</returns>
	public virtual byte[] GetEncoded(string encoding) => ToAsn1Object().InternalGetEncoded(encoding);


	/// <summary>
	///   Gets the der encoding of this object.
	///   If the object is not a valid DER encoding, null is returned.
	/// </summary>
	/// <returns>System.Nullable&lt;System.Byte&gt;[].</returns>
	public byte[]? GetDerEncoded()
	{
		try
		{
			return GetEncoded(Der);
		}
		catch (IOException)
		{
			return null;
		}
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public sealed override int GetHashCode() => ToAsn1Object().CallAsn1GetHashCode();

	/// <summary>
	///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
	/// </summary>
	/// <param name="obj">The object to compare with the current object.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
	public sealed override bool Equals(object? obj)
	{
		if (obj == null) return false;
		if (obj == this) return true;
		if (obj is not IAsn1Convertable that) return false;

		var o1 = ToAsn1Object();
		var o2 = that.ToAsn1Object();

		return o1 == o2 || o1.CallAsn1Equals(o2);
	}
}
