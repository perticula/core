// perticula - core - Asn1UtcTime.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1UtcTime.
///   Implements the <see cref="core.Protocol.asn1.Asn1Object" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1Object" />
public class Asn1UtcTime : Asn1Object
{
	/// <summary>
	///   The default two digit year cutoff
	/// </summary>
	private const int DefaultTwoDigitYearCutoff = 2049;

	/// <summary>
	///   The date time
	/// </summary>
	private readonly DateTime _dateTime;

	/// <summary>
	///   The date time locked
	/// </summary>
	private readonly bool _dateTimeLocked;

	/// <summary>
	///   The two digit year maximum
	/// </summary>
	private readonly int _twoDigitYearMax;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1UtcTime" /> class.
	/// </summary>
	/// <param name="timeString">The time string.</param>
	/// <exception cref="System.ArgumentNullException">timeString</exception>
	/// <exception cref="System.ArgumentException">invalid date string: " + e.Message</exception>
	public Asn1UtcTime(string timeString)
	{
		TimeString = timeString ?? throw new ArgumentNullException(nameof(timeString));

		try
		{
			_dateTime       = FromString(timeString, out _twoDigitYearMax);
			_dateTimeLocked = false;
		}
		catch (FormatException e)
		{
			throw new ArgumentException("invalid date string: " + e.Message);
		}
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1UtcTime" /> class.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <param name="twoDigitYearMax">The two digit year maximum.</param>
	public Asn1UtcTime(DateTime dateTime, int twoDigitYearMax = DefaultTwoDigitYearCutoff)
	{
		dateTime = DateTimeExtensions.WithPrecisionSecond(dateTime.ToUniversalTime());

		Validate(dateTime, twoDigitYearMax);

		_dateTime        = dateTime;
		_dateTimeLocked  = true;
		TimeString       = ToStringCanonical(dateTime);
		_twoDigitYearMax = twoDigitYearMax;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1UtcTime" /> class.
	/// </summary>
	/// <param name="contents">The contents.</param>
	internal Asn1UtcTime(byte[] contents) : this(System.Text.Encoding.ASCII.GetString(contents)) { }

	/// <summary>
	///   Gets the time string.
	/// </summary>
	/// <value>The time string.</value>
	public string TimeString { get; }

	/// <summary>
	///   Gets the two digit year maximum.
	/// </summary>
	/// <value>The two digit year maximum.</value>
	public int TwoDigitYearMax => _twoDigitYearMax;

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Nullable&lt;Asn1UtcTime&gt;.</returns>
	/// <exception cref="System.ArgumentException">failed to construct UTC time from byte[]: {e.Message}</exception>
	/// <exception cref="System.ArgumentException">illegal object in GetInstance: {obj.GetTypeName()} - obj</exception>
	public static Asn1UtcTime? GetInstance(object? obj)
	{
		switch (obj)
		{
			case null:                    return null;
			case Asn1UtcTime asn1UtcTime: return asn1UtcTime;
			case IAsn1Convertable asn1Convertable:
			{
				var asn1Object = asn1Convertable.ToAsn1Object();
				if (asn1Object is Asn1UtcTime converted)
					return converted;
				break;
			}
			case byte[] bytes:
				try
				{
					return (Asn1UtcTime) Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException e)
				{
					throw new ArgumentException($"failed to construct UTC time from byte[]: {e.Message}");
				}
		}

		throw new ArgumentException($"illegal object in GetInstance: {obj.GetTypeName()}", nameof(obj));
	}

	/// <summary>
	///   Gets the instance.
	/// </summary>
	/// <param name="taggedObject">The tagged object.</param>
	/// <param name="declaredExplicit">if set to <c>true</c> [declared explicit].</param>
	/// <returns>Asn1UtcTime.</returns>
	public static Asn1UtcTime GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit) => (Asn1UtcTime) Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);

	/// <summary>
	///   Converts to datetime.
	/// </summary>
	/// <returns>DateTime.</returns>
	public DateTime ToDateTime() => _dateTime;

	/// <summary>
	///   Converts to datetime.
	/// </summary>
	/// <param name="twoDigitYearMax">The two digit year maximum.</param>
	/// <returns>DateTime.</returns>
	/// <exception cref="System.InvalidOperationException"></exception>
	public DateTime ToDateTime(int twoDigitYearMax)
	{
		if (InRange(_dateTime, twoDigitYearMax)) return _dateTime;

		if (_dateTimeLocked) throw new InvalidOperationException();

		var twoDigitYear       = _dateTime.Year  % 100;
		var twoDigitYearCutoff = twoDigitYearMax % 100;

		var diff              = twoDigitYear    - twoDigitYearCutoff;
		var newYear           = twoDigitYearMax + diff;
		if (diff > 0) newYear -= 100;

		return _dateTime.AddYears(newYear - _dateTime.Year);
	}

