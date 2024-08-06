namespace core.Licensing;

/// <summary>
/// Interface IValidationFailure
/// </summary>
public interface IValidationFailure
{
	/// <summary>
	/// Represents a general validation failure message.
	/// </summary>
	string? Message      { get; set; }

	/// <summary>
	/// Represents a description of how to recover from validation failures
	/// </summary>
	string? HowToResolve { get; set; }
}
