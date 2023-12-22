using DragonSpark.Runtime;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Composition.Scopes.Hierarchy;

sealed class ParentServiceProvider : IParentServiceProvider
{
	readonly ParentScopeProvider _store;

	public ParentServiceProvider(ParentScopeProvider store) => _store = store;

	public object? GetService(Type serviceType) => _store.Get().GetService(serviceType);

	public void Dispose()
	{
		DisposeAny.Default.Execute(_store.Get());
	}

	public ValueTask DisposeAsync() => DisposingAny.Default.Get(_store.Get());
}