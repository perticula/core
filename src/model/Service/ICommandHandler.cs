// perticula - model - ICommandHandler.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model.Service;

/// <summary>
///   Interface ICommandHandler
///   Implements the <see cref="model.Service.IRequestHandler{TCommand, TResponse}" />
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <typeparam name="TResponse">The type of the t response.</typeparam>
/// <seealso cref="model.Service.IRequestHandler{TCommand, TResponse}" />
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
	where TCommand : ICommand<TResponse>
	where TResponse : notnull { }
