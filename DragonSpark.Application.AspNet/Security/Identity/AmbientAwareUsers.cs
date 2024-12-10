using System;
using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class AmbientAwareUsers<T>(IUsers<T> previous, IResult<IServiceProvider?> provider) : IUsers<T>
	where T : class
{
	public AmbientAwareUsers(IUsers<T> previous) : this(previous, AmbientProvider.Default) {}

	[MustDisposeResource]
	public UsersSession<T> Get()
	{
		var current = provider.Get();
		var result  = current is not null ? new(current.GetRequiredService<UserManager<T>>()) : previous.Get();
		return result;
	}
}
