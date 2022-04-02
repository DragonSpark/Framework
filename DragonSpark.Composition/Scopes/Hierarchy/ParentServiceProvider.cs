using System;

namespace DragonSpark.Composition.Scopes.Hierarchy;

sealed class ParentServiceProvider : IParentServiceProvider
{
	readonly ParentScopeProvider _store;

	public ParentServiceProvider(ParentScopeProvider store) => _store = store;

	public object? GetService(Type serviceType) => _store.Get().GetService(serviceType);
}