using System;
using DragonSpark.Application.AspNet.Entities.Configure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Compose.Entities;

public sealed class IdentityStorageConfiguration<T, TContext> where TContext : DbContext where T : class
{
    readonly ApplicationProfileContext _subject;
    readonly Action<IdentityOptions>   _configure;
    readonly Action<IdentityBuilder>   _builder;

    public IdentityStorageConfiguration(ApplicationProfileContext subject, Action<IdentityOptions> configure,
                                        Action<IdentityBuilder> builder)
    {
        _subject   = subject;
        _configure = configure;
        _builder   = builder;
    }

    public ConfiguredIdentityStorage<T, TContext> SqlServer()
        => Configuration(SqlStorageConfiguration<TContext>.Default);

    public ConfiguredIdentityStorage<T, TContext> SqlServer(Action<SqlServerDbContextOptionsBuilder> configuration)
        => Configuration(new SqlStorageConfiguration<TContext>(configuration));

    public ConfiguredIdentityStorage<T, TContext> Configuration(IStorageConfiguration configuration)
        => new(_subject, _configure, _builder, configuration);
}