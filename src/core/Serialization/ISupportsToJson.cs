// perticula - core - ISupportsToJson.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Serialization;

/// <summary>
///   Interface ISupportsToJson
/// </summary>
public interface ISupportsToJson
{
	/// <summary>
	///   Returns this object as json
	/// </summary>
	/// <returns>System.String.</returns>
	string ToJson();
}
