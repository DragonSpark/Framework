using DragonSpark.Composition;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	class Class1 {}

	public interface IAuthentications<T> : IResult<AuthenticationSession<T>> where T : class {}

	public readonly struct AuthenticationSession<T> : IDisposable where T : class
	{
		readonly IServiceScope? _scope;

		public AuthenticationSession(SignInManager<T> subject, IServiceScope? scope = null)
			: this(subject, subject.UserManager, scope) {}

		public AuthenticationSession(SignInManager<T> subject, UserManager<T> users, IServiceScope? scope)
		{
			Subject = subject;
			Users   = users;
			_scope  = scope;
		}

		public SignInManager<T> Subject { get; }
		public UserManager<T> Users { get; }

		public void Deconstruct(out SignInManager<T> subject, out UserManager<T> users)
		{
			subject = Subject;
			users   = Users;
		}

		public void Dispose()
		{
			_scope?.Dispose();
		}
	}

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

	sealed class AmbientAwareAuthentications<T> : IAuthentications<T> where T : class
	{
		readonly IAuthentications<T>         _previous;
		readonly IResult<AsyncServiceScope?> _established;

		public AmbientAwareAuthentications(IAuthentications<T> previous) : this(previous, LogicalScope.Default) {}

		public AmbientAwareAuthentications(IAuthentications<T> previous, IResult<AsyncServiceScope?> established)
		{
			_previous    = previous;
			_established = established;
		}

		public AuthenticationSession<T> Get()
		{
			var current = _established.Get();
			var result = current != null
				             ? new(current.Value.ServiceProvider.GetRequiredService<SignInManager<T>>())
				             : _previous.Get();
			return result;
		}
	}
}