// perticula - core - DerBmpString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Buffers;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerBmpString.
///   Implements the <see cref="core.Protocol.asn1.der.DerString" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.der.DerString" />
public class DerBmpString : DerString
{
	/// <summary>
	///   The string
	/// </summary>
	private readonly string _str;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBmpString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <exception cref="System.ArgumentNullException">contents</exception>
	/// <exception cref="System.ArgumentException">malformed BMPString encoding encountered - contents</exception>
	internal DerBmpString(byte[] contents)
	{
		if (contents == null) throw new ArgumentNullException(nameof(contents));

		var byteLen = contents.Length;
		if (0 != (byteLen & 1))
			throw new ArgumentException("malformed BMPString encoding encountered", nameof(contents));

		var charLen = byteLen / 2;

		_str = string.Create(charLen, contents, (chars, bytes) =>
		{
			for (var i = 0; i < chars.Length; ++i) chars[i] = (char) ((bytes[2 * i] << 8) | (bytes[2 * i + 1] & 0xff));
		});
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBmpString" /> class.
	/// </summary>
	/// <param name="str">The string.</param>
	/// <exception cref="System.ArgumentNullException">str</exception>
	internal DerBmpString(char[] str)
	{
		if (str == null) throw new ArgumentNullException(nameof(str));

		_str = new string(str);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBmpString" /> class.
	/// </summary>
	/// <param name="str">The string.</param>
	/// <exception cref="System.ArgumentNullException">str</exception>
	public DerBmpString(string str) => _str = str ?? throw new ArgumentNullException(nameof(str));

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>DerBmpString.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">failed to construct BMP string from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()}</exception>
	public static DerBmpString GetInstance(object obj)
	{
		switch (obj)
		{
			case null:
				throw new ArgumentNullException(nameof(obj));
			case DerBmpString derBmpString:
				return derBmpString;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerBmpString converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerBmpString) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct BMP string from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}");
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>DerBmpString.</returns>
	public static DerBmpString GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerBmpString) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Gets the current string value.
	/// </summary>
	/// <returns>System.String.</returns>
	public override string GetString() => _str;

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerBmpString that && _str.Equals(that._str);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => _str.GetHashCode();

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.BmpString, GetContents());

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, GetContents());

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.BmpString, GetContents());

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, GetContents());

	/// <summary>
	///   Gets the contents.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	private byte[] GetContents()
	{
		var c = _str.ToCharArray();
		var b = new byte[c.Length * 2];

		for (var i = 0; i != c.Length; i++)
		{
			b[2 * i]     = (byte) (c[i] >> 8);
			b[2 * i + 1] = (byte) c[i];
		}

		return b;
	}

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>DerBmpString.</returns>
	internal static DerBmpString CreatePrimitive(byte[] contents) => new(contents);

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <typeparam name="TState">The type of the t state.</typeparam>
	/// <param name="length">The length.</param>
	/// <param name="state">The state.</param>
	/// <param name="action">The action.</param>
	/// <returns>DerBmpString.</returns>
	internal static DerBmpString CreatePrimitive<TState>(int length, TState state, SpanAction<char, TState> action) => new(string.Create(length, state, action));

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
		private Meta() : base(typeof(DerBmpString), Asn1Tags.BmpString) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
