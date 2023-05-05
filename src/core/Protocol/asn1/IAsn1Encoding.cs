// perticula - core - IAsn1Encoding.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
/// Interface IAsn1Encoding
/// Implements the <see cref="core.Protocol.asn1.IEncoding{core.Protocol.asn1.Asn1OutputStream}" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IEncoding{core.Protocol.asn1.Asn1OutputStream}" />
public interface IAsn1Encoding : IEncoding<Asn1OutputStream> { }
