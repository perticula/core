// perticula - model - IAuthenticatedUser.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model;

public interface IAuthenticatedUser : IIdentified<string>
{
	/// <summary>
	///   Gets the name of this identified user.
	/// </summary>
	/// <value>The name.</value>
	string Name { get; }

	/// <summary>
	///   Gets the email of this identified user.
	/// </summary>
	/// <value>The email.</value>
	string Email { get; }
}
