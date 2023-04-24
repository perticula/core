// perticula - core - OidTokenizer.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol;

/// <summary>
///   Class OidTokenizer.
///   Breaks an OID string into its component tokens.
/// </summary>
public class OidTokenizer
{
	private readonly string _oid;
	private          int    _index;

	/// <summary>
	///   Initializes a new instance of the <see cref="OidTokenizer" /> class.
	/// </summary>
	/// <param name="oid">The oid.</param>
	public OidTokenizer(string oid) => _oid = oid;

	/// <summary>
	///   Gets a value indicating whether this instance has more tokens.
	/// </summary>
	/// <value><c>true</c> if this instance has more tokens; otherwise, <c>false</c>.</value>
	public bool HasMoreTokens => _index != -1;

	/// <summary>
	///   Gets the Nexts token.
	/// </summary>
	/// <returns>System.Nullable&lt;System.String&gt;.</returns>
	public string? NextToken()
	{
		if (_index == -1) return null;

		var end = _oid.IndexOf('.', _index);
		if (end == -1)
		{
			var lastToken = _oid[_index..];
			_index = -1;
			return lastToken;
		}

#pragma warning disable IDE0057 // Disable recommendation for converting check within range.
		var nextToken = _oid.Substring(_index, end - _index);
#pragma warning restore IDE0057
		_index = end + 1;
		return nextToken;
	}
}
