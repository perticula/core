// perticula - core - BerSetParser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1.ber;

public class BerSetParser : Asn1SetParser
{
	private readonly Asn1StreamParser _parser;
	internal BerSetParser(Asn1StreamParser parser) => _parser = parser;
	public override IAsn1Convertable? ReadObject() => _parser.ReadObject();
	public override Asn1Object ToAsn1Object() => Parse(_parser);
	internal static BerSet Parse(Asn1StreamParser sp) => new(sp.LoadVector());
}	
