// perticula - core - DerEnumerated.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Math;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerEnumerated.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class DerEnumerated : Asn1Object
{
	/// <summary>
	///   The cache
	/// </summary>
	private static readonly DerEnumerated?[] Cache = new DerEnumerated[12];

	/// <summary>
	///   The contents
	/// </summary>
	private readonly byte[] _contents;

	/// <summary>
	///   The start
	/// </summary>
	private readonly int _start;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerEnumerated" /> class.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <exception cref="System.ArgumentException">enumerated must be non-negative - val</exception>
	public DerEnumerated(int val)
	{
		if (val < 0) throw new ArgumentException("enumerated must be non-negative", nameof(val));

		_contents = BigInteger.ValueOf(val).ToByteArray();
		_start    = 0;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerEnumerated" /> class.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <exception cref="System.ArgumentException">enumerated must be non-negative - val</exception>
	public DerEnumerated(long val)
	{
		if (val < 0L) throw new ArgumentException("enumerated must be non-negative", nameof(val));

		_contents = BigInteger.ValueOf(val).ToByteArray();
		_start    = 0;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerEnumerated" /> class.
	/// </summary>
	/// <param name="val">The value.</param>
	/// <exception cref="System.ArgumentException">enumerated must be non-negative - val</exception>
	public DerEnumerated(BigInteger val)
	{
		if (val.SignValue < 0) throw new ArgumentException("enumerated must be non-negative", nameof(val));

		_contents = val.ToByteArray();
		_start    = 0;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerEnumerated" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	public DerEnumerated(byte[] contents) : this(contents, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerEnumerated" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	/// <exception cref="System.ArgumentException">malformed enumerated - contents</exception>
	/// <exception cref="System.ArgumentException">enumerated must be non-negative - contents</exception>
	internal DerEnumerated(byte[] contents, bool clone)
	{
		if (DerInteger.IsMalformed(contents)) throw new ArgumentException("malformed enumerated",     nameof(contents));
		if (0 != (contents[0] & 0x80)) throw new ArgumentException("enumerated must be non-negative", nameof(contents));

		_contents = clone ? Arrays.Clone(contents) : contents;
		_start    = DerInteger.SignBytesToSkip(_contents);
	}

	/// <summary>
	///   Gets the value.
	/// </summary>
	/// <value>The value.</value>
	public BigInteger Value => new(_contents);

	/// <summary>
	///   Gets the int value exact.
	/// </summary>
	/// <value>The int value exact.</value>
	/// <exception cref="System.ArithmeticException">ASN.1 Enumerated out of int range</exception>
	public int IntValueExact
	{
		get
		{
			var count = _contents.Length - _start;
			if (count > 4) throw new ArithmeticException("ASN.1 Enumerated out of int range");

			return DerInteger.IntValue(_contents, _start, DerInteger.SignExtSigned);
		}
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;DerEnumerated&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct enumerated from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in {nameof(GetInstance)}: " + obj.GetTypeName()</exception>
	public static DerEnumerated? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:
				return null;
			case DerEnumerated derEnumerated:
				return derEnumerated;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is DerEnumerated converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerEnumerated) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct enumerated from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in {nameof(GetInstance)}: " + obj.GetTypeName());
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>DerEnumerated.</returns>
	public static DerEnumerated GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (DerEnumerated) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Determines whether the specified x has value.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <returns><c>true</c> if the specified x has value; otherwise, <c>false</c>.</returns>
	public bool HasValue(int x) => _contents.Length - _start <= 4 && DerInteger.IntValue(_contents, _start, DerInteger.SignExtSigned) == x;

	/// <summary>
	///   Determines whether the specified x has value.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <returns><c>true</c> if the specified x has value; otherwise, <c>false</c>.</returns>
	public bool HasValue(BigInteger x) => DerInteger.IntValue(_contents, _start, DerInteger.SignExtSigned) == x.IntValue && Value.Equals(x); // Fast check to avoid allocation

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.Enumerated, _contents);

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
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.Enumerated, _contents);

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
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerEnumerated other && Arrays.AreEqual(_contents, other._contents);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Arrays.GetHashCode(_contents);

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	/// <returns>DerEnumerated.</returns>
	/// <exception cref="System.ArgumentException">ENUMERATED has zero length - contents</exception>
	internal static DerEnumerated CreatePrimitive(byte[] contents, bool clone)
	{
		switch (contents.Length)
		{
			case > 1:
				return new DerEnumerated(contents, clone);
			case 0:
				throw new ArgumentException("ENUMERATED has zero length", nameof(contents));
		}

		int value = contents[0];
		if (value >= Cache.Length)
			return new DerEnumerated(contents, clone);

		var possibleMatch = Cache[value];

		if (possibleMatch == null) Cache[value] = possibleMatch = new DerEnumerated(contents, clone);
		return possibleMatch;
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
		private Meta() : base(typeof(DerEnumerated), Asn1Tags.Enumerated) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets(), false);
	}
}
