// perticula - core.test - EncryptionTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.test.Security;

public class EncryptionTests
{
	[Fact]
	public void Encryption_Does_Not_Generate_Same_IV()
	{
		const string test1 = "Perticula is fun";
		const string test2 = "Perticula is fun";

		var e1 = test1.EncryptValue();
		var e2 = test2.EncryptValue();

		Assert.NotNull(e1);
		Assert.NotNull(e2);
		Assert.NotEqual(e1, e2);
	}

	[Theory]
	[MemberData(nameof(RandomStringGenerator), 50)]
	public void Encryption_Restores_The_Correct_Value(string value)
	{
		var e = value.EncryptValue();
		Assert.NotNull(e);
		Assert.NotEqual(value, e);

		var d = e.DecryptValue();
		Assert.NotNull(d);
		Assert.Equal(value, d);
	}

	/// <summary>
	///   Generates a set of strings, of random length and content.
	///   Must exist within the tested class, but implementation is forwarded to a shared helper
	/// </summary>
	/// <param name="numTests">The number tests.</param>
	/// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
	public static IEnumerable<object[]> RandomStringGenerator(int numTests) => TestHelpers.RandomStringGenerator(numTests);
}
