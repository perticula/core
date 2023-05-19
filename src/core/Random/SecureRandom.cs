// perticula - core - SecureRandom.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Cryptography;

namespace core.Random;

/// <summary>
///   Class SecureRandom.
///   Implements the <see cref="System.Random" />
/// </summary>
/// <seealso cref="System.Random" />
public class SecureRandom : System.Random
{
	/// <summary>
	///   The counter
	/// </summary>
	private static long _counter = DateTime.UtcNow.Ticks;

	/// <summary>
	///   The double scale
	/// </summary>
	private static readonly double DoubleScale = 1.0 / Convert.ToDouble(1L << 53);

	/// <summary>
	///   The master random
	/// </summary>
	private static readonly SecureRandom MasterRandom = new(new CryptoApiRandomGenerator());

	/// <summary>
	///   The arbitrary random
	/// </summary>
	internal static readonly SecureRandom ArbitraryRandom = new(new VmpcRandomGenerator(), 16);

	/// <summary>
	///   The generator
	/// </summary>
	protected readonly IRandomGenerator Generator;

	/// <summary>
	///   Initializes a new instance of the <see cref="SecureRandom" /> class.
	/// </summary>
	public SecureRandom() : this(CreatePrng("SHA256", true)) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="SecureRandom" /> class.
	/// </summary>
	/// <param name="generator">The generator.</param>
	public SecureRandom(IRandomGenerator generator) : base(0) => Generator = generator;

	/// <summary>
	///   Initializes a new instance of the <see cref="SecureRandom" /> class.
	/// </summary>
	/// <param name="generator">The generator.</param>
	/// <param name="autoSeedLengthInBytes">The automatic seed length in bytes.</param>
	public SecureRandom(IRandomGenerator generator, int autoSeedLengthInBytes) : base(0)
	{
		AutoSeed(generator, autoSeedLengthInBytes);

		Generator = generator;
	}

	/// <summary>
	///   Nexts the counter value.
	/// </summary>
	/// <returns>System.Int64.</returns>
	private static long NextCounterValue() => Interlocked.Increment(ref _counter);

	/// <summary>
	///   Generates the seed.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public virtual void GenerateSeed(Span<byte> seed) => MasterRandom.NextBytes(seed);

	/// <summary>
	///   Sets the seed.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public virtual void SetSeed(Span<byte> seed) => Generator.AddSeedMaterial(seed);

	/// <summary>
	///   Sets the seed.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public virtual void SetSeed(long seed) => Generator.AddSeedMaterial(seed);

	/// <summary>
	///   Returns a non-negative random integer.
	/// </summary>
	/// <returns>
	///   A 32-bit signed integer that is greater than or equal to 0 and less than
	///   <see cref="F:System.Int32.MaxValue">Int32.MaxValue</see>.
	/// </returns>
	public override int Next() => NextInt() & int.MaxValue;

	/// <summary>
	///   Returns a non-negative random integer that is less than the specified maximum.
	/// </summary>
	/// <param name="maxValue">
	///   The exclusive upper bound of the random number to be generated. <paramref name="maxValue" />
	///   must be greater than or equal to 0.
	/// </param>
	/// <returns>
	///   A 32-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue" />; that
	///   is, the range of return values ordinarily includes 0 but not <paramref name="maxValue" />. However, if
	///   <paramref name="maxValue" /> equals 0, <paramref name="maxValue" /> is returned.
	/// </returns>
	/// <exception cref="System.ArgumentOutOfRangeException">maxValue - cannot be negative</exception>
	public override int Next(int maxValue)
	{
		switch (maxValue)
		{
			case < 2 and < 0: throw new ArgumentOutOfRangeException(nameof(maxValue), "cannot be negative");
			case < 2:         return 0;
		}

		int bits;

		// Test whether maxValue is a power of 2
		if ((maxValue & (maxValue - 1)) == 0)
		{
			bits = NextInt() & int.MaxValue;
			return (int) (((long) bits * maxValue) >> 31);
		}

		int result;
		do
		{
			bits   = NextInt() & int.MaxValue;
			result = bits % maxValue;
		} while (bits - result + (maxValue - 1) < 0); // Ignore results near overflow

		return result;
	}

