// perticula - core - DigestUtilities.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Security.Cryptography;
using core.Protocol.asn1.der;

namespace core.Cryptography;

/// <summary>
///   Class DigestUtilities.
/// </summary>
public static class DigestUtilities
{
	/// <summary>
	///   The aliases
	/// </summary>
	private static readonly IDictionary<string, string> Aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	///   The oids
	/// </summary>
	private static readonly IDictionary<string, DerObjectIdentifier> Oids = new Dictionary<string, DerObjectIdentifier>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	///   Initializes static members of the <see cref="DigestUtilities" /> class.
	/// </summary>
	static DigestUtilities() =>
		// Signal to obfuscation tools not to change enum constants
		_ = Enums.GetArbitraryValue<DigestAlgorithm>().ToString();

	//Aliases[PkcsObjectIdentifiers.MD2.Id] = "MD2";
	//Aliases[PkcsObjectIdentifiers.MD4.Id] = "MD4";
	//Aliases[PkcsObjectIdentifiers.MD5.Id] = "MD5";
	//Aliases["SHA1"]                                    = "SHA-1";
	//Aliases[OiwObjectIdentifiers.IdSha1.Id]            = "SHA-1";
	//Aliases[PkcsObjectIdentifiers.IdHmacWithSha1.Id]   = "SHA-1";
	//Aliases[MiscObjectIdentifiers.HMAC_SHA1.Id]        = "SHA-1";
	//Aliases["SHA224"]                                  = "SHA-224";
	//Aliases[NistObjectIdentifiers.IdSha224.Id]         = "SHA-224";
	//Aliases[PkcsObjectIdentifiers.IdHmacWithSha224.Id] = "SHA-224";
	//Aliases["SHA256"]                                  = "SHA-256";
	//Aliases[NistObjectIdentifiers.IdSha256.Id]         = "SHA-256";
	//Aliases[PkcsObjectIdentifiers.IdHmacWithSha256.Id] = "SHA-256";
	//Aliases["SHA384"]                                  = "SHA-384";
	//Aliases[NistObjectIdentifiers.IdSha384.Id]         = "SHA-384";
	//Aliases[PkcsObjectIdentifiers.IdHmacWithSha384.Id] = "SHA-384";
	//Aliases["SHA512"]                                  = "SHA-512";
	//Aliases[NistObjectIdentifiers.IdSha512.Id]         = "SHA-512";
	//Aliases[PkcsObjectIdentifiers.IdHmacWithSha512.Id] = "SHA-512";
	//Aliases["SHA512/224"]                          = "SHA-512/224";
	//Aliases["SHA512(224)"]                         = "SHA-512/224";
	//Aliases["SHA-512(224)"]                        = "SHA-512/224";
	//Aliases[NistObjectIdentifiers.IdSha512_224.Id] = "SHA-512/224";
	//Aliases["SHA512/256"]                          = "SHA-512/256";
	//Aliases["SHA512(256)"]                         = "SHA-512/256";
	//Aliases["SHA-512(256)"]                        = "SHA-512/256";
	//Aliases[NistObjectIdentifiers.IdSha512_256.Id] = "SHA-512/256";
	//Aliases["RIPEMD-128"]                            = "RIPEMD128";
	//Aliases[TeleTrusTObjectIdentifiers.RipeMD128.Id] = "RIPEMD128";
	//Aliases["RIPEMD-160"]                            = "RIPEMD160";
	//Aliases[TeleTrusTObjectIdentifiers.RipeMD160.Id] = "RIPEMD160";
	//Aliases["RIPEMD-256"]                            = "RIPEMD256";
	//Aliases[TeleTrusTObjectIdentifiers.RipeMD256.Id] = "RIPEMD256";
	//Aliases["RIPEMD-320"]                            = "RIPEMD320";
	//Aliases[TeleTrusTObjectIdentifiers.RipeMD320.Id] = "RIPEMD320";
	//Aliases[CryptoProObjectIdentifiers.GostR3411.Id] = "GOST3411";
	//Aliases["KECCAK224"] = "KECCAK-224";
	//Aliases["KECCAK256"] = "KECCAK-256";
	//Aliases["KECCAK288"] = "KECCAK-288";
	//Aliases["KECCAK384"] = "KECCAK-384";
	//Aliases["KECCAK512"] = "KECCAK-512";
	//Aliases[NistObjectIdentifiers.IdSha3_224.Id]         = "SHA3-224";
	//Aliases[NistObjectIdentifiers.IdHMacWithSha3_224.Id] = "SHA3-224";
	//Aliases[NistObjectIdentifiers.IdSha3_256.Id]         = "SHA3-256";
	//Aliases[NistObjectIdentifiers.IdHMacWithSha3_256.Id] = "SHA3-256";
	//Aliases[NistObjectIdentifiers.IdSha3_384.Id]         = "SHA3-384";
	//Aliases[NistObjectIdentifiers.IdHMacWithSha3_384.Id] = "SHA3-384";
	//Aliases[NistObjectIdentifiers.IdSha3_512.Id]         = "SHA3-512";
	//Aliases[NistObjectIdentifiers.IdHMacWithSha3_512.Id] = "SHA3-512";
	//Aliases["SHAKE128"]                                  = "SHAKE128-256";
	//Aliases[NistObjectIdentifiers.IdShake128.Id]         = "SHAKE128-256";
	//Aliases["SHAKE256"]                                  = "SHAKE256-512";
	//Aliases[NistObjectIdentifiers.IdShake256.Id]         = "SHAKE256-512";
	//Aliases[GMObjectIdentifiers.sm3.Id] = "SM3";
	//Aliases[MiscObjectIdentifiers.id_blake2b160.Id] = "BLAKE2B-160";
	//Aliases[MiscObjectIdentifiers.id_blake2b256.Id] = "BLAKE2B-256";
	//Aliases[MiscObjectIdentifiers.id_blake2b384.Id] = "BLAKE2B-384";
	//Aliases[MiscObjectIdentifiers.id_blake2b512.Id] = "BLAKE2B-512";
	//Aliases[MiscObjectIdentifiers.id_blake2s128.Id] = "BLAKE2S-128";
	//Aliases[MiscObjectIdentifiers.id_blake2s160.Id] = "BLAKE2S-160";
	//Aliases[MiscObjectIdentifiers.id_blake2s224.Id] = "BLAKE2S-224";
	//Aliases[MiscObjectIdentifiers.id_blake2s256.Id] = "BLAKE2S-256";
	//Aliases[MiscObjectIdentifiers.blake3_256.Id]    = "BLAKE3-256";
	//Aliases[RosstandartObjectIdentifiers.id_tc26_gost_3411_12_256.Id] = "GOST3411-2012-256";
	//Aliases[RosstandartObjectIdentifiers.id_tc26_gost_3411_12_512.Id] = "GOST3411-2012-512";
	//Aliases[UAObjectIdentifiers.dstu7564digest_256.Id] = "DSTU7564-256";
	//Aliases[UAObjectIdentifiers.dstu7564digest_384.Id] = "DSTU7564-384";
	//Aliases[UAObjectIdentifiers.dstu7564digest_512.Id] = "DSTU7564-512";
	//Oids["MD2"]               = PkcsObjectIdentifiers.MD2;
	//Oids["MD4"]               = PkcsObjectIdentifiers.MD4;
	//Oids["MD5"]               = PkcsObjectIdentifiers.MD5;
	//Oids["SHA-1"]             = OiwObjectIdentifiers.IdSha1;
	//Oids["SHA-224"]           = NistObjectIdentifiers.IdSha224;
	//Oids["SHA-256"]           = NistObjectIdentifiers.IdSha256;
	//Oids["SHA-384"]           = NistObjectIdentifiers.IdSha384;
	//Oids["SHA-512"]           = NistObjectIdentifiers.IdSha512;
	//Oids["SHA-512/224"]       = NistObjectIdentifiers.IdSha512_224;
	//Oids["SHA-512/256"]       = NistObjectIdentifiers.IdSha512_256;
	//Oids["SHA3-224"]          = NistObjectIdentifiers.IdSha3_224;
	//Oids["SHA3-256"]          = NistObjectIdentifiers.IdSha3_256;
	//Oids["SHA3-384"]          = NistObjectIdentifiers.IdSha3_384;
	//Oids["SHA3-512"]          = NistObjectIdentifiers.IdSha3_512;
	//Oids["SHAKE128-256"]      = NistObjectIdentifiers.IdShake128;
	//Oids["SHAKE256-512"]      = NistObjectIdentifiers.IdShake256;
	//Oids["RIPEMD128"]         = TeleTrusTObjectIdentifiers.RipeMD128;
	//Oids["RIPEMD160"]         = TeleTrusTObjectIdentifiers.RipeMD160;
	//Oids["RIPEMD256"]         = TeleTrusTObjectIdentifiers.RipeMD256;
	//Oids["GOST3411"]          = CryptoProObjectIdentifiers.GostR3411;
	//Oids["SM3"]               = GMObjectIdentifiers.sm3;
	//Oids["BLAKE2B-160"]       = MiscObjectIdentifiers.id_blake2b160;
	//Oids["BLAKE2B-256"]       = MiscObjectIdentifiers.id_blake2b256;
	//Oids["BLAKE2B-384"]       = MiscObjectIdentifiers.id_blake2b384;
	//Oids["BLAKE2B-512"]       = MiscObjectIdentifiers.id_blake2b512;
	//Oids["BLAKE2S-128"]       = MiscObjectIdentifiers.id_blake2s128;
	//Oids["BLAKE2S-160"]       = MiscObjectIdentifiers.id_blake2s160;
	//Oids["BLAKE2S-224"]       = MiscObjectIdentifiers.id_blake2s224;
	//Oids["BLAKE2S-256"]       = MiscObjectIdentifiers.id_blake2s256;
	//Oids["BLAKE3-256"]        = MiscObjectIdentifiers.blake3_256;
	//Oids["GOST3411-2012-256"] = RosstandartObjectIdentifiers.id_tc26_gost_3411_12_256;
	//Oids["GOST3411-2012-512"] = RosstandartObjectIdentifiers.id_tc26_gost_3411_12_512;
	//Oids["DSTU7564-256"]      = UAObjectIdentifiers.dstu7564digest_256;
	//Oids["DSTU7564-384"]      = UAObjectIdentifiers.dstu7564digest_384;
	//Oids["DSTU7564-512"]      = UAObjectIdentifiers.dstu7564digest_512;
	/// <summary>
	///   Gets the object identifier.
	/// </summary>
	/// <param name="mechanism">The mechanism.</param>
	/// <returns>System.Nullable&lt;DerObjectIdentifier&gt;.</returns>
	/// <exception cref="System.ArgumentNullException">mechanism</exception>
	public static DerObjectIdentifier? GetObjectIdentifier(string mechanism)
	{
		if (mechanism == null) throw new ArgumentNullException(nameof(mechanism));

		mechanism = Aliases.GetValueOrKey(mechanism).ToUpperInvariant();

		return Oids.GetValueOrNull(mechanism);
	}

