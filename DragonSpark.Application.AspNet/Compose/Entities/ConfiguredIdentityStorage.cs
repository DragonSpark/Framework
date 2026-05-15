using System;
using DragonSpark.Application.AspNet.Entities.Configure;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Compose.Entities;

public sealed class ConfiguredIdentityStorage<T, TContext> where TContext : DbContext where T : class
{
    readonly ApplicationProfileContext _subject;
    readonly Action<IdentityOptions>   _configure;
    readonly Action<IdentityBuilder>   _builder;
    readonly IStorageConfiguration     _configuration;

    // ReSharper disable once TooManyDependencies
    public ConfiguredIdentityStorage(ApplicationProfileContext subject, Action<IdentityOptions> configure,
                                     Action<IdentityBuilder> builder, IStorageConfiguration configuration)
    {
        _subject       = subject;
        _configure     = configure;
        _builder  = builder;
        _configuration = configuration;
    }

    public ConfiguredIdentityStorage<T, TContext> And(Alter<StorageConfigurationBuilder> configuration)
        => And(configuration(new StorageConfigurationBuilder()).Get());

    public ConfiguredIdentityStorage<T, TContext> And(IStorageConfiguration configuration)
        => new(_subject, _configure, _builder, new AppendedStorageConfiguration(_configuration, configuration));

    public ApplicationProfileContext Then
        => _subject.Append(new AddDefaultIdentity<T, TContext>(_configuration, _configure));

    public ApplicationProfileContext Core
        => _subject.Append(new AddIdentity<T, TContext>(_configuration, _configure, _builder));
}