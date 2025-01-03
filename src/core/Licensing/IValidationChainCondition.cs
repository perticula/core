namespace core.Licensing;

/// <summary>
/// Interface IValidationChainCondition
/// </summary>
public interface IValidationChainCondition
{
	/// <summary>
	/// Adds a when predicate to the current validator.
	/// </summary>
	/// <param name="predicate">The predicate that defines the conditions.</param>
	/// <returns>An instance of <see cref="ICompleteValidationChain"/>.</returns>
	ICompleteValidationChain When(Predicate<License> predicate);
}
