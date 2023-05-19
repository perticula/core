// perticula - core - BerOctetStringGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

/// <summary>
///   Class BerOctetStringGenerator.
///   Implements the <see cref="core.Protocol.asn1.ber.BerGenerator" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.ber.BerGenerator" />
public class BerOctetStringGenerator : BerGenerator
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BerOctetStringGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	public BerOctetStringGenerator(Stream outStream) : base(outStream) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.OctetString);

	/// <summary>
	///   Initializes a new instance of the <see cref="BerOctetStringGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="isExplicit">The is explicit.</param>
	public BerOctetStringGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.OctetString);

	/// <summary>
	///   Gets the octet output stream.
	/// </summary>
	/// <returns>Stream.</returns>
	public Stream GetOctetOutputStream() => GetOctetOutputStream(new byte[1000]); // limit for CER encoding.

	/// <summary>
	///   Gets the octet output stream.
	/// </summary>
	/// <param name="bufSize">Size of the buf.</param>
	/// <returns>Stream.</returns>
	public Stream GetOctetOutputStream(int bufSize) => bufSize < 1 ? GetOctetOutputStream() : GetOctetOutputStream(new byte[bufSize]);

	/// <summary>
	///   Gets the octet output stream.
	/// </summary>
	/// <param name="buf">The buf.</param>
	/// <returns>Stream.</returns>
	public Stream GetOctetOutputStream(byte[] buf) => new BufferedBerOctetStream(GetRawOutputStream(), buf);

	/// <summary>
	///   Class BufferedBerOctetStream.
	///   Implements the <see cref="BaseOutputStream" />
	/// </summary>
	/// <seealso cref="BaseOutputStream" />
	private class BufferedBerOctetStream : BaseOutputStream
	{
		/// <summary>
		///   The buf
		/// </summary>
		private readonly byte[] _buf;

		/// <summary>
		///   The der out
		/// </summary>
		private readonly Asn1OutputStream _derOut;

		/// <summary>
		///   The off
		/// </summary>
		private int _off;

		/// <summary>
		///   Initializes a new instance of the <see cref="BufferedBerOctetStream" /> class.
		/// </summary>
		/// <param name="outStream">The out stream.</param>
		/// <param name="buf">The buf.</param>
		internal BufferedBerOctetStream(Stream outStream, byte[] buf)
		{
			_buf    = buf;
			_off    = 0;
			_derOut = Asn1OutputStream.Create(outStream, Asn1Encodable.Der, true);
		}

		/// <summary>
		///   When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
		///   position within this stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">
		///   An array of bytes. This method copies <paramref name="count" /> bytes from
		///   <paramref name="buffer" /> to the current stream.
		/// </param>
		/// <param name="offset">
		///   The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the
		///   current stream.
		/// </param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			Streams.ValidateBufferArguments(buffer, offset, count);

			Write(buffer.AsSpan(offset, count));
		}

		/// <summary>
		///   When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
		///   position within this stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">A region of memory. This method copies the contents of this region to the current stream.</param>
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			var bufLen    = _buf.Length;
			var available = bufLen - _off;
			if (buffer.Length < available)
			{
				buffer.CopyTo(_buf.AsSpan(_off));
				_off += buffer.Length;
				return;
			}

			if (_off > 0)
			{
				DerOctetString.Encode(_derOut, _buf.AsSpan(0, _off), buffer[..available]);
				buffer = buffer[available..];
				//_off = 0;
			}

			while (buffer.Length >= bufLen)
			{
				DerOctetString.Encode(_derOut, buffer[..bufLen]);
				buffer = buffer[bufLen..];
			}

			buffer.CopyTo(_buf.AsSpan());
			_off = buffer.Length;
		}

		/// <summary>
		///   Writes a byte to the current position in the stream and advances the position within the stream by one byte.
		/// </summary>
		/// <param name="value">The byte to write to the stream.</param>
		public override void WriteByte(byte value)
		{
			_buf[_off++] = value;

			if (_off == _buf.Length)
			{
				DerOctetString.Encode(_derOut, _buf, 0, _off);
				_off = 0;
			}
		}

		/// <summary>
		///   Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed
		///   resources.
		/// </summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources;
		///   <see langword="false" /> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_off != 0)
				{
					DerOctetString.Encode(_derOut, _buf, 0, _off);
					_off = 0;
				}

				_derOut.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