	/// <summary>
	///   Gets the digest.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>IDigest.</returns>
	public static IDigest GetDigest(DerObjectIdentifier id) => GetDigest(id.Id);

	/// <summary>
	///   Gets the digest.
	/// </summary>
	/// <param name="algorithm">The algorithm.</param>
	/// <returns>IDigest.</returns>
	/// <exception cref="System.ArgumentNullException">algorithm</exception>
	/// <exception cref="System.Security.Cryptography.CryptographicException">Digest {mechanism} not recognised.</exception>
	public static IDigest GetDigest(string algorithm)
	{
		if (algorithm == null) throw new ArgumentNullException(nameof(algorithm));

		var mechanism = Aliases.GetValueOrKey(algorithm).ToUpperInvariant();
		try
		{
			var digestAlgorithm = Enums.GetEnumValue<DigestAlgorithm>(mechanism);

			return digestAlgorithm switch
			       {
				       //DigestAlgorithm.BLAKE2B_160       => new Blake2bDigest(160),
				       //DigestAlgorithm.BLAKE2B_256       => new Blake2bDigest(256),
				       //DigestAlgorithm.BLAKE2B_384       => new Blake2bDigest(384),
				       //DigestAlgorithm.BLAKE2B_512       => new Blake2bDigest(512),
				       //DigestAlgorithm.BLAKE2S_128       => new Blake2sDigest(128),
				       //DigestAlgorithm.BLAKE2S_160       => new Blake2sDigest(160),
				       //DigestAlgorithm.BLAKE2S_224       => new Blake2sDigest(224),
				       //DigestAlgorithm.BLAKE2S_256       => new Blake2sDigest(256),
				       //DigestAlgorithm.BLAKE3_256        => new Blake3Digest(256),
				       //DigestAlgorithm.DSTU7564_256      => new Dstu7564Digest(256),
				       //DigestAlgorithm.DSTU7564_384      => new Dstu7564Digest(384),
				       //DigestAlgorithm.DSTU7564_512      => new Dstu7564Digest(512),
				       //DigestAlgorithm.GOST3411          => new Gost3411Digest(),
				       //DigestAlgorithm.GOST3411_2012_256 => new Gost3411_2012_256Digest(),
				       //DigestAlgorithm.GOST3411_2012_512 => new Gost3411_2012_512Digest(),
				       //DigestAlgorithm.KECCAK_224        => new KeccakDigest(224),
				       //DigestAlgorithm.KECCAK_256        => new KeccakDigest(256),
				       //DigestAlgorithm.KECCAK_288        => new KeccakDigest(288),
				       //DigestAlgorithm.KECCAK_384        => new KeccakDigest(384),
				       //DigestAlgorithm.KECCAK_512        => new KeccakDigest(512),
				       //DigestAlgorithm.MD2               => new MD2Digest(),
				       //DigestAlgorithm.MD4               => new MD4Digest(),
				       //DigestAlgorithm.MD5               => new MD5Digest(),
				       //DigestAlgorithm.NONE              => new NullDigest(),
				       //DigestAlgorithm.RIPEMD128         => new RipeMD128Digest(),
				       //DigestAlgorithm.RIPEMD160         => new RipeMD160Digest(),
				       //DigestAlgorithm.RIPEMD256         => new RipeMD256Digest(),
				       //DigestAlgorithm.RIPEMD320         => new RipeMD320Digest(),
				       //DigestAlgorithm.SHA_1             => new Sha1Digest(),
				       //DigestAlgorithm.SHA_224           => new Sha224Digest(),
				       //DigestAlgorithm.SHA_256           => new Sha256Digest(),
				       //DigestAlgorithm.SHA_384           => new Sha384Digest(),
				       //DigestAlgorithm.SHA_512           => new Sha512Digest(),
				       //DigestAlgorithm.SHA_512_224       => new Sha512tDigest(224),
				       //DigestAlgorithm.SHA_512_256       => new Sha512tDigest(256),
				       //DigestAlgorithm.SHA3_224          => new Sha3Digest(224),
				       //DigestAlgorithm.SHA3_256          => new Sha3Digest(256),
				       //DigestAlgorithm.SHA3_384          => new Sha3Digest(384),
				       //DigestAlgorithm.SHA3_512          => new Sha3Digest(512),
				       //DigestAlgorithm.SHAKE128_256      => new ShakeDigest(128),
				       //DigestAlgorithm.SHAKE256_512      => new ShakeDigest(256),
				       //DigestAlgorithm.SM3               => new SM3Digest(),
				       //DigestAlgorithm.TIGER             => new TigerDigest(),
				       //DigestAlgorithm.WHIRLPOOL         => new WhirlpoolDigest(),
				       DigestAlgorithm.Invalid => throw new CryptographicException($"Digest {mechanism} not recognised."),
				       _                       => throw new CryptographicException($"Digest {mechanism} not recognised.")
			       };
		}
		catch (ArgumentException) { }

		throw new CryptographicException($"Digest {mechanism} not recognised.");
	}

