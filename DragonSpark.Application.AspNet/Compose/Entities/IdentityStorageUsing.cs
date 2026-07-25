using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Compose.Entities;

public sealed class IdentityStorageUsing<T, TContext> where TContext : DbContext where T : class
{
    readonly ApplicationProfileContext _subject;
    readonly Action<IdentityOptions>   _configure;
    readonly Action<IdentityBuilder>   _builder;

    public IdentityStorageUsing(ApplicationProfileContext subject, Action<IdentityOptions> configure,
                                Action<IdentityBuilder> builder)
    {
        _subject   = subject;
        _configure = configure;
        _builder   = builder;
    }

    public IdentityStorageConfiguration<T, TContext> Using => new(_subject, _configure, _builder);
}