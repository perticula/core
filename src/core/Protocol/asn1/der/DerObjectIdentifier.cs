// perticula - core - DerObjectIdentifier.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text;
using core.Math;

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerObjectIdentifier.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class DerObjectIdentifier : Asn1Object
{
	/// <summary>
	///   The long limit
	/// </summary>
	private const long LongLimit = (long.MaxValue >> 7) - 0x7F;

	/// <summary>
	///   The cache
	/// </summary>
	private static readonly DerObjectIdentifier[] Cache = new DerObjectIdentifier[1024];

	/// <summary>
	///   The contents
	/// </summary>
	private byte[]? _contents;

	/// <summary>
	///   Initializes a new instance of the <see cref="DerObjectIdentifier" /> class.
	/// </summary>
	/// <param name="identifier">The identifier.</param>
	/// <exception cref="ArgumentNullException">nameof(identifier)</exception>
	/// <exception cref="FormatException">$"string {identifier} not an OID</exception>
	public DerObjectIdentifier(string identifier)
	{
		if (identifier == null) throw new ArgumentNullException(nameof(identifier));
		if (!IsValidIdentifier(identifier)) throw new FormatException($"string {identifier} not an OID");

		Id = identifier;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerObjectIdentifier" /> class.
	/// </summary>
	/// <param name="oid">The oid.</param>
	/// <param name="branchId">The branch identifier.</param>
	/// <exception cref="ArgumentException">$"string {branchId} not a valid OID branch, nameof(branchId)</exception>
	private DerObjectIdentifier(DerObjectIdentifier oid, string branchId)
	{
		if (!Asn1RelativeOid.IsValidIdentifier(branchId, 0)) throw new ArgumentException($"string {branchId} not a valid OID branch", nameof(branchId));

		Id = $"{oid.Id}.{branchId}";
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="DerObjectIdentifier" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	private DerObjectIdentifier(byte[] contents, bool clone)
	{
		Id        = ParseContents(contents);
		_contents = clone ? Arrays.Clone(contents) : contents;
	}

	/// <summary>
	///   Gets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public string Id { get; }

	/// <summary>
	///   return an OID from the passed in object
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;DerObjectIdentifier&gt;.</returns>
	/// <exception cref="ArgumentException">$"failed to construct object identifier from byte[]: {e.Message}</exception>
	/// <exception cref="ArgumentException">$"illegal object in GetInstance: {obj.GetTypeName()}, nameof(obj)</exception>
	/// <exception cref="System.ArgumentException">failed to construct object identifier from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
	public static DerObjectIdentifier? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:                                    return null;
			case DerObjectIdentifier derObjectIdentifier: return derObjectIdentifier;
			case IAsn1Convertable asn1Convertible:
			{
				var asn1Object = asn1Convertible.ToAsn1Object();
				if (asn1Object is DerObjectIdentifier converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (DerObjectIdentifier) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct object identifier from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	/// <summary>
	///   Branches the specified branch identifier.
	/// </summary>
	/// <param name="branchId">The branch identifier.</param>
	/// <returns>core.Protocol.asn1.der.DerObjectIdentifier.</returns>
	public virtual DerObjectIdentifier Branch(string branchId) => new(this, branchId);

	/// <summary>
	///   Return  true if this oid is an extension of the passed in branch (stem).
	///   the arc or branch that is a possible parent.
	/// </summary>
	/// <param name="stem">The stem.</param>
	/// <returns><c>true</c> if the branch is on the passed in stem, <c>false</c> otherwise.</returns>
	public virtual bool On(DerObjectIdentifier stem)
	{
		var stemId = stem.Id;
		return Id.Length > stemId.Length && Id[stemId.Length] == '.' && Id.StartsWith(stemId);
	}

	/// <summary>
	///   Converts to string.
	/// </summary>
	/// <returns>string.</returns>
	public override string ToString() => Id;

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns>bool.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerObjectIdentifier that && Id == that.Id;

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>core.Protocol.asn1.IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.ObjectIdentifier, GetContents());

	/// <summary>
	///   Gets the encoding der.
	/// </summary>
	/// <returns>core.Protocol.asn1.der.DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDer() => new Asn1DerEncoding(Asn1Tags.Universal, Asn1Tags.ObjectIdentifier, GetContents());

	/// <summary>
	///   Gets the encoding implicit.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new Asn1Encoding(tagClass, tagNo, GetContents());

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>core.Protocol.asn1.der.DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new Asn1DerEncoding(tagClass, tagNo, GetContents());

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>int.</returns>
	protected override int Asn1GetHashCode() => Id.GetHashCode();

	/// <summary>
	///   Gets the contents.
	/// </summary>
	/// <returns>byte[].</returns>
	private byte[] GetContents()
	{
		lock (this)
		{
			switch (_contents)
			{
				case null:
				{
					var bOut = new MemoryStream();
					DoOutput(bOut);
					_contents = bOut.ToArray();
					break;
				}
			}
		}

		return _contents;
	}

	/// <summary>
	///   Does the output.
	/// </summary>
	/// <param name="bOut">The b out.</param>
	private void DoOutput(Stream bOut)
	{
		var tok = new OidTokenizer(Id);

		var token = tok.NextToken();
		if (token is not null)
		{
			var first = int.Parse(token) * 40;

			token = tok.NextToken();
			switch (token?.Length)
			{
				case <= 18:
					Asn1RelativeOid.WriteField(bOut, first + long.Parse(token));
					break;
				default:
					Asn1RelativeOid.WriteField(bOut, new BigInteger(token).Add(BigInteger.ValueOf(first)));
					break;
			}
		}

		while (tok.HasMoreTokens)
		{
			token = tok.NextToken();
			switch (token?.Length)
			{
				case <= 18:
					Asn1RelativeOid.WriteField(bOut, long.Parse(token));
					break;
				default:
					Asn1RelativeOid.WriteField(bOut, new BigInteger(token));
					break;
			}
		}
	}

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">The clone.</param>
	/// <returns>core.Protocol.asn1.der.DerObjectIdentifier.</returns>
	internal static DerObjectIdentifier CreatePrimitive(byte[] contents, bool clone)
	{
		var index = Arrays.GetHashCode(contents);

		index ^= index >> 20;
		index ^= index >> 10;
		index &= 1023;

		var originalEntry = Volatile.Read(ref Cache[index]);
		if (Arrays.AreEqual(contents, originalEntry.GetContents()))
			return originalEntry;

		var newEntry = new DerObjectIdentifier(contents, clone);

		var exchangedEntry = Interlocked.CompareExchange(ref Cache[index], newEntry, originalEntry);
		return ((Asn1Encodable) exchangedEntry).Equals(originalEntry) switch
		       {
			       false when Arrays.AreEqual(contents, exchangedEntry.GetContents()) => exchangedEntry,
			       _                                                                  => newEntry
		       };
	}

	/// <summary>
	///   Determines whether [is valid identifier] [the specified identifier].
	/// </summary>
	/// <param name="identifier">The identifier.</param>
	/// <returns>bool.</returns>
	private static bool IsValidIdentifier(string identifier)
	{
		if (identifier.Length < 3 || identifier[1] != '.')
			return false;

		var first = identifier[0];
		return first is >= '0' and <= '2' && Asn1RelativeOid.IsValidIdentifier(identifier, 2);
	}

	/// <summary>
	///   Parses the contents.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>string.</returns>
	private static string ParseContents(IReadOnlyList<byte> contents)
	{
		var         objId    = new StringBuilder();
		long        value    = 0;
		BigInteger? bigValue = null;
		var         first    = true;

		for (var i = 0; i != contents.Count; i++)
		{
			int b = contents[i];

			if (value <= LongLimit)
			{
				value += b & 0x7F;
				switch (b & 0x80)
				{
					case 0:
					{
						if (first)
						{
							switch (value)
							{
								case < 40:
									objId.Append('0');
									break;
								case < 80:
									objId.Append('1');
									value -= 40;
									break;
								default:
									objId.Append('2');
									value -= 80;
									break;
							}

							first = false;
						}

						objId.Append('.');
						objId.Append(value);
						value = 0;
						break;
					}
					default:
						value <<= 7;
						break;
				}
			}
			else
			{
				bigValue ??= BigInteger.ValueOf(value);
				bigValue =   bigValue.Or(BigInteger.ValueOf(b & 0x7F));
				if ((b & 0x80) == 0)
				{
					if (first)
					{
						objId.Append('2');
						bigValue = bigValue.Subtract(BigInteger.ValueOf(80));
						first    = false;
					}

					objId.Append('.');
					objId.Append(bigValue);
					bigValue = null;
					value    = 0;
				}
				else
				{
					bigValue = bigValue.ShiftLeft(7);
				}
			}
		}

		return objId.ToString();
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
		private Meta() : base(typeof(DerObjectIdentifier), Asn1Tags.ObjectIdentifier) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>core.Protocol.asn1.Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets(), false);
	}
}
