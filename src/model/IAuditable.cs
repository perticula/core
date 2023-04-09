// perticula - model - IAuditable.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model;

/// <summary>
///   Interface IAuditable.
///   When attached to a model, specifies meta information about the items modifications.
/// </summary>
public interface IAuditable
{
	/// <summary>
	///   Gets or sets the name of the user creating this item.
	/// </summary>
	/// <value>The created by.</value>
	string CreatedBy { get; set; }

	/// <summary>
	///   Gets or sets name of the user who last modified this item.
	///   This may be null if the item was never modified.
	/// </summary>
	/// <value>The last modified by.</value>
	string? LastModifiedBy { get; set; }

	/// <summary>
	///   Gets or sets the name of the user who deleted this item.
	///   This may be null if the item was never deleted.
	/// </summary>
	/// <value>The deleted by.</value>
	string? DeletedBy { get; set; }

	/// <summary>
	///   Gets or sets the created on date UTC.
	/// </summary>
	/// <value>The created on UTC.</value>
	DateTime CreatedOnUtc { get; set; }

	/// <summary>
	///   Gets or sets the last modified date UTC.
	///   This may be null if the item was never modified.
	/// </summary>
	/// <value>The last modified on UTC.</value>
	DateTime? LastModifiedOnUtc { get; set; }

	/// <summary>
	///   Gets or sets the deleted on date UTC.
	///   This may be null if the item was never deleted.
	/// </summary>
	/// <value>The deleted on UTC.</value>
	DateTime? DeletedOnUtc { get; set; }
}
