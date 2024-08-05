// perticula - core - License.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using core.Cryptography;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security;

namespace core.Licensing;

/// <summary>
///   Class License.
/// </summary>
public class License
{
	private readonly string   _signatureAlgorithm = X9ObjectIdentifiers.ECDsaWithSha512.Id;
	private readonly XElement _xmlData;

	/// <summary>
	///   Initializes a new instance of the <see cref="License" /> class.
	/// </summary>
	internal License() => _xmlData = new XElement("License");

	/// <summary>
	///   Initializes a new instance of the <see cref="License" /> class.
	/// </summary>
	/// <param name="xmlData">The XML data.</param>
	internal License(XElement xmlData) => _xmlData = xmlData;

	/// <summary>
	///   Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public Guid Id
	{
		get => new(GetTag("Id") ?? Guid.Empty.ToString());
		set
		{
			if (!IsSigned) SetTag("Id", value.ToString());
		}
	}

	/// <summary>
	///   Gets or sets the type.
	/// </summary>
	/// <value>The type.</value>
	public LicenseType Type
	{
		get => (LicenseType)Enum.Parse(typeof(LicenseType), GetTag("Type") ?? LicenseType.Community.ToString(), false);
		set
		{
			if (!IsSigned) SetTag("Type", value.ToString());
		}
	}

	/// <summary>
	///   Gets or sets the quantity.
	/// </summary>
	/// <value>The quantity.</value>
	public int Quantity
	{
		get => int.Parse(GetTag("Quantity") ?? "0");
		set
		{
			if (!IsSigned) SetTag("Quantity", value.ToString());
		}
	}

	/// <summary>
	///   Gets the product features.
	/// </summary>
	/// <value>The product features.</value>
	public LicenseAttributes? ProductFeatures
	{
		get
		{
			var xmlElement = _xmlData.Element("ProductFeatures");

			switch (IsSigned)
			{
				case false when xmlElement == null:
					_xmlData.Add(new XElement("ProductFeatures"));
					xmlElement = _xmlData.Element("ProductFeatures");
					break;
				case true when xmlElement == null:
					return null;
			}

			return new LicenseAttributes(xmlElement!, "Feature");
		}
	}

	/// <summary>
	///   Gets the customer.
	/// </summary>
	/// <value>The customer.</value>
	public Customer? Customer
	{
		get
		{
			var xmlElement = _xmlData.Element("Customer");

			switch (IsSigned)
			{
				case false when xmlElement == null:
					_xmlData.Add(new XElement("Customer"));
					xmlElement = _xmlData.Element("Customer");
					break;
				case true when xmlElement == null:
					return null;
			}

			return new Customer(xmlElement!);
		}
	}

	public LicenseAttributes? AdditionalAttributes
	{
		get
		{
			var xmlElement = _xmlData.Element("LicenseAttributes");

			switch (IsSigned)
			{
				case false when xmlElement == null:
					_xmlData.Add(new XElement("LicenseAttributes"));
					xmlElement = _xmlData.Element("LicenseAttributes");
					break;
				case true when xmlElement == null:
					return null;
			}

			return new LicenseAttributes(xmlElement!, "Attribute");
		}
	}

