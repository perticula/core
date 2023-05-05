// perticula - core - Asn1GeneralizedTime.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1GeneralizedTime.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class Asn1GeneralizedTime : Asn1Object
{
	/// <summary>
	///   The date time
	/// </summary>
	private readonly DateTime _dateTime;

	/// <summary>
	///   The time string canonical
	/// </summary>
	private readonly bool _timeStringCanonical;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1GeneralizedTime" /> class.
	/// </summary>
	/// <param name="timeString">The time string.</param>
	/// <exception cref="System.ArgumentNullException">timeString</exception>
	/// <exception cref="System.ArgumentException">invalid date string: " + e.Message</exception>
	public Asn1GeneralizedTime(string timeString)
	{
		TimeString           = timeString ?? throw new ArgumentNullException(nameof(timeString));
		_timeStringCanonical = false;

		try
		{
			_dateTime = FromString(timeString);
		}
		catch (FormatException e)
		{
			throw new ArgumentException("invalid date string: " + e.Message);
		}
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1GeneralizedTime" /> class.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	public Asn1GeneralizedTime(DateTime dateTime)
	{
		dateTime = dateTime.ToUniversalTime();

		_dateTime            = dateTime;
		TimeString           = ToStringCanonical(dateTime);
		_timeStringCanonical = true;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1GeneralizedTime" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	internal Asn1GeneralizedTime(byte[] contents) : this(System.Text.Encoding.ASCII.GetString(contents)) { }

	/// <summary>
	///   Gets the time string.
	/// </summary>
	/// <value>The time string.</value>
	public string TimeString { get; }

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>Asn1GeneralizedTime.</returns>
	/// <exception cref="System.ArgumentNullException">obj</exception>
	/// <exception cref="System.ArgumentException">failed to construct generalized time from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
	public static Asn1GeneralizedTime GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:                                    throw new ArgumentNullException(nameof(obj));
			case Asn1GeneralizedTime asn1GeneralizedTime: return asn1GeneralizedTime;
			case IAsn1Convertable asn1Convertible:
			{
				var asn1Object = asn1Convertible.ToAsn1Object();
				if (asn1Object is Asn1GeneralizedTime converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1GeneralizedTime) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct generalized time from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1GeneralizedTime.</returns>
	public static Asn1GeneralizedTime GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1GeneralizedTime) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Converts to datetime.
	/// </summary>
	/// <returns>DateTime.</returns>
	public DateTime ToDateTime() => _dateTime;

	/// <summary>
	///   Gets the contents.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Byte[].</returns>
	internal byte[] GetContents(int encoding) => encoding == Asn1OutputStream.EncodingDer && !_timeStringCanonical
		                                             ? System.Text.Encoding.ASCII.GetBytes(ToStringCanonical(_dateTime))
		                                             : System.Text.Encoding.ASCII.GetBytes(TimeString);

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.GeneralizedTime, GetContents(encoding));

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
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.GeneralizedTime, GetContents(Asn1OutputStream.EncodingDer));

	/// <summary>
	///   Gets the encoding der implicit.
	/// </summary>
	/// <param name="tagClass">The tag class.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <returns>DerEncoding.</returns>
	internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo) => new(tagClass, tagNo, GetContents(Asn1OutputStream.EncodingDer));

	/// <summary>
	///   Asn1s the equals.
	/// </summary>
	/// <param name="asn1Object">The asn1 object.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	protected override bool Asn1Equals(Asn1Object asn1Object)
	{
		var time = asn1Object as Asn1GeneralizedTime;
		return time is not null && Arrays.AreEqual(GetContents(Asn1OutputStream.EncodingDer), time.GetContents(Asn1OutputStream.EncodingDer));
	}

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Arrays.GetHashCode(GetContents(Asn1OutputStream.EncodingDer));

	/// <summary>
	///   Creates the primitive.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>Asn1GeneralizedTime.</returns>
	internal static Asn1GeneralizedTime CreatePrimitive(byte[] contents) => new(contents);

	/// <summary>
	///   Froms the string.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <returns>DateTime.</returns>
	/// <exception cref="System.FormatException"></exception>
	private static DateTime FromString(string s)
	{
		if (s.Length < 10) throw new FormatException();

		s = s.Replace(',', '.');

		if (s.EndsWith("Z"))
			return s.Length switch
			       {
				       11 => ParseUtc(s, @"yyyyMMddHH\Z"),
				       13 => ParseUtc(s, @"yyyyMMddHHmm\Z"),
				       15 => ParseUtc(s, @"yyyyMMddHHmmss\Z"),
				       17 => ParseUtc(s, @"yyyyMMddHHmmss.f\Z"),
				       18 => ParseUtc(s, @"yyyyMMddHHmmss.ff\Z"),
				       19 => ParseUtc(s, @"yyyyMMddHHmmss.fff\Z"),
				       20 => ParseUtc(s, @"yyyyMMddHHmmss.ffff\Z"),
				       21 => ParseUtc(s, @"yyyyMMddHHmmss.fffff\Z"),
				       22 => ParseUtc(s, @"yyyyMMddHHmmss.ffffff\Z"),
				       23 => ParseUtc(s, @"yyyyMMddHHmmss.fffffff\Z"),
				       _  => throw new FormatException()
			       };

		var signIndex = IndexOfSign(s, System.Math.Max(10, s.Length - 5));

		if (signIndex < 0)
			return s.Length switch
			       {
				       10 => ParseLocal(s, @"yyyyMMddHH"),
				       12 => ParseLocal(s, @"yyyyMMddHHmm"),
				       14 => ParseLocal(s, @"yyyyMMddHHmmss"),
				       16 => ParseLocal(s, @"yyyyMMddHHmmss.f"),
				       17 => ParseLocal(s, @"yyyyMMddHHmmss.ff"),
				       18 => ParseLocal(s, @"yyyyMMddHHmmss.fff"),
				       19 => ParseLocal(s, @"yyyyMMddHHmmss.ffff"),
				       20 => ParseLocal(s, @"yyyyMMddHHmmss.fffff"),
				       21 => ParseLocal(s, @"yyyyMMddHHmmss.ffffff"),
				       22 => ParseLocal(s, @"yyyyMMddHHmmss.fffffff"),
				       _  => throw new FormatException()
			       };

		if (signIndex == s.Length - 5)
			return s.Length switch
			       {
				       15 => ParseTimeZone(s, @"yyyyMMddHHzzz"),
				       17 => ParseTimeZone(s, @"yyyyMMddHHmmzzz"),
				       19 => ParseTimeZone(s, @"yyyyMMddHHmmsszzz"),
				       21 => ParseTimeZone(s, @"yyyyMMddHHmmss.fzzz"),
				       22 => ParseTimeZone(s, @"yyyyMMddHHmmss.ffzzz"),
				       23 => ParseTimeZone(s, @"yyyyMMddHHmmss.fffzzz"),
				       24 => ParseTimeZone(s, @"yyyyMMddHHmmss.ffffzzz"),
				       25 => ParseTimeZone(s, @"yyyyMMddHHmmss.fffffzzz"),
				       26 => ParseTimeZone(s, @"yyyyMMddHHmmss.ffffffzzz"),
				       27 => ParseTimeZone(s, @"yyyyMMddHHmmss.fffffffzzz"),
				       _  => throw new FormatException()
			       };

		if (signIndex == s.Length - 3)
			return s.Length switch
			       {
				       13 => ParseTimeZone(s, @"yyyyMMddHHzz"),
				       15 => ParseTimeZone(s, @"yyyyMMddHHmmzz"),
				       17 => ParseTimeZone(s, @"yyyyMMddHHmmsszz"),
				       19 => ParseTimeZone(s, @"yyyyMMddHHmmss.fzz"),
				       20 => ParseTimeZone(s, @"yyyyMMddHHmmss.ffzz"),
				       21 => ParseTimeZone(s, @"yyyyMMddHHmmss.fffzz"),
				       22 => ParseTimeZone(s, @"yyyyMMddHHmmss.ffffzz"),
				       23 => ParseTimeZone(s, @"yyyyMMddHHmmss.fffffzz"),
				       24 => ParseTimeZone(s, @"yyyyMMddHHmmss.ffffffzz"),
				       25 => ParseTimeZone(s, @"yyyyMMddHHmmss.fffffffzz"),
				       _  => throw new FormatException()
			       };

		throw new FormatException();
	}

	/// <summary>
	///   Indexes the of sign.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="startIndex">The start index.</param>
	/// <returns>System.Int32.</returns>
	private static int IndexOfSign(string s, int startIndex)
	{
		var index            = s.IndexOf('+', startIndex);
		if (index < 0) index = s.IndexOf('-', startIndex);
		return index;
	}

	/// <summary>
	///   Parses the local.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="format">The format.</param>
	/// <returns>DateTime.</returns>
	private static DateTime ParseLocal(string s, string format) => DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeLocal);

	/// <summary>
	///   Parses the time zone.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="format">The format.</param>
	/// <returns>DateTime.</returns>
	private static DateTime ParseTimeZone(string s, string format) => DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);

	/// <summary>
	///   Parses the UTC.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="format">The format.</param>
	/// <returns>DateTime.</returns>
	private static DateTime ParseUtc(string s, string format) => DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

	/// <summary>
	///   Converts to stringcanonical.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>System.String.</returns>
	private static string ToStringCanonical(DateTime dateTime) => dateTime.ToUniversalTime().ToString(@"yyyyMMddHHmmss.FFFFFFFK", DateTimeFormatInfo.InvariantInfo);

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
		private Meta() : base(typeof(Asn1GeneralizedTime), Asn1Tags.GeneralizedTime) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreatePrimitive(octetString.GetOctets());
	}
}
