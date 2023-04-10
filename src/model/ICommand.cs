// perticula - model - ICommand.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model;

/// <summary>
/// Interface ICommand
/// Implements the <see cref="model.IRequest{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="model.IRequest{T}" />
public interface ICommand<out T> : IRequest<T> where T : notnull { }
