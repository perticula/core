// perticula - core - LimitedInputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO;

public class LimitedInputStream : BaseInputStream
{
	protected readonly Stream Stream;

	internal LimitedInputStream(Stream stream, int limit)
	{
		Stream       = stream;
		CurrentLimit = limit;
	}

	internal int CurrentLimit { get; private set; }

	public override int Read(byte[] buffer, int offset, int count)
	{
		var numRead = Stream.Read(buffer, offset, count);
		return numRead switch
		       {
			       > 0 when (CurrentLimit -= numRead) < 0 => throw new StreamOverflowException("Data Overflow"),
			       _                                      => numRead
		       };
	}

	protected void SetParentEofDetect()
	{
		if (Stream is IndefiniteLengthInputStream stream) stream.SetEofOn00(true);
	}

	public override int Read(Span<byte> buffer)
	{
		var numRead = Stream.Read(buffer);
		return numRead switch
		       {
			       > 0 when (CurrentLimit -= numRead) < 0 => throw new StreamOverflowException("Data Overflow"),
			       _                                      => numRead
		       };
	}

	public override int ReadByte()
	{
		var b = Stream.ReadByte();
		if (b              < 0) return b;
		if (--CurrentLimit < 0) throw new StreamOverflowException("Data Overflow");
		return b;
	}
}
