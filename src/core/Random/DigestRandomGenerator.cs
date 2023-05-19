// perticula - core - DigestRandomGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Cryptography;

namespace core.Random;

/// <summary>
///   Class DigestRandomGenerator.
///   Implements the <see cref="core.Random.IRandomGenerator" />
/// </summary>
/// <seealso cref="core.Random.IRandomGenerator" />
public class DigestRandomGenerator : IRandomGenerator
{
	/// <summary>
	///   The cycle count
	/// </summary>
	private const long CycleCount = 10;

	/// <summary>
	///   The digest
	/// </summary>
	private readonly IDigest? _digest;

	/// <summary>
	///   The seed
	/// </summary>
	private readonly byte[] _seed;

	/// <summary>
	///   The state
	/// </summary>
	private readonly byte[] _state;

	/// <summary>
	///   The seed counter
	/// </summary>
	private long _seedCounter;

	/// <summary>
	///   The state counter
	/// </summary>
	private long _stateCounter;

	/// <summary>
	///   Initializes a new instance of the <see cref="DigestRandomGenerator" /> class.
	/// </summary>
	/// <param name="digest">The digest.</param>
	public DigestRandomGenerator(IDigest? digest)
	{
		_digest = digest;

		_seed        = new byte[digest?.GetDigestSize() ?? 0];
		_seedCounter = 1;

		_state        = new byte[digest?.GetDigestSize() ?? 0];
		_stateCounter = 1;
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="inSeed">The in seed.</param>
	public void AddSeedMaterial(byte[] inSeed)
	{
		lock (this)
		{
			DigestUpdate(inSeed);
			DigestUpdate(_seed);
			DigestDoFinal(_seed);
		}
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="inSeed">The in seed.</param>
	public void AddSeedMaterial(ReadOnlySpan<byte> inSeed)
	{
		lock (this)
		{
			if (!inSeed.IsEmpty) DigestUpdate(inSeed);
			DigestUpdate(_seed);
			DigestDoFinal(_seed);
		}
	}

	/// <summary>
	///   Adds more seed material.
	/// </summary>
	/// <param name="rSeed">The r seed.</param>
	public void AddSeedMaterial(long rSeed)
	{
		lock (this)
		{
			DigestAddCounter(rSeed);
			DigestUpdate(_seed);
			DigestDoFinal(_seed);
		}
	}

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public void NextBytes(byte[] bytes) => NextBytes(bytes, 0, bytes.Length);

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	/// <param name="start">The start.</param>
	/// <param name="len">The length.</param>
	public void NextBytes(byte[] bytes, int start, int len) => NextBytes(bytes.AsSpan(start, len));

	/// <summary>
	///   Fill a byte array with random bytes.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	public void NextBytes(Span<byte> bytes)
	{
		lock (this)
		{
			var stateOff = 0;

			GenerateState();

			for (var i = 0; i < bytes.Length; ++i)
			{
				if (stateOff == _state.Length)
				{
					GenerateState();
					stateOff = 0;
				}

				bytes[i] = _state[stateOff++];
			}
		}
	}

	/// <summary>
	///   Cycles the seed.
	/// </summary>
	private void CycleSeed()
	{
		DigestUpdate(_seed);
		DigestAddCounter(_seedCounter++);
		DigestDoFinal(_seed);
	}

	/// <summary>
	///   Generates the state.
	/// </summary>
	private void GenerateState()
	{
		DigestAddCounter(_stateCounter++);
		DigestUpdate(_state);
		DigestUpdate(_seed);
		DigestDoFinal(_state);

		if (_stateCounter % CycleCount == 0) CycleSeed();
	}

	/// <summary>
	///   Digests the add counter.
	/// </summary>
	/// <param name="seedVal">The seed value.</param>
	private void DigestAddCounter(long seedVal)
	{
		Span<byte> bytes = stackalloc byte[8];
		Pack.UInt64_To_LittleEndian((ulong) seedVal, bytes);
		_digest?.BlockUpdate(bytes);
	}

	/// <summary>
	///   Digests the update.
	/// </summary>
	/// <param name="inSeed">The in seed.</param>
	private void DigestUpdate(ReadOnlySpan<byte> inSeed) => _digest?.BlockUpdate(inSeed);

	/// <summary>
	///   Digests the do final.
	/// </summary>
	/// <param name="result">The result.</param>
	private void DigestDoFinal(Span<byte> result) => _digest?.DoFinal(result);
}
