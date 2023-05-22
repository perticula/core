// perticula - core - Asn1RelativeOid.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text;
using core.Math;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1RelativeOid.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class Asn1RelativeOid : Asn1Object
{
	/// <summary>
	///   The long limit
	/// </summary>
	private const long LongLimit = (long.MaxValue >> 7) - 0x7F;

	/// <summary>
	///   The contents
	/// </summary>
	private byte[]? _contents;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1RelativeOid" /> class.
	/// </summary>
	/// <param name="identifier">The identifier.</param>
	/// <exception cref="System.ArgumentNullException">identifier</exception>
	/// <exception cref="System.FormatException">string {identifier} not a relative OID</exception>
	public Asn1RelativeOid(string identifier)
	{
		if (identifier == null) throw new ArgumentNullException(nameof(identifier));
		if (!IsValidIdentifier(identifier, 0)) throw new FormatException($"string {identifier} not a relative OID");

		Id = identifier;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1RelativeOid" /> class.
	/// </summary>
	/// <param name="oid">The oid.</param>
	/// <param name="branchId">The branch identifier.</param>
	/// <exception cref="System.FormatException">string {branchId} not a valid relative" + $" OID branch</exception>
	private Asn1RelativeOid(Asn1RelativeOid oid, string branchId)
	{
		if (!IsValidIdentifier(branchId, 0)) throw new FormatException($"string {branchId} not a valid relative" + $" OID branch");

		Id = oid.Id + "." + branchId;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1RelativeOid" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	private Asn1RelativeOid(byte[] contents, bool clone)
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
	///   Froms the contents.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>Asn1RelativeOid.</returns>
	public static Asn1RelativeOid FromContents(byte[] contents) => CreatePrimitive(contents, true);

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>Asn1RelativeOid.</returns>
	/// <exception cref="System.ArgumentException">failed to construct relative OID from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
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
					return (Asn1RelativeOid) FromByteArray(bytes)!;
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct relative OID from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1RelativeOid.</returns>
	public static Asn1RelativeOid GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1RelativeOid) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Branches the specified branch identifier.
	/// </summary>
	/// <param name="branchId">The branch identifier.</param>
	/// <returns>Asn1RelativeOid.</returns>
	public virtual Asn1RelativeOid Branch(string branchId) => new(this, branchId);

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => Id;

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object) => asn1Object is Asn1RelativeOid that && Id == that.Id;

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Id.GetHashCode();

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.RelativeOid, GetContents());

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
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.RelativeOid, GetContents());

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, GetContents());

	/// <summary>
	///   Does the output.
	/// </summary>
	/// <param name="bOut">The b out.</param>
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

	/// <summary>
	///   Gets the contents.
	/// </summary>
	/// <returns>System.Byte[].</returns>
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

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <param name="clone">if set to <c>true</c> [clone].</param>
	/// <returns>Asn1RelativeOid.</returns>
	internal static Asn1RelativeOid CreatePrimitive(byte[] contents, bool clone) => new(contents, clone);

	/// <summary>
	///   Determines whether [is valid identifier] [the specified identifier].
	/// </summary>
	/// <param name="identifier">The identifier.</param>
	/// <param name="from">From.</param>
	/// <returns><c>true</c> if [is valid identifier] [the specified identifier]; otherwise, <c>false</c>.</returns>
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

	/// <summary>
	///   Writes the field.
	/// </summary>
	/// <param name="outputStream">The output stream.</param>
	/// <param name="fieldValue">The field value.</param>
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

	/// <summary>
	///   Writes the field.
	/// </summary>
	/// <param name="outputStream">The output stream.</param>
	/// <param name="fieldValue">The field value.</param>
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

	/// <summary>
	///   Parses the contents.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>System.String.</returns>
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
		private Meta() : base(typeof(Asn1RelativeOid), Asn1Tags.RelativeOid) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets(), false);
	}
}