	/// <summary>
	///   Gets the name of the algorithm.
	/// </summary>
	/// <param name="oid">The oid.</param>
	/// <returns>System.Nullable&lt;System.String&gt;.</returns>
	public static string? GetAlgorithmName(DerObjectIdentifier oid) => Aliases.GetValueOrNull(oid.Id);

	/// <summary>
	///   Calculates the digest.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <param name="input">The input.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] CalculateDigest(DerObjectIdentifier id, byte[] input) => CalculateDigest(id.Id, input);

	/// <summary>
	///   Calculates the digest.
	/// </summary>
	/// <param name="algorithm">The algorithm.</param>
	/// <param name="input">The input.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] CalculateDigest(string algorithm, byte[] input)
	{
		var digest = GetDigest(algorithm);
		return DoFinal(digest, input);
	}

	/// <summary>
	///   Calculates the digest.
	/// </summary>
	/// <param name="algorithm">The algorithm.</param>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] CalculateDigest(string algorithm, byte[] buf, int off, int len)
	{
		var digest = GetDigest(algorithm);
		return DoFinal(digest, buf, off, len);
	}

	/// <summary>
	///   Calculates the digest.
	/// </summary>
	/// <param name="algorithm">The algorithm.</param>
	/// <param name="buffer">The buffer.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] CalculateDigest(string algorithm, ReadOnlySpan<byte> buffer)
	{
		var digest = GetDigest(algorithm);
		return DoFinal(digest, buffer);
	}

