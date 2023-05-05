// perticula - core - Asn1OctetString.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1OctetString.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
///   Implements the <see cref="core.Protocol.asn1.IAsn1OctetStringParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
/// <seealso cref="core.Protocol.asn1.IAsn1OctetStringParser" />
public abstract class Asn1OctetString : Asn1Object, IAsn1OctetStringParser
{
	/// <summary>
	///   The empty octets
	/// </summary>
	internal static readonly byte[] EmptyOctets = Array.Empty<byte>();

	/// <summary>
	///   The contents
	/// </summary>
	internal readonly byte[] Contents;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1OctetString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <exception cref="System.ArgumentNullException">contents</exception>
	internal Asn1OctetString(byte[] contents) => Contents = contents ?? throw new ArgumentNullException(nameof(contents));

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1OctetString" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	internal Asn1OctetString(ReadOnlySpan<byte> contents) => Contents = contents.ToArray();

	/// <summary>
	///   Gets the parser.
	/// </summary>
	/// <value>The parser.</value>
	public IAsn1OctetStringParser Parser => this;

	/// <summary>
	///   Gets the current content of the octet string as a stream.
	/// </summary>
	/// <returns>Stream containing the current OCTET.</returns>
	public Stream GetOctetStream() => new MemoryStream(Contents, false);

	/// <summary>
	///   Gets the octets memory.
	/// </summary>
	/// <returns>ReadOnlyMemory&lt;System.Byte&gt;.</returns>
	internal ReadOnlyMemory<byte> GetOctetsMemory() => Contents.AsMemory();

	/// <summary>
	///   Gets the octets span.
	/// </summary>
	/// <returns>ReadOnlySpan&lt;System.Byte&gt;.</returns>
	internal ReadOnlySpan<byte> GetOctetsSpan() => Contents.AsSpan();

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1OctetString.</returns>
	public static Asn1OctetString GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1OctetString) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Gets the octets.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	public virtual byte[] GetOctets() => Contents;

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Arrays.GetHashCode(GetOctets());

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerOctetString other && Arrays.AreEqual(GetOctets(), other.GetOctets());

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => $"#{Hex.ToHexString(Contents)}";

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>Asn1OctetString.</returns>
	internal static Asn1OctetString CreatePrimitive(byte[] contents) => new DerOctetString(contents);

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;Asn1OctetString&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct OCTET STRING from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
	public static Asn1OctetString GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:                            throw new ArgumentNullException(nameof(obj));
			case Asn1OctetString asn1OctetString: return asn1OctetString;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is Asn1OctetString converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1OctetString) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct OCTET STRING from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
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
		private Meta() : base(typeof(Asn1OctetString), Asn1Tags.OctetString) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => octetString;

		/// <summary>
		///   Froms the implicit constructed.
		/// </summary>
		/// <param name="sequence">The sequence.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence) => sequence.ToAsn1OctetString();
	}
}
