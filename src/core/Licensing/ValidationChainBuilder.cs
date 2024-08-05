namespace core.Licensing;

internal class ValidationChainBuilder : IStartValidationChain, IValidationChain
{
	private readonly License                  _license;
	private readonly Queue<ILicenseValidator> _validators;
	private          ILicenseValidator?       _currentValidatorChain;

	public ValidationChainBuilder(License license)
	{
		_license    = license;
		_validators = new Queue<ILicenseValidator>();
	}

	public ICompleteValidationChain When(Predicate<License> predicate)
	{
		if (_currentValidatorChain != null)
			_currentValidatorChain.ValidateWhen = predicate;
		return this;
	}

	public IStartValidationChain And()
	{
		CompleteValidatorChain();
		return this;
	}

	public IEnumerable<IValidationFailure> AssertValidLicense()
	{
		CompleteValidatorChain();

		while (_validators.Count > 0)
		{
			var validator = _validators.Dequeue();
			if (validator.ValidateWhen != null && !validator.ValidateWhen(_license))
				continue;

			if (validator.Validate != null && !validator.Validate(_license))
				yield return validator.FailureResult ?? new GeneralValidationFailure
				{
					Message = "License validation failed"
				};
		}
	}

	public ILicenseValidator StartValidatorChain() => _currentValidatorChain = new LicenseValidator();

	public void CompleteValidatorChain()
	{
		if (_currentValidatorChain == null) return;

		_validators.Enqueue(_currentValidatorChain);
		_currentValidatorChain = null;
	}
}
