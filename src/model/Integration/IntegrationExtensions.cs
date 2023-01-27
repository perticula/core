// perticula - model - IntegrationExtensions.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model.Integration;

/// <summary>
///   Class IntegrationExtensions.
///   Defines Extension methods that should be available on all models
///   that may support pulling from external sources
/// </summary>
public static class IntegrationExtensions
{
	/// <summary>
	///   Determines whether this instance is externaly synchronized.
	/// </summary>
	/// <param name="model">The model.</param>
	/// <returns><c>true</c> if the specified model is external; otherwise, <c>false</c>.</returns>
	public static bool IsExternal(this ICanSupportIntegration model) => !string.IsNullOrEmpty(model?.ExternalKey);
}
