using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Application.AspNet.Compose.Entities;

public sealed class IdentityStorage<T> where T : IdentityUser
{
    readonly ApplicationProfileContext _subject;
    readonly Action<IdentityOptions>   _configure;
    readonly Action<IdentityBuilder>   _builder;

    public IdentityStorage(ApplicationProfileContext subject) : this(subject, _ => {}, _ => {}) {}

    public IdentityStorage(ApplicationProfileContext subject, Action<IdentityOptions> configure,
                           Action<IdentityBuilder> builder)
    {
        _subject   = subject;
        _configure = configure;
        _builder   = builder;
    }

    public IdentityStorage<T> UsingPasskeys()
        => new(_subject.Append(Security.Identity.Passkeys.Registrations<T>.Default)
                       .Append(Security.Identity.Passkeys.Configure.Default),
               _configure, _builder);

    public IdentityStorage<T, TContext> StoredIn<TContext>() where TContext : DbContext
        => new(_subject, _configure, _builder);
}

public sealed class IdentityStorage<T, TContext> where T : IdentityUser where TContext : DbContext
{
    readonly ApplicationProfileContext _subject;
    readonly Action<IdentityOptions>   _configure;
    readonly Action<IdentityBuilder>   _builder;

    public IdentityStorage(ApplicationProfileContext subject, Action<IdentityOptions> configure,
                           Action<IdentityBuilder> builder)
    {
        _subject   = subject;
        _configure = configure;
        _builder   = builder;
    }

    public IdentityStorageType<T, TContext> As => new(_subject, _configure, _builder);
}