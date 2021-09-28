using DragonSpark.Composition;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	sealed class Users<T> : IUsers<T> where T : class
	{
		readonly IScopes _scoping;

		public Users(IScopes scoping) => _scoping = scoping;

		public UsersSession<T> Get()
		{
			var scope   = _scoping.Get();
			var subject = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
			return new(subject, scope);
		}
	}
}