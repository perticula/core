// perticula - core.test - ConcurrentCacheTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Caching;

namespace core.test.Caching;

public class ConcurrentCacheTests
{
	[Fact]
	public void ConcurrentCache_BaseKey_1()
	{
		const string BASE_KEY = "test";
		var          cache    = new ConcurrentCache();
		var          key      = cache.Key(BASE_KEY, 1);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1", key);
	}

	[Fact]
	public void ConcurrentCache_BaseKey_2()
	{
		const string BASE_KEY = "test";
		var          cache    = new ConcurrentCache();
		var          key      = cache.Key(BASE_KEY, 1, "test", 2);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1-test-2", key);
	}

	[Fact]
	public void ConcurrentCache_BaseKey_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Key("", 1, null, 2);
		});
	}

	[Fact]
	public void ConcurrentCache_BaseKey_no_args()
	{
		const string BASE_KEY = "test";
		var          cache    = new ConcurrentCache();
		var          key      = cache.Key(BASE_KEY);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-", key);
	}

	[Fact]
	public void ConcurrentCache_BaseKey_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Key(null, 1, null, 2);
		});
	}

	[Fact]
	public void ConcurrentCache_BaseKey_null_args()
	{
		const string BASE_KEY = "test";
		var          cache    = new ConcurrentCache();
		var          key      = cache.Key(BASE_KEY, null);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}", key);
	}

	[Fact]
	public void ConcurrentCache_BaseKey_some_args_null()
	{
		const string BASE_KEY = "test";
		var          cache    = new ConcurrentCache();
		var          key      = cache.Key(BASE_KEY, 1, null, 2);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1--2", key);
	}

	[Fact]
	public void ConcurrentCache_ByName_NeverNull()
	{
		var cache  = new ConcurrentCache();
		var name   = Guid.NewGuid().ToString();
		var item   = Guid.NewGuid().ToString();
		var expect = Guid.NewGuid().ToString();

		var found = cache.ByName(name);

		Assert.NotNull(found);
	}

	[Fact]
	public void ConcurrentCache_ByName_Succeeds()
	{
		var cache  = new ConcurrentCache();
		var name   = Guid.NewGuid().ToString();
		var item   = Guid.NewGuid().ToString();
		var expect = Guid.NewGuid().ToString();

		cache.ByName(name).Set(item, expect);
		var found = cache.ByName(name).FindOrDefault<string>(item, null);

		Assert.NotNull(found);
		Assert.Equal(expect, found);
	}

	[Fact]
	public void ConcurrentCache_contains_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Contains("");
		});
	}

	[Fact]
	public void ConcurrentCache_contains_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Contains(null);
		});
	}

	[Fact]
	public void ConcurrentCache_Exist()
	{
		var cache = new ConcurrentCache();

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
	public void ConcurrentCache_find_or_default_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set("item7", true);

			cache.FindOrDefault("", false);
		});
	}

	[Fact]
	public void ConcurrentCache_find_or_default_found()
	{
		var cache = new ConcurrentCache();
		var key   = "item5";
		cache.Set(key, true);

		var item = cache.FindOrDefault(key, false);
		Assert.True(item);
	}

	[Fact]
	public void ConcurrentCache_find_or_default_not_found()
	{
		var cache = new ConcurrentCache();

		var item = cache.FindOrDefault("item8", true);
		Assert.True(item);
	}

	[Fact]
	public void ConcurrentCache_find_or_default_not_found2()
	{
		var cache = new ConcurrentCache();

		var item = cache.FindOrDefault("item9", "def");
		Assert.Equal("def", item);
	}

	[Fact]
	public void ConcurrentCache_find_or_default_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set("item6", true);

			cache.FindOrDefault(null, false);
		});
	}

	[Fact]
	public void ConcurrentCache_find_or_set_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set("item12", true);

			cache.FindOrSet("", i => false);
		});
	}

	[Fact]
	public void ConcurrentCache_find_or_set_found()
	{
		var cache  = new ConcurrentCache();
		var item10 = "item10";
		cache.Set(item10, true);

		var item = cache.FindOrSet(item10, i => false);
		Assert.True(item);
	}

	[Fact]
	public void ConcurrentCache_find_or_set_func_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set("item", true);

			cache.FindOrSet<object>("item13", null);
		});
	}

	[Fact]
	public void ConcurrentCache_find_or_set_not_found()
	{
		var cache = new ConcurrentCache();

		var item = cache.FindOrSet("item14", i => true);
		Assert.True(item);
	}

	[Fact]
	public void ConcurrentCache_find_or_set_not_found2()
	{
		var cache = new ConcurrentCache();

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
	public void ConcurrentCache_find_or_set_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set("item11", true);

			cache.FindOrSet(null, i => false);
		});
	}

	[Fact]
	public void ConcurrentCache_Flush_Returns_Empty()
	{
		var cache = new ConcurrentCache();
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
	public void ConcurrentCache_get_default()
	{
		var cache = new ConcurrentCache();

		var item = cache.Get("item8");
		Assert.Null(item);
	}

	[Fact]
	public void ConcurrentCache_Get_Empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Get<bool>("");
		});
	}

	[Fact]
	public void ConcurrentCache_Get_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Get(null);
		});
	}

	[Fact]
	public void ConcurrentCache_get1_default()
	{
		var cache = new ConcurrentCache();

		var item = cache.Get<bool>("item8");
		Assert.False(item);
	}

	[Fact]
	public void ConcurrentCache_Get1_Empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Get("");
		});
	}

	[Fact]
	public void ConcurrentCache_Get1_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Get<bool>(null);
		});
	}

	[Fact]
	public void ConcurrentCache_Item_Does_Not_Exist()
	{
		var cache = new ConcurrentCache();

		var exists = cache.Contains("nope");
		Assert.False(exists);
	}

	[Fact]
	public void ConcurrentCache_remove_removes_item()
	{
		var cache = new ConcurrentCache();
		var item  = "item3";
		cache.Set(item, true);

		var exists = cache.Contains(item);
		Assert.True(exists, "Item not added to cache");

		cache.Remove(item);

		exists = cache.Contains(item);
		Assert.False(exists, "Item not added to cache");
	}

	[Fact]
	public void ConcurrentCache_remove_removes_item_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			var item  = "item4";
			cache.Set(item, true);

			var exists = cache.Contains(item);
			Assert.True(exists, "Item not added to cache");

			cache.Remove("");
		});
	}

	[Fact]
	public void ConcurrentCache_remove_removes_item_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set("item", true);

			var exists = cache.Contains("item");
			Assert.True(exists, "Item not added to cache");

			cache.Remove(null);
		});
	}

	[Fact]
	public void ConcurrentCache_set_empty()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set("", true);
		});
	}

	[Fact]
	public void ConcurrentCache_set_exists()
	{
		var cache = new ConcurrentCache();

		var key = "itemX";

		cache.Set(key, "5");

		var item = cache.Get(key);
		Assert.Equal("5", item);

		cache.Set(key, "6");

		item = cache.Get(key);
		Assert.Equal("6", item);
	}

	[Fact]
	public void ConcurrentCache_set_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new ConcurrentCache();
			cache.Set(null, true);
		});
	}

	[Fact]
	public void ConcurrentCache_Shared_Not_Exist()
	{
		var cache  = new ConcurrentCache();
		var cache2 = new ConcurrentCache();

		var item = "item2";
		cache.Set(item, true);

		var exists = cache2.Contains(item);
		Assert.False(exists);
	}
}
