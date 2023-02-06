// perticula - core.test - DictionaryExtensionTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics.CodeAnalysis;

namespace core.test;

public class DictionaryExtensionTests
{
	[Fact]
	public void DictionaryUtilities_Verify_Add_Or_Update_Does_Not_Add_Multiple_Instances_If_Value_Exists()
	{
		var sample = new Dictionary<string, int>();
		Assert.True(0 == sample.Count);

		sample.AddOrUpdate("test", 1);
		Assert.True(1 == sample.Count);
		Assert.True(1 == sample["test"]);

		sample.AddOrUpdate("test", 2);
		Assert.True(1 == sample.Count);
		Assert.True(2 == sample["test"]);

		sample.AddOrUpdate("test2", 2);
		Assert.True(2 == sample.Count);
		Assert.True(2 == sample["test"]);
		Assert.True(2 == sample["test2"]);
	}

	[Fact]
	public void DictionaryUtilities_Verify_Add_Or_Update_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var dict = (IDictionary<string, int>) null!;
			dict.AddOrUpdate("test", 0);
		});
	}

	[Fact]
	public void DictionaryUtilities_Verify_Find_Or_Set_Does_Not_Add_Multiple_Instances_If_Value_Exists()
	{
		var sample = new Dictionary<string, int>();
		Assert.True(0 == sample.Count);

		var r1 = sample.FindOrSet("test1", _ => 1);
		Assert.True(1 == sample.Count);
		Assert.True(1 == r1);
		Assert.True(1 == sample["test1"]);

		var r2 = sample.FindOrSet("test1", _ => 2);
		Assert.True(1 == sample.Count);
		Assert.True(1 == r2);
		Assert.True(1 == sample["test1"]);
	}

	[Theory]
	[ClassData(typeof(DictionaryTestData))]
	public void DictionaryUtilities_Add_Once_Null(string key, object value)
	{
		IDictionary<string, object> test = null!;
		Assert.Throws<ArgumentNullException>(() => test.AddOnce(key, _ => value));
	}

	[Theory]
	[ClassData(typeof(DictionaryTestData))]
	public void DictionaryUtilities_Add_Once(string key, object value)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			Assert.Throws<ArgumentNullException>(() => _ = new Dictionary<string, object?> {{key, value}});
			return;
		}

		var test = new Dictionary<string, object>();

		test.AddOnce(key, _ => value);
		Assert.Equal(value, test[key]);
	}

	[Fact]
	public void DictionaryUtilities_Add_Once_keeps_old_value()
	{
		var test = new Dictionary<string, object>
		{
			{"1", 1}
		};
		test.AddOnce("1", _ => 2);

		Assert.True(1 == test.Count);
		Assert.Equal(1, test["1"]);
	}

	[Theory]
	[ClassData(typeof(DictionaryTestData))]
	public void DictionaryUtilities_FindOrDefault(string key, object value)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			Assert.Throws<ArgumentNullException>(() => _ = new Dictionary<string, object?> {{key, value}});
			return;
		}

		Assert.Equal(value, value);
		var test = new Dictionary<string, object?>();
		var x    = test.FindOrDefault(key, 4);
		Assert.Equal(4, x);
	}

	[Theory]
	[ClassData(typeof(DictionaryTestData))]
	public void DictionaryUtilities_FindOrSet_Null(string key, object value)
	{
		Dictionary<string, object?> test = null!;
		Assert.Throws<ArgumentNullException>(() => test.FindOrSet(key, _ => value));
	}

	[Theory]
	[ClassData(typeof(DictionaryTestData))]
	public void DictionaryUtilities_FindOrSet__key_Null(string key, object value)
	{
		var test = new Dictionary<string, object?>();
		if (key == null)
			Assert.Throws<ArgumentNullException>(() => test!.FindOrSet(key, _ => value));
	}

	[Theory]
	[ClassData(typeof(DictionaryTestData))]
	public void DictionaryUtilities_FindOrSet__value_Null(string key, object value)
	{
		var test = new Dictionary<string, object>();
		if (value == null)
			Assert.Throws<ArgumentNullException>(() => test.FindOrSet(key, null));
	}

	[Theory]
	[ClassData(typeof(DictionaryTestData))]
	public void DictionaryUtils_Spreads(string key, object value)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			Assert.Throws<ArgumentNullException>(() => _ = new Dictionary<string, object?> {{key, value}});
			return;
		}

		var test = new Dictionary<string, object?> {{key, value}};
		foreach (var (k, v) in test.Spread())
		{
			Assert.Equal(k, key);
			Assert.Equal(v, value);
		}
	}

	[ExcludeFromCodeCoverage]
	private class DictionaryTestData : TheoryData<string, object>
	{
		public DictionaryTestData()
		{
			Add("1",    1);
			Add("2",    2);
			Add("3",    3);
			Add("4",    4);
			Add("11",   1);
			Add("22",   2);
			Add("33",   3);
			Add("44",   4);
			Add("1111", 1);
			Add("2222", 2);
			Add("3333", 3);
			Add("4444", 4);
			Add(null,   4);
			Add("4",    null);
			Add(null,   null);
		}
	}
}
