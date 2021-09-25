using DragonSpark.Composition;
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
		readonly AsyncServiceScope? _scope;

		public UsersSession(UserManager<T> subject, AsyncServiceScope? scope = null)
		{
			_scope  = scope;
			Subject = subject;
		}

		public UserManager<T> Subject { get; }

		public ValueTask DisposeAsync() => _scope?.DisposeAsync() ?? ValueTask.CompletedTask;
	}

	public interface IUsers<T> : IResult<UsersSession<T>> where T : class {}

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