// perticula - model - ICanSupportIntegration.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model.Integration;

public interface ICanSupportIntegration
{
	/// <summary>
	///   Gets or sets the external key for this item.
	/// </summary>
	/// <value>The external key.</value>
	string ExternalKey { get; set; }

	/// <summary>
	///   Gets or sets the date of the last external update.
	/// </summary>
	/// <value>The last external update.</value>
	DateTime? LastExternalUpdate { get; set; }
}
