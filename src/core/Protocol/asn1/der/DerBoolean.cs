// perticula - core - DerBoolean.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerBoolean.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class DerBoolean : Asn1Object
{
	/// <summary>
	///   The false
	/// </summary>
	public static readonly DerBoolean False = new(false);

	/// <summary>
	///   The true
	/// </summary>
	public static readonly DerBoolean True = new(true);

	/// <summary>
	///   The value
	/// </summary>
	private readonly byte _value;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBoolean" /> class.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <exception cref="System.ArgumentException">byte value should have 1 byte in it - val</exception>
	public DerBoolean(byte[] val)
	{
		if (val.Length != 1)
			throw new ArgumentException("byte value should have 1 byte in it", nameof(val));

		// TODO Are there any constraints on the possible byte values?
		_value = val[0];
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerBoolean" /> class.
	/// </summary>
	/// <param name="value">if set to <c>true</c> [value].</param>
	private DerBoolean(
		bool value) =>
		_value = value ? (byte) 0xff : (byte) 0;

	/// <summary>
	///   Gets a value indicating whether this instance is true.
	/// </summary>
	/// <value><c>true</c> if this instance is true; otherwise, <c>false</c>.</value>
	public bool IsTrue => _value != 0;

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="value">if set to <c>true</c> [value].</param>
	/// <returns>DerBoolean.</returns>
	public static DerBoolean GetInstance(bool value) => value ? True : False;

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>DerBoolean.</returns>
	public static DerBoolean GetInstance(int value) => value != 0 ? True : False;

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>DerBoolean.</returns>
	public static DerBoolean GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerBoolean) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>DerBoolean.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">failed to construct boolean from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in {nameof(GetInstance)}: " + obj.GetTypeName()</exception>
	public static DerBoolean GetInstance(object obj)
	{
		switch (obj)
		{
			case null:                  throw new ArgumentNullException(nameof(obj));
			case DerBoolean derBoolean: return derBoolean;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerBoolean converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerBoolean) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct boolean from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in {nameof(GetInstance)}: " + obj.GetTypeName());
	}

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.Boolean, GetContents(encoding));

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, GetContents(encoding));

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.Boolean, GetContents(Asn1OutputStream.EncodingDer));

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, GetContents(Asn1OutputStream.EncodingDer));

	/// <summary>
	///   Gets the contents.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Byte[].</returns>
	private byte[] GetContents(int encoding)
	{
		var contents = _value;
		if (Asn1OutputStream.EncodingDer == encoding && IsTrue)
			contents = 0xFF;

		return new[] {contents};
	}

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerBoolean other && IsTrue == other.IsTrue;

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => IsTrue.GetHashCode();

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => IsTrue ? "TRUE" : "FALSE";

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>DerBoolean.</returns>
	/// <exception cref="System.ArgumentException">BOOLEAN value should have 1 byte in it - contents</exception>
	internal static DerBoolean CreatePrimitive(byte[] contents)
	{
		if (contents.Length != 1) throw new ArgumentException("BOOLEAN value should have 1 byte in it", nameof(contents));

		var b = contents[0];

		return b switch
		       {
			       0    => False,
			       0xFF => True,
			       _    => new DerBoolean(contents)
		       };
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
		private Meta() : base(typeof(DerBoolean), Asn1Tags.Boolean) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
