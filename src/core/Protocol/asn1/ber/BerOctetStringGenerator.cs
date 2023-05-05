// perticula - core - BerOctetStringGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.IO;
using core.Protocol.asn1.der;

namespace core.Protocol.asn1.ber;

public class BerOctetStringGenerator : BerGenerator
{
	public BerOctetStringGenerator(Stream outStream) : base(outStream) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.OctetString);

	public BerOctetStringGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit) => WriteBerHeader(Asn1Tags.Constructed | Asn1Tags.OctetString);

	public Stream GetOctetOutputStream() => GetOctetOutputStream(new byte[1000]); // limit for CER encoding.

	public Stream GetOctetOutputStream(int bufSize) => bufSize < 1 ? GetOctetOutputStream() : GetOctetOutputStream(new byte[bufSize]);

	public Stream GetOctetOutputStream(byte[] buf) => new BufferedBerOctetStream(GetRawOutputStream(), buf);

	private class BufferedBerOctetStream : BaseOutputStream
	{
		private readonly byte[]           _buf;
		private readonly Asn1OutputStream _derOut;
		private          int              _off;

		internal BufferedBerOctetStream(Stream outStream, byte[] buf)
		{
			_buf    = buf;
			_off    = 0;
			_derOut = Asn1OutputStream.Create(outStream, Asn1Encodable.Der, true);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Streams.ValidateBufferArguments(buffer, offset, count);

			Write(buffer.AsSpan(offset, count));
		}

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

		public override void WriteByte(byte value)
		{
			_buf[_off++] = value;

			if (_off == _buf.Length)
			{
				DerOctetString.Encode(_derOut, _buf, 0, _off);
				_off = 0;
			}
		}

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
