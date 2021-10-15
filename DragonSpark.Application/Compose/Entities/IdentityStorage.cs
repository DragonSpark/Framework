using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities;

public sealed class IdentityStorage<T> where T : IdentityUser
{
	readonly ApplicationProfileContext _subject;
	readonly Action<IdentityOptions>   _configure;

	public IdentityStorage(ApplicationProfileContext subject) : this(subject, _ => {}) {}

	public IdentityStorage(ApplicationProfileContext subject, Action<IdentityOptions> configure)
	{
		_subject   = subject;
		_configure = configure;
	}

	public IdentityStorage<T, TContext> StoredIn<TContext>() where TContext : DbContext
		=> new(_subject, _configure);
}

public sealed class IdentityStorage<T, TContext> where T : IdentityUser where TContext : DbContext
{
	readonly ApplicationProfileContext _subject;
	readonly Action<IdentityOptions>   _configure;

	public IdentityStorage(ApplicationProfileContext subject, Action<IdentityOptions> configure)
	{
		_subject   = subject;
		_configure = configure;
	}

	public IdentityStorageType<T, TContext> As => new(_subject, _configure);
}