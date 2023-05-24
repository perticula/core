// perticula - core - PemParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Text;
using core.Encoding;
using core.Protocol.asn1;

namespace core.Protocol.x509;

/// <summary>
///   Class PemParser.
/// </summary>
public class PemParser
{
	/// <summary>
	///   The footer1
	/// </summary>
	private readonly string _footer1;

	/// <summary>
	///   The footer2
	/// </summary>
	private readonly string _footer2;

	/// <summary>
	///   The header1
	/// </summary>
	private readonly string _header1;

	/// <summary>
	///   The header2
	/// </summary>
	private readonly string _header2;

	/// <summary>
	///   Initializes a new instance of the <see cref="PemParser" /> class.
	/// </summary>
	/// <param name="type">The type.</param>
	public PemParser(string type)
	{
		_header1 = $"-----BEGIN {type}-----";
		_header2 = $"-----BEGIN X509 {type}-----";
		_footer1 = $"-----END {type}-----";
		_footer2 = $"-----END X509 {type}-----";
	}

	/// <summary>
	///   Reads the line.
	/// </summary>
	/// <param name="inStream">The in stream.</param>
	/// <returns>System.Nullable&lt;System.String&gt;.</returns>
	private static string? ReadLine(Stream inStream)
	{
		int c;
		var l = new StringBuilder();

		do
		{
			while ((c = inStream.ReadByte()) != '\r' && c != '\n' && c >= 0)
			{
				if (c == '\r') continue;

				l.Append((char) c);
			}
		} while (c >= 0 && l.Length == 0);

		return c < 0 ? null : l.ToString();
	}

	/// <summary>
	///   Reads in all lines of the stream until it finds the header of the PEM object.
	///   Then reads in all lines until it finds the footer.
	///   Finally, it decodes the base64-encoded content between the header and footer into an Asn1Sequence object.
	/// </summary>
	/// <param name="inStream">The in stream.</param>
	/// <returns>System.Nullable&lt;Asn1Sequence&gt;.</returns>
	/// <exception cref="System.IO.IOException">malformed PEM data encountered</exception>
	public Asn1Sequence? ReadPemObject(Stream inStream)
	{
		string? line;
		var     pemBuf = new StringBuilder();

		while ((line = ReadLine(inStream)) != null)
		{
			if (line.StartsWith(_header1) || line.StartsWith(_header2))
				break;
		}

		while ((line = ReadLine(inStream)) != null)
		{
			if (line.StartsWith(_footer1) || line.StartsWith(_footer2)) break;

			pemBuf.Append(line);
		}

		if (pemBuf.Length == 0) return null;

		var o = Asn1Object.FromByteArray(Base64.Decode(pemBuf.ToString()));
		if (o is not Asn1Sequence sequence) throw new IOException("malformed PEM data encountered");

		return sequence;
	}
}
