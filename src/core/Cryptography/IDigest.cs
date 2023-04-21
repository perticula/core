namespace core.Cryptography;

/// <summary>
///   Interface IDigest
///   base message digest interface.
/// </summary>
public interface IDigest
{
	/// <summary>
	///   Gets the name of the algorithm.
	/// </summary>
	/// <value>The name of the algorithm.</value>
	string AlgorithmName { get; }

	/// <summary>
	///   Return the size, in bytes, of the digest produced by this message digest.
	/// </summary>
	/// <returns>the size, in bytes, of the digest produced by this message digest.</returns>
	int GetDigestSize();

	/// <summary>
	///   Return the size, in bytes, of the internal buffer used by this digest.
	/// </summary>
	/// <returns>the size, in bytes, of the internal buffer used by this digest.</returns>
	int GetByteLength();

	/// <summary>
	///   Update the message digest with a single byte.
	/// </summary>
	/// <param name="input">the input byte to be entered.</param>
	void Update(byte input);

	/// <summary>
	///   Update the message digest with a block of bytes.
	/// </summary>
	/// <param name="input">the byte array containing the data.</param>
	/// <param name="inOff">the offset into the byte array where the data starts.</param>
	/// <param name="inLen">the length of the data.</param>
	void BlockUpdate(byte[] input, int inOff, int inLen);

	/// <summary>
	///   Update the message digest with a span of bytes.
	/// </summary>
	/// <param name="input">the span containing the data.</param>
	void BlockUpdate(ReadOnlySpan<byte> input);

	/// <summary>
	///   Close the digest, producing the final digest value.
	/// </summary>
	/// <param name="output">the byte array the digest is to be copied into.</param>
	/// <param name="outOff">the offset into the byte array the digest is to start at.</param>
	/// <returns>the number of bytes written</returns>
	/// <remarks>This call leaves the digest reset.</remarks>
	int DoFinal(byte[] output, int outOff);

	/// <summary>
	///   Close the digest, producing the final digest value.
	/// </summary>
	/// <param name="output">the span the digest is to be copied into.</param>
	/// <returns>the number of bytes written</returns>
	/// <remarks>This call leaves the digest reset.</remarks>
	int DoFinal(Span<byte> output);

	/// <summary>
	///   Reset the digest back to its initial state.
	/// </summary>
	void Reset();
}
