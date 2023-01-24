// perticula - core.test - SharedMemoryCacheTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Caching;

namespace core.test.Caching;

public class SharedMemoryCacheTests
{
	[Fact]
	public void SharedMemoryCache_ByName_is_never_null()
	{
		// Arrange
		var cache  = new SharedMemoryCache();
		var name   = Guid.NewGuid().ToString();
		var item   = Guid.NewGuid().ToString();
		var expect = Guid.NewGuid().ToString();

		// Act
		var found = cache.ByName(name);

		// Assert
		Assert.NotNull(found);
	}

	[Fact]
	public void SharedMemoryCache_ByName_replaces_non_cache_values()
	{
		// Arrange
		var cache = new SharedMemoryCache();
		var name  = Guid.NewGuid().ToString();

		// Act
		cache.FindOrSet(name, i => "NOT-CACHE");
		var found = cache.ByName(name);

		// Assert
		Assert.NotNull(found);
	}

	[Fact]
	public void SharedMemoryCache_ByName_succeeds()
	{
		// Arrange
		var cache  = new SharedMemoryCache();
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
	public void SharedMemoryCache_Contains_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Contains("");
		});
	}

	[Fact]
	public void SharedMemoryCache_Contains_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Contains(null);
		});
	}

	[Fact]
	public void SharedMemoryCache_Exist()
	{
		var cache = new SharedMemoryCache();

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
	public void SharedMemoryCache_FindOrDefault_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set("item7", true);

			cache.FindOrDefault("", false);
		});
	}

	[Fact]
	public void SharedMemoryCache_FindOrDefault_found()
	{
		var cache = new SharedMemoryCache();
		var key   = "item5";
		cache.Set(key, true);

		var item = cache.FindOrDefault(key, false);
		Assert.True(item);
	}

	[Fact]
	public void SharedMemoryCache_FindOrDefault_not_found()
	{
		var cache = new SharedMemoryCache();

		var item = cache.FindOrDefault("item8", true);
		Assert.True(item);
	}

	[Fact]
	public void SharedMemoryCache_FindOrDefault_not_found2()
	{
		var cache = new SharedMemoryCache();

		var item = cache.FindOrDefault("item9", "def");
		Assert.Equal("def", item);
	}

	[Fact]
	public void SharedMemoryCache_FindOrDefault_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set("item6", true);

			cache.FindOrDefault(null, false);
		});
	}

	[Fact]
	public void SharedMemoryCache_FindOrSet_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set("item12", true);

			cache.FindOrSet("", i => false);
		});
	}

	[Fact]
	public void SharedMemoryCache_FindOrSet_found()
	{
		var cache  = new SharedMemoryCache();
		var item10 = "item10";
		cache.Set(item10, true);

		var item = cache.FindOrSet(item10, i => false);
		Assert.True(item);
	}

	[Fact]
	public void SharedMemoryCache_FindOrSet_func_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set("item", true);

			cache.FindOrSet<object>("item13", null);
		});
	}

	[Fact]
	public void SharedMemoryCache_CanBeReset()
	{
		var cache  = new SharedMemoryCache();
		var item10 = "item10";
		cache.Set(item10, true);

		var item = cache.FindOrSet(item10, i => false);
		Assert.True(item);

		cache.Reset();
		Assert.False(cache.Contains(item10));
	}

	[Fact]
	public void SharedMemoryCache_FindOrSet_not_found()
	{
		var cache = new SharedMemoryCache();

		var item = cache.FindOrSet("item14", i => true);
		Assert.True(item);
	}

	[Fact]
	public void SharedMemoryCache_FindOrSet_not_found2()
	{
		var cache = new SharedMemoryCache();

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
	public void SharedMemoryCache_FindOrSet_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set("item11", true);

			cache.FindOrSet(null, i => false);
		});
	}

	[Fact]
	public void SharedMemoryCache_Flush_not_supported()
	{
		Assert.Throws<NotSupportedException>(() =>
		{
			// Arrange
			var cache = new SharedMemoryCache();
			cache.Set("item11", true);

			// Act
			cache.Flush();

			// Assert
		});
	}

	[Fact]
	public void SharedMemoryCache_Get_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Get<bool>("");
		});
	}

	[Fact]
	public void SharedMemoryCache_Get_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Get(null);
		});
	}

	[Fact]
	public void SharedMemoryCache_Get1_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Get("");
		});
	}

	[Fact]
	public void SharedMemoryCache_Get1_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Get<bool>(null);
		});
	}

	[Fact]
	public void SharedMemoryCache_Item_Does_Not_Exist()
	{
		var cache = new SharedMemoryCache();

		var exists = cache.Contains("nope");
		Assert.False(exists);
	}

	[Fact]
	public void SharedMemoryCache_Key_baseKey_1()
	{
		const string BASE_KEY = "test";
		var          cache    = new SharedMemoryCache();
		var          key      = cache.Key(BASE_KEY, 1);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1", key);
	}

	[Fact]
	public void SharedMemoryCache_Key_baseKey_2()
	{
		const string BASE_KEY = "test";
		var          cache    = new SharedMemoryCache();
		var          key      = cache.Key(BASE_KEY, 1, "test", 2);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1-test-2", key);
	}

	[Fact]
	public void SharedMemoryCache_Key_baseKey_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Key("", 1, null, 2);
		});
	}

	[Fact]
	public void SharedMemoryCache_Key_baseKey_no_args()
	{
		const string BASE_KEY = "test";
		var          cache    = new SharedMemoryCache();
		var          key      = cache.Key(BASE_KEY);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-", key);
	}

	[Fact]
	public void SharedMemoryCache_Key_baseKey_null_args()
	{
		const string BASE_KEY = "test";
		var          cache    = new SharedMemoryCache();
		var          key      = cache.Key(BASE_KEY, null);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}", key);
	}

	[Fact]
	public void SharedMemoryCache_Key_baseKey_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Key(null, 1, null, 2);
		});
	}

	[Fact]
	public void SharedMemoryCache_Key_baseKey_some_args_null()
	{
		const string BASE_KEY = "test";
		var          cache    = new SharedMemoryCache();
		var          key      = cache.Key(BASE_KEY, 1, null, 2);
		Assert.NotNull(key);
		Assert.Equal($"{BASE_KEY}-1--2", key);
	}

	[Fact]
	public void SharedMemoryCache_Remove_removes_item()
	{
		var cache = new SharedMemoryCache();
		var item  = "item3";
		cache.Set(item, true);

		var exists = cache.Contains(item);
		Assert.True(exists, "Item not added to cache");

		cache.Remove(item);

		exists = cache.Contains(item);
		Assert.False(exists, "Item not added to cache");
	}

	[Fact]
	public void SharedMemoryCache_Remove_removes_item_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			var item  = "item4";
			cache.Set(item, true);

			var exists = cache.Contains(item);
			Assert.True(exists, "Item not added to cache");

			cache.Remove("");
		});
	}

	[Fact]
	public void SharedMemoryCache_Remove_removes_item_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set("item", true);

			var exists = cache.Contains("item");
			Assert.True(exists, "Item not added to cache");

			cache.Remove(null);
		});
	}

	[Fact]
	public void SharedMemoryCache_Set_empty_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set("", true);
		});
	}

	[Fact]
	public void SharedMemoryCache_Set_null_throws_exception()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var cache = new SharedMemoryCache();
			cache.Set(null, true);
		});
	}

	[Fact]
	public void SharedMemoryCache_Shared_Exist()
	{
		var cache  = new SharedMemoryCache();
		var cache2 = new SharedMemoryCache();

		var item = "item2";
		cache.Set(item, true);

		var exists = cache2.Contains(item);
		Assert.True(exists);

		var val  = cache.Get(item);
		var val2 = cache2.Get<bool>(item);

		Assert.IsType<bool>(val);
		Assert.True((bool) val);
		Assert.True(val2);
	}
}
