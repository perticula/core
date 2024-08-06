namespace core.Licensing;

/// <summary>
/// Interface IValidationChain
/// Extends the <see cref="core.Licensing.IValidationChainCondition" />
/// Extends the <see cref="core.Licensing.ICompleteValidationChain" />
/// </summary>
/// <seealso cref="core.Licensing.IValidationChainCondition" />
/// <seealso cref="core.Licensing.ICompleteValidationChain" />
public interface IValidationChain : IValidationChainCondition, ICompleteValidationChain
{
	
}