	/// <summary>
	///   Returns a random integer that is within a specified range.
	/// </summary>
	/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
	/// <param name="maxValue">
	///   The exclusive upper bound of the random number returned. <paramref name="maxValue" /> must be
	///   greater than or equal to <paramref name="minValue" />.
	/// </param>
	/// <returns>
	///   A 32-bit signed integer greater than or equal to <paramref name="minValue" /> and less than
	///   <paramref name="maxValue" />; that is, the range of return values includes <paramref name="minValue" /> but not
	///   <paramref name="maxValue" />. If <paramref name="minValue" /> equals <paramref name="maxValue" />,
	///   <paramref name="minValue" /> is returned.
	/// </returns>
	/// <exception cref="System.ArgumentException">maxValue cannot be less than minValue</exception>
	public override int Next(int minValue, int maxValue)
	{
		if (maxValue <= minValue)
			return maxValue == minValue ? minValue : throw new ArgumentException("maxValue cannot be less than minValue", nameof(maxValue));

		var diff = maxValue - minValue;
		if (diff > 0)
			return minValue + Next(diff);

		for (;;)
		{
			var i = NextInt();

			if (i >= minValue && i < maxValue)
				return i;
		}
	}

	/// <summary>
	///   Nexts the bytes.
	/// </summary>
	/// <param name="buf">The buf.</param>
	public override void NextBytes(byte[] buf) => Generator.NextBytes(buf);

	/// <summary>
	///   Nexts the bytes.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	public virtual void NextBytes(byte[] buf, int off, int len) => Generator.NextBytes(buf, off, len);

	/// <summary>
	///   Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
	/// </summary>
	/// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
	public override double NextDouble() => Convert.ToDouble((ulong) NextLong() >> 11) * DoubleScale;

	/// <summary>
	///   Nexts the int.
	/// </summary>
	/// <returns>System.Int32.</returns>
	public virtual int NextInt()
	{
		Span<byte> bytes = stackalloc byte[4];
		NextBytes(bytes);
		return (int) Pack.BigEndian_To_UInt32(bytes);
	}

	/// <summary>
	///   Nexts the long.
	/// </summary>
	/// <returns>System.Int64.</returns>
	public virtual long NextLong()
	{
		Span<byte> bytes = stackalloc byte[8];
		NextBytes(bytes);
		return (long) Pack.BigEndian_To_UInt64(bytes);
	}

	/// <summary>
	///   Create and auto-seed an instance based on the given algorithm.
	/// </summary>
	/// <param name="algorithm">The algorithm.</param>
	/// <returns>SecureRandom.</returns>
	public static SecureRandom GetInstance(string algorithm) => GetInstance(algorithm, true);

	/// <summary>
	///   Create an instance based on the given algorithm, with optional auto-seeding
	/// </summary>
	/// <param name="algorithm">The algorithm.</param>
	/// <param name="autoSeed">if set to <c>true</c> [automatic seed].</param>
	/// <returns>SecureRandom.</returns>
	/// <exception cref="System.ArgumentNullException">algorithm</exception>
	/// <exception cref="System.ArgumentException">Unrecognised PRNG algorithm: {algorithm} - algorithm</exception>
	public static SecureRandom GetInstance(string algorithm, bool autoSeed)
	{
		if (string.IsNullOrWhiteSpace(algorithm))
			throw new ArgumentNullException(nameof(algorithm));

		if (!algorithm.EndsWith("PRNG", StringComparison.OrdinalIgnoreCase))
			throw new ArgumentException($"Unrecognised PRNG algorithm: {algorithm}", nameof(algorithm));

		return new SecureRandom(CreatePrng(algorithm[..^"PRNG".Length], autoSeed));
	}

	/// <summary>
	///   Gets the next bytes.
	/// </summary>
	/// <param name="secureRandom">The secure random.</param>
	/// <param name="length">The length.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] GetNextBytes(SecureRandom secureRandom, int length)
	{
		var result = new byte[length];
		secureRandom.NextBytes(result);
		return result;
	}

	/// <summary>
	///   Automaticly generates the seed value.
	/// </summary>
	/// <param name="generator">The generator.</param>
	/// <param name="seedLength">Length of the seed.</param>
	private static void AutoSeed(IRandomGenerator generator, int seedLength)
	{
		generator.AddSeedMaterial(NextCounterValue());

		var seed = seedLength <= 128 ? stackalloc byte[seedLength] : new byte[seedLength];

		MasterRandom.NextBytes(seed);
		generator.AddSeedMaterial(seed);
	}

	/// <summary>
	///   Creates the Psudo Random Number Genrator.
	/// </summary>
	/// <param name="digestName">Name of the digest.</param>
	/// <param name="autoSeed">if set to <c>true</c> [automatic seed].</param>
	/// <returns>DigestRandomGenerator.</returns>
	private static DigestRandomGenerator CreatePrng(string digestName, bool autoSeed)
	{
		var digest = DigestUtilities.GetDigest(digestName);

		var prng = new DigestRandomGenerator(digest);
		if (autoSeed) AutoSeed(prng, 2 * digest.GetDigestSize());
		return prng;
	}
}
