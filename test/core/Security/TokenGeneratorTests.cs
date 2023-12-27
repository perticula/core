// perticula - core.test - TokenGeneratorTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Configuration;
using System.Diagnostics;
using core.Config;
using core.Security;
using Moq;

namespace core.test.Security;

public class TokenGeneratorTests
{
	private TokenGenerator CreateMock_TokenGenerator(string name = "RETURN", string? testValue = null)
	{
		var mockIAppSettings = new Mock<IAppSettings>();
		testValue ??= "cx3tBnJiXGTM83tFqR4eZhsvkfpWuUZFjWIux6X/dgE=|JJjtY4viwyhk4Trl9odh9w==";
		mockIAppSettings.Setup(m => m.HasSetting("RETURN")).Returns(true);
		mockIAppSettings.Setup(m => m.GetSetting("RETURN")).Returns(testValue);
		return new TokenGenerator(mockIAppSettings.Object, name);
	}

	private ITokenGenerator CreateMock_ITokenGenerator(string name = "RETURN", string testValue = null) => CreateMock_TokenGenerator(name, testValue);

	[Fact]
	public void ITokenGenerator_Expects_SettingParam()
	{
		Assert.Throws<ArgumentNullException>(() => { CreateMock_ITokenGenerator(null); });
	}

	[Fact]
	public void ITokenGenerator_Expects_SettingParam_ToBeConfigSetting()
	{
		Assert.Throws<ConfigurationErrorsException>(() => { CreateMock_ITokenGenerator("FOO"); });
	}

	[Fact]
	public void ITokenGenerator_Expects_SettingParam_Value_ToHaveTwo_Base64Sz_DividedByA_Pipe()
	{
		Assert.Throws<ConfigurationErrorsException>(() => { CreateMock_ITokenGenerator("Test_Bad_Key", "FOO"); });
	}

	[Fact]
	public void ITokenGenerator_toBase64Token_Strips_NoneAlphaNumeric()
	{
		var gen  = CreateMock_TokenGenerator();
		var data = new byte[100];
		new System.Random().NextBytes(data);
		var raw = Convert.ToBase64String(data);

		var token = TokenGenerator.ToBase64Token(data);

		Assert.NotNull(token);
		Assert.False(string.IsNullOrEmpty(token), "toBase64Token returned empty string");
		Assert.True(token.ToCharArray().All(char.IsLetterOrDigit), "token: " + token + " contains none-alpha-numeric chars from: " + raw);
	}

	[Fact]
	public void ITokenGenerator_toUrlFriendlyBase64_Replaces_Slash_Plus_And_Equals()
	{
		var gen = CreateMock_TokenGenerator();

		var bytes = Convert.FromBase64String("Go/Foo+Bar==");
		var token = TokenGenerator.ToUrlFriendlyBase64(bytes);

		Assert.NotNull(token);
		Assert.False(string.IsNullOrEmpty(token),              "toUrlFriendlyBase64 returned empty string");
		Assert.False(token.ToCharArray().Any(ch => ch == '/'), "token: " + token + " contains a slash");
		Assert.False(token.ToCharArray().Any(ch => ch == 'o'), "token: " + token + " contains a o");
		Assert.False(token.ToCharArray().Any(ch => ch == '+'), "token: " + token + " contains a plus");
		Assert.False(token.ToCharArray().Any(ch => ch == '='), "token: " + token + " contains an equals");
	}

	[Fact]
	public void ITokenGenerator_fromUrlFriendlyBase64_Restores_Slash_Plus_And_Equals()
	{
		var gen    = CreateMock_TokenGenerator();
		var expect = Convert.FromBase64String("Go/Foo+Bar==");
		var url    = TokenGenerator.ToUrlFriendlyBase64(expect);

		var result = TokenGenerator.FromUrlFriendlyBase64(url);

		Assert.NotNull(result);
		Assert.False(result.Length < 1, "fromUrlFriendlyBase64 returned empty array");
		Assert.True(expect.SequenceEqual(result), "encoded data does not match");
		Assert.False(url.ToCharArray().Any(ch => ch == '/'), "url: "   + url + " contains a slash");
		Assert.False(url.ToCharArray().Any(ch => ch == 'o'), "token: " + url + " contains a o");
		Assert.False(url.ToCharArray().Any(ch => ch == '+'), "url: "   + url + " contains a plus");
		Assert.False(url.ToCharArray().Any(ch => ch == '='), "url: "   + url + " contains an equals");
	}

