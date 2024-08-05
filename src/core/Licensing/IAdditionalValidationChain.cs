namespace core.Licensing;

public interface IAdditionalValidationChain : IFluentInterface
{
	/// <summary>
	/// Adds another validation chain instance
	/// </summary>
	/// <returns></returns>
	IStartValidationChain And();
}
