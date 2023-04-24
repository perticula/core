namespace core.Protocol.asn1;

/// <summary>
///   Class Asn1SequenceParser.
///   Implements the <see cref="core.Protocol.asn1.IAsn1SequenceParser" />
/// </summary>
/// <seealso cref="core.Protocol.asn1.IAsn1SequenceParser" />
public class Asn1SequenceParser : IAsn1SequenceParser
{
	/// <summary>
	///   The maximum
	/// </summary>
	private readonly int _max;

	/// <summary>
	///   The outer
	/// </summary>
	private readonly Asn1Sequence _outer;

	/// <summary>
	///   The index
	/// </summary>
	private int _index;

	/// <summary>
	///   Initializes a new instance of the <see cref="Asn1SequenceParser" /> class.
	/// </summary>
	/// <param name="outer">The outer.</param>
	public Asn1SequenceParser(Asn1Sequence outer)
	{
		_outer = outer;
		_max   = outer.Count;
	}

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

	/// <summary>
	///   defined the conversion to and asn.1 object.
	/// </summary>
	/// <returns>Asn1Object.</returns>
	public Asn1Object ToAsn1Object() => _outer;
}
