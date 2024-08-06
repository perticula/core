namespace core.Licensing;

/// <summary>
///   Interface IAdditionalValidationChain
///   Extends the <see cref="core.IFluentInterface" />
/// </summary>
/// <seealso cref="core.IFluentInterface" />
public interface IAdditionalValidationChain : IFluentInterface
{
	/// <summary>
	///   Adds another validation chain instance
	/// </summary>
	/// <returns></returns>
	IStartValidationChain And();
}
