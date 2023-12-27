// perticula - core - SerializableUtilities.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Serialization;

/// <summary>
///   Class SerializableUtilities.
/// </summary>
public static class SerializableUtilities
{
	/// <summary>
	///   Deserializes from.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The value.</param>
	/// <param name="delimiter">The delimiter.</param>
	/// <returns>IEnumerable&lt;T&gt;.</returns>
	public static IEnumerable<T?> DeserializeFrom<T>(this string value, string delimiter = ",")
		=> string.IsNullOrEmpty(value)
			? Enumerable.Empty<T>()
			: value.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(Serialize.FromString<T>);

	/// <summary>
	///   Serializes the join.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="values">The values.</param>
	/// <param name="delimiter">The delimiter.</param>
	/// <returns>System.String.</returns>
	public static string SerializedJoin<T>(this IEnumerable<T> values, string delimiter = ", ") =>
		string.Join(delimiter, values.Select(Serialize.ToString));
}
