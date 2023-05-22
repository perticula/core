// perticula - core - DerUniversalString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;
using System.Text;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerUniversalString.
///   Implements the <see cref="core.Protocol.asn1.der.DerString" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.der.DerString" />
public class DerUniversalString : DerString
{
	/// <summary>
	///   The hexadecimal character table
	/// </summary>
	private static readonly char[] HexCharTable = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

	/// <summary>
	///   The contents
	/// </summary>
	private readonly byte[] _contents;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerUniversalString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	public DerUniversalString(byte[] contents) : this(contents, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerUniversalString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	/// <exception cref="System.ArgumentNullException">contents</exception>
	internal DerUniversalString(byte[] contents, bool clone)
	{
		if (null == contents) throw new ArgumentNullException(nameof(contents));

		_contents = clone ? Arrays.Clone(contents) : contents;
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;DerUniversalString&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct universal string from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()}</exception>
	public static DerUniversalString? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:
				return null;
			case DerUniversalString derUniversalString:
				return derUniversalString;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerUniversalString converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerUniversalString) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct universal string from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}");
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>DerUniversalString.</returns>
	public static DerUniversalString GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerUniversalString) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Gets the current string value.
	/// </summary>
	/// <returns>System.String.</returns>
	public override string GetString()
	{
		var dl       = _contents.Length;
		var capacity = 3 + 2 * (Asn1OutputStream.GetLengthOfDefiniteLength(dl) + dl);
		var buf      = new StringBuilder("#1C", capacity);
		EncodeHexDL(buf, dl);

		for (var i = 0; i < dl; ++i) EncodeHexByte(buf, _contents[i]);

		Debug.Assert(buf.Length == capacity);
		return buf.ToString();
	}

	/// <summary>
	///   Gets the octets.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	public byte[] GetOctets() => Arrays.Clone(_contents);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.UniversalString, _contents);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, _contents);

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.UniversalString, _contents);

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, _contents);

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerUniversalString that && Arrays.AreEqual(_contents, that._contents);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Arrays.GetHashCode(_contents);

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>DerUniversalString.</returns>
	internal static DerUniversalString CreatePrimitive(byte[] contents) => new(contents, false);

	/// <summary>
	///   Encodes the hexadecimal byte.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <param name="i">The i.</param>
	private static void EncodeHexByte(StringBuilder buf, int i)
	{
		buf.Append(HexCharTable[(i >> 4) & 0xF]);
		buf.Append(HexCharTable[i        & 0xF]);
	}

	/// <summary>
	///   Encodes the hexadecimal dl.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <param name="dl">The dl.</param>
	private static void EncodeHexDL(StringBuilder buf, int dl)
	{
		if (dl < 128)
		{
			EncodeHexByte(buf, dl);
			return;
		}

		var stack = new byte[5];
		var pos   = 5;

		do
		{
			stack[--pos] =   (byte) dl;
			dl           >>= 8;
		} while (dl != 0);

		var count = stack.Length - pos;
		stack[--pos] = (byte) (0x80 | count);

		do
			EncodeHexByte(buf, stack[pos++]);
		while (pos < stack.Length);
	}


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
		private Meta() : base(typeof(DerUniversalString), Asn1Tags.UniversalString) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
