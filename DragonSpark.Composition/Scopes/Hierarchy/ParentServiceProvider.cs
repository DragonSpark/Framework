using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Runtime;

namespace DragonSpark.Composition.Scopes.Hierarchy;

sealed class ParentServiceProvider : IParentServiceProvider
{
	readonly ParentScopeProvider _store;

	public ParentServiceProvider(ParentScopeProvider store) => _store = store;

	public object? GetService(Type serviceType) => _store.Get().Verify().GetService(serviceType);

	public void Dispose()
	{
		DisposeAny.Default.Execute(_store.Get().Verify());
	}

	public ValueTask DisposeAsync() => DisposingAny.Default.Get(_store.Get().Verify());
}