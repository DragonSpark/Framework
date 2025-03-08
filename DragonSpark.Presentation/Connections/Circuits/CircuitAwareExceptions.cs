using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CircuitAwareExceptions : IExceptions
{
	readonly IExceptions              _exceptions;
	readonly CurrentCircuitStore      _store;
	readonly IMutable<CircuitRecord?> _ambient;

	public CircuitAwareExceptions(IExceptions exceptions, CurrentCircuitStore store)
		: this(exceptions, store, AmbientCircuit.Default) {}

	public CircuitAwareExceptions(IExceptions exceptions, CurrentCircuitStore store, IMutable<CircuitRecord?> ambient)
	{
		_exceptions = exceptions;
		_store      = store;
		_ambient    = ambient;
	}

	public async ValueTask Get(ExceptionInput parameter)
	{
		var record  = _store.Get();
		var current = _ambient.Get();
		_ambient.Execute(record);
		await _exceptions.Off(parameter);
		_ambient.Execute(current);
	}
}