using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity.Authentication;

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