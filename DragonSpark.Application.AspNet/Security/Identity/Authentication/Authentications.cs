using DragonSpark.Composition.Scopes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class Authentications<T>(IScopes scopes) : IAuthentications<T> where T : class
{
	[MustDisposeResource]
	public AuthenticationSession<T> Get()
	{
		var scope   = scopes.Get();
		var subject = scope.ServiceProvider.GetRequiredService<SignInManager<T>>();
		return new(subject, scope);
	}
}
