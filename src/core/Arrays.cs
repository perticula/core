// perticula - core - Arrays.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Security.Cryptography;
using System.Text;

namespace core;

/// <summary>
///   Class Arrays.
/// </summary>
public static class Arrays
{
	/// <summary>
	///   The empty bytes
	/// </summary>
	public static readonly byte[] EmptyBytes = Array.Empty<byte>();

	/// <summary>
	///   The empty ints
	/// </summary>
	public static readonly int[] EmptyInts = Array.Empty<int>();

	/// <summary>
	///   Ares all zeroes.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public static bool AreAllZeroes(byte[] buf, int off, int len)
	{
		uint bits                          = 0;
		for (var i = 0; i < len; ++i) bits |= buf[off + i];
		return bits       == 0;
	}

	/// <summary>
	///   Ares all zeroes.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public static bool AreAllZeroes(ReadOnlySpan<byte> buf)
	{
		uint bits                   = 0;
		foreach (var t in buf) bits |= t;
		return bits == 0;
	}

	/// <summary>
	///   Ares the equal.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public static bool AreEqual<T>(T[] a, T[] b) => a == b || HaveSameContents(a, b);

	/// <summary>
	///   Ares the equal.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <param name="aFromIndex">Index of a from.</param>
	/// <param name="aToIndex">Index of a to.</param>
	/// <param name="b">The b.</param>
	/// <param name="bFromIndex">Index of the b from.</param>
	/// <param name="bToIndex">Index of the b to.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public static bool AreEqual<T>(T[] a, int aFromIndex, int aToIndex, T[] b, int bFromIndex, int bToIndex)
	{
		var aLength = aToIndex - aFromIndex;
		var bLength = bToIndex - bFromIndex;

		if (aLength != bLength) return false;

		for (var i = 0; i < aLength; ++i)
			if (!Equals(a[aFromIndex + i], b[bFromIndex + i]))
				return false;

		return true;
	}

	/// <summary>
	///   Haves the same contents.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private static bool HaveSameContents<T>(IReadOnlyList<T> a, IReadOnlyList<T> b)
	{
		var i = a.Count;
		if (i != b.Count) return false;
		while (i != 0)
		{
			--i;
			if (!Equals(a[i], b[i]))
				return false;
		}

		return true;
	}

	/// <summary>
	///   Fixeds the time equals.
	/// </summary>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public static bool FixedTimeEquals(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b) => CryptographicOperations.FixedTimeEquals(a, b);

	/// <summary>
	///   Fixeds the time equals.
	/// </summary>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public static bool FixedTimeEquals(byte[] a, byte[] b) => CryptographicOperations.FixedTimeEquals(a, b);

	/// <summary>
	///   Fixeds the time equals.
	/// </summary>
	/// <param name="len">The length.</param>
	/// <param name="a">a.</param>
	/// <param name="aOff">a off.</param>
	/// <param name="b">The b.</param>
	/// <param name="bOff">The b off.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	public static bool FixedTimeEquals(int len, byte[] a, int aOff, byte[] b, int bOff) => CryptographicOperations.FixedTimeEquals(a.AsSpan(aOff, len), b.AsSpan(bOff, len));

	/// <summary>
	///   Clones the specified data.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="data">The data.</param>
	/// <returns>T[].</returns>
	public static T[] Clone<T>(T[] data) => (T[]) data.Clone();

	/// <summary>
	///   Clones the specified data.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="data">The data.</param>
	/// <param name="existing">The existing.</param>
	/// <returns>T[].</returns>
	public static T[] Clone<T>(T[] data, T[] existing)
	{
		if (existing.Length != data.Length) return Clone(data);
		Array.Copy(data, 0, existing, 0, existing.Length);
		return existing;
	}

	/// <summary>
	///   Determines whether this instance contains the object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <param name="n">The n.</param>
	/// <returns><c>true</c> if [contains] [the specified a]; otherwise, <c>false</c>.</returns>
	public static bool Contains<T>(T[] a, T n) => a.Contains(n);

	/// <summary>
	///   Copies the of.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="data">The data.</param>
	/// <param name="newLength">The new length.</param>
	/// <returns>T[].</returns>
	public static T[] CopyOf<T>(T[] data, int newLength)
	{
		var tmp = new T[newLength];
		Array.Copy(data, 0, tmp, 0, System.Math.Min(newLength, data.Length));
		return tmp;
	}

	/// <summary>
	///   Copies a range of elements from the source array.
	///   The range can extend beyong the end of the source array, in which case the remaining elements are padded with the
	///   default value of the element type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="data">The data.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <returns>T[].</returns>
	public static T[] CopyOfRange<T>(T[] data, int from, int to)
	{
		var newLength = GetLength(from, to);

		var tmp = new T[newLength];
		Array.Copy(data, from, tmp, 0, System.Math.Min(newLength, data.Length - from));
		return tmp;

		static int GetLength(int from, int to)
		{
			var newLength = to - from;
			if (newLength < 0) throw new ArgumentException(from + " > " + to);
			return newLength;
		}
	}

