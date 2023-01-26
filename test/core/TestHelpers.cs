// perticula - core.test - TestHelpers.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.test;

internal static class TestHelpers
{
	/// <summary>
	///   Generates a set of string of random content and length.
	///   When used on a test, must create a proxy from the class to this shared implementation.
	/// </summary>
	/// <param name="numberOfResults">
	///   The number of random strings to return. (i.e. for xunit, define the number of tests you
	///   wish to execute.)
	/// </param>
	/// <returns>IEnumerable&lt;System.Object[]&gt;.</returns>
	public static IEnumerable<object[]> RandomStringGenerator(int numberOfResults)
	{
		if (numberOfResults < 1) throw new ArgumentOutOfRangeException(nameof(numberOfResults), "Must specify at least on value to generate");
		for (var i = 0; i   < numberOfResults; i++) yield return new object[] {GetRandomString()};

		static string GetRandomString()
		{
			var rand = new System.Random();

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
