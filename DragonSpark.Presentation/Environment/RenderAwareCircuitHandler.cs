using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment;

sealed class RenderAwareCircuitHandler : CircuitHandler
{
	readonly RenderStateStore _store;

	public RenderAwareCircuitHandler(RenderStateStore store) => _store = store;

	public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_store.Execute(RenderState.Established);
		return base.OnConnectionUpAsync(circuit, cancellationToken);
	}

	public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_store.Execute(RenderState.Destroyed);
		return base.OnConnectionDownAsync(circuit, cancellationToken);
	}

	public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_store.Execute(RenderState.Destroyed);
		return base.OnCircuitClosedAsync(circuit, cancellationToken);
	}
}