// perticula - core - IAsn1Encoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers.Binary;
using System.Diagnostics;
using System.Numerics;
using core.IO;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1OutputStream.
///   Implements the <see cref="FilterStream" />
/// </summary>
/// <seealso cref="FilterStream" />
public class Asn1OutputStream : FilterStream
{
	/// <summary>
	///   The encoding ber
	/// </summary>
	internal const int EncodingBer = 1;

	/// <summary>
	///   The encoding der
	/// </summary>
	internal const int EncodingDer = 2;

	/// <summary>
	///   The leave open
	/// </summary>
	private readonly bool _leaveOpen;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1OutputStream" /> class.
	/// </summary>
	/// <param name="output">The output.</param>
	/// <param name="leaveOpen">if set to <c>true</c> [leave open].</param>
	/// <exception cref="System.ArgumentException">Expected stream to be writable - output</exception>
	protected internal Asn1OutputStream(Stream output, bool leaveOpen) : base(output)
	{
		if (!output.CanWrite) throw new ArgumentException("Expected stream to be writable", nameof(output));

		_leaveOpen = leaveOpen;
	}

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <value>The encoding.</value>
	internal virtual int Encoding => EncodingBer;

	/// <summary>
	///   Creates the specified output.
	/// </summary>
	/// <param name="output">The output.</param>
	/// <returns>Asn1OutputStream.</returns>
	public static Asn1OutputStream Create(Stream output) => Create(output, Asn1Encodable.Ber);

	/// <summary>
	///   Creates the specified output.
	/// </summary>
	/// <param name="output">The output.</param>
	/// <param name="encoding">The encoding.</param>
	/// <returns>Asn1OutputStream.</returns>
	public static Asn1OutputStream Create(Stream output, string encoding) => Create(output, encoding, false);

	/// <summary>
	///   Creates the specified output.
	/// </summary>
	/// <param name="output">The output.</param>
	/// <param name="encoding">The encoding.</param>
	/// <param name="leaveOpen">if set to <c>true</c> [leave open].</param>
	/// <returns>Asn1OutputStream.</returns>
	public static Asn1OutputStream Create(Stream output, string encoding, bool leaveOpen)
	{
		if (Asn1Encodable.Der.Equals(encoding)) return new DerOutputStream(output, leaveOpen);

		return new Asn1OutputStream(output, leaveOpen);
	}

	/// <summary>
	///   Gets the type of the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetEncodingType(string encoding) => Asn1Encodable.Der.Equals(encoding) ? EncodingDer : EncodingBer;

	/// <summary>
	///   Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed
	///   resources.
	/// </summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources;
	///   <see langword="false" /> to release only unmanaged resources.
	/// </param>
	protected override void Dispose(bool disposing)
	{
		if (disposing) FlushInternal();

		if (_leaveOpen) Detach(disposing);
		else base.Dispose(disposing);
	}

	/// <summary>
	///   Writes the object.
	/// </summary>
	/// <param name="asn1Encodable">The asn1 encodable.</param>
	/// <exception cref="System.ArgumentNullException">asn1Encodable</exception>
	public virtual void WriteObject(Asn1Encodable asn1Encodable)
	{
		if (null == asn1Encodable) throw new ArgumentNullException(nameof(asn1Encodable));

		asn1Encodable.ToAsn1Object().GetEncoding(Encoding).Encode(this);
		FlushInternal();
	}

	/// <summary>
	///   Writes the object.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <exception cref="System.ArgumentNullException">asn1Object</exception>
	public virtual void WriteObject(Asn1Object asn1Object)
	{
		if (null == asn1Object) throw new ArgumentNullException(nameof(asn1Object));

		asn1Object.GetEncoding(Encoding).Encode(this);
		FlushInternal();
	}

	/// <summary>
	///   Encodes the contents.
	/// </summary>
	/// <param name="contentsEncodings">The contents encodings.</param>
	internal void EncodeContents(IAsn1Encoding[] contentsEncodings)
	{
		for (int i = 0, count = contentsEncodings.Length; i < count; ++i)
			contentsEncodings[i].Encode(this);
	}

