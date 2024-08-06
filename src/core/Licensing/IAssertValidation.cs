namespace core.Licensing;

/// <summary>
///   Interface IAssertValidation
///   Extends the <see cref="core.IFluentInterface" />
/// </summary>
/// <seealso cref="core.IFluentInterface" />
public interface IAssertValidation : IFluentInterface
{
	/// <summary>
	///   Invokes the license assertion.
	/// </summary>
	/// <returns>An array is <see cref="IValidationFailure" /> when the validation fails.</returns>
	IEnumerable<IValidationFailure> AssertValidLicense();
}
