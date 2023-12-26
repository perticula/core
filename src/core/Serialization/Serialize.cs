// perticula - core - Serialize.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace core.Serialization;

/// <summary>
///   This class defines the serialization features used by this application
/// </summary>
public static class Serialize
{
	/// <summary>
	///   Provides a more friendly type conversion mechanism that errs on the side of "it just works"
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The value.</param>
	/// <returns>T.</returns>
	/// <exception cref="System.ArgumentNullException">value</exception>
	/// <exception cref="System.InvalidCastException">unable to convert ${value.GetType()} to ${typeof(T)}</exception>
	public static T ConvertTo<T>(object value)
	{
				ArgumentNullException.ThrowIfNull(value);
				return (T) ConvertToType(value, typeof(T))! ?? throw new InvalidCastException($"unable to convert ${value.GetType()} to ${typeof(T)}");
	}

	/// <summary>
	///   Provides a more friendly type conversion mechanism that errs on the side of "it just works"
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="toType">To type.</param>
	/// <returns>System.Object.</returns>
	/// <exception cref="System.ArgumentNullException">value</exception>
	public static object? ConvertToType(object value, Type toType)
	{
				ArgumentNullException.ThrowIfNull(value);
				if (toType == typeof(string)) return value.ToString();
		var fromType = value.GetType();
		if (fromType == toType) return value;
		if (toType.IsEnum) return Enum.Parse(toType, value.ToString()!);

		// Nullable type conversion
		var realType = Nullable.GetUnderlyingType(toType);
		if (realType != null)
			return ConvertToType(value, realType);

		// ReSharper disable once InvertIf
		if (fromType == typeof(string))
		{
			switch (Type.GetTypeCode(toType))
			{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					var big = Convert.ChangeType(value, typeof(double)); // start big
					return Convert.ChangeType(big, toType);              // then lose precision
			}

			var convertor = TypeDescriptor.GetConverter(toType);
			if (convertor.CanConvertFrom(typeof(string))) return convertor.ConvertFrom(value);
		}

		return Convert.ChangeType(value, toType);
	}

	/// <summary>
	///   This is the preferred method of de-serializing from a string to
	///   an object of type T. This method complements and is compatible with
	///   ToString. This method supports conversion of simple and complex
	///   objects (using json)
	/// </summary>
	/// <typeparam name="T">The expected type of the object stored in the string</typeparam>
	/// <param name="value">The object stored as a string</param>
	/// <returns>T.</returns>
	/// <exception cref="System.InvalidOperationException"></exception>
	/// <exception cref="System.FormatException">Unable to convert '{value}' to type {typeof(T)}</exception>
	public static T? FromString<T>(string? value)
	{
		if (string.IsNullOrWhiteSpace(value)) return default;

		if (Nullable.GetUnderlyingType(typeof(T)) != null)
		{
			// Nullable type conversion
			var realType = Nullable.GetUnderlyingType(typeof(T));
			return (T) Convert.ChangeType(value, realType ?? throw new InvalidOperationException());
		}

		if (typeof(T).IsEnum) return (T) Enum.Parse(typeof(T), value);
		if (typeof(T) == typeof(string)) return (T) (object) value;

		var convertor = TypeDescriptor.GetConverter(typeof(T));
		if (convertor.CanConvertFrom(typeof(string))) return (T) convertor.ConvertFrom(value)!;

		if (typeof(T).IsClass)
		{
			using var sz   = new StringReader(value);
			var       json = new JsonSerializer();
			return (T) json.Deserialize(sz, typeof(T))!;
		}

		if (typeof(T).IsValueType) return (T) Convert.ChangeType(value, typeof(T));

		// Don't know how to convert this type
		throw new FormatException($"Unable to convert '{value}' to type {typeof(T)}");
	}

	/// <summary>
	///   Returns the list of EnumJson values for a given type of enum
	/// </summary>
	/// <typeparam name="TEnum">The num type</typeparam>
	/// <returns>IEnumerable&lt;EnumJson&lt;TEnum&gt;&gt;.</returns>
	public static IEnumerable<EnumJson<TEnum>> GetEnumJsonValues<TEnum>() where TEnum : struct, IConvertible => ((TEnum[]) Enum.GetValues(typeof(TEnum))).Select(ToEnumJson);

	/// <summary>
	///   Loads this object from a Json string
	/// </summary>
	/// <param name="objValue">The string containing the json</param>
	/// <returns>dynamic.</returns>
	public static dynamic GetJsonDynamic(string objValue) => JObject.Parse(objValue);

	/// <summary>
	///   Converts an enum type into a standardized json object (EnumJson)
	/// </summary>
	/// <typeparam name="TEnum">The type of the t enum.</typeparam>
	/// <param name="e">The e.</param>
	/// <returns>EnumJson&lt;TEnum&gt;.</returns>
	public static EnumJson<TEnum> ToEnumJson<TEnum>(this TEnum e) where TEnum : struct, IConvertible => EnumJson<TEnum>.FromEnum(e);

	/// <summary>
	///   Serialize an object to JSON
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>System.String.</returns>
	public static string ToJson(object data)
	{
		using var sz   = new StringWriter();
		var       json = new JsonSerializer();
		json.Serialize(sz, data);
		return sz.ToString();
	}

	/// <summary>
	///   Converts the specified IEnumerable to a json object
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="data">The data.</param>
	/// <returns>System.String.</returns>
	public static string ToJson<T>(IEnumerable<T> data) => JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

	/// <summary>
	///   This is the preferred method of serializing from an object of type T to
	///   a string. This method complements and is compatible with FromString. This
	///   method supports conversion of simple and complex objects (using json)
	/// </summary>
	/// <typeparam name="T">The type of the object stored in the string</typeparam>
	/// <param name="objValue">The object to serialize</param>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	/// <exception cref="System.ArgumentNullException">objValue</exception>
	/// <exception cref="System.NotSupportedException">Unable to convert object of type {typeof(T)} to a string</exception>
	public static string ToString<T>(T objValue)
	{
		if (objValue == null) throw new ArgumentNullException(nameof(objValue));

		if (typeof(T) == typeof(string) || typeof(T).IsValueType)
			return objValue.ToString() ?? throw new ArgumentNullException(nameof(objValue));

		if (typeof(T).IsClass)
		{
			if (objValue is IConvertible convert) return convert.ToString(CultureInfo.InvariantCulture);
			using var sz   = new StringWriter();
			var       json = new JsonSerializer();
			json.Serialize(sz, objValue);
			return sz.ToString();
		}

		// Something we missed?
		throw new NotSupportedException($"Unable to convert object of type {typeof(T)} to a string");
	}
}
