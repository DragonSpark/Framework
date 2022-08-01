using DragonSpark.Model;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CircuitAwareInitializeConnection : IInitializeConnection
{
	readonly IInitializeConnection    _previous;
	readonly CurrentCircuitStore      _store;
	readonly IMutable<CircuitRecord?> _destination;

	public CircuitAwareInitializeConnection(IInitializeConnection previous, CurrentCircuitStore store)
		: this(previous, store, AmbientCircuit.Default) {}

	public CircuitAwareInitializeConnection(IInitializeConnection previous, CurrentCircuitStore store,
	                                        IMutable<CircuitRecord?> destination)
	{
		_previous    = previous;
		_store       = store;
		_destination = destination;
	}

	public void Execute(None parameter)
	{
		_previous.Execute(parameter);
		var current = _store.Get();
		if (current is not null)
		{
			_destination.Execute(current);
		}
	}
}