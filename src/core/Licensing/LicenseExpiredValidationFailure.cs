namespace core.Licensing;

/// <summary>
/// Class LicenseExpiredValidationFailure.
/// Implements the <see cref="core.Licensing.IValidationFailure" />
/// </summary>
/// <seealso cref="core.Licensing.IValidationFailure" />
public class LicenseExpiredValidationFailure : IValidationFailure
{
	/// <summary>
	/// Gets or sets a message that describes the validation failure.
	/// </summary>
	public string? Message { get; set; }

	/// <summary>
	/// Gets or sets a message that describes how to recover from the validation failure.
	/// </summary>
	public string? HowToResolve { get; set; }
}
