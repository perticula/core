// perticula - core.test - SimpleCacheTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Caching;

namespace core.test.Caching;

public class SimpleCacheTests
{
	[Fact]
	public void SimpleCache_BaseKey_1()
	{
		const string BASE_KEY = "test";
		var          cache    = new SimpleCache();
		var          key      = cache.Key(BASE_KEY, 1);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1", key);
	}

	[Fact]
	public void SimpleCache_BaseKey_2()
	{
		const string BASE_KEY = "test";
		var          cache    = new SimpleCache();
		var          key      = cache.Key(BASE_KEY, 1, "test", 2);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1-test-2", key);
	}

	[Fact]
	public void SimpleCache_BaseKey_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Key("", 1, null, 2);
		});
	}

	[Fact]
	public void SimpleCache_BaseKey_no_args()
	{
		const string BASE_KEY = "test";
		var          cache    = new SimpleCache();
		var          key      = cache.Key(BASE_KEY);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-", key);
	}

	[Fact]
	public void SimpleCache_BaseKey_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Key(null, 1, null, 2);
		});
	}

	[Fact]
	public void SimpleCache_BaseKey_null_args()
	{
		const string BASE_KEY = "test";
		var          cache    = new SimpleCache();
		var          key      = cache.Key(BASE_KEY, null);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}", key);
	}

	[Fact]
	public void SimpleCache_BaseKey_some_args_null()
	{
		const string BASE_KEY = "test";
		var          cache    = new SimpleCache();
		var          key      = cache.Key(BASE_KEY, 1, null, 2);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1--2", key);
	}

	[Fact]
	public void SimpleCache_ByName_NeverNull()
	{
		// Arrange
		var cache  = new SimpleCache();
		var name   = Guid.NewGuid().ToString();
		var item   = Guid.NewGuid().ToString();
		var expect = Guid.NewGuid().ToString();

		// Act
		var found = cache.ByName(name);

		// Assert
		Assert.NotNull(found);
	}

	[Fact]
	public void SimpleCache_ByName_Succeeds()
	{
		// Arrange
		var cache  = new SimpleCache();
		var name   = Guid.NewGuid().ToString();
		var item   = Guid.NewGuid().ToString();
		var expect = Guid.NewGuid().ToString();

		// Act
		cache.ByName(name).Set(item, expect);
		var found = cache.ByName(name).FindOrDefault<string>(item, null);

		// Assert
		Assert.NotNull(found);
		Assert.Equal(expect, found);
	}

	[Fact]
	public void SimpleCache_contains_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Contains("");
		});
	}

	[Fact]
	public void SimpleCache_contains_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Contains(null);
		});
	}

	[Fact]
	public void SimpleCache_Exist()
	{
		var cache = new SimpleCache();

		var item = "item1";
		cache.Set(item, true);

		var exists = cache.Contains(item);
		Assert.True(exists);

		var val  = cache.Get(item);
		var val2 = cache.Get<bool>(item);

		Assert.IsType<bool>(val);
		Assert.True((bool) val);
		Assert.True(val2);
	}

	[Fact]
	public void SimpleCache_find_or_default_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set("item7", true);

			cache.FindOrDefault("", false);
		});
	}

	[Fact]
	public void SimpleCache_find_or_default_found()
	{
		var cache = new SimpleCache();
		var key   = "item5";
		cache.Set(key, true);

		var item = cache.FindOrDefault(key, false);
		Assert.True(item);
	}

	[Fact]
	public void SimpleCache_find_or_default_not_found()
	{
		var cache = new SimpleCache();

		var item = cache.FindOrDefault("item8", true);
		Assert.True(item);
	}

	[Fact]
	public void SimpleCache_find_or_default_not_found2()
	{
		var cache = new SimpleCache();

		var item = cache.FindOrDefault("item9", "def");
		Assert.Equal("def", item);
	}

	[Fact]
	public void SimpleCache_find_or_default_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set("item6", true);

			cache.FindOrDefault(null, false);
		});
	}

	[Fact]
	public void SimpleCache_find_or_set_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set("item12", true);

			cache.FindOrSet("", i => false);
		});
	}

	[Fact]
	public void SimpleCache_find_or_set_found()
	{
		var cache  = new SimpleCache();
		var item10 = "item10";
		cache.Set(item10, true);

		var item = cache.FindOrSet(item10, i => false);
		Assert.True(item);
	}

	[Fact]
	public void SimpleCache_find_or_set_func_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set("item", true);

			cache.FindOrSet<object>("item13", null);
		});
	}

	[Fact]
	public void SimpleCache_find_or_set_not_found()
	{
		var cache = new SimpleCache();

		var item = cache.FindOrSet("item14", i => true);
		Assert.True(item);
	}

	[Fact]
	public void SimpleCache_find_or_set_not_found2()
	{
		var cache = new SimpleCache();

		var key    = "item15";
		var exists = cache.Contains(key);
		Assert.False(exists);

		var item = cache.FindOrSet(key, i => "set");
		Assert.NotNull(item);
		Assert.Equal("set", item);

		exists = cache.Contains(key);
		Assert.True(exists);
	}

	[Fact]
	public void SimpleCache_find_or_set_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set("item11", true);

			cache.FindOrSet(null, i => false);
		});
	}

	[Fact]
	public void SimpleCache_Flush_Returns_Empty()
	{
		var cache = new SimpleCache();
		var item  = "item16";
		cache.Set(item, true);

		var exists = cache.Contains(item);
		Assert.True(exists, "Item not added to cache");

		exists = cache.Get<bool>(item);
		Assert.True(exists);

		cache.Flush();

		Assert.NotNull(cache);

		exists = cache.Contains(item);
		Assert.False(exists, "Item not removed from cache");
	}

	[Fact]
	public void SimpleCache_get_default()
	{
		var cache = new SimpleCache();

		var item = cache.Get("item8");
		Assert.Null(item);
	}

	[Fact]
	public void SimpleCache_Get_Empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Get<bool>("");
		});
	}

	[Fact]
	public void SimpleCache_Get_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Get(null);
		});
	}

	[Fact]
	public void SimpleCache_get1_default()
	{
		var cache = new SimpleCache();

		var item = cache.Get<bool>("item8");
		Assert.False(item);
	}

	[Fact]
	public void SimpleCache_Get1_Empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Get("");
		});
	}

	[Fact]
	public void SimpleCache_Get1_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Get<bool>(null);
		});
	}

	[Fact]
	public void SimpleCache_Item_Does_Not_Exist()
	{
		var cache = new SimpleCache();

		var exists = cache.Contains("nope");
		Assert.False(exists);
	}

	[Fact]
	public void SimpleCache_remove_removes_item()
	{
		var cache = new SimpleCache();
		var item  = "item3";
		cache.Set(item, true);

		var exists = cache.Contains(item);
		Assert.True(exists, "Item not added to cache");

		cache.Remove(item);

		exists = cache.Contains(item);
		Assert.False(exists, "Item not added to cache");
	}

	[Fact]
	public void SimpleCache_remove_removes_item_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			var item  = "item4";
			cache.Set(item, true);

			var exists = cache.Contains(item);
			Assert.True(exists, "Item not added to cache");

			cache.Remove("");
		});
	}

	[Fact]
	public void SimpleCache_remove_removes_item_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set("item", true);

			var exists = cache.Contains("item");
			Assert.True(exists, "Item not added to cache");

			cache.Remove(null);
		});
	}

	[Fact]
	public void SimpleCache_set_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set("", true);
		});
	}

	[Fact]
	public void SimpleCache_set_exists()
	{
		var cache = new SimpleCache();

		var key = "itemX";

		cache.Set(key, "5");

		var item = cache.Get(key);
		Assert.Equal("5", item);

		cache.Set(key, "6");

		item = cache.Get(key);
		Assert.Equal("6", item);
	}

	[Fact]
	public void SimpleCache_set_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SimpleCache();
			cache.Set(null, true);
		});
	}

	[Fact]
	public void SimpleCache_Shared_Not_Exist()
	{
		var cache  = new SimpleCache();
		var cache2 = new SimpleCache();

		var item = "item2";
		cache.Set(item, true);

		var exists = cache2.Contains(item);
		Assert.False(exists);
	}
}
