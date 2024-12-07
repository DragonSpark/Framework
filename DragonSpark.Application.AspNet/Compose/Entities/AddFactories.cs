using DragonSpark.Application.Entities.Configure;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities;

sealed class AddFactories<T> : IAlteration<IServiceCollection> where T : DbContext
{
	readonly IStorageConfiguration     _storage;
	readonly Func<IServiceProvider, T> _factory;
	readonly ServiceLifetime           _lifetime;

	public AddFactories(IStorageConfiguration storage, ServiceLifetime lifetime = ServiceLifetime.Scoped)
		: this(storage, DefaultContextFactory<T>.Default.Get, lifetime) {}

	public AddFactories(IStorageConfiguration storage, Func<IServiceProvider, T> factory,
	                    ServiceLifetime lifetime = ServiceLifetime.Scoped)
	{
		_storage  = storage;
		_factory  = factory;
		_lifetime = lifetime;
	}

	public IServiceCollection Get(IServiceCollection parameter)
	{
		var collection = parameter.AddPooledDbContextFactory<T>(_storage.Get(parameter));

		return _lifetime switch
		{
			ServiceLifetime.Singleton => collection.AddSingleton(_factory)
			                                       .AddSingleton<DbContext>(x => x.GetRequiredService<T>()),
			ServiceLifetime.Scoped => collection.AddScoped(_factory)
			                                    .AddScoped<DbContext>(x => x.GetRequiredService<T>()),
			ServiceLifetime.Transient => collection.AddTransient(_factory)
			                                       .AddTransient<DbContext>(x => x.GetRequiredService<T>()),
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}