// perticula - core - Asn1Null.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Null.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public abstract class Asn1Null : Asn1Object
{
	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Null" /> class.
	/// </summary>
	internal Asn1Null() { }

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;Asn1Null&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct NULL from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()}</exception>
	public static Asn1Null? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:              return null;
			case Asn1Null asn1Null: return asn1Null;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is Asn1Null converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1Null) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct NULL from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}");
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1Null.</returns>
	public static Asn1Null GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1Null) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => "NULL";

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>Asn1Null.</returns>
	/// <exception cref="System.InvalidOperationException">malformed NULL encoding encountered</exception>
	internal static Asn1Null CreatePrimitive(byte[] contents)
	{
		if (0 != contents.Length) throw new InvalidOperationException("malformed NULL encoding encountered");

		return DerNull.Instance;
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
		private Meta() : base(typeof(Asn1Null), Asn1Tags.Null) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
