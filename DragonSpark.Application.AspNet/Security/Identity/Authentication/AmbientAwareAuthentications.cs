using System;
using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class AmbientAwareAuthentications<T>(IAuthentications<T> previous, IResult<IServiceProvider?> provider)
	: IAuthentications<T>
	where T : class
{
	public AmbientAwareAuthentications(IAuthentications<T> previous) : this(previous, AmbientProvider.Default) {}

	[MustDisposeResource]
	public AuthenticationSession<T> Get()
	{
		var current = provider.Get();
		var result  = current != null ? new(current.GetRequiredService<SignInManager<T>>()) : previous.Get();
		return result;
	}
}
