// perticula - core.test - EncryptionTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Security;

namespace core.test.Security
{
	public class EncryptionTests
	{
		[Fact]
		public void Encryption_Does_Not_Generate_Same_IV()
		{
			const string test1 = "Perticula is fun";
			const string test2 = "Perticula is fun";

			var e1 = test1.EncryptValue();
			var e2 = test2.EncryptValue();

			Assert.NotEqual(e1, e2);
		}

		[Theory]
		[MemberData(nameof(RandomStringGenerator), 50)]
		public void Encryption_Restores_The_Correct_Value(string value)
		{
			var e = value.EncryptValue();
			Assert.NotEqual(value, e);

			var d = e.DecryptValue();
			Assert.Equal(value, d);
		}

		public static IEnumerable<object[]> RandomStringGenerator(int numTests)
		{
			for (var i = 0; i < numTests; i++) yield return new object[] {GetRandomString()};
		}

		private static string GetRandomString()
		{
			var rand = new Random();

			var stringlen = rand.Next(1, 256);
			var str       = "";
			for (var i = 0; i < stringlen; i++)
			{
				var randValue = rand.Next(0, 26);
				var letter    = Convert.ToChar(randValue + 65);
				str += letter;
			}

			return str;
		}
	}
}
