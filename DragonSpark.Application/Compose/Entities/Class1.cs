using DragonSpark.Application.Security.Identity;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities
{
	class Class1 {}

	public sealed class IdentityStorageType<T, TContext> where TContext : DbContext where T : IdentityUser
	{
		readonly ApplicationProfileContext _subject;
		readonly Action<IdentityOptions>   _configure;

		public IdentityStorageType(ApplicationProfileContext subject, Action<IdentityOptions> configure)
		{
			_subject   = subject;
			_configure = configure;
		}

		public IdentityStorageUsing<T, TContext> Application() => Application(AllClaims.Default);

		public IdentityStorageUsing<T, TContext> Application(IClaims claims)
			=> new(_subject.Then(new IdentityRegistration<T>(claims)).Then(AddIdentityComponents<T>.Default),
			       _configure);

		public IdentityStorageUsing<T, TContext> Is => new(_subject, _configure);
	}

	public sealed class IdentityStorageUsing<T, TContext> where TContext : DbContext where T : class
	{
		readonly ApplicationProfileContext _subject;
		readonly Action<IdentityOptions>   _configure;

		public IdentityStorageUsing(ApplicationProfileContext subject, Action<IdentityOptions> configure)
		{
			_subject   = subject;
			_configure = configure;
		}

		public IdentityStorageConfiguration<T, TContext> Using => new(_subject, _configure);
	}

	public sealed class IdentityStorageConfiguration<T, TContext> where TContext : DbContext where T : class
	{
		readonly ApplicationProfileContext _subject;
		readonly Action<IdentityOptions>   _configure;

		public IdentityStorageConfiguration(ApplicationProfileContext subject, Action<IdentityOptions> configure)
		{
			_subject   = subject;
			_configure = configure;
		}

		public ConfiguredIdentityStorage<T, TContext> SqlServer(string name)
			=> Configuration(new SqlStorageConfiguration<TContext>(name));

		public ConfiguredIdentityStorage<T, TContext> SqlServer()
			=> Configuration(SqlStorageConfiguration<TContext>.Default);

		public ConfiguredIdentityStorage<T, TContext> Configuration(IStorageConfiguration configuration)
			=> new(_subject, _configure, configuration);
	}

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

		public ApplicationProfileContext Then => Register(ServiceLifetime.Scoped);

		public ApplicationProfileContext Register(Func<IServiceProvider, TContext> factory)
			=> _subject.Then(new AddIdentity<T, TContext>(_configuration, factory))
			           .Configure(Initialize<TContext>.Default.Get);

		public ApplicationProfileContext Register(ServiceLifetime lifetime)
			=> _subject.Then(new AddIdentity<T, TContext>(_configuration, _configure, lifetime))
			           .Configure(Initialize<TContext>.Default.Get);
	}
}