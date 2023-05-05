// perticula - core - Asn1SetParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1SetParser.
///   Implements the <see cref="core.Protocol.asn1.IAsn1SetParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1SetParser" />
public class Asn1SetParser : IAsn1SetParser
{
	/// <summary>
	///   The maximum
	/// </summary>
	private readonly int _max;

	/// <summary>
	///   The outer
	/// </summary>
	private readonly Asn1Set _outer;

	/// <summary>
	///   The index
	/// </summary>
	private int _index;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1SetParser" /> class.
	/// </summary>
	/// <param name="outer">The outer.</param>
	public Asn1SetParser(
		Asn1Set outer)
	{
		_outer = outer;
		_max   = outer.Count;
	}

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	public virtual Asn1Object ToAsn1Object() => _outer;

	/// <summary>
	///   Reads the object.
	/// </summary>
	/// <returns>IAsn1Convertable.</returns>
	public IAsn1Convertable? ReadObject()
	{
		if (_index == _max) return null;

		var obj = _outer[_index++];
		return obj switch
		       {
			       Asn1Sequence sequence => sequence.Parser,
			       Asn1Set set           => set.Parser,
			       _                     => obj
		       };
	}
}
