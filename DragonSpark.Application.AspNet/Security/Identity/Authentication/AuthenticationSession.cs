using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

[method: MustDisposeResource]
public readonly struct AuthenticationSession<T>(SignInManager<T> subject, UserManager<T> users, IServiceScope? scope)
	: IDisposable
	where T : class
{
	[MustDisposeResource]
	public AuthenticationSession(SignInManager<T> subject, IServiceScope? scope = null)
		: this(subject, subject.UserManager, scope) {}

	public SignInManager<T> Subject { get; } = subject;
	public UserManager<T> Users { get; } = users;

	public void Deconstruct(out SignInManager<T> subject, [MustDisposeResource(false)] out UserManager<T> users)
	{
		subject = Subject;
		users   = Users;
	}

	public void Dispose()
	{
		scope?.Dispose();
	}
}
