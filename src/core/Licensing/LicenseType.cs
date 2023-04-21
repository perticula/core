// perticula - core - LicenseType.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Licensing;

public enum LicenseType
{
	Invalid = 0x0, // all enums should define the default case as an invalid value.

	Community    = 0x01,
	Standard     = 0x02,
	Professional = 0x03,
	Enterprise   = 0x04,
	Unlimited    = 0x05
}
