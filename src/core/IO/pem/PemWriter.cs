// perticula - core - PemWriter.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Encoding;

namespace core.IO.pem;

/// <summary>
///   Class PemWriter.
///   Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public class PemWriter : IDisposable
{
	/// <summary>
	///   The line length
	/// </summary>
	private const int LineLength = 64;

	/// <summary>
	///   The buf
	/// </summary>
	private readonly char[] _buf = new char[LineLength];

	/// <summary>
	///   The nl length
	/// </summary>
	private readonly int _nlLength;

	/// <summary>
	///   Initializes a new instance of the <see cref="PemWriter" /> class.
	/// </summary>
	/// <param name="writer">The writer.</param>
	/// <exception cref="System.ArgumentNullException">writer</exception>
	public PemWriter(TextWriter writer)
	{
		Writer    = writer ?? throw new ArgumentNullException(nameof(writer));
		_nlLength = Environment.NewLine.Length;
	}

	/// <summary>
	///   Gets the writer.
	/// </summary>
	/// <value>The writer.</value>
	public TextWriter Writer { get; }

	/// <summary>
	///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	///   Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing">
	///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
	///   unmanaged resources.
	/// </param>
	protected virtual void Dispose(bool disposing)
	{
		if (disposing) Writer.Dispose();
	}

	/// <summary>
	///   Gets the size of the output.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>System.Int32.</returns>
	public int GetOutputSize(PemObject obj)
	{
		// BEGIN and END boundaries.
		var size = 2 * (obj.Type.Length + 10 + _nlLength) + 6 + 4;

		if (obj.Headers.Count > 0)
		{
			size += obj.Headers.Sum(header => header.Name.Length + ": ".Length + header.Value.Length + _nlLength);

			size += _nlLength;
		}

		// base64 encoding
		var dataLen = (obj.Content.Length + 2) / 3 * 4;

		size += dataLen + (dataLen + LineLength - 1) / LineLength * _nlLength;

		return size;
	}

	/// <summary>
	///   Writes the object.
	/// </summary>
	/// <param name="objGen">The object gen.</param>
	public void WriteObject(IGenerator<PemObject> objGen)
	{
		var obj = objGen.Generate();

		WritePreEncapsulationBoundary(obj.Type);

		if (obj.Headers.Count > 0)
		{
			foreach (var header in obj.Headers)
			{
				Writer.Write(header.Name);
				Writer.Write(": ");
				Writer.WriteLine(header.Value);
			}

			Writer.WriteLine();
		}

		WriteEncoded(obj.Content);
		WritePostEncapsulationBoundary(obj.Type);
	}

	/// <summary>
	///   Writes the encoded.
	/// </summary>
	/// <param name="bytes">The bytes.</param>
	private void WriteEncoded(byte[] bytes)
	{
		bytes = Base64.Encode(bytes);

		for (var i = 0; i < bytes.Length; i += _buf.Length)
		{
			var index = 0;
			while (index != _buf.Length)
			{
				if (i + index >= bytes.Length)
					break;

				_buf[index] = (char) bytes[i + index];
				index++;
			}

			Writer.WriteLine(_buf, 0, index);
		}
	}

	/// <summary>
	///   Writes the pre encapsulation boundary.
	/// </summary>
	/// <param name="type">The type.</param>
	private void WritePreEncapsulationBoundary(string type) => Writer.WriteLine($"-----BEGIN {type}-----");

	/// <summary>
	///   Writes the post encapsulation boundary.
	/// </summary>
	/// <param name="type">The type.</param>
	private void WritePostEncapsulationBoundary(string type) => Writer.WriteLine($"-----END {type}-----");
}
