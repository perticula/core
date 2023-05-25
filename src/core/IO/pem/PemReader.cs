// perticula - core - PemReader.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Encoding;

namespace core.IO.pem;

/// <summary>
///   Class PemReader.
///   Implements the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public class PemReader : IDisposable
{
	/// <summary>
	///   The line length
	/// </summary>
	private const int LineLength = 64;

	/// <summary>
	///   The buffer
	/// </summary>
	private readonly MemoryStream _buffer;

	/// <summary>
	///   The pushback
	/// </summary>
	private readonly Stack<int> _pushback = new();

	/// <summary>
	///   The text buffer
	/// </summary>
	private readonly StreamWriter _textBuffer;

	/// <summary>
	///   Initializes a new instance of the <see cref="PemReader" /> class.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <exception cref="System.ArgumentNullException">reader</exception>
	public PemReader(TextReader reader)
	{
		Reader = reader ?? throw new ArgumentNullException(nameof(reader));

		_buffer     = new MemoryStream();
		_textBuffer = new StreamWriter(_buffer);
	}

	/// <summary>
	///   Gets the reader.
	/// </summary>
	/// <value>The reader.</value>
	public TextReader Reader { get; init; }

	/// <summary>
	///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	///   Reads the pem object.
	/// </summary>
	/// <returns>System.Nullable&lt;PemObject&gt;.</returns>
	/// <exception cref="System.IO.IOException">no data after consuming leading dashes</exception>
	/// <exception cref="System.IO.IOException">ran out of data before consuming type</exception>
	/// <exception cref="System.IO.IOException">ran out of data consuming header</exception>
	/// <exception cref="System.IO.IOException">ran out of data reading header key value</exception>
	/// <exception cref="System.IO.IOException">expected colon</exception>
	/// <exception cref="System.IO.IOException">ran out of data before consuming header value</exception>
	/// <exception cref="System.IO.IOException">ran out of data before consuming payload</exception>
	/// <exception cref="System.IO.IOException">did not find leading '-'</exception>
	/// <exception cref="System.IO.IOException">no data after consuming trailing dashes</exception>
	/// <exception cref="System.IO.IOException">END " + type + " was not found.</exception>
	/// <exception cref="System.IO.IOException">did not find ending '-'</exception>
	public PemObject? ReadPemObject()
	{
		//
		// Look for BEGIN
		//

		for (;;)
		{
			// Seek a leading dash, ignore anything up to that point.
			if (!SeekDash()) return null;

			// consume dash [-----]BEGIN ...
			if (!ConsumeDash()) throw new IOException("no data after consuming leading dashes");

			SkipWhiteSpace();

			if (Expect("BEGIN"))
				break;
		}

		SkipWhiteSpace();

		//
		// Consume type, accepting whitespace
		//

		if (!BufferUntilStopChar('-', false)) throw new IOException("ran out of data before consuming type");

		var type = BufferedString().Trim();

		// Consume dashes after type.

		if (!ConsumeDash()) throw new IOException("ran out of data consuming header");

		SkipWhiteSpace();

		//
		// Read ahead looking for headers.
		// Look for a colon for up to 64 characters, as an indication there might be a header.
		//

		var headers = new List<PemHeader>();

		while (SeekColon(LineLength))
		{
			if (!BufferUntilStopChar(':', false)) throw new IOException("ran out of data reading header key value");

			var key = BufferedString().Trim();

			var c = Read();
			if (c != ':') throw new IOException("expected colon");

			//
			// We are going to look for well formed headers, if they do not end with a "LF" we cannot
			// discern where they end.
			//

			if (!BufferUntilStopChar('\n', false)) // Now read to the end of the line.
				throw new IOException("ran out of data before consuming header value");

			SkipWhiteSpace();

			var value = BufferedString().Trim();
			headers.Add(new PemHeader(key, value));
		}

		//
		// Consume payload, ignoring all white space until we encounter a '-'
		//

		SkipWhiteSpace();

		if (!BufferUntilStopChar('-', true)) throw new IOException("ran out of data before consuming payload");

		var payload = BufferedString();

		// Seek the start of the end.
		if (!SeekDash()) throw new IOException("did not find leading '-'");

		if (!ConsumeDash()) throw new IOException("no data after consuming trailing dashes");

		if (!Expect($"END {type}")) throw new IOException($"END {type} was not found.");

		if (!SeekDash()) throw new IOException("did not find ending '-'");

		// consume trailing dashes.
		ConsumeDash();

		return new PemObject(type, headers, Base64.Decode(payload));
	}


	/// <summary>
	///   Buffereds the string.
	/// </summary>
	/// <returns>System.String.</returns>
	/// <exception cref="System.InvalidOperationException"></exception>
	private string BufferedString()
	{
		_textBuffer.Flush();

		if (!_buffer.TryGetBuffer(out var data)) throw new InvalidOperationException();

		var value = System.Text.Encoding.UTF8.GetString(data);

		_buffer.Position = 0;
		_buffer.SetLength(0);

		return value;
	}

	/// <summary>
	///   Seeks the dash.
	/// </summary>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private bool SeekDash()
	{
		int c;
		while ((c = Read()) >= 0)
			if (c == '-')
				break;

		PushBack(c);

		return c >= 0;
	}

	/// <summary>
	///   Seeks the colon.
	/// </summary>
	/// <param name="upTo">Up to.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private bool SeekColon(int upTo)
	{
		var c          = 0;
		var colonFound = false;
		var read       = new List<int>();

		for (; upTo >= 0 && c >= 0; upTo--)
		{
			c = Read();
			read.Add(c);
			if (c != ':') continue;
			colonFound = true;
			break;
		}

		var readPos = read.Count;
		while (--readPos >= 0) PushBack(read[readPos]);

		return colonFound;
	}

	/// <summary>
	///   Consumes the dash.
	/// </summary>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private bool ConsumeDash()
	{
		int c;
		while ((c = Read()) >= 0)
			if (c != '-')
				break;

		PushBack(c);

		return c >= 0;
	}

	/// <summary>
	///   Skips the white space.
	/// </summary>
	private void SkipWhiteSpace()
	{
		int c;
		while ((c = Read()) >= 0)
			if (c > ' ')
				break;

		PushBack(c);
	}

	/// <summary>
	///   Expects the specified value.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private bool Expect(string value) => value.All(@char => Read() == @char);

	/// <summary>
	///   Buffers the until stop character.
	/// </summary>
	/// <param name="stopChar">The stop character.</param>
	/// <param name="skipWhiteSpace">if set to <c>true</c> [skip white space].</param>
	/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
	private bool BufferUntilStopChar(char stopChar, bool skipWhiteSpace)
	{
		int c;
		while ((c = Read()) >= 0)
		{
			if (skipWhiteSpace && c <= ' ') continue;

			if (c == stopChar)
			{
				PushBack(c);
				break;
			}

			_textBuffer.Write((char) c);
			_textBuffer.Flush();
		}

		return c >= 0;
	}

	/// <summary>
	///   Pushes the back.
	/// </summary>
	/// <param name="value">The value.</param>
	private void PushBack(int value) => _pushback.Push(value);

	/// <summary>
	///   Reads this instance.
	/// </summary>
	/// <returns>System.Int32.</returns>
	private int Read() => _pushback.Count > 0 ? _pushback.Pop() : Reader.Read();

	/// <summary>
	///   Releases the unmanaged resources.
	/// </summary>
	protected virtual void ReleaseUnmanagedResources() { }

	/// <summary>
	///   Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing">
	///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
	///   unmanaged resources.
	/// </param>
	protected virtual void Dispose(bool disposing)
	{
		ReleaseUnmanagedResources();
		if (disposing) Reader.Dispose();
	}

	/// <summary>
	///   Finalizes an instance of the <see cref="PemReader" /> class.
	/// </summary>
	~PemReader()
	{
		Dispose(false);
	}
}
