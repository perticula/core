// perticula - core - ILicenseValidator.cs
// 
// Copyright © 2015-2024  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Licensing;

/// <summary>
/// Defines the validation criteria for a license
/// </summary>
public interface ILicenseValidator
{
	/// <summary>
	/// Gets or sets the predicate to determine if the <see cref="License"/>
	/// is valid.
	/// </summary>
	Predicate<License>? Validate { get; set; }

	/// <summary>
	/// Gets or sets the predicate to determine if the <see cref="ILicenseValidator"/>
	/// should be executed.
	/// </summary>
	Predicate<License>? ValidateWhen { get; set; }

	/// <summary>
	/// Gets or sets the <see cref="IValidationFailure"/> result. The <see cref="IValidationFailure"/>
	/// will be returned to the application when the <see cref="ILicenseValidator"/> fails.
	/// </summary>
	IValidationFailure? FailureResult { get; set; }
}
