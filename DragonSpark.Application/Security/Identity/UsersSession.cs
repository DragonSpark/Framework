using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity;

public readonly struct UsersSession<T> : IDisposable where T : class
{
	readonly IServiceScope? _scope;

	public UsersSession(UserManager<T> subject, IUserStore<T> store, IServiceScope? scope = null)
	{
		_scope  = scope;
		Subject = subject;
		Store   = store;
	}

	public UserManager<T> Subject { get; }
	public IUserStore<T> Store { get; }

	public void Dispose()
	{
		_scope?.Dispose();
	}
}