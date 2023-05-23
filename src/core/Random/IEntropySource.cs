// perticula - core - IEntropySource.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

public interface IEntropySource
{
	/// <summary>
	///   Gets a value indicating whether this instance is prediction resistant.
	/// </summary>
	/// <value><c>true</c> if this instance is prediction resistant; otherwise, <c>false</c>.</value>
	bool IsPredictionResistant { get; }

	/// <summary>
	///   Gets the size (in bits) the entropy source can produce.
	/// </summary>
	/// <value>The size of the entropy.</value>
	int EntropySize { get; }

	/// <summary>
	///   Gets the entropy.
	/// </summary>
	/// <returns>System.Byte[].</returns>
	byte[] GetEntropy();

	/// <summary>
	///   Gets the entropy.
	/// </summary>
	/// <param name="output">The output.</param>
	/// <returns>System.Int32.</returns>
	int GetEntropy(Span<byte> output);
}
