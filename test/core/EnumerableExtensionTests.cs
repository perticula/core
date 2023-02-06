// perticula - core.test - EnumerableExtensionTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.test;

public class EnumerableExtensionTests
{
	[Theory]
	[ClassData(typeof(EeDivideTestSet))]
	public void EnumTests_Divide_test(List<int> list, int num, int parts, List<int> expected)
	{
		if (num < 0 || parts <= 1)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => { list.Divide(num, parts); });
			return;
		}

		var l = list.Divide(num, parts);
		Assert.Equal(expected, l);
	}

	[Theory]
	[ClassData(typeof(EeZipTests))]
	public void Enumerable_ZipTests(List<int> ints, List<string> strings, IEnumerable<(int, string)> output)
	{
		Assert.Equal(output, ints.Zip(strings));
	}

	[Fact]
	public void Enumerable_To_Enumerable()
	{
		foreach (var x in 0.ToEnumerable()) Assert.Equal(x, x);
	}

	public class EeDivideTestSet : TheoryData<List<int>, int, int, List<int>>
	{
		public EeDivideTestSet()
		{
			Add(new List<int> {1, 2, 3},       0,  -1, new List<int> {1, 2});
			Add(new List<int> {1, 2, 3},       1,  -1, new List<int> {1, 2});
			Add(new List<int> {1, 2, 3},       -1, -1, new List<int> {1, 2});
			Add(new List<int> {1, 2, 3},       1,  2,  new List<int> {1, 2});
			Add(new List<int> {1, 2, 3, 4},    2,  2,  new List<int> {3, 4});
			Add(new List<int> {1, 2, 3, 4, 5}, 2,  3,  new List<int> {3, 4});
		}
	}

	public class EeZipTests : TheoryData<List<int>, List<string>, IEnumerable<(int, string)>>
	{
		public EeZipTests()
		{
			Add(new List<int> {1, 2, 3},
			    new List<string> {"a", "b", "c"},
			    new[] {(1, "a"), (2, "b"), (3, "c")});

			Add(new List<int> {1, 2, 3},
			    new List<string> {"a", "b", "c", "d", "e"},
			    new[] {(1, "a"), (2, "b"), (3, "c")});

			Add(new List<int> {1, 2, 3, 4, 5},
			    new List<string> {"a", "b", "c"},
			    new[] {(1, "a"), (2, "b"), (3, "c")});

			Add(new List<int>(),
			    new List<string> {"a", "b", "c"},
			    Array.Empty<(int, string)>());

			Add(new List<int> {0, 1, 2},
			    new List<string>(),
			    Array.Empty<(int, string)>());
		}
	}
}
