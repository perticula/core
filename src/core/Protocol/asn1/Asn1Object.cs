// perticula - core - Asn1Object.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Object.
///   Implements the <see cref="core.Protocol.asn1.Asn1Encodable" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Encodable" />
public abstract class Asn1Object : Asn1Encodable
{
	/// <summary>
	///   Encodes to.
	/// </summary>
	/// <param name="output">The output.</param>
	public override void EncodeTo(Stream output)
	{
		using var asn1Out = Asn1OutputStream.Create(output, Ber, true);

		GetEncoding(asn1Out.Encoding).Encode(asn1Out);
	}

	/// <summary>
	///   Encodes to.
	/// </summary>
	/// <param name="output">The output.</param>
	/// <param name="encoding">The encoding.</param>
	public override void EncodeTo(Stream output, string encoding)
	{
		using var asn1Out = Asn1OutputStream.Create(output, encoding, true);

		GetEncoding(asn1Out.Encoding).Encode(asn1Out);
	}

	/// <summary>
	///   Internals the get encoded.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Byte[].</returns>
	internal virtual byte[] InternalGetEncoded(string encoding)
	{
		var encodingType = Asn1OutputStream.GetEncodingType(encoding);
		var asn1Encoding = GetEncoding(encodingType);
		var result       = new byte[asn1Encoding.GetLength()];

		using var asn1Out = Asn1OutputStream.Create(new MemoryStream(result, true), encoding);
		asn1Encoding.Encode(asn1Out);
		Debug.Assert(result.Length == asn1Out.Position);

		return result;
	}

	/// <summary>
	///   Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">The other object to check.</param>
	/// <returns>
	///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
	///   <see langword="false" />.
	/// </returns>
	public bool Equals(Asn1Object other) => base.Equals(other) || Asn1Equals(other);


	/// <summary>
	///   Create a base ASN.1 object from a byte array.
	/// </summary>
	/// <param name="data">The byte array to parse.</param>
	/// <returns>The base ASN.1 object represented by the byte array.</returns>
	/// <exception cref="System.IO.IOException">extra data found after object</exception>
	/// <exception cref="System.IO.IOException">cannot recognise object in byte array</exception>
	public static Asn1Object? FromByteArray(byte[] data)
	{
		try
		{
			using var  asn1In = new Asn1InputStream(new MemoryStream(data, false), data.Length);
			var result = asn1In.ReadObject();
			if (data.Length != asn1In.Position) throw new IOException("extra data found after object");
			return result;
		}
		catch (InvalidCastException)
		{
			throw new IOException("cannot recognise object in byte array");
		}
	}

	/// <summary>
	///   Read a base ASN.1 object from a stream.
	/// </summary>
	/// <param name="inStr">The stream to parse.</param>
	/// <returns>The base ASN.1 object represented by the byte array.</returns>
	/// <exception cref="System.IO.IOException">cannot recognise object in stream</exception>
	public static Asn1Object? FromStream(Stream inStr)
	{
		try
		{
			using var asn1In = new Asn1InputStream(inStr, int.MaxValue, leaveOpen: true);
			return asn1In.ReadObject();
		}
		catch (InvalidCastException)
		{
			throw new IOException("cannot recognise object in stream");
		}
	}

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	public sealed override Asn1Object ToAsn1Object() => this;

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal abstract IAsn1Encoding GetEncoding(int encoding);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal abstract IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo);

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal abstract DerEncoding GetEncodingDer();

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal abstract DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo);

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected abstract bool Asn1Equals(Asn1Object asn1Object);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected abstract int Asn1GetHashCode();

	/// <summary>
	///   Calls the asn1 equals.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	internal bool CallAsn1Equals(Asn1Object obj) => Asn1Equals(obj);

	/// <summary>
	///   Calls the asn1 get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	internal int CallAsn1GetHashCode() => Asn1GetHashCode();
}
