// perticula - core - IAsn1Choice.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace core.Protocol.asn1;

/// <summary>
///   Interface IAsn1Choice.
///   Marker interface for CHOICE objects - if you implement this in a roll-your-own
///   object, any attempt to tag object implicitly will convert the tag to anexplicit one as the
///   encoding rules require.
/// </summary>
/// <remarks>
///   If you use this interface your class should also implement the getInstance
///   pattern which takes a tag object and the tagging mode used.
/// </remarks>
public interface IAsn1Choice { }
