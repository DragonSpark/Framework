using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
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
}