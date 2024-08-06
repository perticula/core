// ***********************************************************************
// Assembly         : 
// Author           : Ris Adams
// Created          : 08-05-2024
//
// Last Modified By : Ris Adams
// Last Modified On : 08-05-2024
// ***********************************************************************
// <copyright file="ValidationChainBuilder.cs" company="Ris Adams">
//     Copyright (c) Ris Adams. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace core.Licensing;

/// <summary>
///   Class ValidationChainBuilder.
///   Implements the <see cref="core.Licensing.IStartValidationChain" />
///   Implements the <see cref="core.Licensing.IValidationChain" />
/// </summary>
/// <seealso cref="core.Licensing.IStartValidationChain" />
/// <seealso cref="core.Licensing.IValidationChain" />
internal class ValidationChainBuilder : IStartValidationChain, IValidationChain
{
	/// <summary>
	///   The license
	/// </summary>
	private readonly License _license;

	/// <summary>
	///   The validators
	/// </summary>
	private readonly Queue<ILicenseValidator> _validators;

	/// <summary>
	///   The current validator chain
	/// </summary>
	private ILicenseValidator? _currentValidatorChain;

	/// <summary>
	///   Initializes a new instance of the <see cref="ValidationChainBuilder" /> class.
	/// </summary>
	/// <param name="license">The license.</param>
	public ValidationChainBuilder(License license)
	{
		_license    = license;
		_validators = new Queue<ILicenseValidator>();
	}

	/// <summary>
	///   Whens the specified predicate.
	/// </summary>
	/// <param name="predicate">The predicate.</param>
	/// <returns>ICompleteValidationChain.</returns>
	public ICompleteValidationChain When(Predicate<License> predicate)
	{
		if (_currentValidatorChain != null)
			_currentValidatorChain.ValidateWhen = predicate;
		return this;
	}

	/// <summary>
	///   Ands this instance.
	/// </summary>
	/// <returns>IStartValidationChain.</returns>
	public IStartValidationChain And()
	{
		CompleteValidatorChain();
		return this;
	}

	/// <summary>
	///   Asserts the valid license.
	/// </summary>
	/// <returns>IEnumerable&lt;IValidationFailure&gt;.</returns>
	public IEnumerable<IValidationFailure> AssertValidLicense()
	{
		CompleteValidatorChain();

		while (_validators.Count > 0)
		{
			var validator = _validators.Dequeue();
			if (validator.ValidateWhen != null && !validator.ValidateWhen(_license))
				continue;

			if (validator.Validate != null && !validator.Validate(_license))
				yield return validator.FailureResult ?? new GeneralValidationFailure
				{
					Message = "License validation failed"
				};
		}
	}

	/// <summary>
	///   Starts the validator chain.
	/// </summary>
	/// <returns>ILicenseValidator.</returns>
	public ILicenseValidator StartValidatorChain() => _currentValidatorChain = new LicenseValidator();

	/// <summary>
	///   Completes the validator chain.
	/// </summary>
	public void CompleteValidatorChain()
	{
		if (_currentValidatorChain == null) return;

		_validators.Enqueue(_currentValidatorChain);
		_currentValidatorChain = null;
	}
}
