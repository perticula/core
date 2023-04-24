// perticula - core - Asn1SetParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

public class Asn1SetParser : IAsn1SetParser
{
	private readonly int     _max;
	private readonly Asn1Set _outer;
	private          int     _index;

	public Asn1SetParser(
		Asn1Set outer)
	{
		_outer = outer;
		_max   = outer.Count;
	}

	public virtual Asn1Object ToAsn1Object() => _outer;

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
