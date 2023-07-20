// perticula - core.test - ObjectUtilityTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using Moq;

namespace core.test;

public class ObjectUtilityTests
{
	[Fact]
	public void GetHashCode_Null_ReturnsZero()
	{
		object? obj    = null;
		var     result = Objects.GetHashCode(obj);
		Assert.Equal(0, result);
	}

	[Fact]
	public void GetHashCode_NotNull_ReturnsValidHashCode()
	{
		var obj    = new object();
		var result = obj.GetHashCode();
		Assert.IsType<int>(result);
	}

	[Fact]
	public void EnsureSingleton_NullInitializer_ThrowsTypeInitializationException()
	{
		string? appSettings = null;

		Assert.Throws<TypeInitializationException>(() => Objects.EnsureSingleton(ref appSettings, "", null));
	}

	[Fact]
	public void EnsureSingleton_AlreadyInitialized_ReturnsSingletonInstance()
	{
		var appSettings = "test";
		var result      = Objects.EnsureSingleton(ref appSettings, "", _ => "test2");
		Assert.Equal("test", result);
	}

	[Fact]
	public void EnsureSingleton_Uninitialized_ReturnsInitializedSingleton()
	{
		string? appSettings = null;

		var mockInitializer = new Mock<Func<string, string>>();
		mockInitializer.Setup(x => x.Invoke(It.IsAny<string>())).Returns("test");

		var result = Objects.EnsureSingleton(ref appSettings, "arg", mockInitializer.Object);

		Assert.Equal("test", result);
	}
}
