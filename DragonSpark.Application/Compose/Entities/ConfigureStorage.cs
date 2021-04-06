using DragonSpark.Application.Entities;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfigureStorage<T, TContext> : ICommand<IServiceCollection> where TContext : DbContext where T : class
	{
		readonly IStorageConfiguration            _storage;
		readonly Func<IServiceProvider, TContext> _factory;
		readonly Action<IdentityOptions>          _configure;

		public ConfigureStorage(IStorageConfiguration storage) : this(storage, _ => {}) {}

		public ConfigureStorage(IStorageConfiguration storage, Func<IServiceProvider, TContext> factory)
			: this(storage, factory, _ => {}) {}

		public ConfigureStorage(IStorageConfiguration storage, Action<IdentityOptions> configure)
			: this(storage, DefaultContextFactory<TContext>.Default.Get, configure) {}

		public ConfigureStorage(IStorageConfiguration storage, Func<IServiceProvider, TContext> factory,
		                        Action<IdentityOptions> configure)
		{
			_storage   = storage;
			_factory   = factory;
			_configure = configure;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDbContextFactory<TContext>(_storage.Get(parameter))
			         .AddScoped(_factory)
			         .AddScoped<DbContext>(x => x.GetRequiredService<TContext>())
			         //
			         .Start<IStorageInitializer<TContext>>()
			         .Forward<StorageInitializer<TContext>>()
			         .Singleton()
			         //
			         .Then.Start<IStorageInitializer>()
			         .Forward<StorageInitializer>()
			         .Singleton()
			         //
			         .Then.AddDefaultIdentity<T>(_configure)
			         .AddEntityFrameworkStores<TContext>();
		}
	}

	public sealed class DefaultContextFactory<T> : ISelect<IServiceProvider, T> where T : DbContext
	{
		public static DefaultContextFactory<T> Default { get; } = new DefaultContextFactory<T>();

		DefaultContextFactory() {}

		public T Get(IServiceProvider parameter)
			=> parameter.GetRequiredService<IDbContextFactory<T>>().CreateDbContext();
	}
}