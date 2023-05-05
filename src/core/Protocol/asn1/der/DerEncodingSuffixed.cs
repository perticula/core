// perticula - core - DerEncodingSuffixed.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics;

namespace core.Protocol.asn1.der;

public class DerEncodingSuffixed : DerEncoding
{
		private readonly byte _contentsSuffix;

		internal DerEncodingSuffixed(int tagClass, int tagNo, byte[] contentsOctets, byte contentsSuffix)
			: base(tagClass, tagNo, contentsOctets)
		{
				Debug.Assert(contentsOctets != null);
				Debug.Assert(contentsOctets.Length > 0);
				_contentsSuffix = contentsSuffix;
		}

		protected override int CompareLengthAndContents(DerEncoding other)
		{
				switch (other)
				{
						case DerEncodingSuffixed suff: return CompareSuffixed(ContentsOctets, _contentsSuffix, suff.ContentsOctets, suff._contentsSuffix);
						case not null:
								{
										var length = other.ContentsOctets.Length;
										return length == 0 ? ContentsOctets.Length : CompareSuffixed(ContentsOctets, _contentsSuffix, other.ContentsOctets, other.ContentsOctets[length - 1]);
								}
						default: throw new InvalidOperationException();
				}
		}

		public override void Encode(Asn1OutputStream asn1Out)
		{
				asn1Out.WriteIdentifier(TagClass, TagNo);
				asn1Out.WriteDL(ContentsOctets.Length);
				asn1Out.Write(ContentsOctets, 0, ContentsOctets.Length - 1);
				asn1Out.WriteByte(_contentsSuffix);
		}

		public override int GetLength() => Asn1OutputStream.GetLengthOfEncodingDL(TagNo, ContentsOctets.Length);

		private static int CompareSuffixed(byte[] octetsA, byte suffixA, byte[] octetsB, byte suffixB)
		{
				Debug.Assert(octetsA.Length > 0);
				Debug.Assert(octetsB.Length > 0);

				var length = octetsA.Length;
				if (length != octetsB.Length) return length - octetsB.Length;

				var last = length - 1;
				var c = octetsA.AsSpan(0, last).SequenceCompareTo(
					octetsB.AsSpan(0, last));
				if (c != 0)
						return c;


				return suffixA - suffixB;
		}
}
