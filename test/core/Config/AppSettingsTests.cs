// perticula - core.test - AppSettingsTests.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Configuration;
using System.Globalization;
using System.Reflection;
using core.Config;
using Newtonsoft.Json;

namespace core.test.Config;

[Collection("RunConfigSequential")]
public class AppSettingTests
{
	private IAppSettings createMock_IAppSettings_TestAppSettings(IDictionary<string, string>? mockSettings = null)
	{
		// Clear the current app settings
		ConfigurationManager.RefreshSection("appSettings");

		// Add any?
		if (mockSettings != null)
			foreach (var (key, value) in mockSettings)
				ConfigurationManager.AppSettings[key] = value;

		return new AppSettings();
	}

	private IAppSettings createMock_IAppSettings_TestConnectionSz(IDictionary<string, string>? mockConnectSz = null)
	{
		// Clear the current app settings
		ConfigurationManager.RefreshSection("connectionStrings");

		var config = ConfigurationManager.ConnectionStrings;//.OpenExeConfiguration(ConfigurationUserLevel.None);

		// Disable to readonly flag (for our tests)
		var hackReadonly = typeof(ConfigurationElementCollection).GetField("_readOnly", BindingFlags.Instance | BindingFlags.NonPublic);
		hackReadonly.SetValue(ConfigurationManager.ConnectionStrings, false);

		mockConnectSz ??= new Dictionary<string, string>(0);

		foreach (var (key, value) in mockConnectSz)
			config.Add(new ConnectionStringSettings(key, value));

		return new AppSettings();
	}

	[Fact]
	public void IAppSettings_ConnectionString_Expects_Name()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var store = createMock_IAppSettings_TestConnectionSz();

