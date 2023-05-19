// perticula - core - DerIa5String.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerIa5String.
///   Implements the <see cref="core.Protocol.asn1.der.DerString" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.der.DerString" />
public class DerIa5String : DerString
{
	/// <summary>
	///   The contents
	/// </summary>
	private readonly byte[] _contents;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerIa5String" /> class.
	/// </summary>
	/// <param name="str">The string.</param>
	public DerIa5String(string str) : this(str, false) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerIa5String" /> class.
	/// </summary>
	/// <param name="str">The string.</param>
	/// <param name="validate">if set to <c>true</c> [validate].</param>
	/// <exception cref="System.ArgumentNullException">str</exception>
	/// <exception cref="System.ArgumentException">string contains illegal characters - str</exception>
	public DerIa5String(string str, bool validate)
	{
		if (str == null) throw new ArgumentNullException(nameof(str));
		if (validate && !IsIa5String(str)) throw new ArgumentException("string contains illegal characters", nameof(str));

		_contents = str.ToAsciiByteArray();
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerIa5String" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	public DerIa5String(byte[] contents) : this(contents, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerIa5String" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	/// <exception cref="System.ArgumentNullException">contents</exception>
	internal DerIa5String(byte[] contents, bool clone)
	{
		if (null == contents) throw new ArgumentNullException(nameof(contents));
		_contents = clone ? Arrays.Clone(contents) : contents;
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;DerIa5String&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct Ia5 string from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in {nameof(GetInstance)}: " + obj.GetTypeName()</exception>
	public static DerIa5String? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:
				return null;
			case DerIa5String derIa5String:
				return derIa5String;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerIa5String converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerIa5String) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct Ia5 string from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in {nameof(GetInstance)}: " + obj.GetTypeName());
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>DerIa5String.</returns>
	public static DerIa5String GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerIa5String) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Gets the current string value.
	/// </summary>
	/// <returns>System.String.</returns>
	public override string GetString() => _contents.FromAsciiByteArray();

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
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.Ia5String, _contents);

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
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.Ia5String, _contents);

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
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerIa5String that && Arrays.AreEqual(_contents, that._contents);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Arrays.GetHashCode(_contents);

	/// <summary>
	///   Determines whether [is ia5 string] [the specified string].
	/// </summary>
	/// <param name="str">The string.</param>
	/// <returns><c>true</c> if [is ia5 string] [the specified string]; otherwise, <c>false</c>.</returns>
	public static bool IsIa5String(string str) => str.All(ch => ch <= 0x007f);

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>DerIa5String.</returns>
	internal static DerIa5String CreatePrimitive(byte[] contents) => new(contents, false);

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
		private Meta() : base(typeof(DerIa5String), Asn1Tags.Ia5String) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
