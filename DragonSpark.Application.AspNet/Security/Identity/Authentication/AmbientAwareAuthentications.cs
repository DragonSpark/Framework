using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class AmbientAwareAuthentications<T> : IAuthentications<T> where T : class
{
	readonly IAuthentications<T>        _previous;
	readonly IResult<IServiceProvider?> _provider;

	public AmbientAwareAuthentications(IAuthentications<T> previous) : this(previous, AmbientProvider.Default) {}

	public AmbientAwareAuthentications(IAuthentications<T> previous, IResult<IServiceProvider?> provider)
	{
		_previous = previous;
		_provider = provider;
	}

	public AuthenticationSession<T> Get()
	{
		var current = _provider.Get();
		var result  = current != null ? new(current.GetRequiredService<SignInManager<T>>()) : _previous.Get();
		return result;
	}
}