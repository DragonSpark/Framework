using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	class Class1 {}

	public interface IAuthentications<T> : IResult<AuthenticationSession<T>> where T : class {}

	public readonly struct AuthenticationSession<T> : IAsyncDisposable where T : class
	{
		readonly AsyncServiceScope _scope;

		public AuthenticationSession(AsyncServiceScope scope, SignInManager<T> subject)
			: this(scope, subject, subject.UserManager) {}

		public AuthenticationSession(AsyncServiceScope scope, SignInManager<T> subject, UserManager<T> users)
		{
			_scope     = scope;
			Subject    = subject;
			Users = users;
		}

		public SignInManager<T> Subject { get; }
		public UserManager<T> Users { get; }

		public ValueTask DisposeAsync() => _scope.DisposeAsync();

		public void Deconstruct(out SignInManager<T> subject, out UserManager<T> users)
		{
			subject = Subject;
			users   = Users;
		}
	}

	sealed class Authentications<T> : IAuthentications<T> where T : class
	{
		readonly IServiceScopeFactory _scopes;

		public Authentications(IServiceScopeFactory scopes) => _scopes = scopes;

		public AuthenticationSession<T> Get()
		{
			var scope   = _scopes.CreateAsyncScope();
			var subject = scope.ServiceProvider.GetRequiredService<SignInManager<T>>();
			return new(scope, subject);
		}
	}

}