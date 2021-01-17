using DragonSpark.Application.Entities;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfigureStorage<T, TContext> : ICommand<IServiceCollection> where TContext : DbContext where T : class
	{
		readonly IStorageConfiguration   _storage;
		readonly Action<IdentityOptions> _configure;

		public ConfigureStorage(IStorageConfiguration storage) : this(storage, _ => {}) {}

		public ConfigureStorage(IStorageConfiguration storage, Action<IdentityOptions> configure)
		{
			_storage   = storage;
			_configure = configure;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDbContextFactory<TContext>(_storage.Get(parameter))
			         .AddScoped(p => p.GetRequiredService<IDbContextFactory<TContext>>().CreateDbContext())
			         .AddScoped<DbContext>(x => x.GetRequiredService<TContext>())
			         //
			         .Start<IStorageInitializer<TContext>>()
			         .Forward<StorageInitializer<TContext>>()
			         .Singleton()
			         //
			         .Then.AddDefaultIdentity<T>(_configure)
			         .AddEntityFrameworkStores<TContext>();
		}
	}
}