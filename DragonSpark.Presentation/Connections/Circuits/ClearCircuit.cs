using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class ClearCircuit : IOperation<Circuit>
{
	readonly AssignedCircuit          _assigned;
	readonly IMutable<CircuitRecord?> _current;

	public ClearCircuit(AssignedCircuit assigned) : this(assigned, CurrentCircuit.Default) {}

	public ClearCircuit(AssignedCircuit assigned, IMutable<CircuitRecord?> current)
	{
		_assigned = assigned;
		_current  = current;
	}

	public ValueTask Get(Circuit parameter)
	{
		var current = _current.Get();
		if (current is not null)
		{
			_assigned.Execute(parameter.Id, current.Subject.Id);
			_current.Execute(null);
		}
		/*else if (parameter.Id != current.Subject.Id)
		{
			_unexpected.Execute(parameter.Id, current.Subject.Id);
		}*/

		return ValueTask.CompletedTask;
	}

	public sealed class AssignedCircuit : LogWarning<string, string>
	{
		public AssignedCircuit(ILogger<AssignedCircuit> logger)
			: base(logger,
			       "Attempted to clear circuit {Expected} but was already assigned with {Actual}") {}
	}
}