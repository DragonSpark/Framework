using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class UserSessions<T>(IScopes scopes) : IResult<UsersSession<T>> where T : class
{
	readonly IScopes _scopes = scopes;

	[MustDisposeResource]
	public UsersSession<T> Get()
	{
		var scope = _scopes.Get();
		return new(scope.ServiceProvider.GetRequiredService<UserManager<T>>(), scope);
	}
}