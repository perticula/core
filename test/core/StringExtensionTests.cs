// perticula - core.test - StringExtensionTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Diagnostics.CodeAnalysis;

namespace core.test;

public class StringExtensionTests
{
	[Theory]
	[ClassData(typeof(PlacholderTestData))]
	public void StringUtilsUnderstandsPlaceholderText(string baseSz, bool expected)
	{
		Assert.Equal(expected, baseSz.IsNullOrDefault());
	}

	[Theory]
	[InlineData("i am root",  "iAmRoot")]
	[InlineData("i_am_root",  "iAmRoot")]
	[InlineData("i_am _root", "iAm_root")]
	[InlineData(null,         null)]
	public void StringUtilities_Verify_ToCamelCase_Tests(string input, string expected) => Assert.Equal(expected, input.ToCamelCase());

	[Theory]
	[InlineData("i am root",   "IAmRoot")]
	[InlineData("i_am_root",   "IAmRoot")]
	[InlineData("i_am _root",  "IAm_root")]
	[InlineData("i_am_ _root", "IAm Root")]
	[InlineData(null,          null)]
	public void StringUtilities_Verify_ToPascalCase_Tests(string input, string expected) => Assert.Equal(expected, input.ToPascalCase());

	[Theory]
	[InlineData("i am root",   "i-am-root")]
	[InlineData("i_am_root",   "i-am-root")]
	[InlineData("i_am _root",  "i-am--root")]
	[InlineData("i_am_ _root", "i-am---root")]
	[InlineData(null,          null)]
	public void StringUtilities_Verify_ToKebabCase_Tests(string input, string expected) => Assert.Equal(expected, input.ToKebabCase());

	[Theory]
	[InlineData("i am root",   "i_am_root")]
	[InlineData("i_am_root",   "i_am_root")]
	[InlineData("i_am _root",  "i_am__root")]
	[InlineData("i_am_ _root", "i_am___root")]
	[InlineData(null,          null)]
	public void StringUtilities_Verify_ToUnderscoreCase_Tests(string input, string expected) => Assert.Equal(expected, input.ToUnderscoreCase());

	[Theory]
	[ClassData(typeof(CredentialTestData))]
	public void StringUtilities_Verify_Credentials_Encoding(string user, string password, string result)
	{
		Assert.Equal(result, StringExtensions.EncodeBasicAuthenticationCredentials(user, password));
	}

	[Fact]
	public void StringUtilities_ClipText_ReturnsDefaultEplisis()
	{
		var sample = "This is another test";
		var r1     = sample.ClipText(10);
		Assert.Equal("This is another …", r1);
	}

	[Fact]
	public void StringUtilities_ClipText_ReturnsUpToLength_WholeWord()
	{
		var sample = "This is another test";
		var r1     = sample.ClipText(10, "|");
		Assert.Equal("This is another|", r1);
	}

	[Fact]
	public void StringUtilities_ClipText_ReturnsWholeString()
	{
		var sample = "This is a test";
		var r1     = sample.ClipText(20);
		Assert.Equal(sample, r1);
	}

	[Fact]
	public void StringUtilities_ClipText_WontSplitBRTags()
	{
		var sample = "This is yet<br>another test<br>";
		var r1     = sample.ClipText(13, "|");
		Assert.Equal("This is yet<br>another|", r1);
	}

	[Fact]
	public void StringUtilities_Contains_CaseInsensitive()
	{
		var test = "this is my test!";
		var res1 = test.Contains("MY");
		var res2 = test.Contains("MY", StringComparison.OrdinalIgnoreCase);
		Assert.False(res1);
		Assert.True(res2);
	}

	[Fact]
	public void StringUtilities_get_article_consonant()
	{
		var test = "carrot";
		var repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("a", repl);
	}

	[Fact]
	public void StringUtilities_get_article_empty()
	{
		var test = "";
		var repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("", repl);
	}

	[Fact]
	public void StringUtilities_get_article_null()
	{
		string test = null!;
		var    repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("", repl);
	}

	[Fact]
	public void StringUtilities_get_article_vowel_a()
	{
		var test = "artichoke";
		var repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("an", repl);
	}

