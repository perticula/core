namespace core.Licensing;

/// <summary>
/// Interface ICompleteValidationChain
/// Extends the <see cref="core.Licensing.IAdditionalValidationChain" />
/// Extends the <see cref="core.Licensing.IAssertValidation" />
/// </summary>
/// <seealso cref="core.Licensing.IAdditionalValidationChain" />
/// <seealso cref="core.Licensing.IAssertValidation" />
public interface ICompleteValidationChain : IAdditionalValidationChain, IAssertValidation
{
}