	/// <summary>
	///   Does the final.
	/// </summary>
	/// <param name="digest">The digest.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] DoFinal(IDigest digest)
	{
		var b = new byte[digest.GetDigestSize()];
		digest.DoFinal(b, 0);
		return b;
	}

	/// <summary>
	///   Does the final.
	/// </summary>
	/// <param name="digest">The digest.</param>
	/// <param name="input">The input.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] DoFinal(IDigest digest, byte[] input)
	{
		digest.BlockUpdate(input, 0, input.Length);
		return DoFinal(digest);
	}

	/// <summary>
	///   Does the final.
	/// </summary>
	/// <param name="digest">The digest.</param>
	/// <param name="buf">The buf.</param>
	/// <param name="off">The off.</param>
	/// <param name="len">The length.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] DoFinal(IDigest digest, byte[] buf, int off, int len)
	{
		digest.BlockUpdate(buf, off, len);
		return DoFinal(digest);
	}

	/// <summary>
	///   Does the final.
	/// </summary>
	/// <param name="digest">The digest.</param>
	/// <param name="buffer">The buffer.</param>
	/// <returns>System.Byte[].</returns>
	public static byte[] DoFinal(IDigest digest, ReadOnlySpan<byte> buffer)
	{
		digest.BlockUpdate(buffer);
		return DoFinal(digest);
	}
}
