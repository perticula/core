// perticula - model - IQueryHandler.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model.Service;

/// <summary>
///   Interface IQueryHandler
///   Implements the <see cref="model.Service.IRequestHandler{TQuery, TResponse}" />
/// </summary>
/// <typeparam name="TQuery">The type of the t query.</typeparam>
/// <typeparam name="TResponse">The type of the t response.</typeparam>
/// <seealso cref="model.Service.IRequestHandler{TQuery, TResponse}" />
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
	where TQuery : IQuery<TResponse>
	where TResponse : notnull { }
