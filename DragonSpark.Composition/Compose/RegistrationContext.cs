using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose;

sealed class RegistrationContext : IRegistrationContext
{
	readonly IServiceCollection _collection;
	readonly Type               _type;

	public RegistrationContext(IServiceCollection collection, Type type)
	{
		_collection = collection;
		_type       = type;
	}

	public IServiceCollection Singleton() => _collection.AddSingleton(_type);

	public IServiceCollection Transient() => _collection.AddTransient(_type);

	public IServiceCollection Scoped() => _collection.AddScoped(_type);
}