	[Fact]
	public void ITokenGenerator_GenerateRouteToken_Creates_Consistant_Tokens_PerDay()
	{
		var gen  = CreateMock_ITokenGenerator();
		var date = new DateTime(2015, 12, 07, 0, 0, 0, DateTimeKind.Utc);

		var token1 = gen.GenerateAutoExpireToken(date);
		var token2 = gen.GenerateAutoExpireToken(date);
		var token3 = gen.GenerateAutoExpireToken(date);
		var token4 = gen.GenerateAutoExpireToken();
		var token5 = gen.GenerateAutoExpireToken();
		var token6 = gen.GenerateAutoExpireToken();

		Assert.False(string.IsNullOrEmpty(token1), "GenerateRouteToken returned empty token");
		Assert.False(string.IsNullOrEmpty(token2), "GenerateRouteToken returned empty token");
		Assert.False(string.IsNullOrEmpty(token3), "GenerateRouteToken returned empty token");
		Assert.False(string.IsNullOrEmpty(token4), "GenerateRouteToken returned empty token");
		Assert.False(string.IsNullOrEmpty(token5), "GenerateRouteToken returned empty token");
		Assert.False(string.IsNullOrEmpty(token6), "GenerateRouteToken returned empty token");
		Assert.Equal(token1, token2);
		Assert.Equal(token1, token2);
		Assert.Equal(token4, token5);
		Assert.Equal(token5, token6);
	}

	[Fact]
	public void ITokenGenerator_GenerateRouteToken_Accepts_A_Date()
	{
		const string expected = @"Ebs7YhtfHEnUsfcyrTXYZiho7phbksZbDysuTfzmvF4";
		var          gen      = CreateMock_ITokenGenerator();
		var          date     = new DateTime(2015, 12, 07, 0, 0, 0, DateTimeKind.Utc);

		var token = gen.GenerateAutoExpireToken(date);

		Assert.False(string.IsNullOrEmpty(token), "GenerateRouteToken returned empty token");
		Assert.Equal(expected, token);
	}

	[Fact]
	public void ITokenGenerator_GenerateRouteToken_Uses_UtcNow_For_Default_Date()
	{
		var gen = CreateMock_ITokenGenerator();

		var token1 = gen.GenerateAutoExpireToken(DateTime.UtcNow);
		var token2 = gen.GenerateAutoExpireToken();

		Assert.False(string.IsNullOrEmpty(token1), "GenerateRouteToken returned empty token");
		Assert.False(string.IsNullOrEmpty(token2), "GenerateRouteToken returned empty token");
		Assert.Equal(token1, token2);
	}

	[Fact]
	public void ITokenGenerator_GenerateNewEncryptionKeys_Returns_ValidKeys()
	{
		var gen = CreateMock_ITokenGenerator();

		var newKeys = gen.GenerateNewEncryptionKeys();
		CreateMock_ITokenGenerator("RETURN", newKeys);

		Assert.False(string.IsNullOrEmpty(newKeys), "GenerateNewEncryptionKeys returned empty key");
	}

	[Fact]
	public void ITokenGenerator_GenerateNewEncryptionKeys_Returns_RandomKeys()
	{
		var gen = CreateMock_ITokenGenerator();

		var newKeys1 = gen.GenerateNewEncryptionKeys();
		var newKeys2 = gen.GenerateNewEncryptionKeys();

		Assert.False(string.IsNullOrEmpty(newKeys1), "GenerateNewEncryptionKeys returned empty key");
		Assert.False(string.IsNullOrEmpty(newKeys2), "GenerateNewEncryptionKeys returned empty key");
		Assert.NotEqual(newKeys1, newKeys2);
		Trace.WriteLine(newKeys1);
	}

