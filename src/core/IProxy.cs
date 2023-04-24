// perticula - core - IProxy.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

/// <summary>
///   Creates a proxy for the specified <typeparamref name="T" />
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IProxy<out T>
{
	/// <summary>
	///   Gets the target.
	/// </summary>
	/// <value>The target.</value>
	T Target { get; }
}