	/// <summary>
	///   Gets or sets the expiration.
	/// </summary>
	/// <value>The expiration.</value>
	public DateTime Expiration
	{
		get => DateTime.ParseExact(
			GetTag("Expiration") ?? DateTime.MaxValue.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture), "r",
			CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
		set
		{
			if (!IsSigned) SetTag("Expiration", value.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture));
		}
	}

	/// <summary>
	///   Gets the signature.
	/// </summary>
	/// <value>The signature.</value>
	public string? Signature => GetTag("Signature");

	/// <summary>
	///   Gets a value indicating whether this instance is signed.
	/// </summary>
	/// <value><c>true</c> if this instance is signed; otherwise, <c>false</c>.</value>
	private bool IsSigned => !string.IsNullOrEmpty(Signature);

	/// <summary>
	///   Loads the specified XML string.
	/// </summary>
	/// <param name="xmlString">The XML string.</param>
	/// <returns>License.</returns>
	public static License Load(string xmlString) => new(XElement.Parse(xmlString, LoadOptions.None));

	/// <summary>
	///   Loads the specified stream.
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <returns>License.</returns>
	public static License Load(Stream stream) => new(XElement.Load(stream, LoadOptions.None));

	/// <summary>
	///   Loads the specified reader.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <returns>License.</returns>
	public static License Load(TextReader reader) => new(XElement.Load(reader, LoadOptions.None));

	/// <summary>
	///   Loads the specified reader.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <returns>License.</returns>
	public static License Load(XmlReader reader) => new(XElement.Load(reader, LoadOptions.None));

	/// <summary>
	///   News this instance.
	/// </summary>
	/// <returns>ILicenseBuilder.</returns>
	public static ILicenseBuilder New() => new LicenseBuilder();

	/// <summary>
	///   Saves the specified stream.
	/// </summary>
	/// <param name="stream">The stream.</param>
	public void Save(Stream stream) => _xmlData.Save(stream);

	/// <summary>
	///   Saves the specified text writer.
	/// </summary>
	/// <param name="textWriter">The text writer.</param>
	public void Save(TextWriter textWriter) => _xmlData.Save(textWriter);

	/// <summary>
	///   Saves the specified XML writer.
	/// </summary>
	/// <param name="xmlWriter">The XML writer.</param>
	public void Save(XmlWriter xmlWriter) => _xmlData.Save(xmlWriter);

	/// <summary>
	///   Signs the specified private key.
	/// </summary>
	/// <param name="privateKey">The private key.</param>
	/// <param name="passPhrase">The pass phrase.</param>
	public void Sign(string privateKey, string passPhrase)
	{
		var signTag = _xmlData.Element("Signature") ?? new XElement("Signature");

		try
		{
			if (signTag.Parent != null)
				signTag.Remove();

			var key = KeyFactory.FromEncryptedPrivateKeyString(privateKey, passPhrase);

			var documentToSign = System.Text.Encoding.UTF8.GetBytes(_xmlData.ToString(SaveOptions.DisableFormatting));
			var signer         = SignerUtilities.GetSigner(_signatureAlgorithm);
			signer.Init(true, key);
			signer.BlockUpdate(documentToSign, 0, documentToSign.Length);
			var signature = signer.GenerateSignature();
			signTag.Value = Convert.ToBase64String(signature);
		}
		finally
		{
			_xmlData.Add(signTag);
		}
	}

	/// <summary>
	///   Verifies the signature.
	/// </summary>
	/// <param name="publicKey">The public key.</param>
	/// <returns><c>true</c> if signature is valid, <c>false</c> otherwise.</returns>
	public bool VerifySignature(string publicKey)
	{
		var signTag = _xmlData.Element("Signature");

		if (signTag == null)
			return false;

		try
		{
			signTag.Remove();

			var pubKey = KeyFactory.FromPublicKeyString(publicKey);

			var documentToSign = System.Text.Encoding.UTF8.GetBytes(_xmlData.ToString(SaveOptions.DisableFormatting));
			var signer         = SignerUtilities.GetSigner(_signatureAlgorithm);
			signer.Init(false, pubKey);
			signer.BlockUpdate(documentToSign, 0, documentToSign.Length);

			return signer.VerifySignature(Convert.FromBase64String(signTag.Value));
		}
		finally
		{
			_xmlData.Add(signTag);
		}
	}

	/// <summary>
	///   Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
	public override string ToString() => _xmlData.ToString();

	/// <summary>
	///   Sets the value of the specified tag name.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="value">The value.</param>
	private void SetTag(string name, string value)
	{
		ArgumentNullException.ThrowIfNull(name);
		ArgumentNullException.ThrowIfNull(value);

		var element = _xmlData.Element(name);

		if (element == null)
		{
			element = new XElement(name);
			_xmlData.Add(element);
		}

		element.Value = value;
	}

	/// <summary>
	///   Gets the value of the specified tag.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns>System.Nullable&lt;System.String&gt;.</returns>
	private string? GetTag(string name) => _xmlData.Element(name)?.Value;
}