	/// <summary>
	///   Appends the specified a.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns>T[].</returns>
	public static T[] Append<T>(T[] a, T b)
	{
		var length = a.Length;

		var result = new T[length + 1];
		Array.Copy(a, 0, result, 0, length);
		result[length] = b;
		return result;
	}

	/// <summary>
	///   Concatenates the specified a.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns>T[].</returns>
	public static T[] Concatenate<T>(T[] a, T[] b)
	{
		var rv = new T[a.Length + b.Length];
		Array.Copy(a, 0, rv, 0,        a.Length);
		Array.Copy(b, 0, rv, a.Length, b.Length);
		return rv;
	}

	/// <summary>
	///   Concatenates all.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="vs">The vs.</param>
	/// <returns>T[].</returns>
	public static T[] ConcatenateAll<T>(params T[][] vs)
	{
		var nonNull     = new T[vs.Length][];
		var count       = 0;
		var totalLength = 0;

		foreach (var v in vs)
		{
			nonNull[count++] =  v;
			totalLength      += v.Length;
		}

		var result = new T[totalLength];
		var pos    = 0;

		for (var j = 0; j < count; ++j)
		{
			var v = nonNull[j];
			Array.Copy(v, 0, result, pos, v.Length);
			pos += v.Length;
		}

		return result;
	}

	/// <summary>
	///   Prepends the specified a.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <param name="b">The b.</param>
	/// <returns>T[].</returns>
	public static T[] Prepend<T>(T[] a, T b)
	{
		var length = a.Length;
		var result = new T[length + 1];
		Array.Copy(a, 0, result, 1, length);
		result[0] = b;
		return result;
	}

	/// <summary>
	///   Fills the specified buffer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="buffer">The buffer.</param>
	/// <param name="b">The b.</param>
	public static void Fill<T>(T[] buffer, T b)
	{
		var i = buffer.Length;

		while (i > 0) buffer[--i] = b;
	}


	/// <summary>
	///   Reverses the specified a.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a">a.</param>
	/// <returns>T[].</returns>
	public static T[] Reverse<T>(T[] a)
	{
		int p1 = 0, p2 = a.Length;

		var result = new T[p2];

		while (--p2 >= 0) result[p2] = a[p1++];

		return result;
	}

	/// <summary>
	///   Reverses the in place.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array">The array.</param>
	/// <returns>T[].</returns>
	public static T[] ReverseInPlace<T>(T[] array)
	{
		Array.Reverse(array);
		return array;
	}

	/// <summary>
	///   Clears the specified data.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="data">The data.</param>
	public static void Clear<T>(T[] data) => Array.Clear(data, 0, data.Length);

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <param name="a">a.</param>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public static string ToString(object[] a)
	{
		var sb = new StringBuilder("[");
		if (a.Length > 0)
		{
			sb.Append(a[0]);
			for (var index = 1; index < a.Length; ++index) sb.Append(", ").Append(a[index]);
		}

		sb.Append(']');
		return sb.ToString();
	}

	/// <summary>
	///   Determines whether [is null or contains null] [the specified array].
	/// </summary>
	/// <param name="array">The array.</param>
	/// <returns><c>true</c> if [is null or contains null] [the specified array]; otherwise, <c>false</c>.</returns>
	public static bool IsNullOrContainsNull<T>(T?[]? array)
	{
		if (null == array) return true;

		var count = array.Length;
		for (var i = 0; i < count; ++i)
		{
			if (null == array[i])
				return true;
		}

		return false;
	}

	/// <summary>
	///   Determines whether [is null or empty] [the specified array].
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="array">The array.</param>
	/// <returns><c>true</c> if [is null or empty] [the specified array]; otherwise, <c>false</c>.</returns>
	public static bool IsNullOrEmpty<T>(T[]? array) => null == array || array.Length < 1;

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(byte[] data)
	{
		var i  = data.Length;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= data[i];
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(byte[] data, int off, int len)
	{
		var i  = len;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= data[off + i];
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(int[] data)
	{
		var i  = data.Length;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= data[i];
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(ushort[] data)
	{
		var i  = data.Length;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= data[i];
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(int[] data, int off, int len)
	{
		var i  = len;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= data[off + i];
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(uint[] data)
	{
		var i  = data.Length;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= (int) data[i];
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(uint[] data, int off, int len)
	{
		var i  = len;
		var hc = i + 1;

		while (--i >= 0)
		{
			hc *= 257;
			hc ^= (int) data[off + i];
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(ulong[] data)
	{
		var i  = data.Length;
		var hc = i + 1;

		while (--i >= 0)
		{
			var di = data[i];
			hc *= 257;
			hc ^= (int) di;
			hc *= 257;
			hc ^= (int) (di >> 32);
		}

		return hc;
	}

	/// <summary>
	///   Returns a hash code for this instance.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(ulong[] data, int off, int len)
	{
		var i  = len;
		var hc = i + 1;

		while (--i >= 0)
		{
			var di = data[off + i];
			hc *= 257;
			hc ^= (int) di;
			hc *= 257;
			hc ^= (int) (di >> 32);
		}

		return hc;
	}
}
