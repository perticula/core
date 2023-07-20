// perticula - core - IGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core;

public interface IGenerator<out TType>
{
	/// <summary>
	///   Generates an instance of the given type.
	/// </summary>
	/// <returns>TType.</returns>
	TType Generate();
}

public interface IGenerator<out TType, in TArg>
{
	/// <summary>
	///   Generates an instance of the given type, with one specified initialization argument
	/// </summary>
	/// <param name="arg">The argument.</param>
	/// <returns>TType.</returns>
	TType Generate(TArg arg);
}
