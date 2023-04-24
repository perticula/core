// perticula - core - Asn1RelativeOid.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text;
using core.Math;

namespace core.Protocol.asn1;

public class Asn1RelativeOid : Asn1Object
{
	private const long LongLimit = (long.MaxValue >> 7) - 0x7F;

	private byte[]? _contents;

	public Asn1RelativeOid(string identifier)
	{
		if (identifier == null)
			throw new ArgumentNullException(nameof(identifier));
		if (!IsValidIdentifier(identifier, 0))
			throw new FormatException("string " + identifier + " not a relative OID");

		Id = identifier;
	}

	private Asn1RelativeOid(Asn1RelativeOid oid, string branchId)
	{
		if (!IsValidIdentifier(branchId, 0))
			throw new FormatException($"string {branchId} not a valid relative OID branch");

		Id = oid.Id + "." + branchId;
	}

	private Asn1RelativeOid(byte[] contents, bool clone)
	{
		Id        = ParseContents(contents);
		_contents = clone ? Arrays.Clone(contents) : contents;
	}

	public string Id { get; }

	public static Asn1RelativeOid FromContents(byte[] contents) => CreatePrimitive(contents, true);

	public static Asn1RelativeOid GetInstance(object obj)
	{
		switch (obj)
		{
			case Asn1RelativeOid asn1RelativeOid:
				return asn1RelativeOid;
			case IAsn1Convertable asn1Convertible:
			{
				var asn1Object = asn1Convertible.ToAsn1Object();
				if (asn1Object is Asn1RelativeOid converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1RelativeOid) FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct relative OID from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	public static Asn1RelativeOid GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1RelativeOid) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	public virtual Asn1RelativeOid Branch(string branchId) => new(this, branchId);

	public override string ToString() => Id;

	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is Asn1RelativeOid that && Id == that.Id;

	protected override int Asn1GetHashCode() => Id.GetHashCode();

	internal override IAsn1Encoding GetEncoding(int encoding) => new PrimitiveEncoding(Asn1Tags.Universal, Asn1Tags.RelativeOid, GetContents());

	internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo) => new PrimitiveEncoding(tagClass, tagNo, GetContents());

	internal sealed override DerEncoding GetEncodingDer() => new PrimitiveDerEncoding(Asn1Tags.Universal, Asn1Tags.RelativeOid, GetContents());

	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new PrimitiveDerEncoding(tagClass, tagNo, GetContents());

	private void DoOutput(Stream bOut)
	{
		var tok = new OidTokenizer(Id);
		while (tok.HasMoreTokens)
		{
			var token = tok.NextToken();
			if (token?.Length <= 18)
				WriteField(bOut, long.Parse(token));
			else
				WriteField(bOut, new BigInteger(token));
		}
	}

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

			return _contents;
		}
	}

	internal static Asn1RelativeOid CreatePrimitive(byte[] contents, bool clone) => new(contents, clone);

	internal static bool IsValidIdentifier(string identifier, int from)
	{
		var digitCount = 0;

		var pos = identifier.Length;
		while (--pos >= from)
		{
			var ch = identifier[pos];

			switch (ch)
			{
				case '.' when 0 == digitCount || (digitCount > 1 && identifier[pos + 1] == '0'):
					return false;
				case '.':
					digitCount = 0;
					break;
				case >= '0' and <= '9':
					++digitCount;
					break;
				default:
					return false;
			}
		}

		return 0 != digitCount && (digitCount <= 1 || identifier[pos + 1] != '0');
	}

	internal static void WriteField(Stream outputStream, long fieldValue)
	{
		Span<byte> result = stackalloc byte[9];
		var        pos    = 8;
		result[pos] = (byte) ((int) fieldValue & 0x7F);
		while (fieldValue >= 1L << 7)
		{
			fieldValue    >>= 7;
			result[--pos] =   (byte) ((int) fieldValue | 0x80);
		}

		outputStream.Write(result[pos..]);
	}

	internal static void WriteField(Stream outputStream, BigInteger fieldValue)
	{
		var byteCount = (fieldValue.BitLength + 6) / 7;
		if (byteCount == 0)
		{
			outputStream.WriteByte(0);
		}
		else
		{
			var tmpValue = fieldValue;
			var tmp = byteCount <= 16
				          ? stackalloc byte[byteCount]
				          : new byte[byteCount];
			for (var i = byteCount - 1; i >= 0; i--)
			{
				tmp[i]   = (byte) (tmpValue.IntValue | 0x80);
				tmpValue = tmpValue.ShiftRight(7);
			}

			tmp[byteCount - 1] &= 0x7F;
			outputStream.Write(tmp);
		}
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
				if ((b & 0x80) == 0)
				{
					if (first)
						first = false;
					else
						objId.Append('.');

					objId.Append(value);
					value = 0;
				}
				else
				{
					value <<= 7;
				}
			}
			else
			{
				bigValue ??= BigInteger.ValueOf(value);
				bigValue =   bigValue.Or(BigInteger.ValueOf(b & 0x7F));
				if ((b & 0x80) == 0)
				{
					if (first)
						first = false;
					else
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

		private Meta() : base(typeof(Asn1RelativeOid), Asn1Tags.RelativeOid) { }

		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets(), false);
	}
}
