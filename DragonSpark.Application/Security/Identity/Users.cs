using DragonSpark.Composition.Scopes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity;

sealed class Users<T> : IUsers<T> where T : class
{
	readonly IScopes _scopes;

	public Users(IScopes scopes) => _scopes = scopes;

	public UsersSession<T> Get()
	{
		var scope   = _scopes.Get();
		var subject = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
		return new(subject, scope);
	}
}