// perticula - core.test - DisplayAttributeTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Attributes;

namespace core.test.Attributes;

public class DisplayAttributeTests
{
	private static IEnumerable<SampleEnum> EnumGenerator()
	{
		yield return SampleEnum.One;
		yield return SampleEnum.Two;
		yield return SampleEnum.Three;
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_Format_Value()
	{
		Assert.Equal("test", SampleEnum.Six.DisplayText("test"));
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_Format_Value_1()
	{
		Assert.Equal("1", SampleEnum.Six.DisplayText(1, 2, 3));
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_Format_Value_2()
	{
		Assert.Equal("value: 1 2", SampleEnum.Seven.DisplayText(1, 2, 3));
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_Format_Value_3()
	{
		Assert.Throws<FormatException>(() => { Assert.Equal("value: 1 {1}", SampleEnum.Seven.DisplayText(1)); });
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_ignore_Default_Value_When_value_Provided()
	{
		Assert.Equal("1", SampleEnum.Six.DisplayText(1, 2, 3));
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_return_Default_Value_When_value_not_Provided()
	{
		Assert.Equal("Default Six", SampleEnum.Six.DisplayText(""));
	}


	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_return_to_enum()
	{
		var actual = "Fourth".FromDisplayText<SampleEnum>();
		Assert.Equal(SampleEnum.Four, actual);
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_return_to_enum_direct()
	{
		var actual = "fifth".FromDisplayText<SampleEnum>();
		Assert.Equal(SampleEnum.Fifth, actual);
	}


	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_return_to_enum_multiple_split_1()
	{
		var actual = "eighth".FromDisplayText<SampleEnum>();
		Assert.Equal(SampleEnum.Eight, actual);
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Can_return_to_enum_multiple_split_2()
	{
		var actual = "8th".FromDisplayText<SampleEnum>();
		Assert.Equal(SampleEnum.Eight, actual);
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_InvalidReturn_to_enum()
	{
		Assert.Throws<InvalidCastException>(() =>
		{
			var _ = "bad_value".FromDisplayText<SampleEnum>();
		});
	}

	[Fact]
	public void DisplayTextAttribute_Verify_Display_Text_Values__Correct()
	{
		foreach (var value in EnumGenerator())
		{
			Assert.NotNull(value.DisplayText());
			Assert.Equal("test", value.DisplayText());
		}

		Assert.Equal("Fourth", SampleEnum.Four.DisplayText());
		Assert.NotEqual("test", SampleEnum.Four.DisplayText());
		Assert.Equal(string.Empty, SampleEnum.Fifth.DisplayText());
	}

	private enum SampleEnum
	{
		[DisplayText("test")]   One,
		[DisplayText("test")]   Two,
		[DisplayText("test")]   Three,
		[DisplayText("Fourth")] Four,

		Fifth,

		[DisplayText("{0}", "Default Six")] Six,
		[DisplayText("value: {0} {1}")]     Seven,
		[DisplayText("eighth/8th")]         Eight
	}
}
