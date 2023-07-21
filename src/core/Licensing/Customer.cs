// perticula - core - Customer.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using System.Xml.Linq;

namespace core.Licensing;

/// <summary>
///   Class Customer.
///   Implements the <see cref="core.Licensing.LicenseAttributes" />
/// </summary>
/// <seealso cref="core.Licensing.LicenseAttributes" />
public class Customer : LicenseAttributes
{
	/// <summary>
	///   Initializes a new instance of the <see cref="Customer" /> class.
	/// </summary>
	/// <param name="xmlData">The XML data.</param>
	internal Customer(XElement xmlData)
		: base(xmlData, "CustomerData") { }

	/// <summary>
	///   Gets or sets the name.
	/// </summary>
	/// <value>The name.</value>
	/// <exception cref="core.Licensing.InvalidLicenseException">Error: License name invalid</exception>
	public string Name
	{
		get => GetTag("Name") ?? throw new InvalidLicenseException("Error: License name invalid");
		set => SetTag("Name", value);
	}

	/// <summary>
	///   Gets or sets the company. (Optional. Defaults to Name if omitted)
	/// </summary>
	/// <value>The company.</value>
	public string Company
	{
		get => GetTag("Company") ?? Name;
		set => SetTag("Company", value);
	}

	/// <summary>
	///   Gets or sets the email.
	/// </summary>
	/// <value>The email.</value>
	/// <exception cref="core.Licensing.InvalidLicenseException">Error: License email invalid</exception>
	public string Email
	{
		get => GetTag("Email") ?? throw new InvalidLicenseException("Error: License email invalid");
		set => SetTag("Email", value);
	}
}
