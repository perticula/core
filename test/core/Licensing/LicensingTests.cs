using System.Globalization;
using System.Xml.Linq;
using core.Cryptography;
using core.Licensing;

namespace core.test.Licensing;

public class LicensingTests
{
	private readonly string passPhrase;
	private readonly string privateKey;
	private readonly string publicKey;

	public LicensingTests()
	{
		passPhrase = Guid.NewGuid().ToString();
		var keyGenerator = KeyGenerator.Create();
		var keyPair      = keyGenerator.GenerateKeyPair();
		privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
		publicKey  = keyPair.ToPublicKeyString();
	}

	private static DateTime ConvertToRfc1123(DateTime dateTime) =>
		DateTime.ParseExact(
			dateTime.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture)
			, "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

	[Fact]
	public void Can_Generate_And_Validate_Signature_With_Empty_License()
	{
		var license = License.New().Build(privateKey, passPhrase);

		Assert.NotNull(license);
		Assert.NotNull(license.Signature);

		// validate xml
		var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
		Assert.True(xmlElement.HasElements);

		// validate default values when not set
		Assert.Equal(Guid.Empty,                          license.Id);
		Assert.Equal(LicenseType.Community,               license.Type);
		Assert.Equal(0,                                   license.Quantity);
		Assert.Equal(ConvertToRfc1123(DateTime.MaxValue), license.Expiration);

		Assert.Null(license.ProductFeatures);
		Assert.Null(license.Customer);

		// verify signature
		Assert.True(license.VerifySignature(publicKey));
	}

	[Fact]
	public void Can_Generate_And_Validate_Signature_With_Standard_License()
	{
		var licenseId      = Guid.NewGuid();
		var customerName   = "Happy Customer";
		var customerEmail  = "Happy Customer@example.tld";
		var expirationDate = DateTime.Now.AddYears(1);
		var productFeatures = new Dictionary<string, string>
		{
			{ "Sales Module", "yes" },
			{ "Purchase Module", "yes" },
			{ "Maximum Transactions", "10000" }
		};

		var license = License.New()
			.WithUniqueIdentifier(licenseId)
			.WithLicenseType(LicenseType.Standard)
			.WithMaximumUtilization(10)
			.WithProductFeatures(productFeatures)
			.LicensedTo(customerName, customerEmail)
			.WithLicenseExpiration(expirationDate)
			.Build(privateKey, passPhrase);

		Assert.NotNull(license);
		Assert.NotNull(license.Signature);

		// validate xml
		var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
		Assert.True(xmlElement.HasElements);

		// validate default values when not set
		Assert.Equal(licenseId,            license.Id);
		Assert.Equal(LicenseType.Standard, license.Type);
		Assert.Equal(10,                   license.Quantity);

		Assert.NotNull(license.Customer);
		Assert.Equal(customerName,                     license.Customer.Name);
		Assert.Equal(customerEmail,                    license.Customer.Email);
		Assert.Equal(ConvertToRfc1123(expirationDate), license.Expiration);

		Assert.NotNull(license.ProductFeatures);
		Assert.Equal(productFeatures, license.ProductFeatures.GetAll());

		// verify signature
		Assert.True(license.VerifySignature(publicKey));
	}

	[Fact]
	public void Can_Detect_Hacked_License()
	{
		var licenseId = Guid.NewGuid();
		var customerName = "Hackerman";
		var customerEmail = "hackerman@example.com.tld";
		var expirationDate = DateTime.Now.AddYears(1);
		var productFeatures = new Dictionary<string, string>
																			{
																					{"Sales Module", "yes"},
																					{"Purchase Module", "yes"},
																					{"Maximum Transactions", "10000"}
																			};

		var license = License.New()
												 .WithUniqueIdentifier(licenseId)
												 .WithLicenseType(LicenseType.Standard)
												 .WithMaximumUtilization(10)
												 .WithProductFeatures(productFeatures)
												 .LicensedTo(customerName, customerEmail)
												 .WithLicenseExpiration(expirationDate)
												 .Build(privateKey, passPhrase);

		Assert.NotNull(license);
		Assert.NotNull(license.Signature);

		// verify signature
		Assert.True(license.VerifySignature(publicKey));

		// validate xml
		var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
		Assert.True(xmlElement.HasElements);

		// manipulate xml
		Assert.NotNull( xmlElement.Element("Quantity"));
		xmlElement.Element("Quantity").Value = "11"; // now we want to have 11 licenses

		// load license from manipulated xml
		var hackedLicense = License.Load(xmlElement.ToString());

		// validate default values when not set
		Assert.Equal(licenseId,hackedLicense.Id);
		Assert.Equal(LicenseType.Standard,hackedLicense.Type);
		Assert.Equal(11,hackedLicense.Quantity); // now with 10+1 licenses
		Assert.NotNull(hackedLicense.ProductFeatures);
		Assert.Equal(productFeatures,hackedLicense.ProductFeatures.GetAll());
		Assert.NotNull(hackedLicense.Customer);
		Assert.Equal(customerName,                     hackedLicense.Customer.Name);
		Assert.Equal(customerEmail,                    license.Customer.Email);
		Assert.Equal(ConvertToRfc1123(expirationDate), hackedLicense.Expiration);

		// verify signature
		Assert.False(hackedLicense.VerifySignature(publicKey));
	}
}
