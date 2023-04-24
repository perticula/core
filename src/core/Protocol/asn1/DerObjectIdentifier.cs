// perticula - core - DerObjectIdentifier.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text;
using core.Math;

namespace core.Protocol.asn1;

public class DerObjectIdentifier : Asn1Object
{
	private const long LongLimit = (long.MaxValue >> 7) - 0x7F;

	private static readonly DerObjectIdentifier[] Cache = new DerObjectIdentifier[1024];

	private byte[]? _contents;

	public DerObjectIdentifier(string identifier)
	{
		if (identifier == null)
			throw new ArgumentNullException(nameof(identifier));
		if (!IsValidIdentifier(identifier))
			throw new FormatException($"string {identifier} not an OID");

		Id = identifier;
	}

	private DerObjectIdentifier(DerObjectIdentifier oid, string branchId)
	{
		if (!Asn1RelativeOid.IsValidIdentifier(branchId, 0))
			throw new ArgumentException($"string {branchId} not a valid OID branch", nameof(branchId));

		Id = $"{oid.Id}.{branchId}";
	}

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

	/**
	 * return an OID from the passed in object
	 * 
	 * @exception ArgumentException if the object cannot be converted.
	 */
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

	public override string ToString() => Id;

	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is DerObjectIdentifier that && Id == that.Id;

	internal override        IAsn1Encoding GetEncoding(int encoding) => new PrimitiveEncoding(Asn1Tags.Universal, Asn1Tags.ObjectIdentifier, GetContents());
	internal sealed override DerEncoding   GetEncodingDer()          => new PrimitiveDerEncoding(Asn1Tags.Universal, Asn1Tags.ObjectIdentifier, GetContents());

	internal override        IAsn1Encoding GetEncodingImplicit(int    encoding, int tagClass, int tagNo) => new PrimitiveEncoding(tagClass, tagNo, GetContents());
	internal sealed override DerEncoding   GetEncodingDerImplicit(int tagClass, int tagNo) => new PrimitiveDerEncoding(tagClass, tagNo, GetContents());

	protected override int Asn1GetHashCode() => Id.GetHashCode();

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

	private static bool IsValidIdentifier(string identifier)
	{
		if (identifier.Length < 3 || identifier[1] != '.')
			return false;

		var first = identifier[0];
		return first is >= '0' and <= '2' && Asn1RelativeOid.IsValidIdentifier(identifier, 2);
	}

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

	internal class Meta : Asn1UniversalType
	{
		internal static readonly Asn1UniversalType Instance = new Meta();

		private Meta() : base(typeof(DerObjectIdentifier), Asn1Tags.ObjectIdentifier) { }

		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets(), false);
	}
}
