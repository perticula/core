// perticula - model - IIdentified.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model;

/// <summary>
///   Interface IIdentified.
///   Defines an `Id` field of the specified type.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIdentified<out T>
{
	/// <summary>
	///   Gets the identifier for this instance.
	/// </summary>
	/// <value>The identifier.</value>
	T Id { get; }
}
