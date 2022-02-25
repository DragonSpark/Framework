using DragonSpark.Diagnostics.Logging;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class RecordAwareCircuitHandler : CircuitHandler
{
	readonly RegisterCircuit _register;
	readonly ClearCircuit    _clear;
	readonly Template        _template;

	public RecordAwareCircuitHandler(RegisterCircuit register, ClearCircuit clear, Template template)
	{
		_register      = register;
		_clear         = clear;
		_template = template;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_template.Execute(circuit.Id);
		return _register.Get(circuit).AsTask();
	}

	public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
		=> _clear.Get(circuit).AsTask();

	public sealed class Template : LogMessage<string>
	{
		public Template(ILogger<Template> logger) : base(logger, "Circuit {Identity} connected") {}
	}
}