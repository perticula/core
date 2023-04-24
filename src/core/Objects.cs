// perticula - core - Objects.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

public static class Objects
{
	/// <summary>
	///   Returns a hash code for this object instance.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
	public static int GetHashCode(this object? obj) => obj?.GetHashCode() ?? 0;

	/// <summary>
	///   Ensures a singleton has been initialized and returns the singleton instance.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TArg">The type of the t argument.</typeparam>
	/// <param name="singleton">The singleton.</param>
	/// <param name="arg">The argument.</param>
	/// <param name="initializer">The initializer.</param>
	/// <returns>the singleton instance.</returns>
	public static T EnsureSingleton<T, TArg>(ref T? singleton, TArg arg, Func<TArg, T> initializer) where T : class
	{
		var current = Volatile.Read(ref singleton);
		if (current != null) return current;

		var canidate = initializer(arg);
		return Interlocked.CompareExchange(ref singleton, canidate, null) ?? canidate;
	}
}
