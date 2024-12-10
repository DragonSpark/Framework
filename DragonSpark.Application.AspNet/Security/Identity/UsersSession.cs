using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity;

[method: MustDisposeResource]
public readonly struct UsersSession<T>(UserManager<T> subject, IServiceScope? scope = null)
	: IDisposable where T : class
{
	readonly IServiceScope? _scope = scope;

	public UserManager<T> Subject { get; } = subject;

	public void Dispose()
	{
		_scope?.Dispose();
	}
}
