// perticula - core - LicenseBuilder.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Licensing;

/// <summary>
///   Class LicenseBuilder.
///   Implements the <see cref="core.Licensing.ILicenseBuilder" />
/// </summary>
/// <seealso cref="core.Licensing.ILicenseBuilder" />
internal class LicenseBuilder : ILicenseBuilder
{
	/// <summary>
	///   The license
	/// </summary>
	private readonly License _license;

	/// <summary>
	///   Initializes a new instance of the <see cref="LicenseBuilder" /> class.
	/// </summary>
	public LicenseBuilder() => _license = new License();

	/// <summary>
	///   Withes the unique identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithUniqueIdentifier(Guid id)
	{
		_license.Id = id;
		return this;
	}

	/// <summary>
	///   Withes the type of the license.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithLicenseType(LicenseType type)
	{
		_license.Type = type;
		return this;
	}

	/// <summary>
	///   Withes the license expiration.
	/// </summary>
	/// <param name="date">The date.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithLicenseExpiration(DateTime date)
	{
		_license.Expiration = date.ToUniversalTime();
		return this;
	}

	/// <summary>
	///   Withes the maximum utilization.
	/// </summary>
	/// <param name="utilization">The utilization.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithMaximumUtilization(int utilization)
	{
		_license.Quantity = utilization;
		return this;
	}

	/// <summary>
	///   Configures this to license to a new customer
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="email">The email.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder LicensedTo(string name, string email)
	{
		_license.Customer!.Name  = name;
		_license.Customer!.Email = email;
		return this;
	}

	/// <summary>
	///   Configures this to license to a new customer
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="email">The email.</param>
	/// <param name="configureCustomer">The configure customer.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder LicensedTo(string name, string email, Action<Customer> configureCustomer)
	{
		_license.Customer!.Name  = name;
		_license.Customer!.Email = email;
		configureCustomer.Invoke(_license.Customer);
		return this;
	}

	/// <summary>
	///   Configures this to license to a particular Customer
	/// </summary>
	/// <param name="configureCustomer">The configure customer.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder LicensedTo(Action<Customer> configureCustomer)
	{
		configureCustomer(_license.Customer!);
		return this;
	}

	/// <summary>
	///   Configures the product features.
	/// </summary>
	/// <param name="productFeatures">The product features.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithProductFeatures(IDictionary<string, string> productFeatures)
	{
		_license.ProductFeatures!.AddAll(productFeatures);
		return this;
	}

	/// <summary>
	///   Configures the product features.
	/// </summary>
	/// <param name="configureProductFeatures">The configure product features.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithProductFeatures(Action<LicenseAttributes> configureProductFeatures)
	{
		configureProductFeatures(_license.ProductFeatures!);
		return this;
	}

	/// <summary>
	///   Configures the additional attributes.
	/// </summary>
	/// <param name="configureAdditionalAttributes">The configure additional attributes.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithAdditionalAttributes(Action<LicenseAttributes> configureAdditionalAttributes)
	{
		configureAdditionalAttributes(_license.AdditionalAttributes!);
		return this;
	}

	/// <summary>
	///   Configures the additional attributes.
	/// </summary>
	/// <param name="additionalAttributes">The additional attributes.</param>
	/// <returns>ILicenseBuilder.</returns>
	public ILicenseBuilder WithAdditionalAttributes(IDictionary<string, string> additionalAttributes)
	{
		_license.AdditionalAttributes!.AddAll(additionalAttributes);
		return this;
	}

	/// <summary>
	///   Builds the specified private key.
	/// </summary>
	/// <param name="privateKey">The private key.</param>
	/// <param name="passPhrase">The pass phrase.</param>
	/// <returns>License.</returns>
	public License Build(string privateKey, string passPhrase)
	{
		_license.Sign(privateKey, passPhrase);
		return _license;
	}
}
