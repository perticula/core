// perticula - model - IRequestHandler.cs
// 
// Copyright © 2015-2023  Ris Adams - All Rights Reserved
// 
// You may use, distribute and modify this code under the terms of the MIT license
// You should have received a copy of the MIT license with this file. If not, please write to: perticula@risadams.com, or visit : https://github.com/perticula

namespace model.Service;

/// <summary>
///   Defines a handler for a request
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
/// <typeparam name="TResponse">The type of response from the handler</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : notnull
{
	/// <summary>
	///   Handles the specified request.
	/// </summary>
	/// <param name="request">The request.</param>
	/// <returns>TResponse.</returns>
	TResponse Handle(TRequest request);

	/// <summary>
	///   Handles a request
	/// </summary>
	/// <param name="request">The request</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Response from the request</returns>
	Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
///   Defines a handler for a request with a void response.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
	/// <summary>
	///   Handles the specified request.
	/// </summary>
	/// <param name="request">The request.</param>
	void Handle(TRequest request);

	/// <summary>
	///   Handles a request
	/// </summary>
	/// <param name="request">The request</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Response from the request</returns>
	Task Handle(TRequest request, CancellationToken cancellationToken);
}
