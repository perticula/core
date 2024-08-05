namespace core.Licensing;

public class GeneralValidationFailure : IValidationFailure
{
	public string? Message      { get; set; }
	public string? HowToResolve { get; set; }
}
