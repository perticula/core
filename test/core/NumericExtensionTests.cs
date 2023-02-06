// perticula - core.test - NumericExtensionTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.test;

public class NumericExtensionTests
{
	private const int   NumInt   = 123;
	private const long  NumLong  = 123;
	private const float NumFloat = 123.4f;

	[Fact]
	public void NumberExtensionsTests_IsGreaterThan_Success()
	{
		Assert.True(NumInt.IsGreaterThan(1));
		Assert.True(NumLong.IsGreaterThan(1));
		Assert.True(NumFloat.IsGreaterThan(1));

		Assert.False(NumInt.IsGreaterThan(999));
		Assert.False(NumLong.IsGreaterThan(999));
		Assert.False(NumFloat.IsGreaterThan(999));

		Assert.False(NumInt.IsGreaterThan(123));
	}

	[Fact]
	public void NumberExtensionsTests_IsLessThan_Success()
	{
		Assert.False(NumInt.IsLessThan(1));
		Assert.False(NumLong.IsLessThan(1));
		Assert.False(NumFloat.IsLessThan(1));

		Assert.True(NumInt.IsLessThan(999));
		Assert.True(NumLong.IsLessThan(999));
		Assert.True(NumFloat.IsLessThan(999));

		Assert.False(NumInt.IsGreaterThan(123));
	}

	[Fact]
	public void NumberExtensionsTests_OrMaximumOf_Success()
	{
		Assert.Equal(NumInt,   NumInt.OrMaximumOf(999));
		Assert.Equal(NumLong,  NumLong.OrMaximumOf(999));
		Assert.Equal(NumFloat, NumFloat.OrMaximumOf(999));

		Assert.Equal(1, NumInt.OrMaximumOf(1));
		Assert.Equal(1, NumLong.OrMaximumOf(1));
		Assert.Equal(1, NumFloat.OrMaximumOf(1));
	}

	[Fact]
	public void NumberExtensionsTests_OrMinimumOf_Success()
	{
		Assert.Equal(999, NumInt.OrMinimumOf(999));
		Assert.Equal(999, NumLong.OrMinimumOf(999));
		Assert.Equal(999, NumFloat.OrMinimumOf(999));

		Assert.Equal(NumInt,   NumInt.OrMinimumOf(1));
		Assert.Equal(NumLong,  NumLong.OrMinimumOf(1));
		Assert.Equal(NumFloat, NumFloat.OrMinimumOf(1));
	}

	[Fact]
	public void NumberExtensionsTests_OrMinMaxOf_Success()
	{
		Assert.Equal(NumInt,   NumInt.OrMinMaxOf(1, 999));
		Assert.Equal(NumLong,  NumLong.OrMinMaxOf(1, 999));
		Assert.Equal(NumFloat, NumFloat.OrMinMaxOf(1, 999));

		Assert.Equal(999, NumInt.OrMinMaxOf(999, 9999));
		Assert.Equal(999, NumLong.OrMinMaxOf(999, 9999));
		Assert.Equal(999, NumFloat.OrMinMaxOf(999, 9999));

		Assert.Equal(2, NumInt.OrMinMaxOf(1, 2));
		Assert.Equal(2, NumLong.OrMinMaxOf(1, 2));
		Assert.Equal(2, NumFloat.OrMinMaxOf(1, 2));
	}
}
