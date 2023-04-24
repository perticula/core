// perticula - core - EnumerableProxy.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Collections;

namespace core;

public sealed class EnumerableProxy<T> : IEnumerable<T>
{
	/// <summary>
	///   The target enumerable instance
	/// </summary>
	private readonly IEnumerable<T> _target;

	/// <summary>
	///   Initializes a new instance of the <see cref="EnumerableProxy{T}" /> class.
	/// </summary>
	/// <param name="target">The target.</param>
	/// <exception cref="System.ArgumentNullException">target</exception>
	internal EnumerableProxy(IEnumerable<T> target) => _target = target ?? throw new ArgumentNullException(nameof(target));

	/// <summary>
	///   Returns an enumerator that iterates through a collection.
	/// </summary>
	/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
	IEnumerator IEnumerable.GetEnumerator() => _target.GetEnumerator();

	/// <summary>
	///   Returns an enumerator that iterates through the collection.
	/// </summary>
	/// <returns>An enumerator that can be used to iterate through the collection.</returns>
	public IEnumerator<T> GetEnumerator() => _target.GetEnumerator();
}
