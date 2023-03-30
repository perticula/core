// perticula - core - CryptographicRandomizer.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Security.Cryptography;

namespace core.Random;

public sealed class CryptographicRandomizer : CryptographicRandom, IDisposable
{
	private readonly byte[]                _buf = new byte[sizeof(uint) * 64];
	private readonly RandomNumberGenerator _rng;
	private          int                   _i;

	/// <summary>
	///   Initializes a new instance of the <see cref="CryptographicRandomizer" /> class.
	/// </summary>
	public CryptographicRandomizer() : this(RandomNumberGenerator.Create()) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="CryptographicRandomizer" /> class.
	/// </summary>
	/// <param name="rng">The RNG.</param>
	/// <exception cref="System.ArgumentNullException">rng</exception>
	public CryptographicRandomizer(RandomNumberGenerator rng)
	{
		_rng = rng ?? throw new ArgumentNullException(nameof(rng));
		_i   = _buf.Length;
	}

	/// <summary>
	///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose() => _rng.Dispose();

	/// <summary>
	///   Returns a random number between 0 and <c>uint.MaxValue</c> inclusive
	/// </summary>
	/// <returns>System.UInt32.</returns>
	public override uint GenerateNum()
	{
		if (_i >= _buf.Length)
		{
			_rng.GetBytes(_buf);
			_i = 0;
		}

		var result = BitConverter.ToUInt32(_buf, _i);
		_i += sizeof(uint);
		return result;
	}
}
