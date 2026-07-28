using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output.Compose;

public sealed class StartRegistration<T> where T : notnull
{
	readonly RegistrationComponents _components;

	public StartRegistration(IOutputCacheStore store, IOutputKey[] keys) : this(new(store, keys)) {}

	public StartRegistration(RegistrationComponents components) => _components = components;

	public SelectedKeyRegistration<T, TKey> For<TKey>(Func<T, TKey> select) => new(_components, select);

	public IStopAware<T> Build()
	{
		var (store, keys, registrations) = _components;
		return new Evict<T>(store, new RegistrationAwareTags(registrations), keys);
	}
}