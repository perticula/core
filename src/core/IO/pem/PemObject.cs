// perticula - core - PemObject.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.IO.pem;

/// <summary>
///   Class PemObject.
///   Implements the <see cref="core.IGenerator{core.IO.pem.PemObject}" />
/// </summary>
/// <seealso cref="core.IGenerator{core.IO.pem.PemObject}" />
public class PemObject : IGenerator<PemObject>
{
	/// <summary>
	///   Initializes a new instance of the <see cref="PemObject" /> class.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="content">The content.</param>
	public PemObject(string type, byte[] content) : this(type, new List<PemHeader>(), content) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="PemObject" /> class.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="headers">The headers.</param>
	/// <param name="content">The content.</param>
	public PemObject(string type, IEnumerable<PemHeader> headers, byte[] content)
	{
		Type    = type;
		Headers = new List<PemHeader>(headers);
		Content = content;
	}

	/// <summary>
	///   Gets the type.
	/// </summary>
	/// <value>The type.</value>
	public string Type { get; }

	/// <summary>
	///   Gets the headers.
	/// </summary>
	/// <value>The headers.</value>
	public IList<PemHeader> Headers { get; }

	/// <summary>
	///   Gets the content.
	/// </summary>
	/// <value>The content.</value>
	public byte[] Content { get; }

	/// <summary>
	///   Generates an instance of the given type.
	/// </summary>
	/// <returns>TType.</returns>
	public virtual PemObject Generate() => this;
}
