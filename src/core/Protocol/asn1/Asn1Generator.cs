// perticula - core - Asn1Generator.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1Generator.
///   Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public abstract class Asn1Generator : IDisposable
{
	/// <summary>
	///   The base stream
	/// </summary>
	private Stream? _baseStream;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1Generator" /> class.
	/// </summary>
	/// <param name="outStream">The out stream.</param>
	/// <exception cref="System.ArgumentNullException">outStream</exception>
	protected Asn1Generator(Stream outStream) => _baseStream = outStream ?? throw new ArgumentNullException(nameof(outStream));

	/// <summary>
	///   Gets the out stream.
	/// </summary>
	/// <value>The out stream.</value>
	/// <exception cref="System.InvalidOperationException"></exception>
	protected Stream OutStream => _baseStream ?? throw new InvalidOperationException();

	/// <summary>
	///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	///   Finishes this instance.
	/// </summary>
	protected abstract void Finish();

	/// <summary>
	///   Adds the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	public abstract void AddObject(Asn1Encodable obj);

	/// <summary>
	///   Adds the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	public abstract void AddObject(Asn1Object obj);

	/// <summary>
	///   Gets the raw output stream.
	/// </summary>
	/// <returns>Stream.</returns>
	public abstract Stream GetRawOutputStream();

	/// <summary>
	///   Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing">
	///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
	///   unmanaged resources.
	/// </param>
	protected virtual void Dispose(bool disposing)
	{
		switch (disposing)
		{
			case true:
			{
				if (_baseStream != null)
				{
					Finish();
					_baseStream = null;
				}

				break;
			}
		}
	}
}
