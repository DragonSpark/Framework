using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Environment;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CurrentCircuitRecord : IMutable<CircuitRecord?>
{
	public static CurrentCircuitRecord Default { get; } = new();

	CurrentCircuitRecord() : this(A.Result(AmbientContext.Default).Then().Select(x => x?.Items).Get()) {}

	readonly object                                 _key;
	readonly IResult<IDictionary<object, object?>?> _store;

	public CurrentCircuitRecord(IResult<IDictionary<object, object?>?> store) : this(new object(), store) {}

	public CurrentCircuitRecord(object key, IResult<IDictionary<object, object?>?> store)
	{
		_key   = key;
		_store = store;
	}

	public CircuitRecord? Get() => _store.Get()?[_key] as CircuitRecord;

	public void Execute(CircuitRecord? parameter)
	{
		var store = _store.Get();
		if (store is not null)
		{
			store[_key] = parameter;
		}
	}
}