namespace core.Licensing;

/// <summary>
///   Class LicenseValidator.
///   Implements the <see cref="core.Licensing.ILicenseValidator" />
/// </summary>
/// <seealso cref="core.Licensing.ILicenseValidator" />
internal class LicenseValidator : ILicenseValidator
{
	/// <summary>
	///   Gets or sets the predicate to determine if the <see cref="License" />
	///   is valid.
	/// </summary>
	/// <value>The validate.</value>
	public Predicate<License>? Validate { get; set; }

	/// <summary>
	///   Gets or sets the predicate to determine if the <see cref="ILicenseValidator" />
	///   should be executed.
	/// </summary>
	/// <value>The validate when.</value>
	public Predicate<License>? ValidateWhen { get; set; }

	/// <summary>
	///   Gets or sets the <see cref="IValidationFailure" /> result. The <see cref="IValidationFailure" />
	///   will be returned to the application when the <see cref="ILicenseValidator" /> fails.
	/// </summary>
	/// <value>The failure result.</value>
	public IValidationFailure? FailureResult { get; set; }
}
