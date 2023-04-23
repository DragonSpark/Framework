using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity;

sealed class AmbientAwareUsers<T> : IUsers<T> where T : class
{
	readonly IUsers<T>                  _previous;
	readonly IResult<IServiceProvider?> _provider;

	public AmbientAwareUsers(IUsers<T> previous) : this(previous, AmbientProvider.Default) {}

	public AmbientAwareUsers(IUsers<T> previous, IResult<IServiceProvider?> provider)
	{
		_previous    = previous;
		_provider = provider;
	}

	public UsersSession<T> Get()
	{
		var current = _provider.Get();
		var result  = current != null ? new(current.GetRequiredService<UserManager<T>>()) : _previous.Get();
		return result;
	}
}