using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity;

public class UserSessions<T> : IResult<UsersSession<T>> where T : class
{
	readonly IScopes _scopes;

	public UserSessions(IScopes scopes) => _scopes = scopes;

	public UsersSession<T> Get()
	{
		var scope = _scopes.Get();
		return new(scope.ServiceProvider.GetRequiredService<UserManager<T>>(), scope);
	}
}