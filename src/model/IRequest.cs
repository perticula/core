// perticula - model - IRequest.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model;

/// <summary>
///   A Request without a response
/// </summary>
public interface IRequest
{
}

/// <summary>
///   A Request that returns a response model
///   Implements the <see cref="model.IRequest" />
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <seealso cref="model.IRequest" />
public interface IRequest<out TResponse> : IRequest
{
}
