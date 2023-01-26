// perticula - core.test - SerializeTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.ComponentModel;
using System.Globalization;
using core.Serialization;

namespace core.test.Serialization;

public class SerializeTests
{
	[Fact]
	public void Convert_ToString_FromString_String_Succeeds()
	{
		var expect = "sample";
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<string>(stored);

		Assert.Equal(expect,   result);
		Assert.Equal("sample", stored);
	}

	[Fact]
	public void Convert_ToString_Succeeds()
	{
		Serialize.ToString("foo");
	}

	[Fact]
	public void Convert_ToString_FromString_Int_Succeeds()
	{
		var expect = 12345;
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<int>(stored);

		Assert.Equal(expect,  result);
		Assert.Equal("12345", stored);
	}

	[Fact]
	public void Convert_ToString_FromString_NullableInt_Succeeds()
	{
		int? expect = 12345;
		var  stored = Serialize.ToString(expect);
		var  result = Serialize.FromString<int?>(stored);

		Assert.Equal(expect,  result);
		Assert.Equal("12345", stored);
	}

	[Fact]
	public void Convert_ToString_FromString_NullableInt_fails_when_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			int? expect = null;
			var  stored = Serialize.ToString(expect);
		});
	}

	[Fact]
	public void Convert_ToString_FromString_Long_Succeeds()
	{
		var expect = 12345L;
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<long>(stored);

		Assert.Equal(expect,  result);
		Assert.Equal("12345", stored);
	}

	[Fact]
	public void Convert_ToString_FromString_Bool_Succeeds()
	{
		var expect = true;
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<bool>(stored);

		Assert.Equal(expect, result);
		Assert.Equal("True", stored);
	}

	[Fact]
	public void Convert_ToString_FromString_Double_Succeeds()
	{
		var expect = 1.2345f;
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<double>(stored);

		Assert.Equal(expect,   result, 6);
		Assert.Equal("1.2345", stored);
	}

	[Fact]
	public void Convert_ToString_FromString_DateTime_Succeeds()
	{
		var expect = DateTime.Now;
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<DateTime>(stored);

		Assert.Equal(expect.ToString(), result.ToString());
	}

	[Fact]
	public void Convert_ToString_FromString_Enum_Succeeds()
	{
		var expect = FileMode.OpenOrCreate;
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<FileMode>(stored);

		Assert.Equal(expect,         result);
		Assert.Equal("OpenOrCreate", stored);
	}

	[Fact]
	public void Convert_ToString_FromString_int_Succeeds()
	{
		var expect = 12345;
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<int>(stored);

		Assert.Equal(expect, result);
		Assert.Equal(expect, result);
	}

	[Fact]
	public void Convert_ToString_FromString_Array_Succeeds()
	{
		var expect = new[] {"this", "should", "store", "as", "json"};
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<string[]>(stored);

		Assert.NotNull(result);
		Assert.True(expect.SequenceEqual(result));
	}

	[Fact]
	public void Convert_ToString_FromString_Struct_Fails()
	{
		Assert.Throws<InvalidCastException>(() =>
		{
			// Because of a problem with detecting structures as type
			// that must be serialized using JSon, structures are
			// not supported.


			var expect = new TestStruct
			{
				A = "hello",
				B = 123,
				C = DateTime.Now.Date,
				D = true
			};
			var stored = Serialize.ToString(expect);
			var result = Serialize.FromString<TestStruct>(stored);
		});
	}

	[Fact]
	public void Convert_ToString_FromString_Dictionary_Succeeds()
	{
		var guidA = Guid.NewGuid();
		var guidB = Guid.NewGuid();
		var expect = new Dictionary<string, Guid>
		{
			{"valueA", guidA},
			{"valueB", guidB}
		};
		var stored = Serialize.ToString(expect);
		var result = Serialize.FromString<Dictionary<string, Guid>>(stored);

		Assert.Equal(expect.Count,     result.Count);
		Assert.Equal(expect["valueA"], result["valueA"]);
		Assert.Equal(expect["valueB"], result["valueB"]);
	}

	[Fact]
	public void Convert_ToString_FromString_TypeConverter_Succeeds()
	{
		var typeX  = new TestStruct2 {X = 123};
		var stored = Serialize.ToString(typeX);
		var result = Serialize.FromString<TestStruct2>(stored);

		Assert.Equal(typeX.X, result.X);
	}

	[Fact]
	public void ConvertToType_Any_To_String_Succeeds()
	{
		var r1  = Serialize.ConvertTo<string>(123);
		var r2  = Serialize.ConvertTo<string>(123.456);
		var r3  = Serialize.ConvertTo<string>(true);
		var r4  = Serialize.ConvertTo<string>(DateTime.Today);
		var r5  = Serialize.ConvertTo<string>((byte) 123);
		var r6  = Serialize.ConvertTo<string>('x');
		Assert.Throws<ArgumentNullException>(() =>
		{
			var r8 = Serialize.ConvertTo<string>(null);
		});
		var r9  = Serialize.ConvertTo<string>((int?) 123);
		var r10 = Serialize.ConvertTo<string>(123);

		Assert.Equal("123",                     r1);
		Assert.Equal("123.456",                 r2);
		Assert.Equal("True",                    r3);
		Assert.Equal(DateTime.Today.ToString(), r4);
		Assert.Equal("123",                     r5);
		Assert.Equal("x",                       r6);
		Assert.Equal("123", r9);
		Assert.Equal("123", r10);
	}

	[Fact]
	public void ConvertToType_String_To_Any_Succeeds()
	{
		var r1  = Serialize.ConvertTo<int>("123");
		var r2  = Serialize.ConvertTo<double>("123.456");
		var r3  = Serialize.ConvertTo<bool>("True");
		var r4  = Serialize.ConvertTo<DateTime>(DateTime.Today.ToString());
		var r5  = Serialize.ConvertTo<byte>("123");
		var r6  = Serialize.ConvertTo<char>("x");
		Assert.Throws<ArgumentNullException>(() =>
		{
			var r8 = Serialize.ConvertTo<string>(null);
		});
		var r9  = Serialize.ConvertTo<int?>("123");
		var r10 = Serialize.ConvertTo<int>("123");

		Assert.Equal(123,     r1);
		Assert.Equal(123.456, r2);
		Assert.True(r3);
		Assert.Equal(DateTime.Today, r4);
		Assert.Equal(123,            r5);
		Assert.Equal('x',            r6);
		Assert.Equal(123, r9);
		Assert.Equal(123, r10);
	}

	[Fact]
	public void ConvertToType_String_To_Any_CanLose_Precession()
	{
		var r1 = Serialize.ConvertTo<int>("123.456");
		var r2 = Serialize.ConvertTo<double>("123.456");
		var r3 = Serialize.ConvertTo<float>("123.456");
		var r4 = Serialize.ConvertTo<byte>("123.456");
		var r5 = Serialize.ConvertTo<long>("123.456");
		var n1 = Serialize.ConvertTo<int?>("123.456");
		var n2 = Serialize.ConvertTo<double?>("123.456");
		var n3 = Serialize.ConvertTo<float?>("123.456");
		var n4 = Serialize.ConvertTo<byte?>("123.456");
		var n5 = Serialize.ConvertTo<long?>("123.456");

		Assert.Equal((int?) 123,        r1);
		Assert.Equal((double?) 123.456, r2);
		Assert.Equal((float?) 123.456,  r3);
		Assert.Equal((byte?) 123,       r4);
		Assert.Equal((long?) 123,       r5);
		Assert.Equal(123,               n1);
		Assert.Equal(123.456,           n2);
		Assert.Equal((float?) 123.456,  n3);
		Assert.Equal((byte?) 123,       n4);
		Assert.Equal(123,               n5);
	}

	public struct TestStruct
	{
		public string   A;
		public int      B;
		public DateTime C;
		public bool     D;
	}

	[TypeConverter(typeof(TestStruct2Converter))]
	public class TestStruct2 : IConvertible
	{
		public int            X;
		public TypeCode       GetTypeCode()                                                          => X.GetTypeCode();
		bool IConvertible.    ToBoolean(IFormatProvider?  provider)                                  => ((IConvertible) X).ToBoolean(provider);
		char IConvertible.    ToChar(IFormatProvider?     provider)                                  => ((IConvertible) X).ToChar(provider);
		sbyte IConvertible.   ToSByte(IFormatProvider?    provider)                                  => ((IConvertible) X).ToSByte(provider);
		byte IConvertible.    ToByte(IFormatProvider?     provider)                                  => ((IConvertible) X).ToByte(provider);
		short IConvertible.   ToInt16(IFormatProvider?    provider)                                  => ((IConvertible) X).ToInt16(provider);
		ushort IConvertible.  ToUInt16(IFormatProvider?   provider)                                  => ((IConvertible) X).ToUInt16(provider);
		int IConvertible.     ToInt32(IFormatProvider?    provider)                                  => ((IConvertible) X).ToInt32(provider);
		uint IConvertible.    ToUInt32(IFormatProvider?   provider)                                  => ((IConvertible) X).ToUInt32(provider);
		long IConvertible.    ToInt64(IFormatProvider?    provider)                                  => ((IConvertible) X).ToInt64(provider);
		ulong IConvertible.   ToUInt64(IFormatProvider?   provider)                                  => ((IConvertible) X).ToUInt64(provider);
		float IConvertible.   ToSingle(IFormatProvider?   provider)                                  => ((IConvertible) X).ToSingle(provider);
		double IConvertible.  ToDouble(IFormatProvider?   provider)                                  => ((IConvertible) X).ToDouble(provider);
		decimal IConvertible. ToDecimal(IFormatProvider?  provider)                                  => ((IConvertible) X).ToDecimal(provider);
		DateTime IConvertible.ToDateTime(IFormatProvider? provider)                                  => ((IConvertible) X).ToDateTime(provider);
		string IConvertible.  ToString(IFormatProvider?   provider)                                  => X.ToString(provider);
		object IConvertible.  ToType(Type                 conversionType, IFormatProvider? provider) => ((IConvertible) X).ToType(conversionType, provider);
	}

	public class TestStruct2Converter : Int32Converter
	{
		public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
		{
			var x = (int?) base.ConvertFrom(context, culture, value);
			if (x != null)
				return new TestStruct2 {X = x.Value};
			throw new InvalidCastException("Unable to convert value");
		}
	}
}