	[Fact]
	public void StringUtilities_get_article_vowel_e()
	{
		var test = "ear";
		var repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("an", repl);
	}

	[Fact]
	public void StringUtilities_get_article_vowel_i()
	{
		var test = "iceburg";
		var repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("an", repl);
	}

	[Fact]
	public void StringUtilities_get_article_vowel_o()
	{
		var test = "otter";
		var repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("an", repl);
	}


	[Fact]
	public void StringUtilities_get_article_vowel_u()
	{
		var test = "umbrella";
		var repl = test.GetArticle();
		Assert.NotNull(repl);
		Assert.Equal("an", repl);
	}

	[Fact]
	public void StringUtilities_GetFileExtension_Verify_File_Extension_Not_Retrieved_From_Bad_Value()
	{
		const string file = "item";
		var          ext  = file.GetFileExtension();
		Assert.Equal(string.Empty, ext);
	}

	[Fact]
	public void StringUtilities_GetFileExtension_Verify_File_Extension_Retrieved_From_File()
	{
		const string file = "item.cs";
		var          ext  = file.GetFileExtension();
		Assert.Equal(".cs", ext);
	}

	[Fact]
	public void StringUtilities_GetFileExtension_Verify_File_Extension_Retrieved_From_File_With_Period_In_Name()
	{
		const string file = "item.cs.txt";
		var          ext  = file.GetFileExtension();
		Assert.Equal(".txt", ext);
	}


	[Fact]
	public void StringUtilities_Highlight_AddsPrefix()
	{
		var sample = "This is a test message";
		var expect = "This is a |test message";

		var r = sample.Highlight("|", new[] {"test"});
		Assert.Equal(expect, r);
	}

	[Fact]
	public void StringUtilities_Highlight_AddsSuffix()
	{
		var sample = "This is a test message";
		var expect = "This is a <test> message";
		var r      = sample.Highlight("<", new[] {"test"}, ">");
		Assert.Equal(expect, r);
	}

	[Fact]
	public void StringUtilities_Highlight_HandlesMultipleWords()
	{
		var sample = "This is a test message";
		var expect = "This is a <test> <message>";
		var r      = sample.Highlight("<", new[] {"test", "message"}, ">");
		Assert.Equal(expect, r);
	}

	[Fact]
	public void StringUtilities_Highlight_HandlesWordsStartingWith()
	{
		var sample = "This is a test message";
		var expect = "This is a test <message>";

		var r = sample.Highlight("<", new[] {"est", "mess"}, ">");

		Assert.Equal(expect, r);
	}

	[Fact]
	public void StringUtilities_Highlight_IngoresEmptyAndNull()
	{
		string sample = null!;
		var    r1     = sample.Highlight("before", Enumerable.Empty<string>());
		var    r2     = "".Highlight("before", Enumerable.Empty<string>());
		Assert.Equal("", r1);
		Assert.Equal("", r2);
	}

	[Fact]
	public void StringUtilities_Highlight_MergesPrefixAndSuffixNextToEachOther()
	{
		var sample = "This is a test message";
		var expect = "This is a <test message>";
		var r      = sample.Highlight("<", new[] {"test", "message"}, ">", true);
		Assert.Equal(expect, r);
	}

	[Fact]
	public void StringUtilities_Highlight_NotCaseSensitive()
	{
		var sample = "This is a test message";
		var expect = "This is a <test> message";
		var r      = sample.Highlight("<", new[] {"tEsT"}, ">");
		Assert.Equal(expect, r);
	}

	[Fact]
	public void StringUtilities_HtmlDecode_works_on_null()
	{
		string isNull  = null!;
		var    decoded = isNull.HtmlDecode();

		Assert.True(string.IsNullOrEmpty(decoded));
	}

	[Fact]
	public void StringUtilities_HtmlEncode_HtmlDecode_succeed()
	{
		var before  = "foo <bar>";
		var encoded = before.HtmlEncode();
		var after   = encoded.HtmlDecode();
		Assert.Equal(before, after);
		Assert.NotEqual(before, encoded);
	}

	[Fact]
	public void StringUtilities_HtmlEncode_works_on_null()
	{
		string isNull  = null!;
		var    encoded = isNull.HtmlEncode();
		Assert.True(string.IsNullOrEmpty(encoded));
	}

