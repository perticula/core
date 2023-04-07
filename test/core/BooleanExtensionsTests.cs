// perticula - core.test - BooleanExtensionsTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.test;

public class BooleanExtensionsTests
{
	[Theory]
	[InlineData(false, false, false, false)]
	[InlineData(true,  true,  true,  true)]
	[InlineData(true,  true,  false, false)]
	[InlineData(true,  false, true,  false)]
	[InlineData(true,  false, false, true)]
	[InlineData(false)]
	public void BoolUtils_Test_Any_All_False(bool expectation, params bool[] args)
	{
		Assert.Equal(expectation, BooleanExtensions.AreAnyTrue(args));
	}
}