	/// <summary>
	///   Converts to datetime.
	/// </summary>
	/// <param name="calendar">The calendar.</param>
	/// <returns>DateTime.</returns>
	public DateTime ToDateTime(Calendar calendar) => ToDateTime(calendar.TwoDigitYearMax);

	/// <summary>
	///   Gets the contents.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>System.Byte[].</returns>
	internal byte[] GetContents(int encoding)
	{
		switch (encoding)
		{
			case Asn1OutputStream.EncodingDer when TimeString.Length != 13:
			{
				var canonical = ToStringCanonical(_dateTime);
				return System.Text.Encoding.ASCII.GetBytes(canonical);
			}
			default: return System.Text.Encoding.ASCII.GetBytes(TimeString);
		}
	}

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	/// <returns>IAsn1Encoding.</returns>
	internal override IAsn1Encoding GetEncoding(int encoding) => new Asn1Encoding(Asn1Tags.Universal, Asn1Tags.UtcTime, GetContents(encoding));

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
	internal sealed override DerEncoding GetEncodingDer() => new(Asn1Tags.Universal, Asn1Tags.UtcTime, GetContents(Asn1OutputStream.EncodingDer));

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
	protected override bool Asn1Equals(Asn1Object? asn1Object)
	{
		var that = asn1Object as Asn1UtcTime;
		return that is not null && Arrays.AreEqual(GetContents(Asn1OutputStream.EncodingDer), that.GetContents(Asn1OutputStream.EncodingDer));
	}

	/// <summary>
	///   Asn1s the get hash code.
	/// </summary>
	/// <returns>System.Int32.</returns>
	protected override int Asn1GetHashCode() => Arrays.GetHashCode(GetContents(Asn1OutputStream.EncodingDer));

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => TimeString;

	/// <summary>
	///   Creates the asn1.
	/// </summary>
	/// <param name="contents">The contents.</param>
	/// <returns>Asn1UtcTime.</returns>
	internal static Asn1UtcTime CreateAsn1(byte[] contents) => new(contents);

	/// <summary>
	///   Froms the string.
	/// </summary>
	/// <param name="s">The s.</param>
	/// <param name="twoDigitYearMax">The two digit year maximum.</param>
	/// <returns>DateTime.</returns>
	private static DateTime FromString(string s, out int twoDigitYearMax)
	{
		var provider = DateTimeFormatInfo.InvariantInfo;
		twoDigitYearMax = provider.Calendar.TwoDigitYearMax;

		return s.Length switch
		       {
			       11 => DateTime.ParseExact(s, @"yyMMddHHmm\Z",    provider, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
			       13 => DateTime.ParseExact(s, @"yyMMddHHmmss\Z",  provider, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal),
			       15 => DateTime.ParseExact(s, @"yyMMddHHmmzzz",   provider, DateTimeStyles.AdjustToUniversal),
			       17 => DateTime.ParseExact(s, @"yyMMddHHmmsszzz", provider, DateTimeStyles.AdjustToUniversal),
			       _  => throw new FormatException()
		       };
	}

	/// <summary>
	///   Ins the range.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <param name="twoDigitYearMax">The two digit year maximum.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private static bool InRange(DateTime dateTime, int twoDigitYearMax) => (uint) (twoDigitYearMax - dateTime.Year) < 100;

	/// <summary>
	///   Converts to stringcanonical.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <param name="twoDigitYearMax">The two digit year maximum.</param>
	/// <returns>System.String.</returns>
	public static string ToStringCanonical(DateTime dateTime, out int twoDigitYearMax)
	{
		var provider = DateTimeFormatInfo.InvariantInfo;
		twoDigitYearMax = provider.Calendar.TwoDigitYearMax;

		Validate(dateTime, twoDigitYearMax);

		return dateTime.ToString(@"yyMMddHHmmss\Z", provider);
	}

	/// <summary>
	///   Converts to stringcanonical.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <returns>System.String.</returns>
	public static string ToStringCanonical(DateTime dateTime) => dateTime.ToString(@"yyMMddHHmmss\Z", DateTimeFormatInfo.InvariantInfo);

	/// <summary>
	///   Validates the specified date time.
	/// </summary>
	/// <param name="dateTime">The date time.</param>
	/// <param name="twoDigitYearMax">The two digit year maximum.</param>
	/// <exception cref="System.ArgumentOutOfRangeException">dateTime</exception>
	private static void Validate(DateTime dateTime, int twoDigitYearMax)
	{
		if (!InRange(dateTime, twoDigitYearMax)) throw new ArgumentOutOfRangeException(nameof(dateTime));
	}

	public static Asn1UtcTime CreatePrimitive(byte[] contents) => new(contents);

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
		private Meta() : base(typeof(Asn1UtcTime), Asn1Tags.UtcTime) { }

		/// <summary>
		///   Froms the implicit primitive.
		/// </summary>
		/// <param name="octetString">The octet string.</param>
		/// <returns>Asn1Object.</returns>
		internal override Asn1Object FromImplicitPrimitive(DerOctetString octetString) => CreateAsn1(octetString.GetOctets());
	}
}
