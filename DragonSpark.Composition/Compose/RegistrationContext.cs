using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public sealed class RegistrationContext<TFrom, TTo> : IRegistrationContext
		where TTo : class, TFrom where TFrom : class
	{
		readonly IServiceCollection _collection;

		public RegistrationContext(IServiceCollection collection) => _collection = collection;

		public IRegistrationContext WithDependencies
			=> new DependencyRegistrationContext(_collection, this, A.Type<TTo>());

		public IServiceCollection Singleton() => _collection.AddSingleton<TFrom, TTo>();

		public IServiceCollection Transient() => _collection.AddTransient<TFrom, TTo>();

		public IServiceCollection Scoped() => _collection.AddScoped<TFrom, TTo>();
	}

	public sealed class RegistrationContext : IRegistrationContext
	{
		readonly IServiceCollection _collection;
		readonly Type               _type;

		public RegistrationContext(IServiceCollection collection, Type type)
		{
			_collection = collection;
			_type       = type;
		}

		public IRegistrationContext WithDependencies => new DependencyRegistrationContext(_collection, this, _type);

		public IServiceCollection Singleton() => _collection.AddSingleton(_type);

		public IServiceCollection Transient() => _collection.AddTransient(_type);

		public IServiceCollection Scoped() => _collection.AddScoped(_type);
	}

	public sealed class RegistrationContext<T> where T : class
	{
		readonly IServiceCollection _collection;

		public RegistrationContext(IServiceCollection collection) => _collection = collection;

		public RegistrationContext Register => new RegistrationContext(_collection, A.Type<T>());

		public RegistrationContext<T, TTo> Map<TTo>() where TTo : class, T
			=> new RegistrationContext<T, TTo>(_collection);
	}
}