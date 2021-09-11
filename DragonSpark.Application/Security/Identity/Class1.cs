using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	class Class1 {}

	public readonly struct UsersSession<T> : IAsyncDisposable where T : class
	{
		readonly AsyncServiceScope _scope;

		public UsersSession(AsyncServiceScope scope, UserManager<T> subject)
		{
			_scope  = scope;
			Subject = subject;
		}

		public UserManager<T> Subject { get; }

		public ValueTask DisposeAsync() => _scope.DisposeAsync();
	}

	public interface IUsers<T> : IResult<UsersSession<T>> where T : class {}

	sealed class Users<T> : IUsers<T> where T : class
	{
		readonly IServiceScopeFactory _scopes;

		public Users(IServiceScopeFactory scopes) => _scopes = scopes;

		public UsersSession<T> Get()
		{
			var scope   = _scopes.CreateAsyncScope();
			var subject = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
			return new(scope, subject);
		}
	}
}