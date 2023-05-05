// perticula - core - IndefiniteLengthInputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO;

public class IndefiniteLengthInputStream : LimitedInputStream
{
	private bool _eofOn00 = true;
	private int  _lookAhead;

	internal IndefiniteLengthInputStream(Stream inStream, int limit) : base(inStream, limit)
	{
		_lookAhead = RequireByte();

		if (0 == _lookAhead) CheckEndOfContents();
	}

	internal void SetEofOn00(bool eofOn00)
	{
		_eofOn00 = eofOn00;
		if (_eofOn00 && 0 == _lookAhead) CheckEndOfContents();
	}

	private void CheckEndOfContents()
	{
		if (0 != RequireByte()) throw new IOException("malformed end-of-contents marker");

		_lookAhead = -1;
		SetParentEofDetect();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		// Only use this optimisation if we aren't checking for 00
		if (_eofOn00 || count <= 1) return base.Read(buffer, offset, count);

		if (_lookAhead < 0) return 0;

		var numRead = Stream.Read(buffer, offset + 1, count - 1);
		if (numRead <= 0) throw new EndOfStreamException();

		buffer[offset] = (byte) _lookAhead;
		_lookAhead     = RequireByte();

		return numRead + 1;
	}

	public override int Read(Span<byte> buffer)
	{
		// Only use this optimisation if we aren't checking for 00
		if (_eofOn00 || buffer.Length <= 1) return base.Read(buffer);
		if (_lookAhead < 0) return 0;

		var numRead = Stream.Read(buffer[1..]);
		if (numRead <= 0) throw new EndOfStreamException();

		buffer[0]  = (byte) _lookAhead;
		_lookAhead = RequireByte();

		return numRead + 1;
	}

	public override int ReadByte()
	{
		if (_eofOn00 && _lookAhead <= 0)
		{
			if (0 == _lookAhead) CheckEndOfContents();
			return -1;
		}

		var result = _lookAhead;
		_lookAhead = RequireByte();
		return result;
	}

	private int RequireByte()
	{
		var b = Stream.ReadByte();
		if (b < 0) throw new EndOfStreamException();

		return b;
	}
}
