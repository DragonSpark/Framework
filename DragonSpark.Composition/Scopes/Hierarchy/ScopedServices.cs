using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Scopes.Hierarchy;

sealed class ScopedServices : IScopedServices
{
	readonly IServiceScopeFactory _factory;
	readonly IServiceProvider     _parent;

	public ScopedServices(IServiceScopeFactory factory, IServiceProvider parent)
	{
		_factory = factory;
		_parent  = parent;
	}

	public IScopedServiceProvider Get()
	{
		var result = new ScopedServiceProvider(_factory.CreateAsyncScope());
		result.GetRequiredService<ParentScopeProvider>().Execute(_parent);
		return result;
	}
}