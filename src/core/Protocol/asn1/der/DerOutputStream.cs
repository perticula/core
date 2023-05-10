// perticula - core - DerOutputStream.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerOutputStream.
///   Implements the <see cref="core.Protocol.asn1.Asn1OutputStream" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.Asn1OutputStream" />
public class DerOutputStream : Asn1OutputStream
{
	/// <summary>
	///   Initializes a new instance of the <see cref="DerOutputStream" /> class.
	/// </summary>
	/// <param name="os">The os.</param>
	/// <param name="leaveOpen">if set to <c>true</c> [leave open].</param>
	public DerOutputStream(Stream os, bool leaveOpen) : base(os, leaveOpen) { }

	/// <summary>
	///   Gets the encoding.
	/// </summary>
	/// <value>The encoding.</value>
	internal override int Encoding => EncodingDer;
}
