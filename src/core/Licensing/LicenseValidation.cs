namespace core.Licensing;

public static class LicenseValidation
{
	/// <summary>
	///   Starts the validation chain of the <see cref="License" />.
	/// </summary>
	/// <param name="license">The <see cref="License" /> to validate.</param>
	/// <returns>An instance of <see cref="IStartValidationChain" />.</returns>
	public static IStartValidationChain Validate(this License license) => new ValidationChainBuilder(license);

	/// <summary>
	///   Validates if the license has been expired.
	/// </summary>
	/// <param name="validationChain">The current <see cref="IStartValidationChain" />.</param>
	/// <returns>An instance of <see cref="IStartValidationChain" />.</returns>
	public static IValidationChain ExpirationDate(this IStartValidationChain validationChain)
	{
		var validationChainBuilder = validationChain as ValidationChainBuilder;
		var validator              = validationChainBuilder!.StartValidatorChain();
		validator.Validate = license => license.Expiration > DateTime.Now;

		validator.FailureResult = new LicenseExpiredValidationFailure
		{
			Message      = "Licensing for this product has expired!",
			HowToResolve = @"Your license is expired. Please contact your distributor/vendor to renew the license."
		};

		return validationChainBuilder;
	}

	/// <summary>
	///   Allows you to specify a custom assertion that validates the <see cref="License" />.
	/// </summary>
	/// <param name="validationChain">The current <see cref="IStartValidationChain" />.</param>
	/// <param name="predicate">The predicate to determine of the <see cref="License" /> is valid.</param>
	/// <param name="failure">
	///   The <see cref="IValidationFailure" /> will be returned to the application when the
	///   <see cref="ILicenseValidator" /> fails.
	/// </param>
	/// <returns>An instance of <see cref="IStartValidationChain" />.</returns>
	public static IValidationChain AssertThat(this IStartValidationChain validationChain, Predicate<License> predicate, IValidationFailure failure)
	{
		var validationChainBuilder = validationChain as ValidationChainBuilder;
		var validator              = validationChainBuilder!.StartValidatorChain();

		validator.Validate      = predicate;
		validator.FailureResult = failure;

		return validationChainBuilder;
	}

	/// <summary>
	///   Validates the <see cref="License.Signature" />.
	/// </summary>
	/// <param name="validationChain">The current <see cref="IStartValidationChain" />.</param>
	/// <param name="publicKey">The public product key to validate the signature..</param>
	/// <returns>An instance of <see cref="IStartValidationChain" />.</returns>
	public static IValidationChain Signature(this IStartValidationChain validationChain, string publicKey)
	{
		var validationChainBuilder = validationChain as ValidationChainBuilder;
		var validator              = validationChainBuilder!.StartValidatorChain();
		validator.Validate = license => license.VerifySignature(publicKey);

		validator.FailureResult = new InvalidSignatureValidationFailure
		{
			Message = "License signature validation error!",
			HowToResolve =
				@"The license signature and data does not match. This usually happens when a license file is corrupted or has been altered."
		};

		return validationChainBuilder;
	}
}
