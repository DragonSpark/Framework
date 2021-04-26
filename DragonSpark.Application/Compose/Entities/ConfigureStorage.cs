using DragonSpark.Application.Entities;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfigureServices<T> : IAlteration<IServiceCollection> where T : DbContext
	{
		readonly IStorageConfiguration     _storage;
		readonly Func<IServiceProvider, T> _factory;
		readonly ServiceLifetime           _lifetime;

		public ConfigureServices(IStorageConfiguration storage, ServiceLifetime lifetime = ServiceLifetime.Scoped)
			: this(storage, DefaultContextFactory<T>.Default.Get, lifetime) {}

		public ConfigureServices(IStorageConfiguration storage, Func<IServiceProvider, T> factory,
		                         ServiceLifetime lifetime = ServiceLifetime.Scoped)
		{
			_storage  = storage;
			_factory  = factory;
			_lifetime = lifetime;
		}

		public IServiceCollection Get(IServiceCollection parameter)
		{
			var collection = parameter.AddDbContextFactory<T>(_storage.Get(parameter), _lifetime);

			switch (_lifetime)
			{
				case ServiceLifetime.Singleton:
					return collection.AddSingleton(_factory)
					                 .AddSingleton<DbContext>(x => x.GetRequiredService<T>());
				case ServiceLifetime.Scoped:
					return collection.AddScoped(_factory)
					                 .AddScoped<DbContext>(x => x.GetRequiredService<T>());
				case ServiceLifetime.Transient:
					return collection.AddTransient(_factory)
					                 .AddTransient<DbContext>(x => x.GetRequiredService<T>());
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	sealed class ConfigureStorage<T, TContext> : ICommand<IServiceCollection> where TContext : DbContext where T : class
	{
		readonly ConfigureServices<TContext> _services;
		readonly Action<IdentityOptions>     _identity;

		
		public ConfigureStorage(IStorageConfiguration storage, ServiceLifetime lifetime = ServiceLifetime.Scoped)
			: this(storage, DefaultContextFactory<TContext>.Default.Get, lifetime) {}

		public ConfigureStorage(IStorageConfiguration storage, Func<IServiceProvider, TContext> factory,
		                        ServiceLifetime lifetime = ServiceLifetime.Scoped)
			: this(new ConfigureServices<TContext>(storage, factory, lifetime), _ => {}) {}

		public ConfigureStorage(IStorageConfiguration storage, Action<IdentityOptions> configure,
		                        ServiceLifetime lifetime = ServiceLifetime.Scoped)
			: this(new ConfigureServices<TContext>(storage, lifetime), configure) {}

		public ConfigureStorage(ConfigureServices<TContext> services, Action<IdentityOptions> identity)
		{
			_services = services;
			_identity = identity;
		}

		public void Execute(IServiceCollection parameter)
		{
			_services.Get(parameter)
			         .Start<IStorageInitializer<TContext>>()
			         .Forward<StorageInitializer<TContext>>()
			         .Singleton()
			         //
			         .Then.Start<IStorageInitializer>()
			         .Forward<StorageInitializer>()
			         .Singleton()
			         //
			         .Then.AddDefaultIdentity<T>(_identity)
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