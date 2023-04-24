// perticula - core - DigestAlgorithm.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Cryptography;

public enum DigestAlgorithm
{
	Invalid = 0x000, // all enums should define the default case as an invalid value.

	// ReSharper disable InconsistentNaming
	NONE = 0x0001,

	BLAKE2B_160 = 0x010,
	BLAKE2B_256 = 0x011,
	BLAKE2B_384 = 0x012,
	BLAKE2B_512 = 0x013,
	BLAKE2S_128 = 0x014,
	BLAKE2S_160 = 0x015,
	BLAKE2S_224 = 0x016,
	BLAKE2S_256 = 0x017,
	BLAKE3_256  = 0x018,

	DSTU7564_256 = 0x020,
	DSTU7564_384 = 0x021,
	DSTU7564_512 = 0x022,

	GOST3411          = 0x30,
	GOST3411_2012_256 = 0x31,
	GOST3411_2012_512 = 0x32,

	KECCAK_224 = 0x041,
	KECCAK_256 = 0x042,
	KECCAK_288 = 0x043,
	KECCAK_384 = 0x044,
	KECCAK_512 = 0x045,

	MD2 = 0x051,
	MD4 = 0x052,
	MD5 = 0x053,

	RIPEMD128 = 0x061,
	RIPEMD160 = 0x062,
	RIPEMD256 = 0x063,
	RIPEMD320 = 0x064,

	SHA_1       = 0x071,
	SHA_224     = 0x072,
	SHA_256     = 0x073,
	SHA_384     = 0x074,
	SHA_512     = 0x075,
	SHA_512_224 = 0x076,
	SHA_512_256 = 0x077,

	SHA3_224 = 0x081,
	SHA3_256 = 0x082,
	SHA3_384 = 0x083,
	SHA3_512 = 0x084,

	SHAKE128_256 = 0x091,
	SHAKE256_512 = 0x092,

	SM3 = 0x100,

	TIGER = 0x110,

	WHIRLPOOL = 0x120
	// ReSharper restore InconsistentNaming
}
