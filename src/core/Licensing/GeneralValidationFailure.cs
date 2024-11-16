namespace core.Licensing;

/// <summary>
///   Class GeneralValidationFailure.
///   Implements the <see cref="core.Licensing.IValidationFailure" />
/// </summary>
/// <seealso cref="core.Licensing.IValidationFailure" />
public class GeneralValidationFailure : IValidationFailure
{
	/// <summary>
	///   Represents a general validation failure message.
	/// </summary>
	/// <value>The message.</value>
	public string? Message { get; set; }

	/// <summary>
	///   Represents a description of how to recover from validation failures
	/// </summary>
	/// <value>The how to resolve.</value>
	public string? HowToResolve { get; set; }
}
