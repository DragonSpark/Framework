using DragonSpark.Composition.Scopes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Authentications<T> : IAuthentications<T> where T : class
{
	readonly IScopes _scopes;

	public Authentications(IScopes scopes) => _scopes = scopes;

	public AuthenticationSession<T> Get()
	{
		var scope   = _scopes.Get();
		var subject = scope.ServiceProvider.GetRequiredService<SignInManager<T>>();
		return new(subject, scope);
	}
}