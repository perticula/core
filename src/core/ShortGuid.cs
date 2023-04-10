// perticula - model - ShortGuid.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

public readonly struct ShortGuid
{
	/// <summary>
	///   The empty value instance
	/// </summary>
	public static readonly ShortGuid Empty = new(Guid.Empty);

	/// <summary>
	///   Initializes a new instance of the <see cref="ShortGuid" /> struct.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <exception cref="System.ArgumentNullException">value</exception>
	public ShortGuid(string value)
	{
		if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

		Value     = value;
		GuidValue = Decode(value);
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="ShortGuid" /> struct.
	/// </summary>
	/// <param name="guidValue">The unique identifier.</param>
	public ShortGuid(Guid guidValue)
	{
		Value     = Encode(guidValue);
		GuidValue = guidValue;
	}

	/// <summary>
	///   Gets the unique identifier.
	/// </summary>
	/// <value>The unique identifier.</value>
	public Guid GuidValue { get; }

	/// <summary>
	///   Gets the value.
	/// </summary>
	/// <value>The value.</value>
	public string Value { get; }

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => Value;

	/// <summary>
	///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
	/// </summary>
	/// <param name="val">The object to compare with the current instance.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
	public override bool Equals(object? val) => val is ShortGuid guid && Equals(guid);

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public override int GetHashCode()
	{
		unchecked
		{
			return (GuidValue.GetHashCode() * 6291469) ^ (Value != null ? Value.GetHashCode() : 0);
		}
	}

	/// <summary>
	///   News the unique identifier.
	/// </summary>
	public static ShortGuid NewGuid() => new(Guid.NewGuid());

	/// <summary>
	///   Encodes the specified standard Guid into a ShortGuid instance
	/// </summary>
	/// <param name="value">The unique identifier.</param>
	/// <returns>System.String.</returns>
	public static string Encode(Guid value)
	{
		var encoded = Convert.ToBase64String(value.ToByteArray());
		encoded = encoded.Replace("/", "_").Replace("+", "-");
		return encoded[..22];
	}

	/// <summary>
	///   Decodes the specified value into a standard Guid
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>System.Guid.</returns>
	/// <exception cref="System.ArgumentNullException"></exception>
	/// <exception cref="System.InvalidOperationException">ShortGuid::Decode Specified value could not be converted.</exception>
	public static Guid Decode(string value)
	{
		if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(value);
		try
		{
			value = value.Replace("_", "/").Replace("-", "+");
			var buffer = Convert.FromBase64String(value + "==");
			return new Guid(buffer);
		}
		catch (Exception e)
		{
			throw new InvalidOperationException("ShortGuid::Decode Specified value could not be converted.", e);
		}
	}

	/// <summary>
	///   Implements the operator ==.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <returns>The result of the operator.</returns>
	public static bool operator ==(ShortGuid x, ShortGuid y) => x.GuidValue == y.GuidValue;

	/// <summary>
	///   Implements the operator !=.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	/// <returns>The result of the operator.</returns>
	public static bool operator !=(ShortGuid x, ShortGuid y) => !(x == y);

	// Add implicit conversions between standard strings and Guids.
	/// <summary>
	///   Performs an implicit conversion from <see cref="ShortGuid" /> to <see cref="System.String" />.
	/// </summary>
	/// <param name="shortGuid">The short unique identifier.</param>
	/// <returns>The result of the conversion.</returns>
	public static implicit operator string(ShortGuid shortGuid) => shortGuid.Value;

	/// <summary>
	///   Performs an implicit conversion from <see cref="ShortGuid" /> to <see cref="Guid" />.
	/// </summary>
	/// <param name="shortGuid">The short unique identifier.</param>
	/// <returns>The result of the conversion.</returns>
	public static implicit operator Guid(ShortGuid shortGuid) => shortGuid.GuidValue;

	/// <summary>
	///   Performs an implicit conversion from <see cref="System.String" /> to <see cref="ShortGuid" />.
	/// </summary>
	/// <param name="shortGuid">The short unique identifier.</param>
	/// <returns>The result of the conversion.</returns>
	public static implicit operator ShortGuid(string shortGuid) => new(shortGuid);

	/// <summary>
	///   Performs an implicit conversion from <see cref="Guid" /> to <see cref="ShortGuid" />.
	/// </summary>
	/// <param name="shortguid">The shortguid.</param>
	/// <returns>The result of the conversion.</returns>
	public static implicit operator ShortGuid(Guid shortguid) => new(shortguid);

	/// <summary>
	///   Determines whether the specified <see cref="ShortGuid" /> is equal to this instance.
	/// </summary>
	/// <param name="other">The other.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public bool Equals(ShortGuid other) => GuidValue.Equals(other.GuidValue) && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
}