	[Fact]
	public void StringUtilities_QuickHash_empty()
	{
		Assert.Throws<ArgumentException>(() =>
		{
			var test = "";
			var repl = test.QuickHash();
			Assert.NotNull(repl);
			Assert.Equal("", repl);
		});
	}

	[Fact]
	public void StringUtilities_QuickHash_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			string test = null!;
			var    repl = test.QuickHash();
			Assert.NotNull(repl);
			Assert.Equal("", repl);
		});
	}

	[Fact]
	public void StringUtilities_QuickHash_returns_hash()
	{
		var test = "654";
		var repl = test.QuickHash();
		Assert.NotNull(repl);
	}

	[Fact]
	public void StringUtilities_TrimEnd_Success()
	{
		var test = "this is my test test";
		var res  = test.TrimEnd(" test");
		Assert.NotNull(res);
		Assert.Equal("this is my", res);
	}

	[Fact]
	public void StringUtilities_TrimStart_Success()
	{
		var test = "this this is my test";
		var res  = test.TrimStart("this ");
		Assert.NotNull(res);
		Assert.Equal("is my test", res);
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_1()
	{
		var s = "1";
		Assert.True(s.AsBool());
	}


	[Fact]
	public void StringUtilities_Verify_AS_Bool_empty1()
	{
		var s = "";
		Assert.False(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_empty2()
	{
		var s = string.Empty;
		Assert.False(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_Null()
	{
		string s = null!;
		Assert.False(s.AsBool());
	}


	[Fact]
	public void StringUtilities_Verify_AS_Bool_T()
	{
		var s = "T";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_T2()
	{
		var s = "t";
		Assert.True(s.AsBool());
	}


	[Fact]
	public void StringUtilities_Verify_AS_Bool_T3()
	{
		var s = "true";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_T4()
	{
		var s = "TrUe";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_True()
	{
		var s = "true";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_y()
	{
		var s = "y";
		Assert.True(s.AsBool());
	}


	[Fact]
	public void StringUtilities_Verify_AS_Bool_y2()
	{
		var s = "Y";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_yes()
	{
		var s = "Yes";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_yes2()
	{
		var s = "YES";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_AS_Bool_yes3()
	{
		var s = "yes";
		Assert.True(s.AsBool());
	}

	[Fact]
	public void StringUtilities_Verify_As_Password_replaces_empty()
	{
		var test = string.Empty;

		var test1 = test.AsPassword();

		Assert.Equal(string.Empty, test1);
	}

	[Fact]
	public void StringUtilities_Verify_As_Password_replaces_null()
	{
			const string test = null!;
			var          _    = test.AsPassword();
	}

	[Fact]
	public void StringUtilities_Verify_As_Password_replaces_string()
	{
		const string test = "Password";
		var          len  = test.Length;

		var test1 = test.AsPassword();
		var test2 = test.AsPassword("|");

		Assert.NotEqual(test, test1);
		Assert.NotEqual(test, test2);

		foreach (var s in test1.ToCharArray()) Assert.Equal('*', s);
		foreach (var s in test2.ToCharArray()) Assert.Equal('|', s);

		Assert.NotEqual(len, test1.Length);
		Assert.NotEqual(len, test2.Length);
	}

	[Fact]
	public void StringUtilities_Verify_camelCase_split_collapse_no_caps()
	{
		var test = "some word";
		var repl = test.SplitCamelCase();
		Assert.Equal("some word", repl);
	}

	[Fact]
	public void StringUtilities_Verify_camelCase_split_collapse_whitespace()
	{
		var test = "Some Word";
		var repl = test.SplitCamelCase();
		Assert.Equal("Some Word", repl);
	}

	[Fact]
	public void StringUtilities_Verify_camelCase_split_empty()
	{
		var test = "";
		var repl = test.SplitCamelCase();
		Assert.Equal("", repl);
	}

	[Fact]
	public void StringUtilities_Verify_camelCase_split_good()
	{
		var test = "someWord";
		var repl = test.SplitCamelCase();
		Assert.Equal("some Word", repl);
	}

	[Fact]
	public void StringUtilities_Verify_camelCase_split_good2()
	{
		var test = "SomeWord";
		var repl = test.SplitCamelCase();
		Assert.Equal("Some Word", repl);
	}


	[Fact]
	public void StringUtilities_Verify_camelCase_split_null()
	{
		string test = null!;
		var    repl = test.SplitCamelCase();
		Assert.Null(repl);
	}

	[Fact]
	public void StringUtilities_Verify_count_chars_empty()
	{
		var test  = "";
		var count = test.CountChars(' ');
		Assert.Equal(0, count);
	}

	[Fact]
	public void StringUtilities_Verify_count_chars_none()
	{
		var test  = "2";
		var count = test.CountChars('1');
		Assert.Equal(0, count);
	}

	[Fact]
	public void StringUtilities_Verify_count_chars_null()
	{
		var test  = (string) null!;
		var count = test.CountChars(' ');
		Assert.Equal(0, count);
	}

	[Fact]
	public void StringUtilities_Verify_count_chars_single()
	{
		var test  = "1";
		var count = test.CountChars('1');
		Assert.Equal(1, count);
	}

	[Fact]
	public void StringUtilities_Verify_count_chars_split()
	{
		var test  = "121";
		var count = test.CountChars('1');
		Assert.Equal(2, count);
	}


	[Fact]
	public void StringUtilities_Verify_count_chars_two_row()
	{
		var test  = "11";
		var count = test.CountChars('1');
		Assert.Equal(2, count);
	}

	[Fact]
	public void StringUtilities_Verify_File_Item_Parse_Returns_File_Name_For_Normal_Server_Item_Path()
	{
		const string path = "C:\\Folder\\Item.ext";
		var          item = path.GetFileName();
		Assert.Equal("Item.ext", item);
	}

	[Fact]
	public void StringUtilities_Verify_File_Item_Parse_Returns_Original_Value_For_Invalid_Path()
	{
		const string path = "Item.ext";
		var          item = path.GetFileName();
		Assert.Equal(path, item);
	}

	[Fact]
	public void StringUtilities_Verify_FilterChars()
	{
		var          filterChars = new[] {"/", "=", "?", "&", "+", "@"};
		const string expected    = @"Ih4ynRdrWwRejPwinunrDrNO68aNAvRbLmMIflciDkk";
		var          value       = @"Ih4/ynR//drWwRejPwi=nunr&&&DrNO?68aNA=v+RbL@mMIflciD@kk";

		value = value.FilterCharacters(filterChars);

		Assert.Equal(expected, value);

		foreach (var filterChar in filterChars) Assert.DoesNotContain(filterChar, value, StringComparison.OrdinalIgnoreCase);
	}

	[Fact]
	public void StringUtilities_Verify_FilterChars_empty()
	{
		Assert.Throws<ArgumentException>(() =>
		{
			var filterChars = new[] {"/", "=", "?", "&", "+", "@"};
			var value       = "";

			_ = value.FilterCharacters(filterChars);
		});
	}

	[Fact]
	public void StringUtilities_Verify_FilterChars_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var    filterChars = new[] {"/", "=", "?", "&", "+", "@"};
			string value       = null!;

			_ = value.FilterCharacters(filterChars);
		});
	}


	[Fact]
	public void StringUtilities_Verify_Hash_fails_emptyl()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var s = string.Empty;
			var _ = s.Hash();
		});
	}

	[Fact]
	public void StringUtilities_Verify_Hash_fails_null()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			string s = null!;
			var    _ = s.Hash();
		});
	}

	[Fact]
	public void StringUtilities_Verify_Hash_generates_hash()
	{
		var s    = "test";
		var hash = s.Hash();

		Assert.NotEqual(s, hash);
	}

	[Fact]
	public void StringUtilities_Verify_Hash_generates_hash_differences()
	{
		var s     = "test";
		var hash  = s.Hash();
		var s2    = "test2";
		var hash2 = s2.Hash();

		Assert.NotEqual(s,    hash);
		Assert.NotEqual(s2,   hash);
		Assert.NotEqual(hash, hash2);
	}

	[Theory]
	[InlineData("abc",                                                      "abc")]
	[InlineData("abc.jpg",                                                  "abc.jpg")]
	[InlineData("ab\"c.jpg",                                                "ab-c.jpg")]
	[InlineData("ab?c.jpg",                                                 "ab-c.jpg")]
	[InlineData("abc's.jpg",                                                "abc-s.jpg")]
	[InlineData("\"M<>\"\\a/ry/ h**ad:>> a\\/:*?\"<>| li*tt|le|| la\"mb.?", "-M----a-ry- h--ad--- a--------- li-tt-le-- la-mb.-")]
	public void StringUtilities_Verify_InvaildFileNameCharsStripped(string test, string expected)
	{
		var sanitized = test.ToValidPathName();
		Assert.Equal(expected, sanitized);
	}

	[Fact]
	public void StringUtilities_AssertFileNameHasValue() => Assert.Throws<ArgumentNullException>(() => { ((string) null!).ToValidPathName(); });

	[Fact]
	public void StringUtilities_Verify_Join_Empty()
	{
		var list   = new List<string>();
		var joined = list.Join(",");
		Assert.True(string.IsNullOrEmpty(joined));
	}

	[Fact]
	public void StringUtilities_Verify_Join_object_delim()
	{
		var          list     = new List<object> {1, 2, 3};
		const string expected = @"1-2-3";
		var          joined   = list.Join("-");
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_Join_object_valid()
	{
		var          list     = new List<object> {1, 2, 3};
		const string expected = @"1, 2, 3";
		var          joined   = list.Join(", ");
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_Join_Valid()
	{
		var          list     = new List<string> {"1", "2", "3"};
		const string expected = @"1, 2, 3";
		var          joined   = list.Join(", ");
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_Join_Valid_custom_delimiter()
	{
		var          list     = new List<string> {"1", "2", "3"};
		const string expected = @"1-2-3";
		var          joined   = list.Join("-");
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_Join_Valid_empty_prefix()
	{
		var          list     = new List<string> {"1", "2", "3"};
		const string expected = @"1, 2, 3";
		var          joined   = list.Join(", ", "");
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_Join_Valid_null_prefix()
	{
		var          list     = new List<string> {"1", "2", "3"};
		const string expected = @"1, 2, 3";
		var          joined   = list.Join(", ");
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_Join_Valid_with_prefix()
	{
		var          list     = new List<string> {"1", "2", "3"};
		const string expected = @"num-1, num-2, num-3";
		var          joined   = list.Join(", ", "num-");
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_JoinWith_Valid_delimiter()
	{
		var          list     = new List<string> {"1", "2", "3"};
		const string expected = @"foo-1-2-3";
		var          joined   = "foo".JoinWith("-", list);
		Assert.Equal(expected, joined);
	}

	[Fact]
	public void StringUtilities_Verify_Last_Word_Wrap()
	{
		const string sample   = "this is my sample";
		const string expected = "this is my <span>sample</span>";
		var          actual   = sample.WrapLastWord("<span>", "</span>");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_replace_after_empty()
	{
		var test = "";
		var repl = test.ReplaceAfter(2, ".");
		Assert.Equal("", repl);
	}

	[Fact]
	public void StringUtilities_Verify_replace_after_equal()
	{
		var test = "12";
		var repl = test.ReplaceAfter(2, ".");
		Assert.Equal("12", repl);
	}

	[Fact]
	public void StringUtilities_Verify_replace_after_exceeds()
	{
		var test = "123";
		var repl = test.ReplaceAfter(2, ".");
		Assert.Equal(".", repl);
	}

	[Fact]
	public void StringUtilities_Verify_replace_after_null()
	{
		string test = null!;
		var    repl = test.ReplaceAfter(2, ".");
		Assert.Equal("", repl);
	}


	[Fact]
	public void StringUtilities_Verify_replace_after_within()
	{
		var test = "1";
		var repl = test.ReplaceAfter(2, ".");
		Assert.Equal("1", repl);
	}

	[Fact]
	public void StringUtilities_Verify_replace_all_chars()
	{
		var alpha    = "abcdef";
		var novowels = "-bcd-f";

		var test = alpha.ReplaceAll(new[] {'a', 'e'}, '-');
		Assert.Equal(test, novowels);
	}

	[Fact]
	public void StringUtilities_Verify_Replace_Last_Occurrence_Empty()
	{
		const string sample   = "";
		const string expected = "";
		var          actual   = sample.ReplaceLastOccurrence("NOT_FOUND", "replaced");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_Replace_Last_Occurrence_Normal_Case()
	{
		const string sample   = "this_is_my_sample_this";
		const string expected = "this_is_my_sample_replaced";
		var          actual   = sample.ReplaceLastOccurrence("this", "replaced");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_Replace_Last_Occurrence_Not_Found()
	{
		const string sample   = "this_is_my_sample_this";
		const string expected = "this_is_my_sample_this";
		var          actual   = sample.ReplaceLastOccurrence("NOT_FOUND", "replaced");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_Replace_Last_Occurrence_Null()
	{
		const string sample   = null!;
		const string expected = "";
		var          actual   = sample.ReplaceLastOccurrence("NOT_FOUND", "replaced");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_Return_Last_Occurrence()
	{
		const string test = "file.name.ext";
		var          last = test.ReturnLastOccurrence('.');
		Assert.Equal("ext", last);
	}


	[Fact]
	public void StringUtilities_Verify_Split_And_Return_Does_Not_Split_String_Without_Demarcation()
	{
		const string list = "test";
		var          item = list.SplitAndReturnFirst();
		Assert.Equal("test", item);
	}


	[Fact]
	public void StringUtilities_Verify_Split_Does_Not_Fail_On_Null_Value()
	{
		const string list = null!;
		var          item = list.SplitAndReturnFirst();
		Assert.Null(item);
	}

	[Fact]
	public void StringUtilities_Verify_Split_Does_Not_String_Empty_String()
	{
		var list = string.Empty;
		var item = list.SplitAndReturnFirst();
		Assert.NotNull(item);
		Assert.Equal(string.Empty, item);
	}

	[Fact]
	public void StringUtilities_Verify_Split_Returns_First_Value_From_List()
	{
		const string list = "a,b,c,d,e";
		var          item = list.SplitAndReturnFirst();
		Assert.Equal("a", item);
	}

	[Fact]
	public void StringUtilities_Verify_Split_Wrap_Quotes_Empty()
	{
		const string list     = "";
		const string expected = "";
		var          actual   = list.SplitAndWrapQuotes();
		Assert.Equal(expected, actual);
	}


	[Fact]
	public void StringUtilities_Verify_Split_Wrap_Quotes_Empty_Entry_Case()
	{
		const string list     = "item1,,item2";
		const string expected = "'item1','item2'";
		var          actual   = list.SplitAndWrapQuotes();
		Assert.Equal(expected, actual);
	}


	[Fact]
	public void StringUtilities_Verify_Split_Wrap_Quotes_Normal_Case()
	{
		const string list     = "item1,item2";
		const string expected = "'item1','item2'";
		var          actual   = list.SplitAndWrapQuotes();
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_Split_Wrap_Quotes_Null()
	{
		const string list     = null!;
		const string expected = "";
		var          actual   = list.SplitAndWrapQuotes();
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_Split_Wrap_Quotes_Single_Entry_Case()
	{
		const string list     = "item1";
		const string expected = "'item1'";
		var          actual   = list.SplitAndWrapQuotes();
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Condense_Multiple_NewLines()
	{
		const string html  = "<html>th<b>e</b>       \r\n\r\n        le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the \r\n\r\n le", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Convert_Br_to_newline()
	{
		const string html  = "<html>th<b>e</b>    <br>           le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the \r\n le", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Convert_Br_to_newline_2()
	{
		const string html  = "<html>th<b>e</b>    <br/>           le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the \r\n le", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Convert_Br_to_newline_3()
	{
		const string html  = "<html>th<b>e</b>    <br />           le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the \r\n le", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Convert_p_to_newline()
	{
		const string html  = "<html>th<b>e</b>    <p>           le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the \r\n le", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Convert_p_to_newline_2()
	{
		const string html  = "<html>th<b>e</b>    </p>           le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the \r\n le", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Not_Condense_Multiple_Spaces()
	{
		const string html  = "<html>th<b>e</b>               le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the le", strip);
	}


	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_NOT_Strip_SOME_Encoded_Entities()
	{
		const string html  = "<html>sam<b>p</b>le&nbsp;&lt;a&gt;link&lt;/a&gt;</html>"; // sbs-3428
		var          strip = html.StripHtml();
		Assert.Equal("sample &lt;a&gt;link&lt;/a&gt;", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Space_table_cells()
	{
		const string html  = "<html><table><tr><td>two</td><td>words</td></tr></table></html>";
		var          strip = html.StripHtml();
		Assert.Equal("two words", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Strip_Html_Tags()
	{
		const string html  = "<html>sam<b>p</b>le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("sample", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Strip_Null_Values()
	{
		const string html = null!;
		var          b    = html.StripHtml();
		Assert.NotNull(b);
		Assert.Equal(string.Empty, b);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Strip_SOME_Encoded_Entities()
	{
		const string html  = "<html>sam<b>p</b>le&nbsp;&amp;&nbsp;<i>&quot;boo&quot;</i><i>&nbsp;&tilde;</i></html>";
		var          strip = html.StripHtml();
		Assert.Equal("sample & \"boo\" ~", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_Strip_Spaces()
	{
		const string html  = "<html>th<b>e</b>&nbsp;le</html>";
		var          strip = html.StripHtml();
		Assert.Equal("the le", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_trim_new_lines()
	{
		const string html  = "<html><table><tr><td>two</td><td>words</td></tr></table></html> \r\n ";
		var          strip = html.StripHtml();
		Assert.Equal("two words", strip);
	}

	[Fact]
	public void StringUtilities_Verify_StripHtml_Will_trim_spaces()
	{
		const string html  = "<html><table><tr><td>two</td><td>words</td></tr></table></html>  ";
		var          strip = html.StripHtml();
		Assert.Equal("two words", strip);
	}

	[Fact]
	public void StringUtilities_Verify_Truncation_Empty_returns_empty()
	{
		var word = "";
		var res  = word.Truncate(2);
		Assert.Equal("", res);
	}

	[Fact]
	public void StringUtilities_Verify_Truncation_Null_returns_empty()
	{
		string word = null!;
		var    res  = word.Truncate(2);
		Assert.Equal("", res);
	}

	[Fact]
	public void StringUtilities_Verify_Truncation_returns_correct_Length()
	{
		var word = "test";
		var res  = word.Truncate(2, null);
		Assert.Equal("te", res);
	}


	[Fact]
	public void StringUtilities_Verify_Truncation_returns_correct_Length_with_elip()
	{
		var word = "test";
		var res  = word.Truncate(2);
		Assert.Equal("te…", res);
	}

	[Fact]
	public void StringUtilities_Verify_Url_Filter_Empty()
	{
		var test = string.Empty;
		var res  = test.FilterUrlString();
		Assert.Equal("", res);
	}

	[Fact]
	public void StringUtilities_Verify_Url_Filter_Null()
	{
		string test = null!;
		var    res  = test.FilterUrlString();
		Assert.Null(res);
	}


	[Fact]
	public void StringUtilities_Verify_Url_Filter_ReplacesSpacesWithHyphen()
	{
		var test = "this is my test";
		var res  = test.FilterUrlString();
		Assert.NotNull(res);
		Assert.Equal("this-is-my-test", res);
	}


	[Fact]
	public void StringUtilities_Verify_Url_Filter_StripsNonAlpha()
	{
		var test = "this is my test! 22";
		var res  = test.FilterUrlString();
		Assert.NotNull(res);
		Assert.Equal("this-is-my-test-22", res);
	}

	public class PlacholderTestData : TheoryData<string, bool>
	{
		public PlacholderTestData()
		{
			Add(null,                          true);
			Add("",                            true);
			Add(string.Empty,                  true);
			Add("test",                        false);
			Add("(test)",                      true);
			Add("I'm a (test)",                false);
			Add("Lorem Ipsum doler",           true);
			Add("This is a Lorem Ipsum doler", false);
		}
	}

	[ExcludeFromCodeCoverage]
	private class CredentialTestData : TheoryData<string, string, string>
	{
		public CredentialTestData()
		{
			Add("un",  "pw",  "Basic dW46cHc=");
			Add("un2", "pw2", "Basic dW4yOnB3Mg==");
			Add(null,  null,  "Basic Og==");
			Add(null,  "pw",  "Basic OnB3");
			Add("un",  null,  "Basic dW46");
		}
	}
}
