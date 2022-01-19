using DragonSpark.Application.Entities.Configure;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities;

public sealed class ConfiguredIdentityStorage<T, TContext> where TContext : DbContext where T : class
{
	readonly ApplicationProfileContext _subject;
	readonly Action<IdentityOptions>   _configure;
	readonly IStorageConfiguration     _configuration;

	public ConfiguredIdentityStorage(ApplicationProfileContext subject, Action<IdentityOptions> configure,
	                                 IStorageConfiguration configuration)
	{
		_subject       = subject;
		_configure     = configure;
		_configuration = configuration;
	}

	public ConfiguredIdentityStorage<T, TContext> And(Alter<StorageConfigurationBuilder> configuration)
		=> And(configuration(new StorageConfigurationBuilder()).Get());

	public ConfiguredIdentityStorage<T, TContext> And(IStorageConfiguration configuration)
		=> new(_subject, _configure, new AppendedStorageConfiguration(_configuration, configuration));

	public ApplicationProfileContext Then
		=> _subject.Append(new AddIdentity<T, TContext>(_configuration, _configure));

	/*public ApplicationProfileContext Register(Func<IServiceProvider, TContext> factory)
		=> _subject.Then(new AddIdentity<T, TContext>(_configuration, factory));*/

	/*public ApplicationProfileContext Register(ServiceLifetime lifetime)
		=> _subject.Then(new AddIdentity<T, TContext>(_configuration, _configure, lifetime));*/
}