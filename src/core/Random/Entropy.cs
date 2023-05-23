// perticula - core - Entropy.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Random;

public abstract class Entropy
{
	/// <summary>
	///   Generate numBytes worth of entropy from the passed in entropy source.
	/// </summary>
	/// <param name="entropySource">The entropy source.</param>
	/// <param name="numBytes">The number bytes to request.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] GenerateSeed(IEntropySource entropySource, int numBytes)
	{
		var bytes = new byte[numBytes];
		GenerateSeed(entropySource, bytes);
		return bytes;
	}

	/// <summary>
	///   .
	/// </summary>
	/// <param name="entropySource">The entropy source.</param>
	/// <param name="seed">The seed.</param>
	public static void GenerateSeed(IEntropySource entropySource, Span<byte> seed)
	{
		while (!seed.IsEmpty)
		{
			var len = entropySource.GetEntropy(seed);
			seed = seed[len..];
		}
	}
}
