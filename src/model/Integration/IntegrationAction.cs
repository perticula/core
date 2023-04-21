// perticula - model - IntegrationAction.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

using core.Attributes;

namespace model.Integration;

public enum IntegrationAction
{
	Invalid = 0x0, // all enums should define the default case as an invalid value.

	// Actions to perform C(r)UD operation from an external source
	[DisplayText("Create item: {1}")] Create = 0x01,
	[DisplayText("Update item {1}")]  Update = 0x02,
	[DisplayText("Drop item: {1}")]   Drop   = 0x03,

	// Actions defined when an external item should be ignored
	[DisplayText("Skipping item: {1}")] Skip = 0x10
}
