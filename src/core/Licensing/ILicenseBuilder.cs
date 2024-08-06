// perticula - core - ILicenseBuilder.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Licensing;

/// <summary>
///   Interface ILicenseBuilder
///   Implements the <see cref="core.IFluentInterface" />
/// </summary>
/// <seealso cref="core.IFluentInterface" />
public interface ILicenseBuilder : IFluentInterface
{
	/// <summary>
	///   Configures the unique identifier.
	/// </summary>
	/// <param name="uniqueIdentifier">The unique identifier.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithUniqueIdentifier(Guid uniqueIdentifier);

	/// <summary>
	///   Configures the type of the license.
	/// </summary>
	/// <param name="licenseType">Type of the license.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithLicenseType(LicenseType licenseType);

	/// <summary>
	///   Configures the license expiration.
	/// </summary>
	/// <param name="licenseExpiration">The license expiration.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithLicenseExpiration(DateTime licenseExpiration);

	/// <summary>
	///   Configures the maximum utilization.
	/// </summary>
	/// <param name="maximumUtilization">The maximum utilization.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithMaximumUtilization(int maximumUtilization);

	/// <summary>
	///   Configures the product features.
	/// </summary>
	/// <param name="productFeatures">The product features.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithProductFeatures(IDictionary<string, string> productFeatures);

	/// <summary>
	///   Configures the product features.
	/// </summary>
	/// <param name="configureProductFeatures">The configure product features.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithProductFeatures(Action<LicenseAttributes> configureProductFeatures);

	/// <summary>
	///   Configures the additional attributes.
	/// </summary>
	/// <param name="additionalAttributes">The additional attributes.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithAdditionalAttributes(IDictionary<string, string> additionalAttributes);

	/// <summary>
	///   Configures the additional attributes.
	/// </summary>
	/// <param name="configureAdditionalAttributes">The configure additional attributes.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder WithAdditionalAttributes(Action<LicenseAttributes> configureAdditionalAttributes);

	/// <summary>
	///   Configures the customer to whom this license applies.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="email">The email.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder LicensedTo(string name, string email);

	/// <summary>
	///   Configures the customer to whom this license applies.
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="email">The email.</param>
	/// <param name="configureCustomer">The configure customer.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder LicensedTo(string name, string email, Action<Customer> configureCustomer);

	/// <summary>
	///   Configures the customer to whom this license applies.
	/// </summary>
	/// <param name="configureCustomer">The configure customer.</param>
	/// <returns>ILicenseBuilder.</returns>
	ILicenseBuilder LicensedTo(Action<Customer> configureCustomer);

	/// <summary>
	///   Builds the specified private key.
	/// </summary>
	/// <param name="privateKey">The private key.</param>
	/// <param name="passPhrase">The pass phrase.</param>
	/// <returns>License.</returns>
	License Build(string privateKey, string passPhrase);
}