	/// <summary>
	///   Flushes the internal.
	/// </summary>
	protected virtual void FlushInternal()
	{
		// Placeholder to support future internal buffering
	}

	/// <summary>
	///   Writes the dl.
	/// </summary>
	/// <param name="dl">The dl.</param>
	internal void WriteDL(int dl)
	{
		if (dl < 128)
		{
			Debug.Assert(dl >= 0);
			WriteByte((byte) dl);
			return;
		}

		Span<byte> encoding = stackalloc byte[5];
		BinaryPrimitives.WriteUInt32BigEndian(encoding[1..], (uint) dl);
		var leadingZeroBytes = BitOperations.LeadingZeroCount((uint) dl) / 8;
		encoding[leadingZeroBytes] = (byte) (0x84 - leadingZeroBytes);
		Write(encoding[leadingZeroBytes..]);
	}

	/// <summary>
	///   Writes the identifier.
	/// </summary>
	/// <param name="flags">The flags.</param>
	/// <param name="tagNo">The tag no.</param>
	internal void WriteIdentifier(int flags, int tagNo)
	{
		if (tagNo < 31)
		{
			WriteByte((byte) (flags | tagNo));
			return;
		}

		Span<byte> stack = stackalloc byte[6];

		var pos = stack.Length;

		stack[--pos] = (byte) (tagNo & 0x7F);
		while (tagNo > 127)
		{
			tagNo        >>= 7;
			stack[--pos] =   (byte) ((tagNo & 0x7F) | 0x80);
		}

		stack[--pos] = (byte) (flags | 0x1F);
		Write(stack[pos..]);
	}

	/// <summary>
	///   Gets the contents encodings.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="elements">The elements.</param>
	/// <returns>IAsn1Encoding[].</returns>
	internal static IAsn1Encoding[] GetContentsEncodings(int encoding, Asn1Encodable[] elements)
	{
		var count             = elements.Length;
		var contentsEncodings = new IAsn1Encoding[count];
		for (var i = 0; i < count; ++i)
			contentsEncodings[i] = elements[i].ToAsn1Object().GetEncoding(encoding);
		return contentsEncodings;
	}

	/// <summary>
	///   Gets the contents encodings der.
	/// </summary>
	/// <param name="elements">The elements.</param>
	/// <returns>DerEncoding[].</returns>
	internal static DerEncoding[] GetContentsEncodingsDer(Asn1Encodable[] elements)
	{
		var count             = elements.Length;
		var contentsEncodings = new DerEncoding[count];
		for (var i = 0; i < count; ++i)
			contentsEncodings[i] = elements[i].ToAsn1Object().GetEncodingDer();
		return contentsEncodings;
	}

	/// <summary>
	///   Gets the length of contents.
	/// </summary>
	/// <param name="contentsEncodings">The contents encodings.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetLengthOfContents(IAsn1Encoding[] contentsEncodings)
	{
		var contentsLength = 0;
		for (int i = 0, count = contentsEncodings.Length; i < count; ++i)
			contentsLength += contentsEncodings[i].GetLength();
		return contentsLength;
	}

	/// <summary>
	///   Gets the length of dl.
	/// </summary>
	/// <param name="dl">The dl.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetLengthOfDL(int dl)
	{
		if (dl < 128)
			return 1;

		var length = 2;
		while ((dl >>= 8) > 0) ++length;
		return length;
	}

	/// <summary>
	///   Gets the length of encoding dl.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsLength">Length of the contents.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetLengthOfEncodingDL(int tagNo, int contentsLength) => GetLengthOfIdentifier(tagNo) + GetLengthOfDL(contentsLength) + contentsLength;

	/// <summary>
	///   Gets the length of encoding il.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="contentsEncodings">The contents encodings.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetLengthOfEncodingIL(int tagNo, IAsn1Encoding[] contentsEncodings) => GetLengthOfIdentifier(tagNo) + 3 + GetLengthOfContents(contentsEncodings);

	/// <summary>
	///   Gets the length of identifier.
	/// </summary>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>System.Int32.</returns>
	internal static int GetLengthOfIdentifier(int tagNo)
	{
		if (tagNo < 31)
			return 1;

		var length = 2;
		while ((tagNo >>= 7) > 0) ++length;
		return length;
	}
}
