using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity;

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