			store.ConnectionString(null);
		});
	}


	[Fact]
	public void IAppSettings_ConnectionString_MachineSpecific_SettingNames_TakePriority()
	{
		var store = createMock_IAppSettings_TestConnectionSz(new Dictionary<string, string>
			{
				{Environment.MachineName + ":FOO", "BAR"},
				{"FOO", "NOT_ME!"}
			}
		);

		var r = store.ConnectionString("FOO");

		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_ConnectionString_NotCaseSensitive()
	{
		var store = createMock_IAppSettings_TestConnectionSz(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);

		var r = store.ConnectionString("foo");

		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_ConnectionString_ReturnsNullIfNotFound()
	{
		Assert.Throws<KeyNotFoundException>(() =>
		{
			var store = createMock_IAppSettings_TestConnectionSz();
			var r     = store.ConnectionString("FOO");
		});
	}

	[Fact]
	public void IAppSettings_ConnectionString_Succeeds()
	{
		var store = createMock_IAppSettings_TestConnectionSz(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);
		var r = store.ConnectionString("FOO");

		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_ConnectionString_Supports_MachineSpecific_SettingNames()
	{
		var store = createMock_IAppSettings_TestConnectionSz(new Dictionary<string, string>
			{
				{Environment.MachineName + ":FOO", "BAR"}
			}
		);


		var r = store.ConnectionString("FOO");


		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_GetSetting_Expects_Name()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var store = createMock_IAppSettings_TestAppSettings();
			store.GetSetting(null);
		});
	}

	[Fact]
	public void IAppSettings_GetSetting_MachineSpecific_SettingNames_TakePriority()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{Environment.MachineName + ":FOO", "BAR"},
				{"FOO", "NOT_ME!"}
			}
		);

		var r = store.GetSetting("FOO");

		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_GetSetting_NotCaseSensitive()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);

		var r = store.GetSetting("foo");

		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_GetSetting_Succeeds()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);
		var r = store.GetSetting("FOO");
		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_GetSetting_Supports_MachineSpecific_SettingNames()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{Environment.MachineName + ":FOO", "BAR"}
			}
		);

		var r = store.GetSetting("FOO");

		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}

	[Fact]
	public void IAppSettings_GetSetting_T_Bool_Succeeds()
	{
		var expect = true;
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOOlala", expect.ToString()}
			}
		);

		var r = store.GetSetting<bool>("FOOlala");

		Assert.Equal(expect, r);
	}

	[Fact]
	public void IAppSettings_GetSetting_T_DateTime_Succeeds()
	{
		var expect = DateTime.Now;
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", expect.ToString()}
			}
		);

		var r = store.GetSetting<DateTime>("FOO");

		Assert.Equal(expect.ToString(CultureInfo.InvariantCulture), r.ToString(CultureInfo.InvariantCulture));
	}

	[Fact]
	public void IAppSettings_GetSetting_T_Double_Succeeds()
	{
		var expect = 1.2345d;
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", expect.ToString(CultureInfo.InvariantCulture)}
			}
		);

		var r = store.GetSetting<double>("FOO");

		Assert.Equal(expect, r);
	}


	[Fact]
	public void IAppSettings_GetSetting_T_Expects_Name()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var store = createMock_IAppSettings_TestAppSettings();


			store.GetSetting(null);
		});
	}

	[Fact]
	public void IAppSettings_GetSetting_T_Fails_When_Types_Dont_Match()
	{
		Assert.Throws<FormatException>(() =>
		{
			var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
				{
					{"FOO", DateTime.Now.ToString()}
				}
			);

			store.GetSetting<bool>("FOO");
		});
	}

	[Fact]
	public void IAppSettings_GetSetting_T_Int_Succeeds()
	{
		var expect = 555;
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", expect.ToString()}
			}
		);

		var r = store.GetSetting<int>("FOO");

		Assert.Equal(expect, r);
	}

	[Fact]
	public void IAppSettings_GetSetting_T_JSon_Succeeds()
	{
		var expect = new[] {"this", "should", "store", "as", "json"};
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", JsonConvert.SerializeObject(expect)}
			}
		);

		var r = store.GetSetting<string[]>("FOO");

		Assert.True(expect.SequenceEqual(r));
	}

	[Fact]
	public void IAppSettings_GetSetting_T_Long_Succeeds()
	{
		var expect = 555L;
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"f555l", expect.ToString()}
			}
		);

		var r = store.GetSetting<long>("f555l");

		//TODO: TEST is flaky,  find option to fix
		Assert.Equal(expect, r);
	}

	[Fact]
	public void IAppSettings_GetSetting_T_NotCaseSensitive()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", 2.ToString()}
			}
		);

		var r = store.GetSetting<int>("foo");
		Assert.Equal(2, r);
	}

	[Fact]
	public void IAppSettings_GetSetting_T_Succeeds()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", 1.ToString()}
			}
		);

		var r = store.GetSetting<int>("FOO");
		Assert.Equal(1, r);
	}

	[Fact]
	public void IAppSettings_GetSetting_T_Supports_MachineSpecific_SettingNames()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{Environment.MachineName + ":FOO", 456.ToString()}
			}
		);
		var r = store.GetSetting<int>("FOO");
		Assert.Equal(456, r);
	}

	[Fact]
	public void IAppSettings_HasConnectionString_NotCaseSensitive()
	{
		var store = createMock_IAppSettings_TestConnectionSz(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);
		var r = store.HasConnectionString("foo");
		Assert.True(r, "HasConnectionString returned false");
	}

	[Fact]
	public void IAppSettings_HasConnectionString_OnMachineSettings_ReturnsTrue()
	{
		var store = createMock_IAppSettings_TestConnectionSz(new Dictionary<string, string>
			{
				{Environment.MachineName + ":FOO", "BAR"}
			}
		);
		var r = store.HasConnectionString("FOO");
		Assert.True(r, "HasConnectionString returned false");
	}

	[Fact]
	public void IAppSettings_HasConnectionString_ReturnsFalse()
	{
		var store = createMock_IAppSettings_TestConnectionSz();
		var r     = store.HasConnectionString("FOO");
		Assert.False(r, "HasConnectionString returned true");
	}

	[Fact]
	public void IAppSettings_HasConnectionString_ReturnsFalse_OnMissingName()
	{
		var store = createMock_IAppSettings_TestConnectionSz();
		var r     = store.HasConnectionString(null);
		Assert.False(r, "HasConnectionString returned true");
	}

	[Fact]
	public void IAppSettings_HasConnectionString_ReturnsTrue()
	{
		var store = createMock_IAppSettings_TestConnectionSz(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);

		var r = store.HasConnectionString("FOO");
		Assert.True(r, "HasConnectionString returned false");
	}

	[Fact]
	public void IAppSettings_HasSetting_NotCaseSensitive()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);
		var r = store.HasSetting("foo");
		Assert.True(r, "HasSetting returned false");
	}

	[Fact]
	public void IAppSettings_HasSetting_ReturnsFalse()
	{
		var store = createMock_IAppSettings_TestAppSettings();
		var r     = store.HasSetting("FOO");
		Assert.False(r, "HasSetting returned true");
	}

	[Fact]
	public void IAppSettings_HasSetting_ReturnsFalse_OnMissingName()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var store = createMock_IAppSettings_TestAppSettings();
			var r     = store.HasSetting(null);
		});
	}

	[Fact]
	public void IAppSettings_HasSetting_ReturnsTrue()
	{
		var store = createMock_IAppSettings_TestAppSettings(new Dictionary<string, string>
			{
				{"FOO", "BAR"}
			}
		);
		var r = store.HasSetting("FOO");
		Assert.True(r, "HasSetting returned false");
	}

	[Fact]
	public void IAppSettings_Uses_ConfigurationManager()
	{
		var store = createMock_IAppSettings_TestAppSettings();
		ConfigurationManager.AppSettings["FOO"] = "BAR";
		var r = store.GetSetting("FOO");

		Assert.NotNull(r);
		Assert.Equal("BAR", r);
	}
}
