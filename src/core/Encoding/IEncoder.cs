// perticula - core - IEncoder.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Encoding;

public interface IEncoder
{
	int Encode(byte[] data, int off, int length, Stream outStream);

	int Encode(ReadOnlySpan<byte> data, Stream outStream);

	int Decode(byte[] data, int off, int length, Stream outStream);

	int Decode(ReadOnlySpan<byte> data, Stream outStream);

	int DecodeString(string data, Stream outStream);
}