	[Fact]
	public void ITokenGenerator_GenerateAuthorizationToken_Expects_Duration_1_to_9999_Over()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			var gen = CreateMock_ITokenGenerator();
			var _   = gen.GenerateAuthorizationToken(Guid.NewGuid(), 9999 + 1);
		});
	}

	[Fact]
	public void ITokenGenerator_GenerateAuthorizationToken_Expects_Duration_1_to_9999_Under()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			var gen = CreateMock_ITokenGenerator();
			var _   = gen.GenerateAuthorizationToken(Guid.NewGuid(), 1 - 1);
		});
	}

	[Fact]
	public void ITokenGenerator_GenerateAuthorizationToken_Succeeds()
	{
		var gen = CreateMock_ITokenGenerator();

		var token = gen.GenerateAuthorizationToken(Guid.NewGuid(), 20);

		Assert.False(string.IsNullOrEmpty(token), "GenerateAuthorizationToken returned empty token");
	}

	[Fact]
	public void ITokenGenerator_GenerateAuthorizationToken_Accepts_StartDate()
	{
		var gen = CreateMock_ITokenGenerator();

		var token = gen.GenerateAuthorizationToken(Guid.NewGuid(), 20, DateTime.UtcNow.AddDays(-7));

		Assert.False(string.IsNullOrEmpty(token), "GenerateAuthorizationToken returned empty token");
	}

	[Fact]
	public void ITokenGenerator_GenerateAuthorizationToken_Expects_StartDate_CurrentOrInPast()
	{
		Assert.Throws<ArgumentException>(() =>
		{
			var gen = CreateMock_ITokenGenerator();

			var _ = gen.GenerateAuthorizationToken(Guid.NewGuid(), 20, DateTime.UtcNow.AddHours(2));
		});
	}

	[Fact]
	public void ITokenGenerator_GenerateAuthorizationToken_Uses_UrlFriendlyBase64()
	{
		var gen = CreateMock_ITokenGenerator();

		var token = gen.GenerateAuthorizationToken(Guid.NewGuid(), 20, DateTime.UtcNow.AddDays(-7));

		Assert.False(string.IsNullOrEmpty(token),              "GenerateAuthorizationToken returned empty token");
		Assert.False(token.ToCharArray().Any(ch => ch == '/'), "token: " + token + " contains a slash");
		Assert.False(token.ToCharArray().Any(ch => ch == '+'), "token: " + token + " contains a plus");
		Assert.False(token.ToCharArray().Any(ch => ch == 'o'), "token: " + token + " contains a o");
		Assert.False(token.ToCharArray().Any(ch => ch == '='), "token: " + token + " contains an equals");
	}

	[Fact]
	public void ITokenGenerator_ValidateAuthorizationToken_Returns_GuidEmpty_OnBadInput()
	{
		var gen = CreateMock_ITokenGenerator();

		var r1 = gen.ValidateAuthorizationToken(null);
		var r2 = gen.ValidateAuthorizationToken("");
		var r3 = gen.ValidateAuthorizationToken("Zm9vIGJhcg==");
		var r4 = gen.ValidateAuthorizationToken("Zm9vIGJhcg..");
		var r5 = gen.ValidateAuthorizationToken("%foo% (bar)");

		Assert.Equal(Guid.Empty, r1);
		Assert.Equal(Guid.Empty, r2);
		Assert.Equal(Guid.Empty, r3);
		Assert.Equal(Guid.Empty, r4);
		Assert.Equal(Guid.Empty, r5);
	}

	[Fact]
	public void ITokenGenerator_ValidateAuthorizationToken_Succeeds()
	{
		var gen = CreateMock_ITokenGenerator();

		var expect = Guid.NewGuid();
		var token  = gen.GenerateAuthorizationToken(expect, 600);
		var result = gen.ValidateAuthorizationToken(token);

		Assert.NotEqual(result, Guid.Empty);
		Assert.Equal(expect, result);
		Trace.WriteLine(token);
	}

	[Fact]
	public void ITokenGenerator_ValidateAuthorizationToken_Tokens_AreLikeSnowflakes()
	{
		var gen = CreateMock_ITokenGenerator();

		var expect  = Guid.NewGuid();
		var token1  = gen.GenerateAuthorizationToken(expect, 600);
		var token2  = gen.GenerateAuthorizationToken(expect, 600);
		var result1 = gen.ValidateAuthorizationToken(token1);
		var result2 = gen.ValidateAuthorizationToken(token2);

		Assert.NotEqual(token1,  token2);
		Assert.NotEqual(result1, Guid.Empty);
		Assert.NotEqual(result2, Guid.Empty);
		Assert.Equal(expect, result1);
		Assert.Equal(expect, result2);
	}

	[Fact]
	public void ITokenGenerator_ValidateAuthorizationToken_Returns_GuidEmpty_OnExpiredToken()
	{
		var gen = CreateMock_ITokenGenerator();

		var token  = gen.GenerateAuthorizationToken(Guid.NewGuid(), 60, DateTime.UtcNow.AddSeconds(-90));
		var result = gen.ValidateAuthorizationToken(token);

		Assert.Equal(result, Guid.Empty);
	}

	[Fact]
	public void ITokenGenerator_ValidateAuthorizationToken_Returns_GuidEmpty_TokensCreatedBy_OtherKeys()
	{
		var gen1    = CreateMock_ITokenGenerator();
		var newKeys = gen1.GenerateNewEncryptionKeys();
		var gen2    = CreateMock_ITokenGenerator("RETURN", newKeys);

		var token  = gen1.GenerateAuthorizationToken(Guid.NewGuid(), 600);
		var result = gen2.ValidateAuthorizationToken(token);

		Assert.Equal(result, Guid.Empty);
	}
}
