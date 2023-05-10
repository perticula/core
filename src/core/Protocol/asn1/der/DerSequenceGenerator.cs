// perticula - core - DerSequenceGenerator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.der;

/// <summary>
///   Class DerSequenceGenerator.
///   Implements the <see cref="core.Protocol.asn1.der.DerGenerator" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.der.DerGenerator" />
public class DerSequenceGenerator : DerGenerator
{
	/// <summary>
	///   The base stream
	/// </summary>
	private readonly MemoryStream _baseStream = new();

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequenceGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	public DerSequenceGenerator(Stream outStream) : base(outStream) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="DerSequenceGenerator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <param name="tagNo">The tag no.</param>
	/// <param name="isExplicit">if set to <c>true</c> [is explicit].</param>
	public DerSequenceGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit) { }

	/// <summary>
	///   Finishes this instance.
	/// </summary>
	protected override void Finish() => WriteDerEncoded(Asn1Tags.Constructed | Asn1Tags.Sequence, _baseStream.ToArray());

	/// <summary>
	///   Adds the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	public override void AddObject(Asn1Encodable obj) => obj.EncodeTo(_baseStream, Asn1Encodable.Der);

	/// <summary>
	///   Adds the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	public override void AddObject(Asn1Object obj) => obj.EncodeTo(_baseStream, Asn1Encodable.Der);

	/// <summary>
	///   Gets the raw output stream.
	/// </summary>
	/// <returns>Stream.</returns>
	public override Stream GetRawOutputStream() => _baseStream;
}
