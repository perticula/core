// perticula - core.test - GroupNameTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Attributes;

namespace core.test.Attributes;

public class GroupNameAttributeTests
{
	private static IEnumerable<SampleEnum> EnumGenerator()
	{
		yield return SampleEnum.One;
		yield return SampleEnum.Two;
		yield return SampleEnum.Three;
	}


	[Fact]
	public void GroupNameAttribute_Verify_Display_Text_Can_return_to_enum()
	{
		var actual = "test".FromGroupName<SampleEnum>().ToList();
		Assert.Equal(3, actual.Count);

		Assert.Contains(SampleEnum.One,   actual);
		Assert.Contains(SampleEnum.Two,   actual);
		Assert.Contains(SampleEnum.Three, actual);

		Assert.DoesNotContain(SampleEnum.Four,  actual);
		Assert.DoesNotContain(SampleEnum.Fifth, actual);
	}


	[Fact]
	public void GroupNameAttribute_Verify_Display_Text_Invalidreturn_to_enum()
	{
		var actual = "bad_value".FromGroupName<SampleEnum>();
		Assert.True(!actual.Any());
	}

	[Fact]
	public void GroupNameAttribute_Verify_Group_Text_Values__Correct()
	{
		foreach (var value in EnumGenerator())
		{
			Assert.NotNull(value.GroupName());
			Assert.Equal("test", value.GroupName());
		}

		Assert.Equal("Fourth", SampleEnum.Four.GroupName());
		Assert.NotEqual("test", SampleEnum.Four.GroupName());
		Assert.NotNull(SampleEnum.Fifth.GroupName());
		Assert.Equal("", SampleEnum.Fifth.GroupName());
	}

	private enum SampleEnum
	{
		[GroupName("test")]   One,
		[GroupName("test")]   Two,
		[GroupName("test")]   Three,
		[GroupName("Fourth")] Four,
		Fifth
	}
}
