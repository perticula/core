// perticula - core - Asn1Type.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

public abstract class Asn1Type
{
	internal Asn1Type(Type platformType) => PlatformType = platformType;
	internal Type PlatformType { get; }

	public sealed override bool Equals(object? that) => this == that;
	public sealed override int  GetHashCode()        => PlatformType.GetHashCode();
}
