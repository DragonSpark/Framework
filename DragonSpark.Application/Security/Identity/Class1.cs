using DragonSpark.Composition;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity
{
	class Class1 {}

	public readonly struct UsersSession<T> : IDisposable where T : class
	{
		readonly IServiceScope? _scope;

		public UsersSession(UserManager<T> subject, IServiceScope? scope = null)
		{
			_scope  = scope;
			Subject = subject;
		}

		public UserManager<T> Subject { get; }

		public void Dispose()
		{
			_scope?.Dispose();
		}
	}

	public interface IUsers<T> : IResult<UsersSession<T>> where T : class {}

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

	sealed class AmbientAwareUsers<T> : IUsers<T> where T : class
	{
		readonly IUsers<T>                   _previous;
		readonly IResult<AsyncServiceScope?> _established;

		public AmbientAwareUsers(IUsers<T> previous) : this(previous, LogicalScope.Default) {}

		public AmbientAwareUsers(IUsers<T> previous, IResult<AsyncServiceScope?> established)
		{
			_previous    = previous;
			_established = established;
		}

		public UsersSession<T> Get()
		{
			var current = _established.Get();
			var result = current != null
				             ? new(current.Value.ServiceProvider.GetRequiredService<UserManager<T>>())
				             : _previous.Get();
			return result;
		}
	}
}