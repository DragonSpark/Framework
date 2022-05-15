using DragonSpark.Compose;
using DragonSpark.Model;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class DeferredRegistrationStateAccessor : IDeferredRegistrationStateAccessor
{
	public static DeferredRegistrationStateAccessor Default { get; } = new();

	DeferredRegistrationStateAccessor() : this(typeof(DeferredRegistrationStateAccessor)) { }

	readonly object _key;

	public DeferredRegistrationStateAccessor(object key) => _key = key;

	public DeferredRegistrations Get(IDictionary<object, object> parameter)
		=> parameter[_key].To<DeferredRegistrations>();

	public void Execute(Pair<IDictionary<object, object>, DeferredRegistrations> parameter)
	{
		var (key, value) = parameter;
		key[_key]        = value;
	}
}