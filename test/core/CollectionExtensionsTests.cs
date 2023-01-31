// perticula - core.test - CollectionExtensionsTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.test;

public class CollectionExtensionsTests
{
	[Theory]
	[InlineData(false, 5)]
	[InlineData(true,  6, 0)]
	[InlineData(false, 5, 1)]
	[InlineData(false, 5, 2)]
	[InlineData(false, 5, 3)]
	[InlineData(false, 5, 4)]
	[InlineData(false, 5, 5)]
	[InlineData(true,  7, 6,  7)]
	[InlineData(true,  8, 6,  7,  8)]
	[InlineData(true,  9, 6,  7,  8,  9)]
	[InlineData(true,  9, -6, -7, -8, -9)]
	public void CollectionExtensions_ConditionalAdd_Added(bool expectation, int count, params int[] extra)
	{
		var list = new List<int> {1, 2, 3, 4, 5};
		Assert.Equal(5, list.Count);
		foreach (var item in extra) list.ConditionalAdd(item, (_, i) => !list.Contains(i));
		Assert.Equal(expectation, list.Count > 5);
		Assert.Equal(count,       list.Count);
	}

	[Fact]
	public void CollectionExtensions_GroupByCount_Normal()
	{
		var list = new List<int> {1, 2, 3, 4, 5, 6};
		Assert.Equal(6, list.Count);
		Assert.NotNull(list);

		var grouped = list.GroupByCount(3)!.ToList();

		Assert.NotNull(grouped);
		Assert.True(grouped.Any());


		Assert.Equal(2, grouped.Count);
		foreach (var group in grouped) Assert.Equal(3, group.Count());
	}

	[Fact]
	public void CollectionExtensions_GroupByCount_Null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			List<int>? list = null;
			var        _    = list!.GroupByCount(3);
		});
	}

	[Fact]
	public void CollectionExtensions_NullOrEmpty_Empty()
	{
		// ReSharper disable once CollectionNeverUpdated.Local
		var list  = new List<int>();
		var empty = list.IsNullOrEmpty();
		Assert.True(empty);
	}

	[Fact]
	public void CollectionExtensions_NullOrEmpty_Null()
	{
		List<int>? list  = null;
		var        empty = list.IsNullOrEmpty();
		Assert.True(empty);
	}

	[Fact]
	public void CollectionExtensions_NullOrEmpty_Valid()
	{
		var list  = new List<int> {1, 2, 3, 4, 5};
		var empty = list.IsNullOrEmpty();
		Assert.False(empty);
	}
}
