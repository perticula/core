// perticula - core - DigestAlgorithm.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Cryptography;

/// <summary>
///   Enum DigestAlgorithm
/// </summary>
public enum DigestAlgorithm
{
	/// <summary>
	///   The invalid
	/// </summary>
	Invalid = 0x000, // all enums should define the default case as an invalid value.

	// ReSharper disable InconsistentNaming
	/// <summary>
	///   The none
	/// </summary>
	NONE = 0x0001,

	/// <summary>
	///   The blak e2 b 160
	/// </summary>
	BLAKE2B_160 = 0x010,

	/// <summary>
	///   The blak e2 b 256
	/// </summary>
	BLAKE2B_256 = 0x011,

	/// <summary>
	///   The blak e2 b 384
	/// </summary>
	BLAKE2B_384 = 0x012,

	/// <summary>
	///   The blak e2 b 512
	/// </summary>
	BLAKE2B_512 = 0x013,

	/// <summary>
	///   The blak e2 s 128
	/// </summary>
	BLAKE2S_128 = 0x014,

	/// <summary>
	///   The blak e2 s 160
	/// </summary>
	BLAKE2S_160 = 0x015,

	/// <summary>
	///   The blak e2 s 224
	/// </summary>
	BLAKE2S_224 = 0x016,

	/// <summary>
	///   The blak e2 s 256
	/// </summary>
	BLAKE2S_256 = 0x017,

	/// <summary>
	///   The blak e3 256
	/// </summary>
	BLAKE3_256 = 0x018,

	/// <summary>
	///   The DST u7564 256
	/// </summary>
	DSTU7564_256 = 0x020,

	/// <summary>
	///   The DST u7564 384
	/// </summary>
	DSTU7564_384 = 0x021,

	/// <summary>
	///   The DST u7564 512
	/// </summary>
	DSTU7564_512 = 0x022,

	/// <summary>
	///   The gos T3411
	/// </summary>
	GOST3411 = 0x30,

	/// <summary>
	///   The gos T3411 2012 256
	/// </summary>
	GOST3411_2012_256 = 0x31,

	/// <summary>
	///   The gos T3411 2012 512
	/// </summary>
	GOST3411_2012_512 = 0x32,

	/// <summary>
	///   The keccak 224
	/// </summary>
	KECCAK_224 = 0x041,

	/// <summary>
	///   The keccak 256
	/// </summary>
	KECCAK_256 = 0x042,

	/// <summary>
	///   The keccak 288
	/// </summary>
	KECCAK_288 = 0x043,

	/// <summary>
	///   The keccak 384
	/// </summary>
	KECCAK_384 = 0x044,

	/// <summary>
	///   The keccak 512
	/// </summary>
	KECCAK_512 = 0x045,

	/// <summary>
	///   The m d2
	/// </summary>
	MD2 = 0x051,

	/// <summary>
	///   The m d4
	/// </summary>
	MD4 = 0x052,

	/// <summary>
	///   The m d5
	/// </summary>
	MD5 = 0x053,

	/// <summary>
	///   The ripem D128
	/// </summary>
	RIPEMD128 = 0x061,

	/// <summary>
	///   The ripem D160
	/// </summary>
	RIPEMD160 = 0x062,

	/// <summary>
	///   The ripem D256
	/// </summary>
	RIPEMD256 = 0x063,

	/// <summary>
	///   The ripem D320
	/// </summary>
	RIPEMD320 = 0x064,

	/// <summary>
	///   The sha 1
	/// </summary>
	SHA_1 = 0x071,

	/// <summary>
	///   The sha 224
	/// </summary>
	SHA_224 = 0x072,

	/// <summary>
	///   The sha 256
	/// </summary>
	SHA_256 = 0x073,

	/// <summary>
	///   The sha 384
	/// </summary>
	SHA_384 = 0x074,

	/// <summary>
	///   The sha 512
	/// </summary>
	SHA_512 = 0x075,

	/// <summary>
	///   The sha 512 224
	/// </summary>
	SHA_512_224 = 0x076,

	/// <summary>
	///   The sha 512 256
	/// </summary>
	SHA_512_256 = 0x077,

	/// <summary>
	///   The sh a3 224
	/// </summary>
	SHA3_224 = 0x081,

	/// <summary>
	///   The sh a3 256
	/// </summary>
	SHA3_256 = 0x082,

	/// <summary>
	///   The sh a3 384
	/// </summary>
	SHA3_384 = 0x083,

	/// <summary>
	///   The sh a3 512
	/// </summary>
	SHA3_512 = 0x084,

	/// <summary>
	///   The shak e128 256
	/// </summary>
	SHAKE128_256 = 0x091,

	/// <summary>
	///   The shak e256 512
	/// </summary>
	SHAKE256_512 = 0x092,

	/// <summary>
	///   The s m3
	/// </summary>
	SM3 = 0x100,

	/// <summary>
	///   The tiger
	/// </summary>
	TIGER = 0x110,

	/// <summary>
	///   The whirlpool
	/// </summary>
	WHIRLPOOL = 0x120
	// ReSharper restore InconsistentNaming
}
