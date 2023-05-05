// perticula - core - Asn1ObjectDescriptor.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1ObjectDescriptor.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class Asn1ObjectDescriptor : Asn1Object
{
	/// <summary>
	///   The base graphic string
	/// </summary>
	private readonly DerGraphicString _baseGraphicString;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1ObjectDescriptor" /> class.
	/// </summary>
	/// <param name="baseGraphicString">The base graphic string.</param>
	/// <exception cref="System.ArgumentNullException">baseGraphicString</exception>
	public Asn1ObjectDescriptor(DerGraphicString baseGraphicString)
	{
		if (null == baseGraphicString)
			throw new ArgumentNullException(nameof(baseGraphicString));

		_baseGraphicString = baseGraphicString;
	}

	/// <summary>
	///   Gets the base graphic string.
	/// </summary>
	/// <value>The base graphic string.</value>
	public DerGraphicString BaseGraphicString => _baseGraphicString;

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;Asn1ObjectDescriptor&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct object descriptor from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in {nameof(GetInstance)}: " + obj.GetTypeName() - obj</exception>
	public static Asn1ObjectDescriptor? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:                                      return null;
			case Asn1ObjectDescriptor asn1ObjectDescriptor: return asn1ObjectDescriptor;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is Asn1ObjectDescriptor converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1ObjectDescriptor) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct object descriptor from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in {nameof(GetInstance)}: " + obj.GetTypeName(), nameof(obj));
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1ObjectDescriptor.</returns>
	public static Asn1ObjectDescriptor GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1ObjectDescriptor) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => _baseGraphicString.GetEncodingImplicit(encoding, Asn1Tags.Universal, Asn1Tags.ObjectDescriptor);

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => _baseGraphicString.GetEncodingImplicit(encoding, tagClass, tagNo);

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => _baseGraphicString.GetEncodingDerImplicit(Asn1Tags.Universal, Asn1Tags.ObjectDescriptor);

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => _baseGraphicString.GetEncodingDerImplicit(tagClass, tagNo);

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => ~_baseGraphicString.CallAsn1GetHashCode();

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is Asn1ObjectDescriptor that && _baseGraphicString.Equals(that._baseGraphicString);

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>Asn1ObjectDescriptor.</returns>
	internal static Asn1ObjectDescriptor CreatePrimitive(byte[] contents) => new(DerGraphicString.CreatePrimitive(contents));

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
		private Meta() : base(typeof(Asn1ObjectDescriptor), Asn1Tags.ObjectDescriptor) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => new Asn1ObjectDescriptor((DerGraphicString) DerGraphicString.Meta.Instance.FromImplicitPrimitive(octetString));

		/// <summary>
		///   Froms the implicit constructed.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => new Asn1ObjectDescriptor((DerGraphicString) DerGraphicString.Meta.Instance.FromImplicitConstructed(sequence));
	}
}
